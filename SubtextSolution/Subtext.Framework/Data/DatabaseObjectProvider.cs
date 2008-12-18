#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Concrete implementation of <see cref="ObjectProvider"/>. This 
	/// provides objects persisted to a database.
	/// </summary>
	public partial class DatabaseObjectProvider : ObjectProvider
	{
        StoredProcedures _procedures = new StoredProcedures(Config.ConnectionString);

        public int BlogId {
            get
            {
                if (InstallationManager.IsInHostAdminDirectory)
                    return NullValue.NullInt32;
                else {
                    return Config.CurrentBlog.Id;
                }
            }
        }

        public DateTime CurrentDateTime {
            get {
                return Config.CurrentBlog.TimeZone.Now;
            }
        }

        private static bool FormatEntry(Entry e, bool useKeyWords)
		{
			//Do this before we validate the text
			if(useKeyWords)
			{
				KeyWords.Format(e);
			}

			//TODO: Make this a configuration option.
			e.Body = Transform.EmoticonTransforms(e.Body);

			if (!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Body))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			//Never allow scripts in the title.
			if(HtmlHelper.HasIllegalContent(e.Title))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			if (!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Description))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			//never allow scripts in the url.
			if(HtmlHelper.HasIllegalContent(e.Url))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			if(!HtmlHelper.ConvertHtmlToXHtml(e))
			{
				return false;
			}

			return true;
		}

        public override bool TrackEntry(EntryView entryView)
        {
            return _procedures.TrackEntry(entryView.EntryId, entryView.BlogId, entryView.ReferralUrl, entryView.PageViewType == PageViewType.WebView);
        }

        public override bool TrackEntry(IEnumerable<EntryView> entryViews)
        {
            if (entryViews != null)
            {
                foreach (EntryView ev in entryViews)
                {
                    TrackEntry(ev);
                }
                return true;
            }

            return false;
        }

        public override ICollection<LinkCategory> GetActiveCategories()
		{
            using (IDataReader reader = _procedures.GetActiveCategoriesWithLinkCollection(BlogId.NullIfMinValue())) {
                return reader.LoadLinkCategories(true);
            }
		}

        public override IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
            using (IDataReader reader = _procedures.GetPageableReferrers(BlogId, entryId.NullIfMinValue(), pageIndex, pageSize)) {
                return reader.GetPagedCollection(r => DataHelper.LoadReferrer(r));
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

	    public override IPagedCollection<MetaTag> GetMetaTagsForBlog(BlogInfo blog, int pageIndex, int pageSize)
		{
			using (IDataReader reader = _procedures.GetMetaTags(blog.Id, null, pageIndex, pageSize))
			{
                return reader.GetPagedCollection(r => DataHelper.LoadObject<MetaTag>(r));
			}
		}

        public override IPagedCollection<MetaTag> GetMetaTagsForEntry(Entry entry, int pageIndex, int pageSize)
	    {
            using (IDataReader reader = _procedures.GetMetaTags(entry.BlogId, entry.Id, pageIndex, pageSize))
	        {
                return reader.GetPagedCollection(r => DataHelper.LoadObject<MetaTag>(r));
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
					kw = DataHelper.LoadObject<KeyWord>(reader);
					break;
				}
				return kw;
			}
		}
		
		public override ICollection<KeyWord> GetKeyWords()
		{
			using(IDataReader reader = _procedures.GetBlogKeyWords(BlogId)) 
            {
				List<KeyWord> kwc = new List<KeyWord>();
				while(reader.Read())
				{
					kwc.Add(DataHelper.LoadObject<KeyWord>(reader));
				}
				return kwc;
			}
		}

		public override IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize)
		{
            using (IDataReader reader = _procedures.GetPageableKeyWords(BlogId, pageIndex, pageSize)) {
                return reader.GetPagedCollection(r => DataHelper.LoadObject<KeyWord>(r));
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

		public override ImageCollection GetImagesByCategoryID(int categoryId, bool activeOnly)
		{
            using(IDataReader reader = _procedures.GetImageCategory(categoryId, activeOnly, BlogId))
			{
				ImageCollection ic = new ImageCollection();
				while(reader.Read())
				{
					ic.Category = DataHelper.LoadLinkCategory(reader);
					break;
				}
				reader.NextResult();
				while(reader.Read())
				{
					ic.Add(DataHelper.LoadImage(reader));
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
					image = DataHelper.LoadImage(reader);
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
                BlogId);
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
                image.ImageID);
		}

		public override bool DeleteImage(int imageId)
		{
            return _procedures.DeleteImage(BlogId, imageId);
		}
	}
}
