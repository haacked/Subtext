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
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Encapsulates obtaining content from the cache.
	/// </summary>
	public static class Cacher
	{
		#region LinkCategoryCollection

		private static readonly string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
		/// <summary>
		/// Gets the active categories from the cache. 
		/// If they aren't in the cache, queries the database and puts the 
		/// result in the cache.
		/// </summary>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
        public static ICollection<LinkCategory> GetActiveCategories(CacheDuration cacheDuration)
		{
			string key = string.Format(ActiveLCCKey, Config.CurrentBlog.Id);

			ContentCache cache = ContentCache.Instantiate();

            ICollection<LinkCategory> categories = (ICollection<LinkCategory>)cache[key];
			if(categories == null)
			{
				categories = Links.GetActiveCategories();
				if(categories != null)
				{
					cache.Insert(key, categories, cacheDuration);
				}
			}
			return categories;
		}

		#endregion

		#region Month

		private static readonly string EntryMonthKey = "EntryMonth:Date{0:yyyyMM}Blog{1}";
		/// <summary>
		/// Gets the entries for the specified month.
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static ICollection<Entry> GetMonth(DateTime dt, CacheDuration cacheDuration)
		{
			string key = string.Format(CultureInfo.InvariantCulture, EntryMonthKey, dt, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
            ICollection<Entry> month = (ICollection<Entry>)cache[key];
			if(month == null)
			{
				month = Entries.GetPostsByMonth(dt.Month,dt.Year);
				if(month != null)
				{
					cache.Insert(key, month, cacheDuration);
				}
			}
			return month;
		}

		#endregion
		
		#region EntryDay

		private static readonly string EntryDayKey = "EntryDay:Date{0:yyyyMMdd}Blog{1}";
		public static EntryDay GetDay(DateTime dt, CacheDuration cacheDuration)
		{
			string key = string.Format(CultureInfo.InvariantCulture, EntryDayKey, dt, Config.CurrentBlog.Id);
			
			ContentCache cache = ContentCache.Instantiate();
			
			EntryDay day = (EntryDay)cache[key];
			if(day == null)
			{
				day = Entries.GetSingleDay(dt);
				if(day != null)
				{
					cache.Insert(key, day, cacheDuration);
				}
			}
			return day;

		}

		#endregion
		
		#region Helpers


		#endregion

		#region EntriesByCategory
		private static readonly string ECKey="EC:Count{0}Category{1}BlogId{2}";
        public static ICollection<Entry> GetEntriesByCategory(int count, CacheDuration cacheDuration, int categoryID)
		{
			string key = string.Format(ECKey, count, categoryID, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
            ICollection<Entry> ec = (ICollection<Entry>)cache[key];
			if(ec == null)
			{
				ec = Entries.GetEntriesByCategory(count, categoryID, true);
				
				if(ec != null)
				{
					cache.Insert(key, ec, cacheDuration);
				}
			}
			return ec;
		}
		#endregion

        #region EntriesByTag
        private static readonly string ETKey = "ET:Count{0}Tag{1}BlogId{2}";
        public static ICollection<Entry> GetEntriesByTag(int count, CacheDuration cacheDuration, string tag)
        {
            string key = string.Format(ETKey, count, tag, Config.CurrentBlog.Id);
            ContentCache cache = ContentCache.Instantiate();
            ICollection<Entry> et = (ICollection<Entry>)cache[key];
            if (et == null)
            {
                et = Entries.GetEntriesByTag(count, tag);

                if (et != null)
                {
                    cache.Insert(key, et, cacheDuration);
                }
            }
            return et;
        }
        #endregion 

        #region LinkCategory

        /// <summary>
		/// Returns a LinkCategory for a single category based on the request url.
		/// </summary>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static LinkCategory SingleCategory(CacheDuration cacheDuration)
		{
			if (HttpContext.Current == null)
				throw new InvalidOperationException("This method requires the HttpContext. Argue all you want about whether that is good design. That's just the way it is for now.");

			string path = WebPathStripper.RemoveRssSlash(HttpContext.Current.Request.Path);
			string categoryName = Path.GetFileNameWithoutExtension(path);
			if(categoryName.IsNumeric())
			{
				int categoryID = Int32.Parse(categoryName);
				return SingleCategory(cacheDuration, categoryID, true);
			}
			else
			{
				return SingleCategory(cacheDuration, categoryName, true);
			}
		}

		private static readonly string LCKey="LC{0}BlogId{1}";

        public static LinkCategory SingleCategory(CacheDuration cacheDuration, int categoryId, bool isActive)
        {
			LinkCategoryRetrieval retrieval = delegate { return Links.GetLinkCategory(categoryId, isActive); };
			return SingleCategory(retrieval, cacheDuration, categoryId);
        }

		public static LinkCategory SingleCategory(CacheDuration cacheDuration, string categoryName, bool isActive)
        {
        	LinkCategoryRetrieval retrieval = delegate { return Links.GetLinkCategory(categoryName, isActive); }; 
            LinkCategory category = SingleCategory(retrieval, cacheDuration, categoryName);
			if(category != null)
				return category;
			
			if(Config.CurrentBlog.AutoFriendlyUrlEnabled)
			{
				categoryName = categoryName.Replace(FriendlyUrlSettings.Settings.SeparatingCharacter, " ");
				retrieval = delegate { return Links.GetLinkCategory(categoryName, isActive); };
				return SingleCategory(retrieval, cacheDuration, categoryName);
			}
			
			return null; //couldn't find category
        }

		private static LinkCategory SingleCategory<T>(LinkCategoryRetrieval retrievalDelegate, CacheDuration cacheDuration, T categoryKey)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(LCKey, categoryKey, Config.CurrentBlog.Id);
			LinkCategory lc = (LinkCategory)cache[key];
			if(lc == null)
			{
				lc = retrievalDelegate();
				if (lc != null)
					cache.Insert(key, lc, cacheDuration);
			}
			return lc;
		}

		delegate LinkCategory LinkCategoryRetrieval();
		#endregion

		#region Entry
        //TODO: This should only be called in one place total. And it needs to be tested.
        public static Entry GetEntryFromRequest(CacheDuration cacheDuration, bool allowRedirectToEntryName, ISubtextContext context)
        {
            string idText = (string)context.RequestContext.RouteData.Values["id"];
            int id;

            if (int.TryParse(idText, out id))
            {
                Entry entry = GetEntry(id, cacheDuration);
                if (entry == null) {
                    return null;
                }

                //TODO: Violation of SRP here!
                //Second condition avoids infinite redirect loop. Should never happen.
                if (allowRedirectToEntryName && entry.HasEntryName && !entry.EntryName.IsNumeric())
                {
                    HttpContext.Current.Response.StatusCode = 301;
                    HttpContext.Current.Response.Status = "301 Moved Permanently";
                    HttpContext.Current.Response.RedirectLocation = context.UrlHelper.EntryUrl(entry).ToFullyQualifiedUrl(Config.CurrentBlog).ToString();
                    HttpContext.Current.Response.End();
                }
                return entry;
            }
            else
            {
                return GetEntry(id, cacheDuration);
            }
        }

		private const string EntryKeyID = "Entry{0}BlogId{1}";
		private const string EntryKeyName = "EntryName{0}BlogId{1}";

		/// <summary>
		/// Retrieves a single entry from the cache by the entry name.  
		/// If it is not in the cache, gets it from the database and 
		/// inserts it into the cache.
		/// </summary>
		/// <param name="EntryName">Name of the entry.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static Entry GetEntry(string entryName, CacheDuration cacheDuration)
		{
			int blogId = Config.CurrentBlog.Id;
			
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(EntryKeyName, entryName, blogId);
			
			Entry entry = (Entry)cache[key];
			if(entry == null)
			{
                entry = Entries.GetEntry(entryName, PostConfig.IsActive, true);

				if(entry != null)
				{
                    if (entry.DateSyndicated > Config.CurrentBlog.TimeZone.Now)
                        return null;

					cache.Insert(key, entry, cacheDuration);

					//Most other page items will use the entryID. Add entry to cache for id key as well.
					//Bind them together with a cache dependency.
					string entryIDKey = string.Format(EntryKeyID, entry.Id, blogId);
					CacheDependency cd = new CacheDependency(null, new string[]{key});
					cache.Insert(entryIDKey, entry, cd);

				}
			}
			return entry;
		}

		/// <summary>
		/// Retrieves a single entry from the cache by the id.
		/// If it is not in the cache, gets it from the database and
		/// inserts it into the cache.
		/// </summary>
		/// <param name="entryID">The entry ID.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static Entry GetEntry(int entryId, CacheDuration cacheDuration)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(EntryKeyID, entryId, Config.CurrentBlog.Id);
			
			Entry entry = (Entry)cache[key];
			if(entry == null)
			{
				entry = Entries.GetEntry(entryId, PostConfig.IsActive, true);
				if(entry != null)
				{
					cache.Insert(key, entry, cacheDuration);
				}
			}
			return entry;
		}
		#endregion

        #region Tags

        private static readonly string TagsKey = "TagsCount{0}BlogId{1}";
        /// <summary>
        /// Retrieves the current tags from the cache based on the ItemCount and
        /// Blog Id. If it is not in the cache, it gets it from the database and 
        /// inserts it into the cache.
        /// </summary>
        /// <param name="ItemCount">The item count</param>
        /// <param name="cacheDuration">The cache duration.</param>
        /// <returns></returns>
        public static IEnumerable<Tag> GetTopTags(int ItemCount, CacheDuration cacheDuration)
        {
            ContentCache cache = ContentCache.Instantiate();
            string key = string.Format(TagsKey, ItemCount, Config.CurrentBlog.Id);

            IEnumerable<Tag> tags = (IEnumerable<Tag>)cache[key];
            if (tags == null)
            {
                tags = Tags.GetTopTags(ItemCount);
                if (tags != null)
                {
                    cache.Insert(key, tags, cacheDuration);
                }
            }
            return tags;
        }

        #endregion

        #region Comments/FeedBack

        /// <summary>
		/// Clears the comment cache.
		/// </summary>
		/// <param name="entryID">The entry ID.</param>
		public static void ClearCommentCache(int entryID)
		{
			string key = string.Format(ParentCommentEntryKey, entryID, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
			cache.Remove(key);
		}
		
		private static readonly string ParentCommentEntryKey = "ParentEntry:Comments:EntryID{0}:BlogId{1}";

		/// <summary>
		/// Returns all the feedback for the specified entry. Checks the cache first.
		/// </summary>
		/// <param name="parentEntry"></param>
		/// <param name="cacheDuration"></param>
		/// <returns></returns>
        /// <param name="fromCache"></param>
        public static ICollection<FeedbackItem> GetFeedback(Entry parentEntry, CacheDuration cacheDuration, bool fromCache)
		{
			ICollection<FeedbackItem> comments = null;
			ContentCache cache = null;
			string key = null;
			if (fromCache)
			{
				key = string.Format(ParentCommentEntryKey, parentEntry.Id, Config.CurrentBlog.Id);
				cache = ContentCache.Instantiate();
				comments = (ICollection<FeedbackItem>)cache[key];
			}
			if(comments == null)
			{
				comments = Entries.GetFeedBack(parentEntry);
				if(comments != null && fromCache)
				{
					cache.Insert(key, comments, cacheDuration);
				}
			}
			return comments;
		}		

		#endregion
		
	}
}
