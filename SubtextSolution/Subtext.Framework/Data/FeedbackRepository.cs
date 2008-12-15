using System.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Text;
using Subtext.Extensibility.Interfaces;
using System.Collections.Generic;
using System;

namespace Subtext.Framework.Data
{
    public partial class DatabaseObjectProvider
    {
        /// <summary>
        /// Returns the feedback by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override FeedbackItem GetFeedback(int id)
        {
            using (IDataReader reader = _procedures.GetFeedback(id))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadFeedbackItem(reader);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the feedback counts for the various top level statuses.
        /// </summary>
        /// <param name="approved">The approved.</param>
        /// <param name="needsModeration">The needs moderation.</param>
        /// <param name="flaggedAsSpam">The flagged as spam.</param>
        /// <param name="deleted">The deleted.</param>
        public override void GetFeedbackCounts(out int approved, out int needsModeration, out int flaggedAsSpam, out int deleted)
        {
            _procedures.GetFeedbackCountsByStatus(BlogId, out approved, out needsModeration, out flaggedAsSpam, out deleted);
        }

        /// <summary>
        /// Gets the paged feedback.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="status">A flag for the status types to return.</param>
        /// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
        /// <param name="type">The type of feedback to return.</param>
        /// <returns></returns>
        public override IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlag status, FeedbackStatusFlag excludeStatusMask, FeedbackType type)
        {
            int? feedbackType = (type == FeedbackType.None ? null : (int?)type);
            int? excludeStatus = (excludeStatusMask == FeedbackStatusFlag.None ? null : (int?)excludeStatusMask);
            using (IDataReader reader = _procedures.GetPageableFeedback(BlogId, pageIndex, pageSize, (int)status, excludeStatus, feedbackType))
            {
                return reader.GetPagedCollection(r => DataHelper.LoadFeedbackItem(reader));
            }
        }

        /// <summary>
        /// Returns all the active entries for the specified post.
        /// </summary>
        /// <param name="parentEntry"></param>
        /// <returns></returns>
        public override ICollection<FeedbackItem> GetFeedbackForEntry(Entry parentEntry)
        {
            using (IDataReader reader = _procedures.GetFeedbackCollection(parentEntry.Id))
            {
                List<FeedbackItem> ec = new List<FeedbackItem>();
                while (reader.Read())
                {
                    //Don't build links.
                    FeedbackItem feedbackItem = DataHelper.LoadFeedbackItem(reader, parentEntry);
                    ec.Add(feedbackItem);
                }
                return ec;
            }
        }

        /// <summary>
        /// Searches the data store for the first comment with a 
        /// matching checksum hash.
        /// </summary>
        /// <param name="checksumHash">Checksum hash.</param>
        /// <returns></returns>
        //TODO: This needs a unit test.
        public override FeedbackItem GetFeedbackByChecksumHash(string checksumHash)
        {
            using (IDataReader reader = _procedures.GetCommentByChecksumHash(checksumHash, BlogId))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadFeedbackItem(reader);
                }
                return null;
            }
        }

        public override void DestroyFeedback(int id)
        {
            _procedures.DeleteFeedback(id, CurrentDateTime);
        }

        public override void DestroyFeedback(FeedbackStatusFlag status)
        {
            _procedures.DeleteFeedbackByStatus(BlogId, (int)status);
        }

        public override int Create(FeedbackItem feedbackItem)
        {
            if (feedbackItem == null)
                throw new ArgumentNullException("feedbackItem", "Cannot insert a null feedback item.");

            string ipAddress = null;
            if (feedbackItem.IpAddress != null)
                ipAddress = feedbackItem.IpAddress.ToString();

            string sourceUrl = null;
            if (feedbackItem.SourceUrl != null)
                sourceUrl = feedbackItem.SourceUrl.ToString();

            return _procedures.InsertFeedback(feedbackItem.Title,
                feedbackItem.Body,
                BlogId,
                feedbackItem.EntryId.NullIfMinValue(),
                feedbackItem.Author,
                feedbackItem.IsBlogAuthor,
                feedbackItem.Email,
                sourceUrl,
                (int)feedbackItem.FeedbackType,
                (int)feedbackItem.Status,
                feedbackItem.CreatedViaCommentAPI,
                feedbackItem.Referrer,
                ipAddress,
                feedbackItem.UserAgent,
                feedbackItem.ChecksumHash,
                feedbackItem.DateCreated,
                feedbackItem.DateModified,
                CurrentDateTime);
        }

        /// <summary>
        /// Saves changes to the specified feedback.
        /// </summary>
        /// <param name="feedbackItem">The feedback item.</param>
        /// <returns></returns>
        public override bool Update(FeedbackItem feedbackItem)
        {
            string sourceUrl = null;
            if (feedbackItem.SourceUrl != null)
                sourceUrl = feedbackItem.SourceUrl.ToString();

            return _procedures.UpdateFeedback(feedbackItem.Id,
                feedbackItem.Title,
                feedbackItem.Body.NullIfEmpty(),
                feedbackItem.Author.NullIfEmpty(),
                feedbackItem.Email.NullIfEmpty(),
                sourceUrl,
                (int)feedbackItem.Status,
                feedbackItem.ChecksumHash,
                CurrentDateTime,
                CurrentDateTime);
        }
    }
}
