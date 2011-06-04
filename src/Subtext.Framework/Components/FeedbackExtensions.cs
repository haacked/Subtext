using System;
using System.Collections.Generic;
using Subtext.Extensibility;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace Subtext.Framework.Components
{
    public static class FeedbackExtensions
    {
        /// <summary>
        /// Gets the specified feedback by id.
        /// </summary>
        /// <param name="feedbackId">The feedback id.</param>
        /// <returns></returns>
        public static FeedbackItem Get(this ObjectRepository repository, int feedbackId)
        {
            return repository.GetFeedback(feedbackId);
        }

        /// <summary>
        /// Gets the feedback counts for the various top level statuses.
        /// </summary>
        public static FeedbackCounts GetFeedbackCounts(this ObjectRepository repository)
        {
            FeedbackCounts counts;
            repository.GetFeedbackCounts(out counts.ApprovedCount, out counts.NeedsModerationCount,
                                                        out counts.FlaggedAsSpamCount, out counts.DeletedCount);
            return counts;
        }

        /// <summary>
        /// Returns the itemCount most recent active comments.
        /// </summary>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static ICollection<FeedbackItem> GetRecentComments(this ObjectRepository repository, int itemCount)
        {
            return repository.GetPagedFeedback(0, itemCount, FeedbackStatusFlag.Approved,
                                                              FeedbackStatusFlag.None, FeedbackType.Comment);
        }

        /// <summary>
        /// Updates the specified entry in the data provider.
        /// </summary>
        /// <param name="feedbackItem">Entry.</param>
        /// <returns></returns>
        public static bool Update(this ObjectRepository repository, FeedbackItem feedbackItem)
        {
            if (feedbackItem == null)
            {
                throw new ArgumentNullException("feedbackItem");
            }

            return repository.UpdateInternal(feedbackItem);
        }

        /// <summary>
        /// Approves the comment, and removes it from the SPAM folder or from the 
        /// Trash folder.
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="spamService"></param>
        /// <returns></returns>
        public static void Approve(this ObjectRepository repository, FeedbackItem feedback, ICommentSpamService spamService)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException("feedback");
            }

            feedback.SetStatus(FeedbackStatusFlag.Approved, true);
            feedback.SetStatus(FeedbackStatusFlag.Deleted, false);
            if (spamService != null)
            {
                spamService.SubmitGoodFeedback(feedback);
            }

            repository.Update(feedback);
        }

        /// <summary>
        /// Confirms the feedback as spam and moves it to the trash.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        /// <param name="spamService"></param>
        public static void ConfirmSpam(this ObjectRepository repository, FeedbackItem feedback, ICommentSpamService spamService)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException("feedback");
            }

            feedback.SetStatus(FeedbackStatusFlag.Approved, false);
            feedback.SetStatus(FeedbackStatusFlag.ConfirmedSpam, true);

            if (spamService != null)
            {
                spamService.SubmitGoodFeedback(feedback);
            }

            repository.Update(feedback);
        }

        /// <summary>
        /// Confirms the feedback as spam and moves it to the trash.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        public static void Delete(this ObjectRepository repository, FeedbackItem feedback)
        {
            if (feedback == null)
            {
                throw new ArgumentNullException("feedback");
            }

            feedback.SetStatus(FeedbackStatusFlag.Approved, false);
            feedback.SetStatus(FeedbackStatusFlag.Deleted, true);

            repository.Update(feedback);
        }


        /// <summary>
        /// Destroys all non-active emails that meet the status.
        /// </summary>
        /// <param name="feedbackStatus">The feedback.</param>
        public static void Destroy(this ObjectRepository repository, FeedbackStatusFlag feedbackStatus)
        {
            if ((feedbackStatus & FeedbackStatusFlag.Approved) == FeedbackStatusFlag.Approved)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_DestroyActiveComment);
            }

            repository.DestroyFeedback(feedbackStatus);
        }

    }
}
