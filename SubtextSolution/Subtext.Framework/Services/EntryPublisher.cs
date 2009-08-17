﻿#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
using Subtext.Framework.Text;

namespace Subtext.Framework.Services
{
    public class EntryPublisher : IEntryPublisher
    {
        public EntryPublisher(ISubtextContext context, ITextTransformation transformation, ISlugGenerator slugGenerator)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            SubtextContext = context;
            Transformation = transformation ?? EmptyTextTransformation.Instance;
            SlugGenerator = slugGenerator ?? new SlugGenerator(FriendlyUrlSettings.Settings, context.Repository);
        }

        public ITextTransformation Transformation
        {
            get;
            private set;
        }

        public ISubtextContext SubtextContext
        {
            get;
            private set;
        }

        public ISlugGenerator SlugGenerator
        {
            get;
            private set;
        }

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
            if (NullValue.IsNull(entry.DateCreated))
            {
                entry.DateCreated = SubtextContext.Blog.TimeZone.Now;
            }
            if (entry.IsActive)
            {
                if (NullValue.IsNull(entry.DateSyndicated))
                {
                    entry.DateSyndicated = SubtextContext.Blog.TimeZone.Now;
                }
            }
            else
            {
                entry.DateSyndicated = NullValue.NullDateTime;
            }

            IEnumerable<int> categoryIds = null;
            if (entry.Categories.Count > 0)
            {
                categoryIds = GetCategoryIdsFromCategoryTitles(entry);
            }

            try
            {
                SubtextContext.Repository.Create(entry, categoryIds);
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
            SubtextContext.Repository.SetEntryTagList(entry.Id, HtmlHelper.ParseTags(entry.Body));
            return entry.Id;
        }

        private bool ValidateEntry(Entry e)
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

            return true;
        }

        private IEnumerable<int> GetCategoryIdsFromCategoryTitles(Entry entry)
        {
            var categoryIds = from categoryName in entry.Categories
                              let category = SubtextContext.Repository.GetLinkCategory(categoryName, true)
                              where category != null
                              select category.Id;

            return categoryIds;
        }
    }
}