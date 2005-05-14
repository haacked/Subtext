using System;
using System.Web;
using System.Web.Caching;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace Subtext.Common.Data
{
	public enum CacheTime
	{
		Short = 10,
		Medium = 20,
		Long = 30
	};

	/// <summary>
	/// Summary description for Cacher.
	/// </summary>
	public class Cacher
	{
		public Cacher()
		{
			
		}

		#region LinkCategoryCollection

		private static readonly string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
		public static LinkCategoryCollection GetActiveCategories(CacheTime ct, HttpContext context)
		{
			string key = string.Format(ActiveLCCKey,BlogID(context));
			LinkCategoryCollection categories = (LinkCategoryCollection)context.Cache[key];
			if(categories == null)
			{
				categories = Links.GetActiveCategories();
				if(categories != null)
				{
					Cacher.CacherCache(key,context,categories,ct);
				}
			}
			return categories;
		}

		#endregion

		#region Month

		private static readonly string EntryMonthKey = "EntryMonth:Date{0}Blog{1}";
		public static EntryCollection GetMonth(DateTime dt, CacheTime ct, HttpContext context)
		{
			string key = string.Format(EntryMonthKey,dt.ToString("yyyyMM"),BlogID(context));
			EntryCollection month = (EntryCollection)context.Cache[key];
			if(month == null)
			{
				month = Entries.GetPostCollectionByMonth(dt.Month,dt.Year);
				if(month != null)
				{
					Cacher.CacherCache(key,context,month,ct);
				}
			}
			return month;

		}

		#endregion
		
		#region EntryDay

		private static readonly string EntryDayKey = "EntryDay:Date{0}Blog{1}";
		public static EntryDay GetDay(DateTime dt, CacheTime ct, HttpContext context)
		{
			string key = string.Format(EntryDayKey,dt.ToString("yyyyMMdd"),BlogID(context));
			EntryDay day = (EntryDay)context.Cache[key];
			if(day == null)
			{
				day = Entries.GetSingleDay(dt);
				if(day != null)
				{
					Cacher.CacherCache(key,context,day,ct);
				}
			}
			return day;

		}

		#endregion
		
		#region Helpers

		private static int BlogID(HttpContext context)
		{
			return Framework.Configuration.Config.CurrentBlog.BlogID;
		}

		public static void CacherCache(string cacheKey, HttpContext context, object obj, CacheTime ct)
		{
			if(obj != null)
			{
				context.Cache.Insert(cacheKey,obj,null,DateTime.Now.AddSeconds((int)ct),TimeSpan.Zero,CacheItemPriority.Normal,null);
			}
		}

		#endregion

		#region EntriesByCategory

		public static EntryCollection GetEntriesByCategory(int count, CacheTime ct, HttpContext context)
		{
			string path = WebPathStripper.RemoveRssSlash(context.Request.Path);
			if(WebPathStripper.IsNumeric(path))
			{
				int CategoryID = WebPathStripper.GetEntryIDFromUrl(path);
				return GetEntriesByCategory(count,ct,context,CategoryID);
			}
			else
			{
				string CategoryName = WebPathStripper.GetReqeustedFileName(path);
				return GetEntriesByCategory(count,ct,context,CategoryName);
			}
		}

		private static readonly string ECKey="EC:Count{0}Category{1}BlogID{2}";
		public static EntryCollection GetEntriesByCategory(int count, CacheTime ct, HttpContext context, int categoryID)
		{
			string key = string.Format(ECKey,count,categoryID,BlogID(context));
			EntryCollection ec = (EntryCollection)context.Cache[key];
			if(ec == null)
			{
				ec = Entries.GetEntriesByCategory(count,categoryID,true);
				
				if(ec != null)
				{
					CacherCache(key,context,ec,ct);
				}
			}
			return ec;
		}

		private static readonly string ECNameKey="EC:Count{0}CategoryName{1}BlogID{2}";
		public static EntryCollection GetEntriesByCategory(int count, CacheTime ct, HttpContext context, string CategoryName)
		{
			string key = string.Format(ECNameKey,count,CategoryName,BlogID(context));
			EntryCollection ec = (EntryCollection)context.Cache[key];
			if(ec == null)
			{
				ec = Entries.GetEntriesByCategory(count,CategoryName,true);
				
				if(ec != null)
				{
					CacherCache(key,context,ec,ct);
				}
			}
			return ec;
		}

		#endregion

		#region LinkCategory

		public static LinkCategory SingleCategory(CacheTime ct, HttpContext context)
		{
			string path = WebPathStripper.RemoveRssSlash(context.Request.Path);
			string CategoryName = WebPathStripper.GetReqeustedFileName(path);
			if(WebPathStripper.IsNumeric(CategoryName))
			{
				int CategoryID =Int32.Parse(CategoryName);
				return SingleCategory(ct,context,CategoryID);
			}
			else
			{
				
				return SingleCategory(ct,context,CategoryName);
			}
		}

		private static readonly string LCKey="LC{0}BlogID{1}";

		public static LinkCategory SingleCategory(CacheTime ct, HttpContext context,int CategoryID)
		{
			string key = string.Format(LCKey,CategoryID,BlogID(context));
			LinkCategory lc = (LinkCategory)context.Cache[key];
			if(lc == null)
			{
				lc = Links.GetLinkCategory(CategoryID,true);
				CacherCache(key,context,lc,ct);
			}
			return lc;
		}

		public static LinkCategory SingleCategory(CacheTime ct, HttpContext context, string CategoryName)
		{
			string key = string.Format(LCKey,CategoryName,BlogID(context));
			LinkCategory lc = (LinkCategory)context.Cache[key];
			if(lc == null)
			{
				lc = Links.GetLinkCategory(CategoryName,true);
				CacherCache(key,context,lc,ct);
			}
			return lc;
		}

		#endregion

		#region Entry

		public static Entry GetEntryFromRequest(HttpContext context, CacheTime ct)
		{
			string id = WebPathStripper.GetReqeustedFileName(context.Request.Path);

			if(WebPathStripper.IsNumeric(id))
			{
				return SingleEntry(Int32.Parse(id),CacheTime.Short,context);
			}
			else
			{
				return SingleEntry(id,CacheTime.Short,context);
			}
		}

		private static readonly string EntryKeyID="Entry{0}BlogID{1}";
		private static readonly string EntryKeyName="EntryName{0}BlogID{1}";

		public static Entry SingleEntry(string EntryName, CacheTime ct, HttpContext context)
		{
			int BID  =  BlogID(context);
			string key = string.Format(EntryKeyName,EntryName,BID);
			
			Entry entry = (Entry)context.Cache[key];
			if(entry == null)
			{
				entry = Entries.GetEntry(EntryName,true);		
				if(entry != null)
				{
					CacherCache(key,context,entry,ct);

					//Most other page items will use the entryID. Add entry to cache for id key as well.
					//Bind them together with a cache dependency.
					string entryIDKey = string.Format(EntryKeyID,entry.EntryID,BID);
					CacheDependency cd = new CacheDependency(null,new string[]{key});
					context.Cache.Insert(entryIDKey,entry,cd);

				}
			}
			return entry;
		}

		public static Entry SingleEntry(int EntryID, CacheTime ct, HttpContext context)
		{
			string key = string.Format(EntryKeyID,EntryID,BlogID(context));
			
			Entry entry = (Entry)context.Cache[key];
			if(entry == null)
			{
				entry = Entries.GetEntry(EntryID,true);		
				if(entry != null)
				{
					CacherCache(key,context,entry,ct);
				}
			}
			return entry;
		}
		#endregion

		#region Comments/FeedBack

		public static void ClearCommentCache(int EntryID, HttpContext context)
		{
			string key = string.Format(ParentCommentEntryKey,EntryID,BlogID(context));
			context.Cache.Remove(key);
		}
			
		private static readonly string ParentCommentEntryKey = "ParentEntry:Comments:EntryID{0}:BlogID{1}";
		public static EntryCollection GetComments(Entry ParentEntry, CacheTime ct, HttpContext context)
		{
			string key = string.Format(ParentCommentEntryKey,ParentEntry.EntryID,BlogID(context));
			
			EntryCollection comments = (EntryCollection)context.Cache[key];
			if(comments == null)
			{
				comments = Entries.GetFeedBack(ParentEntry);
				if(comments != null)
				{
					CacherCache(key,context,comments,ct);
				}
			}
			return comments;
		}

		private static readonly string ParentCommentEntryID = "ParentEntry:Comments:EntryID{0}:BlogID{1}";
		public static EntryCollection GetComments(int EntryID, CacheTime ct, HttpContext context)
		{
			string key = string.Format(ParentCommentEntryID,EntryID,BlogID(context));
			
			EntryCollection comments = (EntryCollection)context.Cache[key];
			if(comments == null)
			{
				comments = Entries.GetFeedBack(EntryID);
				if(comments != null)
				{
					CacherCache(key,context,comments,ct);
				}
			}
			return comments;
		}

		#endregion
		
	}
}
