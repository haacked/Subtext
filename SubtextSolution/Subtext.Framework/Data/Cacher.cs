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
using Subtext.Extensibility;
using Subtext.Framework;
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
		public static IList<Entry> GetMonth(DateTime dt, CacheDuration cacheDuration)
		{
			string key = string.Format(CultureInfo.InvariantCulture, EntryMonthKey, dt, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
            IList<Entry> month = (IList<Entry>)cache[key];
			if(month == null)
			{
				month = Entries.GetPostCollectionByMonth(dt.Month,dt.Year);
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
        public static IList<Entry> GetEntriesByCategory(int count, CacheDuration cacheDuration, int categoryID)
		{
			string key = string.Format(ECKey, count, categoryID, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
            IList<Entry> ec = (IList<Entry>)cache[key];
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

		#region LinkCategory

		public static LinkCategory SingleCategory(CacheDuration cacheDuration)
		{
			string path = WebPathStripper.RemoveRssSlash(HttpContext.Current.Request.Path);
			string categoryName = Path.GetFileNameWithoutExtension(path);
			if(StringHelper.IsNumeric(categoryName))
			{
				int categoryID = Int32.Parse(categoryName);
				return SingleCategory(cacheDuration, categoryID);
			}
			else
			{
				return SingleCategory(cacheDuration, categoryName);
			}
		}

		private static readonly string LCKey="LC{0}BlogId{1}";

		public static LinkCategory SingleCategory(CacheDuration cacheDuration, int categoryID)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(LCKey, categoryID, Config.CurrentBlog.Id);
			LinkCategory lc = (LinkCategory)cache[key];
			if(lc == null)
			{
				lc = Links.GetLinkCategory(categoryID,true);
				cache.Insert(key, lc, cacheDuration);
			}
			return lc;
		}

		public static LinkCategory SingleCategory(CacheDuration cacheDuration, string categoryName)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(LCKey, categoryName, Config.CurrentBlog.Id);
			LinkCategory lc = (LinkCategory)cache[key];
			if(lc == null)
			{
				lc = Links.GetLinkCategory(categoryName,true);
				cache.Insert(key, lc, cacheDuration);
			}
			return lc;
		}

		#endregion

		#region Entry

		public static Entry GetEntryFromRequest(CacheDuration cacheDuration)
		{
			string id = Path.GetFileNameWithoutExtension(HttpContext.Current.Request.Path);

			if(StringHelper.IsNumeric(id))
			{
				return GetEntry(Int32.Parse(id), cacheDuration);
			}
			else
			{
				return GetEntry(id, cacheDuration);
			}
		}

		private static readonly string EntryKeyID="Entry{0}BlogId{1}";
		private static readonly string EntryKeyName="EntryName{0}BlogId{1}";

		/// <summary>
		/// Retrieves a single entry from the cache by the entry name.  
		/// If it is not in the cache, gets it from the database and 
		/// inserts it into the cache.
		/// </summary>
		/// <param name="EntryName">Name of the entry.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static Entry GetEntry(string EntryName, CacheDuration cacheDuration)
		{
			int blogId = Config.CurrentBlog.Id;
			
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(EntryKeyName, EntryName, blogId);
			
			Entry entry = (Entry)cache[key];
			if(entry == null)
			{
                entry = Entries.GetEntry(EntryName, PostConfig.IsActive, true);
				if(entry != null)
				{
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
		public static Entry GetEntry(int entryID, CacheDuration cacheDuration)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(EntryKeyID, entryID, Config.CurrentBlog.Id);
			
			Entry entry = (Entry)cache[key];
			if(entry == null)
			{
				entry = Entries.GetEntry(entryID, PostConfig.IsActive, true);
				if(entry != null)
				{
					cache.Insert(key, entry, cacheDuration);
				}
			}
			return entry;
		}
		#endregion

		#region Comments/FeedBack

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
        public static IList<FeedbackItem> GetFeedback(Entry parentEntry, CacheDuration cacheDuration)
		{
			string key = string.Format(ParentCommentEntryKey, parentEntry.Id, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
            IList<FeedbackItem> comments = (IList<FeedbackItem>)cache[key];
			if(comments == null)
			{
				comments = Entries.GetFeedBack(parentEntry);
				if(comments != null)
				{
					cache.Insert(key, comments, cacheDuration);
				}
			}
			return comments;
		}		

		#endregion
		
	}
}
