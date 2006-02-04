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
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to filter incoming comments.  This will get replaced 
	/// with a plugin once the plugin architecture is complete, but the 
	/// logic will probably get ported.
	/// </summary>
	public sealed class CommentFilter
	{
		private const string FILTER_CACHE_KEY = "COMMENT FILTER:";
		private const string BLACKLIST_CACHE_KEY = "BLACKLIST:";
		private const int BLACKLIST_TIMEOUT = 60; //minutes.
		private CommentFilter() {}

		/// <summary>
		/// Filters the comment. Throws an exception should the comment not be allowed. 
		/// Otherwise returns true.  This interface may be changed.
		/// </summary>
		/// <remarks>
		/// <p>
		/// The first filter examines whether comments are coming in too quickly 
		/// from the same SourceUrl.  Looks at the <see cref="BlogInfo.CommentDelayInMinutes"/>.
		/// </p>
		/// <p>
		/// The second filter checks for duplicate comments. It only looks at the body 
		/// of the comment.
		/// </p>
		/// </remarks>
		/// <param name="entry">Entry.</param>
		public static void FilterComment(Entry entry)
		{
			if(!SourceFrequencyIsValid(entry))
				throw new Subtext.Framework.Exceptions.CommentFrequencyException();

			if(!Config.CurrentBlog.DuplicateCommentsEnabled && IsDuplicateComment(entry))
				throw new Subtext.Framework.Exceptions.CommentDuplicateException();

			if(ContainsSpam(entry))
				throw new CommentSpamException("Sorry, spam is not allowed.");

			if(IsInBlackList(entry))
				throw new CommentBlackListException("Sorry, you've been temporarily blacklisted for spamming.");
		}

		static bool ContainsSpam(Entry entry)
		{
			string spamWordsText = System.Configuration.ConfigurationSettings.AppSettings["SpamWords"];
			if(spamWordsText != null && spamWordsText.Length > 0)
			{
				string[] spamWords = spamWordsText.Split(' ');
				foreach(string spamWord in spamWords)
				{
					Regex regex = new Regex(@"(^|[^a-zA-Z])" + spamWord + @"([^a-zA-Z]|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
					//We're not going to filter the body yet since most of the 
					//spam puts casino in title.
					
					if(regex.IsMatch(entry.Title) 
						|| regex.IsMatch(entry.Author)
						|| regex.IsMatch(entry.TitleUrl))
					{
						AddToBlackList(entry);
						return true;
					}					
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether [is in black list] [the specified entry].
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns>
		/// 	<c>true</c> if [is in black list] [the specified entry]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInBlackList(Entry entry)
		{
			Cache cache = HttpContext.Current.Cache;
			if(cache.Get(BLACKLIST_CACHE_KEY + entry.SourceName) != null)
				return true;
			return false;
		}

		/// <summary>
		/// Adds the source IP of this entry to black list.  The 
		/// black list is temporary for now.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		public static void AddToBlackList(Entry entry)
		{
			Cache cache = HttpContext.Current.Cache;
			cache.Insert(BLACKLIST_CACHE_KEY + entry.SourceName, string.Empty, null, DateTime.Now.AddMinutes(BLACKLIST_TIMEOUT), TimeSpan.Zero);
		}

		// Returns true if the source of the entry is not 
		// posting too many.
		static bool SourceFrequencyIsValid(Entry entry)
		{
			if(Config.CurrentBlog.CommentDelayInMinutes <= 0)
				return true;

			Cache cache = HttpContext.Current.Cache;
			object lastComment = cache.Get(FILTER_CACHE_KEY + entry.SourceName);
			
			if(lastComment != null)
			{
				//Comment was made too frequently.
				return false;
			}

			//Add to cache.
            cache.Insert(FILTER_CACHE_KEY + entry.SourceName, string.Empty, null, DateTime.Now.AddMinutes(Config.CurrentBlog.CommentDelayInMinutes), TimeSpan.Zero);
			return true;
		}

		// Returns true if this entry is a duplicate.
		static bool IsDuplicateComment(Entry entry)
		{
			const int RECENT_ENTRY_CAPACITY = 10;

			// Check the cache for the last 10 comments
			// Chances are, if a spam attack is occurring, then 
			// this entry will be a duplicate of a recent entry.
			// This checks in memory before going to the database (or other persistent store).
			Cache cache = HttpContext.Current.Cache;
			Queue recentComments = cache[FILTER_CACHE_KEY + ".RECENT_COMMENTS"] as Queue;
			if(recentComments != null)
			{
				if(QueueContainsChecksumHash(recentComments, entry))
					return true;
			}
			else
			{
				recentComments = new Queue(RECENT_ENTRY_CAPACITY);	
				cache[FILTER_CACHE_KEY + ".RECENT_COMMENTS"] = recentComments;
			}

			// Check the database
			Entry duplicate = Entries.GetCommentByChecksumHash(entry.ContentChecksumHash);
			if(duplicate != null)
				return true;

			//Ok, this is not a duplicate... Update recent comments.
            if(recentComments.Count == RECENT_ENTRY_CAPACITY)
				recentComments.Dequeue();

			recentComments.Enqueue(entry.ContentChecksumHash);
			return false;
		}

		private static bool QueueContainsChecksumHash(Queue recentComments, Entry entry)
		{
			foreach(string contentChecksumHash in recentComments)
			{
				if(entry.ContentChecksumHash == contentChecksumHash)
				{
					return true;
				}
			}
			return false;
		}
	}
}
