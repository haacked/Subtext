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
using System.Data;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Data
{
    /// <summary>
    /// Concrete implementation of <see cref="ObjectProvider"/>. This 
    /// provides objects persisted to a database.
    /// </summary>
    public partial class DatabaseObjectProvider : ObjectProvider
    {
        readonly StoredProcedures _procedures = new StoredProcedures(Config.ConnectionString);

        public int BlogId
        {
            get
            {
                //Fix this up...
                return BlogRequest.Current.IsHostAdminRequest ? NullValue.NullInt32 : BlogRequest.Current.Blog.Id;
            }
        }

        public DateTime CurrentDateTime
        {
            get { return BlogRequest.Current.Blog.TimeZone.Now; }
        }

        private static void ValidateEntry(Entry e)
        {
            //TODO: The following doesn't belong here. It's verification code.
            if(!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Body))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            //Never allow scripts in the title.
            if(HtmlHelper.HasIllegalContent(e.Title))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            if(!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Description))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }

            //never allow scripts in the url.
            if(HtmlHelper.HasIllegalContent(e.EntryName))
            {
                throw new IllegalPostCharactersException(Resources.IllegalPostCharacters);
            }
        }

        public override bool TrackEntry(EntryView entryView)
        {
            return ThreadHelper.FireAndForget(o =>
                                              _procedures.TrackEntry(entryView.EntryId, entryView.BlogId, entryView.ReferralUrl,
                                                                     entryView.PageViewType == PageViewType.WebView), "Exception while tracking an entry");
        }

        public override ICollection<LinkCategory> GetActiveCategories()
        {
            using(IDataReader reader = _procedures.GetActiveCategoriesWithLinkCollection(BlogId.NullIfMinValue()))
            {
                return reader.ReadLinkCategories(true);
            }
        }

        public override IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
        {
            using(
                IDataReader reader = _procedures.GetPageableReferrers(BlogId, entryId.NullIfMinValue(), pageIndex,
                                                                      pageSize))
            {
                return reader.ReadPagedCollection(r => DataHelper.ReadReferrer(r, Config.CurrentBlog));
            }
        }

        public override int Create(MetaTag metaTag)
        {
            return _procedures.InsertMetaTag(metaTag.Content,
                                             metaTag.Name.NullIfEmpty(),
                                             metaTag.HttpEquiv.NullIfEmpty(),
                                             BlogId,
                                             metaTag.EntryId,
                                             metaTag.DateCreated);
        }

        public override bool Update(MetaTag metaTag)
        {
            return _procedures.UpdateMetaTag(metaTag.Id,
                                             metaTag.Content,
                                             metaTag.Name.NullIfEmpty(),
                                             metaTag.HttpEquiv.NullIfEmpty(),
                                             BlogId,
                                             metaTag.EntryId);
        }

        public override IPagedCollection<MetaTag> GetMetaTagsForBlog(Blog blog, int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetMetaTags(blog.Id, null, pageIndex, pageSize))
            {
                return reader.ReadPagedCollection(r => r.ReadObject<MetaTag>());
            }
        }

        public override IPagedCollection<MetaTag> GetMetaTagsForEntry(Entry entry, int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetMetaTags(entry.BlogId, entry.Id, pageIndex, pageSize))
            {
                return reader.ReadPagedCollection(r => r.ReadObject<MetaTag>());
            }
        }

        public override bool DeleteMetaTag(int metaTagId)
        {
            return _procedures.DeleteMetaTag(metaTagId);
        }

        public override int Create(Enclosure enclosure)
        {
            return _procedures.InsertEnclosure(enclosure.Title ?? string.Empty,
                                               enclosure.Url,
                                               enclosure.MimeType,
                                               enclosure.Size,
                                               enclosure.AddToFeed,
                                               enclosure.ShowWithPost,
                                               enclosure.EntryId);
        }

        public override bool Update(Enclosure enclosure)
        {
            return _procedures.UpdateEnclosure(enclosure.Title,
                                               enclosure.Url,
                                               enclosure.MimeType,
                                               enclosure.Size,
                                               enclosure.AddToFeed,
                                               enclosure.ShowWithPost,
                                               enclosure.EntryId,
                                               enclosure.Id);
        }

        public override bool DeleteEnclosure(int enclosureId)
        {
            return _procedures.DeleteEnclosure(enclosureId);
        }

        public override KeyWord GetKeyWord(int keyWordId)
        {
            using(IDataReader reader = _procedures.GetKeyWord(keyWordId, BlogId))
            {
                KeyWord kw = null;
                while(reader.Read())
                {
                    kw = reader.ReadObject<KeyWord>();
                    break;
                }
                return kw;
            }
        }

        public override ICollection<KeyWord> GetKeyWords()
        {
            using(IDataReader reader = _procedures.GetBlogKeyWords(BlogId))
            {
                return reader.ReadCollection<KeyWord>();
            }
        }

        public override IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize)
        {
            using(IDataReader reader = _procedures.GetPageableKeyWords(BlogId, pageIndex, pageSize))
            {
                return reader.ReadPagedCollection(r => r.ReadObject<KeyWord>());
            }
        }

        public override bool UpdateKeyWord(KeyWord keyWord)
        {
            return _procedures.UpdateKeyWord(keyWord.Id,
                                             keyWord.Word,
                                             keyWord.Rel,
                                             keyWord.Text,
                                             keyWord.ReplaceFirstTimeOnly,
                                             keyWord.OpenInNewWindow,
                                             keyWord.CaseSensitive,
                                             keyWord.Url,
                                             keyWord.Title,
                                             BlogId);
        }

        public override int InsertKeyWord(KeyWord keyWord)
        {
            return _procedures.InsertKeyWord(keyWord.Word,
                                             keyWord.Rel,
                                             keyWord.Text,
                                             keyWord.ReplaceFirstTimeOnly,
                                             keyWord.OpenInNewWindow,
                                             keyWord.CaseSensitive,
                                             keyWord.Url,
                                             keyWord.Title,
                                             BlogId);
        }

        public override bool DeleteKeyWord(int id)
        {
            return _procedures.DeleteKeyWord(id, BlogId);
        }

        public override ImageCollection GetImagesByCategoryId(int categoryId, bool activeOnly)
        {
            using(IDataReader reader = _procedures.GetImageCategory(categoryId, activeOnly, BlogId))
            {
                var ic = new ImageCollection();
                while(reader.Read())
                {
                    ic.Category = reader.ReadLinkCategory();
                    break;
                }
                reader.NextResult();
                while(reader.Read())
                {
                    ic.Add(reader.ReadImage());
                }
                return ic;
            }
        }

        public override Image GetImage(int imageId, bool activeOnly)
        {
            using(IDataReader reader = _procedures.GetSingleImage(imageId, activeOnly, BlogId))
            {
                Image image = null;
                while(reader.Read())
                {
                    image = reader.ReadImage();
                }
                return image;
            }
        }

        public override int InsertImage(Image image)
        {
            return _procedures.InsertImage(image.Title,
                                           image.CategoryID,
                                           image.Width,
                                           image.Height,
                                           image.FileName,
                                           image.IsActive,
                                           BlogId,
                                           image.Url);
        }

        public override bool UpdateImage(Image image)
        {
            return _procedures.UpdateImage(image.Title,
                                           image.CategoryID,
                                           image.Width,
                                           image.Height,
                                           image.FileName,
                                           image.IsActive,
                                           BlogId,
                                           image.ImageID,
                                           image.Url);
        }

        public override bool DeleteImage(int imageId)
        {
            return _procedures.DeleteImage(BlogId, imageId);
        }

        private static LinkCategory ReadLinkCategory(IDataReader reader)
        {
            return !reader.Read() ? null : reader.ReadLinkCategory();
        }
    }
}