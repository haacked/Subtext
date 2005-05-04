#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Email;
using Subtext.Framework.Providers;
using Subtext.Framework.Util;

namespace Subtext.Framework
{
	/// <summary>
	/// Summary description for Entries.
	/// </summary>
	public class Entries
	{
		private Entries()
		{
		}

		#region Paged Posts

		/// <summary>
		/// Returns a collection of Posts for a give page and index size.
		/// </summary>
		/// <param name="postType"></param>
		/// <param name="categoryID">-1 means not to filter by a categoryID</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="sortDescending"></param>
		/// <returns></returns>
		public static PagedEntryCollection GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			return DTOProvider.Instance().GetPagedEntries(postType,categoryID,pageIndex,pageSize,sortDescending);
		}


		public static PagedEntryCollection GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			return DTOProvider.Instance().GetPagedFeedback(pageIndex,pageSize,sortDescending);
		}


		#endregion

		#region EntryDays

		public static EntryDay GetSingleDay(DateTime dt)
		{
			return DTOProvider.Instance().GetSingleDay(dt);

		}

		public static EntryDayCollection GetHomePageEntries(int ItemCount)
		{
			return GetConditionalEntries(ItemCount,PostConfig.DisplayOnHomePage|PostConfig.IsActive);
		}

		public static EntryDayCollection GetConditionalEntries(int ItemCount, PostConfig pc)
		{
			return DTOProvider.Instance().GetConditionalEntries(ItemCount,pc);
		}

		/// <summary>
		/// Returns a collection of Entries grouped by Day
		/// </summary>
		/// <param name="ItemCount">Number of entries total</param>
		/// <param name="ActiveOnly">Return only Active Posts</param>
		/// <returns></returns>
		public static EntryDayCollection GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetRecentDayPosts(ItemCount,ActiveOnly);

		}

		public static EntryDayCollection GetPostsByMonth(int month, int year)
		{
			return DTOProvider.Instance().GetPostsByMonth(month,year);
		}

		public static EntryDayCollection GetPostsByCategoryID(int ItemCount, int catID)
		{
			return DTOProvider.Instance().GetPostsByCategoryID(ItemCount,catID);
		}

		#endregion

		#region EntryCollections



		public static EntryCollection GetMainSyndicationEntries(int ItemCount)
		{
			return GetConditionalEntries(ItemCount,PostType.BlogPost,PostConfig.IncludeInMainSyndication|PostConfig.IsActive);
		}

		public static EntryCollection GetMainSyndicationEntries(DateTime dt)
		{
			return GetConditionalEntries(0,PostType.BlogPost,PostConfig.IncludeInMainSyndication|PostConfig.IsActive,dt);
		}


		public static EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc)
		{
			return DTOProvider.Instance().GetConditionalEntries(ItemCount,pt,pc);
		}

		public static EntryCollection GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc, DateTime DateUpdated)
		{
			return DTOProvider.Instance().GetConditionalEntries(ItemCount,pt,pc, DateUpdated);
		}

		/// <summary>
		/// Returns a collection of Entries containing the feedback for a given post (via ParentID)
		/// </summary>
		/// <param name="ParentId">Parent (EntryID) of the collection</param>
		/// <returns></returns>
		public static EntryCollection GetFeedBack(int ParentId)
		{
			return DTOProvider.Instance().GetFeedBack(ParentId);
		}

		public static EntryCollection GetFeedBack(Entry ParentEntry)
		{
			return DTOProvider.Instance().GetFeedBack(ParentEntry);
		}

		public static EntryCollection GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetRecentPostsWithCategories(ItemCount,ActiveOnly);
		}

		public static EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetRecentPosts(ItemCount,postType,ActiveOnly);
		}

		public static EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated)
		{
			return DTOProvider.Instance().GetRecentPosts(ItemCount,postType,ActiveOnly,DateUpdated);
		}

		public static EntryCollection GetPostCollectionByMonth(int month, int year)
		{
			return DTOProvider.Instance().GetPostCollectionByMonth(month,year);
		}

		public static EntryCollection GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			return  DTOProvider.Instance().GetPostsByDayRange(start,stop,postType,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,int catID,bool ActiveOnly)
		{
			return DTOProvider.Instance().GetEntriesByCategory(ItemCount,catID,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,int catID, DateTime DateUpdated,bool ActiveOnly)
		{
			return DTOProvider.Instance().GetEntriesByCategory(ItemCount,catID,DateUpdated,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,string categoryName,bool ActiveOnly)
		{
			return DTOProvider.Instance().GetEntriesByCategory(ItemCount,categoryName,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,string categoryName, DateTime DateUpdated,bool ActiveOnly)
		{
			return DTOProvider.Instance().GetEntriesByCategory(ItemCount,categoryName,DateUpdated,ActiveOnly);
		}

		#endregion

		#region Single Entry

		public static Entry GetEntry(int postID, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetEntry(postID,ActiveOnly);
		}

		public static Entry GetEntry(string EntryName, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetEntry(EntryName,ActiveOnly);
		}


		public static CategoryEntry GetCategoryEntry(int postid, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetCategoryEntry(postid,ActiveOnly);
		}

		public static CategoryEntry GetCategoryEntry(string EntryName, bool ActiveOnly)
		{
			return DTOProvider.Instance().GetCategoryEntry(EntryName,ActiveOnly);
		}

		#endregion

		#region Delete
	
		public static bool Delete(int PostID)
		{
			return DTOProvider.Instance().Delete(PostID);
		}

		#endregion

		#region Create

		public static int Create(Entry entry)
		{
			return Create(entry,null);
		}

		public static int Create(Entry entry, int[] CategoryIDs)
		{
			return DTOProvider.Instance().Create(entry,CategoryIDs);
		}

		#endregion

		#region Update

		public static bool Update(Entry entry)
		{
			return Update(entry,null);
		}

		public static bool Update(Entry entry, int[] CategoryIDs)
		{
			return DTOProvider.Instance().Update(entry,CategoryIDs);
		}

		#endregion

		#region Entry Category List

		public static bool SetEntryCategoryList(int EntryID, int[] Categories)
		{
			return DTOProvider.Instance().SetEntryCategoryList(EntryID,Categories);
		}

		#endregion

		public static void InsertComment(Entry entry)
		{
			// what follows relies on context, so guard
			if (null == HttpContext.Current) return;

			entry.Author = Globals.SafeFormat(entry.Author);
			entry.TitleUrl =  Globals.SafeFormat(entry.TitleUrl);
			entry.Body = Globals.SafeFormatWithUrl(entry.Body);
			entry.Title = Globals.SafeFormat(entry.Title);
			//entry.SourceUrl = Globals.PostsUrl(entry.ParentID);
			entry.IsXHMTL = false;
			entry.IsActive = true;
			entry.DateCreated = entry.DateUpdated = BlogTime.CurrentBloggerTime;
			
			

			if (null == entry.SourceName || String.Empty == entry.SourceName)
				entry.SourceName = "N/A";


			// insert comment into backend, save the returned entryid for permalink anchor below
			int entryID = Entries.Create(entry);

			// if it's not the administrator commenting
			if(!Security.IsAdmin)
			{
				try
				{
					
					string blogTitle = Config.CurrentBlog().Title;

					// create and format an email to the site admin with comment details
					IMailProvider im = EmailProvider.Instance();

					string To = Config.CurrentBlog().Email;
					string From = Config.Settings.BlogProviders.EmailProvider.AdminEmail;
					string Subject = String.Format("Comment: {0} (via {1})", entry.Title, blogTitle);
					string Body = String.Format("Comments from {0}:\r\n\r\nSender: {1}\r\nUrl: {2}\r\nIP Address: {3}\r\n=====================================\r\n\r\n{4}\r\n\r\n{5}\r\n\r\nSource: {6}#{7}", 
						blogTitle,
						entry.Author,
						entry.TitleUrl,
						entry.SourceName,
						entry.Title,					
						// we're sending plain text email by default, but body includes <br>s for crlf
						entry.Body.Replace("<br>", "\n"), 
						entry.SourceUrl,
						entryID);			
					
					im.Send(To,From,Subject,Body);
				}
				catch{}
			}
		}
		

	}
}

