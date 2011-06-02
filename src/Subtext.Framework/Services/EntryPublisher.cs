#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Properties;
using Subtext.Framework.Services.SearchEngine;
using Subtext.Framework.Text;

namespace Subtext.Framework.Services
{
    public class EntryPublisher : IEntryPublisher
    {
        public EntryPublisher(ISubtextContext context, ITextTransformation transformation, ISlugGenerator slugGenerator,
            IIndexingService indexingService)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (indexingService == null)
            {
                throw new ArgumentNullException("indexingService");
            }
            SubtextContext = context;
            Transformation = transformation ?? EmptyTextTransformation.Instance;
            SlugGenerator = slugGenerator ?? new SlugGenerator(FriendlyUrlSettings.Settings, context.Repository);
            IndexingService = indexingService;
        }

        public ITextTransformation Transformation { get; private set; }

        public ISubtextContext SubtextContext { get; private set; }

        public ISlugGenerator SlugGenerator { get; private set; }

        public IIndexingService IndexingService { get; private set; }

        #region IEntryPublisher Members

        public int Publish(Entry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            if (entry.PostType == PostType.None)
            {
                throw new ArgumentException(Resources.InvalidOperation_PostTypeIsNone, "entry");
            }

            entry.Body = Transformation.Transform(entry.Body);

            if (String.IsNullOrEmpty(entry.EntryName))
            {
                entry.EntryName = SlugGenerator.GetSlugFromTitle(entry);
            }
            if (entry.EntryName.IsNumeric())
            {
                entry.EntryName = "n_" + entry.EntryName;
            }
            if (entry.IsActive)
            {
                if (entry.DatePublishedUtc.IsNull() && entry.IncludeInMainSyndication)
                {
                    entry.DatePublishedUtc = DateTime.UtcNow;
                }
            }
            else
            {
                entry.DatePublishedUtc = NullValue.NullDateTime;
            }

            IEnumerable<int> categoryIds = null;
            if (entry.Categories.Count > 0)
            {
                categoryIds = GetCategoryIdsFromCategoryTitles(entry);
            }

            try
            {
                entry.DateModifiedUtc = entry.DateModifiedUtc.IsNull() ? DateTime.UtcNow : entry.DateModifiedUtc;
                if (entry.Id.IsNull())
                {
                    entry.DateCreatedUtc = entry.DateCreatedUtc.IsNull() ? DateTime.UtcNow : entry.DateCreatedUtc;
                    SubtextContext.Repository.Create(entry, categoryIds);
                }
                else
                {
                    SubtextContext.Repository.Update(entry, categoryIds);
                }
            }
            catch (DbException e)
            {
                if (e.Message.Contains("pick a unique EntryName"))
                {
                    throw new DuplicateEntryException(Resources.DuplicateEntryException_EntryNameAlreadyExists, e);
                }
                throw;
            }

            ValidateEntry(entry);
            IList<string> tags = entry.Body.ParseTags();
            SubtextContext.Repository.SetEntryTagList(entry.Id, tags);
            IndexingService.AddPost(entry, tags);
            return entry.Id;
        }

        #endregion

        private static void ValidateEntry(Entry e)
        {
            //TODO: The following doesn't belong here. It's verification code.
            if (!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Body))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            //Never allow scripts in the title.
            if (HtmlHelper.HasIllegalContent(e.Title))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            if (!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Description))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            //never allow scripts in the url.
            if (HtmlHelper.HasIllegalContent(e.EntryName))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            return;
        }

        private IEnumerable<int> GetCategoryIdsFromCategoryTitles(Entry entry)
        {
            IEnumerable<int> categoryIds = from categoryName in entry.Categories
                                           let category = SubtextContext.Repository.GetLinkCategory(categoryName, true)
                                           where category != null
                                           select category.Id;

            return categoryIds;
        }
    }
}