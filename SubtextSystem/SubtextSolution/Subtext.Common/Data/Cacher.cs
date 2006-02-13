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
using System.Globalization;
using System.Web;
using System.Web.Caching;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace Subtext.Common.Data
{
	/// <summary>
	/// Encapsulates obtaining content from the cache.
	/// </summary>
	public class Cacher
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Cacher"/> class.
		/// </summary>
		public Cacher()
		{
		}

		#region LinkCategoryCollection

		private static readonly string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
		/// <summary>
		/// Gets the active categories from the cache. 
		/// If they aren't in the cache, queries the database and puts the 
		/// result in the cache.
		/// </summary>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static LinkCategoryCollection GetActiveCategories(CacheDuration cacheDuration)
		{
			string key = string.Format(ActiveLCCKey, BlogId());

			ContentCache cache = ContentCache.Instantiate();

			LinkCategoryCollection categories = (LinkCategoryCollection)cache[key];
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

		private static readonly string EntryMonthKey = "EntryMonth:Date{0}Blog{1}";
		/// <summary>
		/// Gets the entries for the specified month.
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static EntryCollection GetMonth(DateTime dt, CacheDuration cacheDuration)
		{
			string key = string.Format(EntryMonthKey, dt.ToString("yyyyMM", CultureInfo.InvariantCulture), BlogId());
			ContentCache cache = ContentCache.Instantiate();
			EntryCollection month = (EntryCollection)cache[key];
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

		private static readonly string EntryDayKey = "EntryDay:Date{0}Blog{1}";
		public static EntryDay GetDay(DateTime dt, CacheDuration cacheDuration)
		{
			string key = string.Format(EntryDayKey, dt.ToString("yyyyMMdd", CultureInfo.InvariantCulture),BlogId());
			
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

		private static int BlogId()
		{
			return Framework.Configuration.Config.CurrentBlog.BlogId;
		}

		#endregion

		#region EntriesByCategory

		public static EntryCollection GetEntriesByCategory(int count, CacheDuration cacheDuration)
		{
			string path = WebPathStripper.RemoveRssSlash(HttpContext.Current.Request.Path);
			if(WebPathStripper.IsNumeric(path))
			{
				int CategoryID = WebPathStripper.GetEntryIDFromUrl(path);
				return GetEntriesByCategory(count, cacheDuration, CategoryID);
			}
			else
			{
				string CategoryName = WebPathStripper.GetRequestedFileName(path);
				return GetEntriesByCategory(count, cacheDuration, CategoryName);
			}
		}

		private static readonly string ECKey="EC:Count{0}Category{1}BlogId{2}";
		public static EntryCollection GetEntriesByCategory(int count, CacheDuration cacheDuration, int categoryID)
		{
			string key = string.Format(ECKey,count,categoryID,BlogId());
			ContentCache cache = ContentCache.Instantiate();
			EntryCollection ec = (EntryCollection)cache[key];
			if(ec == null)
			{
				ec = Entries.GetEntriesByCategory(count,categoryID,true);
				
				if(ec != null)
				{
					cache.Insert(key, ec, cacheDuration);
				}
			}
			return ec;
		}

		private static readonly string ECNameKey="EC:Count{0}CategoryName{1}BlogId{2}";
		public static EntryCollection GetEntriesByCategory(int count, CacheDuration cacheDuration, string CategoryName)
		{
			string key = string.Format(ECNameKey,count,CategoryName,BlogId());
			ContentCache cache = ContentCache.Instantiate();
			EntryCollection ec = (EntryCollection)cache[key];
			if(ec == null)
			{
				ec = Entries.GetEntriesByCategory(count,CategoryName,true);
				
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
			string CategoryName = WebPathStripper.GetRequestedFileName(path);
			if(WebPathStripper.IsNumeric(CategoryName))
			{
				int CategoryID =Int32.Parse(CategoryName);
				return SingleCategory(cacheDuration, CategoryID);
			}
			else
			{
				return SingleCategory(cacheDuration, CategoryName);
			}
		}

		private static readonly string LCKey="LC{0}BlogId{1}";

		public static LinkCategory SingleCategory(CacheDuration cacheDuration, int CategoryID)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(LCKey,CategoryID,BlogId());
			LinkCategory lc = (LinkCategory)cache[key];
			if(lc == null)
			{
				lc = Links.GetLinkCategory(CategoryID,true);
				cache.Insert(key, lc, cacheDuration);
			}
			return lc;
		}

		public static LinkCategory SingleCategory(CacheDuration cacheDuration, string CategoryName)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(LCKey,CategoryName,BlogId());
			LinkCategory lc = (LinkCategory)cache[key];
			if(lc == null)
			{
				lc = Links.GetLinkCategory(CategoryName,true);
				cache.Insert(key, lc, cacheDuration);
			}
			return lc;
		}

		#endregion

		#region Entry

		public static Entry GetEntryFromRequest(CacheDuration cacheDuration)
		{
			string id = WebPathStripper.GetRequestedFileName(HttpContext.Current.Request.Path);

			if(WebPathStripper.IsNumeric(id))
			{
				return GetSingleEntry(Int32.Parse(id), cacheDuration);
			}
			else
			{
				return GetSingleEntry(id, cacheDuration);
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
		public static Entry GetSingleEntry(string EntryName, CacheDuration cacheDuration)
		{
			int blogId  =  BlogId();
			
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(EntryKeyName, EntryName, blogId);
			
			Entry entry = (Entry)cache[key];
			if(entry == null)
			{
				entry = Entries.GetEntry(EntryName, EntryGetOption.ActiveOnly);		
				if(entry != null)
				{
					cache.Insert(key, entry, cacheDuration);

					//Most other page items will use the entryID. Add entry to cache for id key as well.
					//Bind them together with a cache dependency.
					string entryIDKey = string.Format(EntryKeyID, entry.EntryID, blogId);
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
		/// <param name="EntryID">The entry ID.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		/// <returns></returns>
		public static Entry GetSingleEntry(int EntryID, CacheDuration cacheDuration)
		{
			ContentCache cache = ContentCache.Instantiate();
			string key = string.Format(EntryKeyID, EntryID, BlogId());
			
			Entry entry = (Entry)cache[key];
			if(entry == null)
			{
				entry = Entries.GetEntry(EntryID, EntryGetOption.ActiveOnly);
				if(entry != null)
				{
					cache.Insert(key, entry, cacheDuration);
				}
			}
			return entry;
		}
		#endregion

		#region Comments/FeedBack

		public static void ClearCommentCache(int EntryID)
		{
			string key = string.Format(ParentCommentEntryKey,EntryID,BlogId());
			ContentCache cache = ContentCache.Instantiate();
			cache.Remove(key);
		}
		
		private static readonly string ParentCommentEntryKey = "ParentEntry:Comments:EntryID{0}:BlogId{1}";
		public static EntryCollection GetComments(Entry ParentEntry, CacheDuration cacheDuration)
		{
			string key = string.Format(ParentCommentEntryKey,ParentEntry.EntryID,BlogId());
			ContentCache cache = ContentCache.Instantiate();
			EntryCollection comments = (EntryCollection)cache[key];
			if(comments == null)
			{
				comments = Entries.GetFeedBack(ParentEntry);
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
