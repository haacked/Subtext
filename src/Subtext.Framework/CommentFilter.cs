#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using Subtext.Framework.Components;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Infrastructure;

namespace Subtext.Framework
{
    /// <summary>
    /// Class used to filter incoming comments.  This will get replaced 
    /// with a plugin once the plugin architecture is complete, but the 
    /// logic will probably get ported.
    /// </summary>
    public class CommentFilter : ICommentFilter
    {
        private const string FilterCacheKey = "COMMENT FILTER:";

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFilter"/> class.
        /// </summary>
        public CommentFilter(ISubtextContext context, ICommentSpamService spamService)
        {
            SubtextContext = context;
            SpamService = spamService;
            Blog = context.Blog;
            Cache = context.Cache;
        }

        public ISubtextContext SubtextContext { get; private set; }

        protected ICommentSpamService SpamService { get; private set; }

        protected Blog Blog { get; private set; }

        protected ICache Cache { get; private set; }

        #region ICommentFilter Members

        /// <summary>
        /// Validates the feedback before it has been persisted.
        /// </summary>
        /// <param name="feedback"></param>
        /// <exception type="CommentFrequencyException">Thrown if too many comments are received from the same source in a short period.</exception>
        /// <exception type="CommentDuplicateException">Thrown if the blog does not allow duplicate comments and too many are received in a short period of time.</exception>
        public void FilterBeforePersist(FeedbackItem feedback)
        {
            if(!SubtextContext.User.IsAdministrator())
            {
                if(!SourceFrequencyIsValid(feedback))
                {
                    throw new CommentFrequencyException(Blog.CommentDelayInMinutes);
                }

                if(!Blog.DuplicateCommentsEnabled && IsDuplicateComment(feedback))
                {
                    throw new CommentDuplicateException();
                }
            }
        }

        /// <summary>
        /// Filters the comment. Throws an exception should the comment not be allowed. 
        /// Otherwise returns true.  This interface may be changed.
        /// </summary>
        /// <remarks>
        /// <p>
        /// The first filter examines whether comments are coming in too quickly 
        /// from the same SourceUrl.  Looks at the Blog.CommentDelayInMinutes.
        /// </p>
        /// <p>
        /// The second filter checks for duplicate comments. It only looks at the body 
        /// of the comment.
        /// </p>
        /// </remarks>
        /// <param name="feedbackItem">Entry.</param>
        public void FilterAfterPersist(FeedbackItem feedbackItem)
        {
            if(!SubtextContext.User.IsAdministrator())
            {
                if(!Blog.ModerationEnabled)
                {
                    //Akismet Check...
                    if(Blog.FeedbackSpamServiceEnabled && SpamService != null)
                    {
                        if(SpamService.IsSpam(feedbackItem))
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
            feedbackItem.DateModified = Blog.TimeZone.Now;
            SubtextContext.Repository.Update(feedbackItem);
        }

        #endregion

        private void FlagAsSpam(FeedbackItem feedbackItem)
        {
            feedbackItem.FlaggedAsSpam = true;
            feedbackItem.Approved = false;
            feedbackItem.DateModified = Blog.TimeZone.Now;
            SubtextContext.Repository.Update(feedbackItem);
        }

        // Returns true if the source of the entry is not 
        // posting too many.
        bool SourceFrequencyIsValid(FeedbackItem feedbackItem)
        {
            if(Blog.CommentDelayInMinutes <= 0)
            {
                return true;
            }

            object lastComment = Cache[FilterCacheKey + feedbackItem.IpAddress];

            if(lastComment != null)
            {
                //Comment was made too frequently.
                return false;
            }

            //Add to cache.
            Cache.Insert(FilterCacheKey + feedbackItem.IpAddress, string.Empty, null,
                         DateTime.Now.AddMinutes(Blog.CommentDelayInMinutes), TimeSpan.Zero);
            return true;
        }

        // Returns true if this entry is a duplicate.
        bool IsDuplicateComment(FeedbackItem feedbackItem)
        {
            const int recentEntryCapacity = 10;

            if(Cache == null)
            {
                return false;
            }

            // Check the cache for the last 10 comments
            // Chances are, if a spam attack is occurring, then 
            // this entry will be a duplicate of a recent entry.
            // This checks in memory before going to the database (or other persistent store).
            var recentCommentChecksums = Cache[FilterCacheKey + ".RECENT_COMMENTS"] as Queue<string>;
            if(recentCommentChecksums != null)
            {
                if(recentCommentChecksums.Contains(feedbackItem.ChecksumHash))
                {
                    return true;
                }
            }
            else
            {
                recentCommentChecksums = new Queue<string>(recentEntryCapacity);
                Cache[FilterCacheKey + ".RECENT_COMMENTS"] = recentCommentChecksums;
            }

            // Check the database
            FeedbackItem duplicate = SubtextContext.Repository.GetFeedbackByChecksumHash(feedbackItem.ChecksumHash);
            if(duplicate != null)
            {
                return true;
            }

            //Ok, this is not a duplicate... Update recent comments.
            if(recentCommentChecksums.Count == recentEntryCapacity)
            {
                recentCommentChecksums.Dequeue();
            }

            recentCommentChecksums.Enqueue(feedbackItem.ChecksumHash);
            return false;
        }
    }
}