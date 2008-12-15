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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using log4net;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;
using System.Data.SqlClient;
using Subtext.Framework.Exceptions;

namespace Subtext.Framework
{
	/// <summary>
	/// Static class used to get entries (blog posts, comments, etc...) 
	/// from the data store.
	/// </summary>
	public static class Entries
	{
		private readonly static ILog log = new Log();		
	
		#region Paged Posts

		/// <summary>
		/// Returns a collection of Posts for a give page and index size.
		/// </summary>
		/// <param name="postType"></param>
		/// <param name="categoryID">-1 means not to filter by a categoryID</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
        public static IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize)
		{
			return ObjectProvider.Instance().GetPagedEntries(postType, categoryID, pageIndex, pageSize);
		}
		#endregion

		public static EntryDay GetSingleDay(DateTime dt)
		{
			return ObjectProvider.Instance().GetEntryDay(dt);
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

        public static ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int catID)
		{
			return ObjectProvider.Instance().GetPostsByCategoryID(itemCount,catID);
		}
		#region EntryCollections

		/// <summary>
		/// Gets the main syndicated entries.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <returns></returns>
		public static ICollection<Entry> GetMainSyndicationEntries(int itemCount)
		{
            return GetRecentPosts(itemCount, PostType.BlogPost, PostConfig.IncludeInMainSyndication | PostConfig.IsActive, true /* includeCategories */);
		}

		/// <summary>
		/// Gets the comments (including trackback, etc...) for the specified entry.
		/// </summary>
		/// <param name="parentEntry">Parent entry.</param>
		/// <returns></returns>
        public static ICollection<FeedbackItem> GetFeedBack(Entry parentEntry)
		{
			return ObjectProvider.Instance().GetFeedbackForEntry(parentEntry);
		}

		/// <summary>
		/// Returns the itemCount most recent posts.  
		/// This is used to support MetaBlogAPI...
		/// </summary>
		/// <param name="itemCount"></param>
		/// <param name="postType"></param>
		/// <param name="postConfig"></param>
		/// <param name="includeCategories"></param>
		/// <returns></returns>
		public static ICollection<Entry> GetRecentPosts(int itemCount, PostType postType, PostConfig postConfig, bool includeCategories)
		{
			return ObjectProvider.Instance().GetEntries(itemCount, postType, postConfig, includeCategories);
		}

        /// <summary>
        /// Returns the posts for the specified month for the Month Archive section.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
	    public static ICollection<Entry> GetPostsByMonth(int month, int year)
		{
			return ObjectProvider.Instance().GetPostsByMonth(month,year);
		}

        public static ICollection<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
		{
			return  ObjectProvider.Instance().GetPostsByDayRange(start,stop,postType, activeOnly);
		}

        public static ICollection<Entry> GetEntriesByCategory(int itemCount, int catID, bool activeOnly)
		{
			return ObjectProvider.Instance().GetEntriesByCategory(itemCount,catID, activeOnly);
		}
        public static ICollection<Entry> GetEntriesByTag(int itemCount, string tagName)
        {
            return ObjectProvider.Instance().GetEntriesByTag(itemCount, tagName);
        }
        #endregion

		#region Single Entry

		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public static FeedbackItem GetFeedbackByChecksumHash(string checksumHash)
		{
			return ObjectProvider.Instance().GetFeedbackByChecksumHash(checksumHash);
		}

		/// <summary>
		/// Returns an active entry by the id regardless of which blog it is 
		/// located in.
		/// </summary>
		/// <param name="entryId">The ID of the entry.</param>
		/// <param name="includeCategories">Whether the returned entry should have its categories collection populated.</param>
		/// <returns></returns>
		public static Entry GetEntry(int entryId, bool includeCategories)
		{
			return ObjectProvider.Instance().GetEntry(entryId, true, includeCategories);
		}

