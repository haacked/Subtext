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
using System.Web.Caching;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Data;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to filter incoming comments.  This will get replaced 
	/// with a plugin once the plugin architecture is complete, but the 
	/// logic will probably get ported.
	/// </summary>
	public class CommentFilter
	{
		private const string FILTER_CACHE_KEY = "COMMENT FILTER:";

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentFilter"/> class.
		/// </summary>
		public CommentFilter(ICache cache, IFeedbackSpamService spamService)
		{
			Cache = cache;
            SpamService = spamService;
		}

        protected ICache Cache {
            get;
            private set;
        }

        protected IFeedbackSpamService SpamService {
            get;
            private set;
        }
		
		/// <summary>
		/// Validates the feedback before it has been persisted.
		/// </summary>
		/// <param name="feedback"></param>
		/// <exception type="CommentFrequencyException">Thrown if too many comments are received from the same source in a short period.</exception>
		/// <exception type="CommentDuplicateException">Thrown if the blog does not allow duplicate comments and too many are received in a short period of time.</exception>
		public void FilterBeforePersist(FeedbackItem feedback)
		{
			if (!SecurityHelper.IsAdmin)
			{
				if (!SourceFrequencyIsValid(feedback))
					throw new CommentFrequencyException();

				if (!Config.CurrentBlog.DuplicateCommentsEnabled && IsDuplicateComment(feedback))
					throw new CommentDuplicateException();
			}
		}

		/// <summary>
		/// Filters the comment. Throws an exception should the comment not be allowed. 
		/// Otherwise returns true.  This interface may be changed.
		/// </summary>
		/// <remarks>
		/// <p>
		/// The first filter examines whether comments are coming in too quickly 
		/// from the same SourceUrl.  Looks at the <see cref="Blog.CommentDelayInMinutes"/>.
		/// </p>
		/// <p>
		/// The second filter checks for duplicate comments. It only looks at the body 
		/// of the comment.
		/// </p>
		/// </remarks>
		/// <param name="feedbackItem">Entry.</param>
		public void FilterAfterPersist(FeedbackItem feedbackItem)
		{
			if (!SecurityHelper.IsAdmin)
			{
				if (!Config.CurrentBlog.ModerationEnabled)
				{
					//Akismet Check...
					if (Config.CurrentBlog.FeedbackSpamServiceEnabled && SpamService != null) {
						if (SpamService.IsSpam(feedbackItem))
						{
							FlagAsSpam(feedbackItem);
							return;
						}
					}
					//Note, we need to explicitely set the status flag here.
					//Just setting Approved = true would not reset any other bits in the flag that may be set.
					feedbackItem.Status = FeedbackStatusFlag.Approved;
				}
				else //Moderated!
				{
					//Note, we need to explicitely set the status flag here.
					//Just setting NeedsModeration = true would not reset any other bits in the flag that may be set.
					feedbackItem.Status = FeedbackStatusFlag.NeedsModeration;
				}
			}
			else
			{
				//Note, we need to explicitely set the status flag here.
				//Just setting Approved = true would not reset any other bits in the flag that may be set.
				feedbackItem.Status = FeedbackStatusFlag.Approved;
			}
			FeedbackItem.Update(feedbackItem);
		}

		private static void FlagAsSpam(FeedbackItem feedbackItem)
		{
			feedbackItem.FlaggedAsSpam = true;
			feedbackItem.Approved = false;
			FeedbackItem.Update(feedbackItem);
		}

		// Returns true if the source of the entry is not 
		// posting too many.
		bool SourceFrequencyIsValid(FeedbackItem feedbackItem)
		{
			if(Config.CurrentBlog.CommentDelayInMinutes <= 0)
				return true;

			object lastComment = Cache[FILTER_CACHE_KEY + feedbackItem.IpAddress];
			
			if(lastComment != null)
			{
				//Comment was made too frequently.
				return false;
			}

			//Add to cache.
            Cache.Insert(FILTER_CACHE_KEY + feedbackItem.IpAddress, string.Empty, null, DateTime.Now.AddMinutes(Config.CurrentBlog.CommentDelayInMinutes), TimeSpan.Zero);
			return true;
		}

		// Returns true if this entry is a duplicate.
		bool IsDuplicateComment(FeedbackItem feedbackItem)
		{
			const int RECENT_ENTRY_CAPACITY = 10;

			if(Cache == null)
				return false;
			
			// Check the cache for the last 10 comments
			// Chances are, if a spam attack is occurring, then 
			// this entry will be a duplicate of a recent entry.
			// This checks in memory before going to the database (or other persistent store).
			Queue<string> recentComments = Cache[FILTER_CACHE_KEY + ".RECENT_COMMENTS"] as Queue<string>;
			if(recentComments != null)
			{
				if (recentComments.Contains(feedbackItem.ChecksumHash))
					return true;
			}
			else
			{
				recentComments = new Queue<string>(RECENT_ENTRY_CAPACITY);	
				Cache[FILTER_CACHE_KEY + ".RECENT_COMMENTS"] = recentComments;
			}

			// Check the database
			FeedbackItem duplicate = Entries.GetFeedbackByChecksumHash(feedbackItem.ChecksumHash);
			if(duplicate != null)
				return true;

			//Ok, this is not a duplicate... Update recent comments.
            if(recentComments.Count == RECENT_ENTRY_CAPACITY)
				recentComments.Dequeue();

			recentComments.Enqueue(feedbackItem.ChecksumHash);
			return false;
		}

		/// <summary>
		/// Clears the comment cache.
		/// </summary>
		public void ClearCommentCache()
		{
			Cache.Remove(FILTER_CACHE_KEY + ".RECENT_COMMENTS");
		}
	}
}
