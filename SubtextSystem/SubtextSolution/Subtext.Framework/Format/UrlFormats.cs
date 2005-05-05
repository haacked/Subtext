using System;
using Subtext.Framework.Components;

namespace Subtext.Framework.Format
{
	/// <summary>
	/// Default Implemenation of UrlFormats
	/// </summary>
	public class UrlFormats
	{
		protected string fullyQualifiedUrl = null;

		public UrlFormats(string fullyQualifiedUrl)
		{
			this.fullyQualifiedUrl = fullyQualifiedUrl;
		}

		public virtual string PostCategoryUrl(string categoryName, int categoryID)
		{
			return GetUrl("category/{0}.aspx",categoryID);
		}
		
		public virtual string ArticleCategoryUrl(string categoryName, int categoryID)
		{
			return GetUrl("category/{0}.aspx",categoryID);
		}

		public virtual string EntryUrl(Entry entry)
		{
			return GetUrl("archive/" + entry.DateCreated.ToString("yyyy/MM/dd") + "/{0}.aspx", entry.HasEntryName ? entry.EntryName : entry.EntryID.ToString());
		}

		public virtual string ImageUrl(string category, int ImageID)
		{
			return GetUrl("gallery/image/{0}.aspx",ImageID);
		}

		public virtual string YearUrl(DateTime dt)
		{
			return GetUrl("archive/{0}.aspx",dt.ToString("yyyy"));
		}

		public virtual string DayUrl(DateTime dt)
		{
			//return GetUrl("archive/{0}/{1}/{2}.aspx",dt.Year,dt.Month,dt.Day);
			return GetUrl("archive/{0}.aspx",dt.ToString("yyyy/MM/dd"));
		}

		public virtual string GalleryUrl(string category, int GalleryID)
		{
			return GetUrl("gallery/{0}.aspx",GalleryID);
		}

		public virtual string ArticleUrl(Entry entry)
		{
			if(entry.HasEntryName)
			{
				return GetUrl("articles/{0}.aspx",entry.EntryName);
			}

			return GetUrl("articles/{0}.aspx",entry.EntryID);
			
		}

		public virtual string MonthUrl(DateTime dt)
		{
			return GetUrl("archive/{0}.aspx",dt.ToString("yyyy/MM"));
		}

		public virtual string CommentRssUrl(int EntryID)
		{
			return GetUrl("comments/commentRss/{0}.aspx",EntryID);
		}

		public virtual string CommentUrl(Entry ParentEntry, Entry ChildEntry)
		{
			return string.Format("{0}#{1}",ParentEntry.Link,ChildEntry.EntryID);
			//return PostUrl(dt,EntryID) + "#FeedBack";
		}

		public virtual string CommentUrl(Entry entry)
		{
			return 	GetUrl("archive/" + entry.DateCreated.ToString("yyyy/MM/dd") + "/{0}.aspx#{1}", entry.HasEntryName ? entry.EntryName : entry.ParentID.ToString(),entry.EntryID);
		}

		public virtual string CommentApiUrl(int EntryID)
		{
			return GetUrl("comments/{0}.aspx",EntryID);
		}

		public virtual string TrackBackUrl(int EntryID)
		{
			return GetUrl("services/trackbacks/{0}.aspx",EntryID);
		}

		public virtual string AggBugkUrl(int EntryID)
		{
			return GetUrl("aggbug/{0}.aspx",EntryID);
		}

		protected virtual string GetUrl(string pattern, params object[] items)
		{
			return this.fullyQualifiedUrl + string.Format(pattern,items);
		}


	}
}
