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
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using log4net;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;
using Subtext.Framework.Util;

namespace Subtext.Framework
{
	/// <summary>
	/// Static class used to get entries (blog posts, comments, etc...) 
	/// from the data store.
	/// </summary>
	public static class Entries
	{
		private readonly static ILog log = new Logging.Log();		
	
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
        public static IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			return ObjectProvider.Instance().GetPagedEntries(postType,categoryID,pageIndex,pageSize,sortDescending);
		}


        public static IPagedCollection<Entry> GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
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
		/// <param name="itemCount">Item count.</param>
		/// <returns></returns>
        public static ICollection<EntryDay> GetHomePageEntries(int itemCount)
		{
			return GetBlogPosts(itemCount, PostConfig.DisplayOnHomePage | PostConfig.IsActive);
		}

		/// <summary>
		/// Gets the specified number of entries using the <see cref="PostConfig"/> flags 
		/// specified.
		/// </summary>
		/// <remarks>
		/// This is used to get the posts displayed on the home page.
		/// </remarks>
		/// <param name="itemCount">Item count.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
        public static ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig pc)
		{
			return ObjectProvider.Instance().GetBlogPosts(itemCount, pc);
		}

		public static ICollection<EntryDay> GetPostsByMonth(int month, int year)
		{
			return ObjectProvider.Instance().GetPostsByMonth(month,year);
		}

        public static ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int catID)
		{
			return ObjectProvider.Instance().GetPostsByCategoryID(itemCount,catID);
		}

		#endregion

		#region EntryCollections

		/// <summary>
		/// Gets the main syndicated entries.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <returns></returns>
		public static IList<Entry> GetMainSyndicationEntries(int itemCount)
		{
			return ObjectProvider.Instance().GetConditionalEntries(itemCount, PostType.BlogPost, PostConfig.IncludeInMainSyndication | PostConfig.IsActive);
		}

		/// <summary>
		/// Gets the comments (including trackback, etc...) for the specified entry.
		/// </summary>
		/// <param name="parentEntry">Parent entry.</param>
		/// <returns></returns>
        public static IList<Entry> GetFeedBack(Entry parentEntry)
		{
			return ObjectProvider.Instance().GetFeedBack(parentEntry);
		}

        public static ICollection<Entry> GetRecentPostsWithCategories(int itemCount, bool activeOnly)
		{
			return ObjectProvider.Instance().GetRecentPostsWithCategories(itemCount, activeOnly);
		}

		/// <summary>
		/// Gets recent posts used to support the MetaBlogAPI. 
		/// Could be used for a Recent Posts control as well.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="postType">Post type.</param>
		/// <param name="activeOnly">Active only.</param>
		/// <returns></returns>
        public static IList<Entry> GetRecentPosts(int itemCount, PostType postType, bool activeOnly)
		{
			return ObjectProvider.Instance().GetRecentPosts(itemCount, postType, activeOnly);
		}

        public static IList<Entry> GetPostCollectionByMonth(int month, int year)
		{
			return ObjectProvider.Instance().GetPostCollectionByMonth(month,year);
		}

        public static IList<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
		{
			return  ObjectProvider.Instance().GetPostsByDayRange(start,stop,postType, activeOnly);
		}

        public static IList<Entry> GetEntriesByCategory(int itemCount, int catID, bool activeOnly)
		{
			return ObjectProvider.Instance().GetEntriesByCategory(itemCount,catID, activeOnly);
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
		public static Entry GetCategoryEntry(int entryId, EntryGetOption entryOption)
		{
			return ObjectProvider.Instance().GetCategoryEntry(entryId, (entryOption == EntryGetOption.ActiveOnly));
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
		public static int Create(Entry entry, params int[] CategoryIDs)
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
				entry.TitleUrl = entry.Url;
			}
			
			if(NullValue.IsNull(entry.DateCreated))
			{
				entry.DateCreated = DateTime.Now;
			}

			if(entry.IsActive && entry.IncludeInMainSyndication)
				entry.DateSyndicated = DateTime.Now;
			else
				entry.DateSyndicated = NullValue.NullDateTime;
			
			int id = ObjectProvider.Instance().Create(entry, CategoryIDs);
			NotificationServices.Run(entry);
			return id;
		}

		/// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title)
		{
			if(title == null)
				throw new ArgumentNullException("title", "Cannot generate friendly url from null title.");

            NameValueCollection friendlyUrlSettings = (NameValueCollection)ConfigurationManager.GetSection("FriendlyUrlSettings");
			if(friendlyUrlSettings == null)
			{
				//Default to old behavior.
				return AutoGenerateFriendlyUrl(title, char.MinValue);
			}

			string wordSeparator = friendlyUrlSettings["separatingCharacter"];;
			int wordCount;

			if (friendlyUrlSettings["limitWordCount"] == null)
			{
				wordCount = 0;
			}
			else
			{
				wordCount = int.Parse(friendlyUrlSettings["limitWordCount"]);
			}
			
			// break down to number of words. If 0 (or less) don't mess with the title
			if (wordCount > 0)
			{
				//only do this is there are more words than allowed.
				string[] words;
				words = title.Split(" ".ToCharArray());

				if (words.Length > wordCount)
				{
					//now strip the title down to the number of allowed words
					int wordCharCounter = 0;
					for (int i = 0; i < wordCount; i++)
					{
						wordCharCounter = wordCharCounter + words[i].Length + 1;
					}

					title = title.Substring(0, wordCharCounter-1);
				}
			}

			// separating characters are limited due to the problems certain chars
			// can cause. Only - _ and . are allowed
			if ((wordSeparator == "_") || (wordSeparator == ".") || (wordSeparator =="-"))
			{
				return AutoGenerateFriendlyUrl(title, wordSeparator[0]);
			}
			else
			{
				//invalid separator or none defined.
				return AutoGenerateFriendlyUrl(title, char.MinValue);
			}

		}

		/// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <param name="wordSeparator">The string used to separate words in the title.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title, char wordSeparator)
		{
			if(title == null)
				throw new ArgumentNullException("title", "Cannot generate friendly url from null title.");

			string entryName = StripSpaces(title, wordSeparator);
			entryName = RemoveNonWordCharacters(entryName);
			entryName = HttpUtility.UrlEncode(entryName);
			entryName = RemoveTrailingPeriods(entryName);
			entryName = entryName.Trim(new char[] {wordSeparator});
			entryName = RemoveDoublePeriods(entryName);

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

		static string StripSpaces(string text, char wordSeparator)
		{
			if(wordSeparator == char.MinValue)
			{
				//Special case if we are just removing spaces.
				return StringHelper.PascalCase(text);
			}
			else
			{
				return text.Replace(' ', wordSeparator);
			}
		}

		static string RemoveNonWordCharacters(string text)
		{
			Regex regex = new Regex(@"[\w\d\.]+", RegexOptions.Compiled);
			MatchCollection matches = regex.Matches(text);
			string cleansedText = string.Empty;

			foreach(Match match in matches)
			{
				if(match.Value.Length > 0)
				{
					cleansedText += match.Value;
				}
			}			
			return cleansedText;
		}
		
		static string RemoveDoublePeriods(string text)
		{
			while(text.IndexOf("..") > -1)
			{
				text = text.Replace("..", ".");
			}
			return text;
		}

		static string RemoveTrailingPeriods(string text)
		{
			Regex regex = new Regex(@"\.+$", RegexOptions.Compiled);
			return regex.Replace(text, string.Empty);
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
			if(NullValue.IsNull(entry.DateSyndicated) && entry.IsActive && entry.IncludeInMainSyndication)
			{
				entry.DateSyndicated = DateTime.Now;
			}
			
			if(!entry.IncludeInMainSyndication)
			{
				entry.DateSyndicated = NullValue.NullDateTime;
			}

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
			entry.DateUpdated = DateTime.Now;
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
			entry.Body = HtmlHelper.ConvertToAllowedHtml(entry.Body);
			entry.Title = HtmlHelper.SafeFormat(entry.Title);
			entry.IsXHMTL = false;
			entry.IsActive = true;
			entry.DateCreated = entry.DateUpdated = BlogTime.CurrentBloggerTime;
			
			if (null == entry.SourceName || String.Empty == entry.SourceName)
				entry.SourceName = "N/A";

			// insert comment into backend, save the returned entryid for permalink anchor below
			int entryID = Create(entry);

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
				catch(Exception e)
				{
					log.Error("Exception occurred while inserting comment", e);
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

