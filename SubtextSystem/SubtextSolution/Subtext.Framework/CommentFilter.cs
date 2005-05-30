using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

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
		private CommentFilter() {}

		/// <summary>
		/// Filters the comment. Throws an exception should the comment not be allowed. 
		/// Otherwise returns true.  This interface may be changed.
		/// </summary>
		/// <param name="entry">Entry.</param>
		public static void FilterComment(Entry entry)
		{
			if(!SourceFrequencyIsValid(entry))
				throw new Subtext.Framework.Exceptions.CommentFrequencyException();

			if(!Config.CurrentBlog.EnableDuplicateComments && IsDuplicateComment(entry))
				throw new Subtext.Framework.Exceptions.CommentDuplicateException();
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
