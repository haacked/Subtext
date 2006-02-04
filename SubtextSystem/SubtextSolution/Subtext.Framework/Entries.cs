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
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework
{
	/// <summary>
	/// Static class used to get entries (blog posts, comments, etc...) 
	/// from the data store.
	/// </summary>
	public sealed class Entries
	{
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
			return ObjectProvider.Instance().GetPagedEntries(postType,categoryID,pageIndex,pageSize,sortDescending);
		}


		public static PagedEntryCollection GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			return ObjectProvider.Instance().GetPagedFeedback(pageIndex,pageSize,sortDescending);
		}


		#endregion

		#region EntryDays

		public static EntryDay GetSingleDay(DateTime dt)
		{
			return ObjectProvider.Instance().GetSingleDay(dt);

		}

		/// <summary>
		/// Gets the entries to display on the home page.
		/// </summary>
		/// <param name="ItemCount">Item count.</param>
		/// <returns></returns>
		public static EntryDayCollection GetHomePageEntries(int ItemCount)
		{
			return GetBlogPosts(ItemCount, PostConfig.DisplayOnHomePage | PostConfig.IsActive);
		}

		/// <summary>
		/// Gets the specified number of entries using the <see cref="PostConfig"/> flags 
		/// specified.
		/// </summary>
		/// <remarks>
		/// This is used to get the posts displayed on the home page.
		/// </remarks>
		/// <param name="ItemCount">Item count.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
		public static EntryDayCollection GetBlogPosts(int ItemCount, PostConfig pc)
		{
			return ObjectProvider.Instance().GetBlogPosts(ItemCount, pc);
		}

		/// <summary>
		/// Returns a collection of Entries grouped by Day
		/// </summary>
		/// <param name="ItemCount">Number of entries total</param>
		/// <param name="ActiveOnly">Return only Active Posts</param>
		/// <returns></returns>
		public static EntryDayCollection GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetRecentDayPosts(ItemCount,ActiveOnly);

		}

		public static EntryDayCollection GetPostsByMonth(int month, int year)
		{
			return ObjectProvider.Instance().GetPostsByMonth(month,year);
		}

		public static EntryDayCollection GetPostsByCategoryID(int ItemCount, int catID)
		{
			return ObjectProvider.Instance().GetPostsByCategoryID(ItemCount,catID);
		}

		#endregion

		#region EntryCollections

		/// <summary>
		/// Gets the main syndicated entries.
		/// </summary>
		/// <param name="ItemCount">Item count.</param>
		/// <returns></returns>
		public static EntryCollection GetMainSyndicationEntries(int ItemCount)
		{
			return ObjectProvider.Instance().GetConditionalEntries(ItemCount, PostType.BlogPost, PostConfig.IncludeInMainSyndication | PostConfig.IsActive);
		}

		/// <summary>
		/// Gets the comments (including trackback, etc...) for the specified entry.
		/// </summary>
		/// <param name="parentEntry">Parent entry.</param>
		/// <returns></returns>
		public static EntryCollection GetFeedBack(Entry parentEntry)
		{
			return ObjectProvider.Instance().GetFeedBack(parentEntry);
		}

		public static EntryCollection GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetRecentPostsWithCategories(ItemCount,ActiveOnly);
		}

		/// <summary>
		/// Gets recent posts used to support the MetaBlogAPI. 
		/// Could be used for a Recent Posts control as well.
		/// </summary>
		/// <param name="ItemCount">Item count.</param>
		/// <param name="postType">Post type.</param>
		/// <param name="ActiveOnly">Active only.</param>
		/// <returns></returns>
		public static EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetRecentPosts(ItemCount,postType,ActiveOnly);
		}

		public static EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated)
		{
			return ObjectProvider.Instance().GetRecentPosts(ItemCount,postType,ActiveOnly,DateUpdated);
		}

		public static EntryCollection GetPostCollectionByMonth(int month, int year)
		{
			return ObjectProvider.Instance().GetPostCollectionByMonth(month,year);
		}

		public static EntryCollection GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			return  ObjectProvider.Instance().GetPostsByDayRange(start,stop,postType,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,int catID,bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetEntriesByCategory(ItemCount,catID,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,int catID, DateTime DateUpdated,bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetEntriesByCategory(ItemCount,catID,DateUpdated,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,string categoryName,bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetEntriesByCategory(ItemCount,categoryName,ActiveOnly);
		}

		public static EntryCollection GetEntriesByCategory(int ItemCount,string categoryName, DateTime DateUpdated,bool ActiveOnly)
		{
			return ObjectProvider.Instance().GetEntriesByCategory(ItemCount,categoryName,DateUpdated,ActiveOnly);
		}

		#endregion

		#region Single Entry

		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public static Entry GetCommentByChecksumHash(string checksumHash)
		{
			return ObjectProvider.Instance().GetCommentByChecksumHash(checksumHash);
		}
		
		/// <summary>
		/// Gets the entry from the data store by id.
		/// </summary>
		/// <param name="entryId">The ID of the entry.</param>
		/// <param name="entryOption">The entry option used to constrain the search.</param>
		/// <returns></returns>
		public static Entry GetEntry(int entryId, EntryGetOption entryOption)
		{
			return ObjectProvider.Instance().GetEntry(entryId, (entryOption == EntryGetOption.ActiveOnly));
		}

		/// <summary>
		/// Gets the entry from the data store by entry name.
		/// </summary>
		/// <param name="EntryName">Name of the entry.</param>
		/// <param name="entryOption">The entry option used to constrain the search.</param>
		/// <returns></returns>
		public static Entry GetEntry(string EntryName, EntryGetOption entryOption)
		{
			return ObjectProvider.Instance().GetEntry(EntryName, (entryOption == EntryGetOption.ActiveOnly));
		}

		/// <summary>
		/// Gets the category entry by id.
		/// </summary>
		/// <param name="entryId">The entryId.</param>
		/// <param name="entryOption">The entry option used to constrain the search.</param>
		/// <returns></returns>
		public static CategoryEntry GetCategoryEntry(int entryId, EntryGetOption entryOption)
		{
			return ObjectProvider.Instance().GetCategoryEntry(entryId, (entryOption == EntryGetOption.ActiveOnly));
		}

		/// <summary>
		/// Gets the category entry by its entry name.
		/// </summary>
		/// <param name="entryName">Name of the entry.</param>
		/// <param name="entryOption">The entry option used to constrain the search.</param>
		/// <returns></returns>
		public static CategoryEntry GetCategoryEntry(string entryName, EntryGetOption entryOption)
		{
			return ObjectProvider.Instance().GetCategoryEntry(entryName, (entryOption == EntryGetOption.ActiveOnly));
		}

		#endregion

		#region Delete
	
		public static bool Delete(int PostID)
		{
			return ObjectProvider.Instance().Delete(PostID);
		}

		#endregion

		#region Create

		/// <summary>
		/// Creates the specified entry and returns its ID.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public static int Create(Entry entry)
		{
			return Create(entry, null);
		}

		/// <summary>
		/// Creates the specified entry and returns its ID.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="CategoryIDs">The ids of the categories this entry belongs to.</param>
		/// <returns></returns>
		public static int Create(Entry entry, int[] CategoryIDs)
		{
			// check if we're admin, if not filter the comment. We do this to help when Importing 
			// a blog using the BlogML import process. A better solution may be developing a way to 
			// determine if we're currently in the BlogML import process and use that here.
			if(!Security.IsAdmin
				&& entry.PostType == PostType.Comment 
				|| entry.PostType == PostType.PingTrack)
				CommentFilter.FilterComment(entry);

			if(Config.CurrentBlog.AutoFriendlyUrlEnabled
				&& (entry.PostType == PostType.BlogPost || entry.PostType == PostType.Story)
				&& entry.Title != null
				&& entry.Title.Length > 0)
			{
				entry.EntryName = AutoGenerateFriendlyUrl(entry.Title);
				entry.TitleUrl = entry.Link;
				
			}

			if(entry.IsActive && entry.IncludeInMainSyndication)
				entry.DateSyndicated = DateTime.Now;
			else
				entry.DateSyndicated = NullValue.NullDateTime;
			
			return ObjectProvider.Instance().Create(entry, CategoryIDs);
		}

		/// <summary>
		/// Returns a friendly title that's safe for url.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title)
		{
			Regex regex = new Regex(@"[""'`~@#$%^&*(){\[}\]?+/=\\|<> ]+", RegexOptions.Compiled);
			string entryName = regex.Replace(title, "");
			if(entryName.Length == 0)
				return null;

			string newEntryName = entryName;
			int tryCount = 0;
			while(ObjectProvider.Instance().GetEntry(newEntryName, false) != null)
			{
				if(tryCount == 1)
					newEntryName = entryName + "Again";
				if(tryCount == 2)
					newEntryName = entryName + "YetAgain";
				if(tryCount == 3)
					newEntryName = entryName + "AndAgain";
				if(tryCount == 4)
					newEntryName = entryName + "OnceMore";
				if(tryCount == 5)
					newEntryName = entryName + "ToBeatADeadHorse";

				if(tryCount++ > 5)
					break; //Allow an exception to get thrown later.
			}

			return newEntryName;
		}

		#endregion

		#region Update

		/// <summary>
		/// Updates the specified entry in the data provider.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public static bool Update(Entry entry)
		{
			if(entry.DateSyndicated == NullValue.NullDateTime && entry.IsActive && entry.IncludeInMainSyndication)
				entry.DateSyndicated = DateTime.Now;
			else 
				//Note, this could cause updated items to get republished to the feed for RFC3229. 
				//This should be fine since the GUID won't change.
				entry.DateSyndicated = NullValue.NullDateTime;

			return Update(entry, null);
		}

		/// <summary>
		/// Updates the specified entry in the data provider 
		/// and attaches the specified categories.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="CategoryIDs">Category Ids this entry belongs to.</param>
		/// <returns></returns>
		public static bool Update(Entry entry, int[] CategoryIDs)
		{
			return ObjectProvider.Instance().Update(entry, CategoryIDs);
		}

		#endregion

		#region Entry Category List

		public static bool SetEntryCategoryList(int EntryID, int[] Categories)
		{
			return ObjectProvider.Instance().SetEntryCategoryList(EntryID,Categories);
		}

		#endregion

		/// <summary>
		/// Inserts a comment for the specified entry.
		/// </summary>
		/// <remarks>
		/// If it's not the admin posting the comment, an email is sent 
		/// to the Admin with the contents of the comment.
		/// </remarks>
		/// <param name="entry">Entry.</param>
		public static void InsertComment(Entry entry)
		{
			// what follows relies on context, so guard
			if (null == HttpContext.Current) return;

			entry.Author = HtmlHelper.SafeFormat(entry.Author);
			entry.TitleUrl =  HtmlHelper.SafeFormat(entry.TitleUrl);
			entry.Body = HtmlHelper.SafeFormatWithUrl(entry.Body);
			entry.Title = HtmlHelper.SafeFormat(entry.Title);
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
					string blogTitle = Config.CurrentBlog.Title;

					// create and format an email to the site admin with comment details
					EmailProvider im = EmailProvider.Instance();

					string To = Config.CurrentBlog.Email;
					string From = im.AdminEmail;
					string Subject = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Comment: {0} (via {1})", entry.Title, blogTitle);
					string Body = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Comments from {0}:\r\n\r\nSender: {1}\r\nUrl: {2}\r\nIP Address: {3}\r\n=====================================\r\n\r\n{4}\r\n\r\n{5}\r\n\r\nSource: {6}#{7}", 
						blogTitle,
						entry.Author,
						entry.TitleUrl,
						entry.SourceName,
						entry.Title,					
						// we're sending plain text email by default, but body includes <br />s for crlf
						entry.Body.Replace("<br />", Environment.NewLine), 
						entry.SourceUrl,
						entryID);			
				
					im.Send(To, From, Subject, Body);
				}
				catch
				{
					//TODO: Log this.
				}
			}
		}
	}

	/// <summary>
	/// Enum used to determine which type of entries to retrieve.
	/// </summary>
	public enum EntryGetOption
	{
		All = 0,
		ActiveOnly = 1,
		//TODO: At some point let's add InactiveOnly = 2,
	}
}