		/// <summary>
		/// Gets the entry from the data store by id. Only returns an entry if it is 
		/// within the current blog (Config.CurrentBlog).
		/// </summary>
		/// <param name="entryId">The ID of the entry.</param>
		/// <param name="postConfig">The entry option used to constrain the search.</param>
		/// <param name="includeCategories">Whether the returned entry should have its categories collection populated.</param>
		/// <returns></returns>
		public static Entry GetEntry(int entryId, PostConfig postConfig, bool includeCategories)
		{
            bool isActive = ((postConfig & PostConfig.IsActive) == PostConfig.IsActive);
            return ObjectProvider.Instance().GetEntry(entryId, isActive, includeCategories);
		}

		/// <summary>
		/// Gets the entry from the data store by entry name. Only returns an entry if it is 
		/// within the current blog (Config.CurrentBlog).
		/// </summary>
		/// <param name="EntryName">Name of the entry.</param>
		/// <param name="postConfig">The entry option used to constrain the search.</param>
        /// <param name="includeCategories">Whether the returned entry should have its categories collection populated.</param>
		/// <returns></returns>
        public static Entry GetEntry(string EntryName, PostConfig postConfig, bool includeCategories)
		{
            bool isActive = ((postConfig & PostConfig.IsActive) == PostConfig.IsActive);
            return ObjectProvider.Instance().GetEntry(EntryName, isActive, includeCategories);
		}
		#endregion

		#region Delete
	
		/// <summary>
		/// Deletes the entry with the specified entryId.
		/// </summary>
		/// <param name="entryId"></param>
		/// <returns></returns>
		public static bool Delete(int entryId)
		{
			return ObjectProvider.Instance().DeleteEntry(entryId);
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
            if (entry == null)
            {
                throw new ArgumentNullException("entry");//Resources.ArgumentNull_Generic);
            }

            Debug.Assert(entry.PostType != PostType.None, "Posttype should never be null.");

			if (Config.CurrentBlog.AutoFriendlyUrlEnabled
				&& String.IsNullOrEmpty(entry.EntryName)
				&& !String.IsNullOrEmpty(entry.Title))
			{
				entry.EntryName = AutoGenerateFriendlyUrl(entry.Title, entry.Id);
			}
            else if (!String.IsNullOrEmpty(entry.EntryName))
            {
				entry.EntryName = AutoGenerateFriendlyUrl(entry.EntryName, entry.Id);
            }
			
			if(NullValue.IsNull(entry.DateCreated))
			{
				entry.DateCreated = Config.CurrentBlog.TimeZone.Now;
			}

            if (entry.IsActive)
            {
                if (NullValue.IsNull(entry.DateSyndicated))
                {
                    entry.DateSyndicated = Config.CurrentBlog.TimeZone.Now;
                }
            }
            else
            {
                entry.DateSyndicated = NullValue.NullDateTime;
            }

			int[] categoryIds = {};
			if(entry.Categories.Count > 0)
			{
				categoryIds = GetCategoryIdsFromCategoryTitles(entry);
			}

			try
			{
				entry.Id = ObjectProvider.Instance().Create(entry, categoryIds);
			}
			catch(SqlException e)
			{
				if(e.Message.Contains("pick a unique EntryName"))
				{
					throw new DuplicateEntryException("An entry with that EntryName already exists.", e);
				}
				throw;
			}
			Tags.SetTagsOnEntry(entry);

			log.Debug("Created entry, running notification services.");
			NotificationServices.Run(entry);
			return entry.Id;
		}

		public static int[] GetCategoryIdsFromCategoryTitles(Entry entry)
		{
			int[] categoryIds;
			Collection<int> catIds = new Collection<int>();
			//Ok, we have categories specified in the entry, but not the IDs.
			//We need to do something.
			foreach(string category in entry.Categories)
			{
				LinkCategory cat = Links.GetLinkCategory(category, true);
				if(cat != null)
				{
					catIds.Add(cat.Id);
				}
			}
			categoryIds = new int[catIds.Count];
			catIds.CopyTo(categoryIds, 0);
			return categoryIds;
        }

        #endregion


		/// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// Defaults entryId to 0 as if it was a new entry
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title)
		{
			return AutoGenerateFriendlyUrl(title, 0);
		}


        /// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <param name="entryId">The id of the current entry.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title, int entryId)
		{
			if(title == null)
				throw new ArgumentNullException("title", "Cannot generate friendly url from null title.");

        	FriendlyUrlSettings friendlyUrlSettings = FriendlyUrlSettings.Settings;
			if(friendlyUrlSettings == null)
			{
				//Default to old behavior.
				return AutoGenerateFriendlyUrl(title, char.MinValue, entryId, TextTransform.None);
			}

        	TextTransform textTransform = friendlyUrlSettings.TextTransformation;
			string wordSeparator = friendlyUrlSettings.SeparatingCharacter;
			int wordCount = friendlyUrlSettings.WordCountLimit;

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
				return AutoGenerateFriendlyUrl(title, wordSeparator[0], entryId, textTransform);
			}
			else
			{
				//invalid separator or none defined.
				return AutoGenerateFriendlyUrl(title, char.MinValue, entryId, textTransform);
			}

		}

		/// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// Defaults entryId to 0 as if it was a new entry
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <param name="wordSeparator">The string used to separate words in the title.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title, char wordSeparator)
		{
			return AutoGenerateFriendlyUrl(title, wordSeparator, 0, TextTransform.None);
		}

		/// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// Defaults entryId to 0 as if it was a new entry
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <param name="wordSeparator">The string used to separate words in the title.</param>
		/// <param name="textTransform">Used to specify a change to the casing of the string.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title, char wordSeparator, TextTransform textTransform)
		{
			return AutoGenerateFriendlyUrl(title, wordSeparator, 0, textTransform);
		}

		private static string ReplaceUnicodeCharacters(string text)
		{
			string normalized = text.Normalize(NormalizationForm.FormKD);
			Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage, new EncoderReplacementFallback(string.Empty), new DecoderReplacementFallback(string.Empty));
			byte[] bytes = removal.GetBytes(normalized);
			return Encoding.ASCII.GetString(bytes);
		} 

		/// <summary>
		/// Converts a title of a blog post into a friendly, but URL safe string.
		/// </summary>
		/// <param name="title">The original title of the blog post.</param>
		/// <param name="wordSeparator">The string used to separate words in the title.</param>
		/// <param name="entryId">The id of the current entry.</param>
		/// <param name="textTransform">Used to specify a change to the casing of the string.</param>
		/// <returns></returns>
		public static string AutoGenerateFriendlyUrl(string title, char wordSeparator, int entryId, TextTransform textTransform)
		{
            if (title == null)
            {
                throw new ArgumentNullException("title", "Cannot generate friendly url from null title.");
            }
			
			string entryName = RemoveNonWordCharacters(title);
			entryName = ReplaceSpacesWithSeparator(entryName, wordSeparator);
			entryName = ReplaceUnicodeCharacters(entryName);
			entryName = HttpUtility.UrlEncode(entryName);
			entryName = RemoveTrailingPeriods(entryName);
			entryName = entryName.Trim(new char[] {wordSeparator});
			entryName = entryName.RemoveDoubleCharacter('.');
			if (wordSeparator != char.MinValue && wordSeparator != '.')
                entryName = entryName.RemoveDoubleCharacter(wordSeparator);

            if (entryName.IsNumeric())
		    {
                entryName = "n" + wordSeparator + entryName;
		    }

			string newEntryName = FriendlyUrlSettings.TransformString(entryName, textTransform);

			int tryCount = 0;
			Entry currentEntry = ObjectProvider.Instance().GetEntry(newEntryName, false, false);

			while (currentEntry != null)
			{
				if (currentEntry.Id == entryId) //This means that we are updating the same entry, so should allow same entryname
					break; 
				switch(tryCount)
				{
					case 0:
						newEntryName = entryName + wordSeparator + "Again";
						break;
					case 1:
						newEntryName = entryName + wordSeparator + "Yet" + wordSeparator + "Again";
						break;
					case 2:
						newEntryName = entryName + wordSeparator + "And" + wordSeparator + "Again";
						break;
					case 3:
						newEntryName = entryName + wordSeparator + "Once" + wordSeparator + "More";
						break;
					case 4:
						newEntryName = entryName + wordSeparator + "To" + wordSeparator + "Beat" + wordSeparator + "A" + wordSeparator +
							               "Dead" + wordSeparator + "Horse";
						break;
				}
				if (tryCount++ > 5)
					break; //Allow an exception to get thrown later.

				newEntryName = FriendlyUrlSettings.TransformString(newEntryName, textTransform);
				currentEntry = ObjectProvider.Instance().GetEntry(newEntryName, false, false);
			}

			return newEntryName;
		}

        private static string ReplaceSpacesWithSeparator(string text, char wordSeparator)
		{
			if(wordSeparator == char.MinValue)
			{
				//Special case if we are just removing spaces.
				return text.ToPascalCase();
			}
			else
			{
				return text.Replace(' ', wordSeparator);
			}
		}

		private static string RemoveNonWordCharacters(string text)
		{
			Regex regex = new Regex(@"[\w\d\.\- ]+", RegexOptions.Compiled);
			MatchCollection matches = regex.Matches(text);
            StringBuilder cleansedText = new StringBuilder();

			foreach(Match match in matches)
			{
				if(match.Value.Length > 0)
				{
					cleansedText.Append(match.Value);
				}
			}			
			return cleansedText.ToString();
		}

        private static string RemoveTrailingPeriods(string text)
		{
			Regex regex = new Regex(@"\.+$", RegexOptions.Compiled);
			return regex.Replace(text, string.Empty);
		}

		#region Update

		/// <summary>
		/// Updates the specified entry in the data provider.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public static void Update(Entry entry)
		{
            if (entry == null)
                throw new ArgumentNullException("entry", "Entry cannot be null.");//Resources.ArgumentNull_Generic);

			if (NullValue.IsNull(entry.DateSyndicated) && entry.IsActive && entry.IncludeInMainSyndication)
				entry.DateSyndicated = Config.CurrentBlog.TimeZone.Now;

            Update(entry, null /* categoryIds */);
		}

		/// <summary>
		/// Updates the specified entry in the data provider 
		/// and attaches the specified categories.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="categoryIDs">Category Ids this entry belongs to.</param>
		/// <returns></returns>
		public static void Update(Entry entry, params int[] categoryIds)
		{
            if (entry == null)
            {
                throw new ArgumentNullException("entry", "entry cannot be null");//Resources.ArgumentNull_Generic);
            }

            entry.DateModified = Config.CurrentBlog.TimeZone.Now;

            if (!string.IsNullOrEmpty(entry.EntryName)) {
                entry.EntryName = AutoGenerateFriendlyUrl(entry.EntryName, entry.Id);
            }

            ObjectProvider.Instance().Update(entry, categoryIds);

            ICollection<string> tags = HtmlHelper.ParseTags(entry.Body);
            ObjectProvider.Instance().SetEntryTagList(entry.Id, tags);
		}

		#endregion

		#region Entry Category List

		/// <summary>
		/// Sets the categories for this entry.
		/// </summary>
		/// <param name="entryId">The entry id.</param>
		/// <param name="categories">The categories.</param>
		public static void SetEntryCategoryList(int entryId, params int[] categories)
		{
			ObjectProvider.Instance().SetEntryCategoryList(entryId, categories);
		}

		#endregion

        #region Tag Utility Functions

        public static bool RebuildAllTags()
        {
            foreach (EntryDay day in GetBlogPosts(0, PostConfig.None))
            {
                foreach (Entry e in day)
                {
                    ObjectProvider.Instance().SetEntryTagList(e.Id, HtmlHelper.ParseTags(e.Body));
                }
            }
            return true;
        }

        #endregion
    }
}




