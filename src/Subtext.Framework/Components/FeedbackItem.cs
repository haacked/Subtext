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
using System.Net;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;
using Subtext.Framework.Services;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for Entry.
    /// </summary>
    [Serializable]
    public class FeedbackItem : IIdentifiable
    {
        private string _body;
        private string _email;
        string _feedbackChecksumHash = string.Empty;
        private Entry _entry;
        DateTime _parentDateCreated = NullValue.NullDateTime;
        string _parentEntryName;
        string _referrer;
        string _userAgent;

        /// <summary>
        /// Ctor. Creates a new <see cref="FeedbackItem"/> instance.
        /// </summary>
        /// <param name="type">Ptype.</param>
        public FeedbackItem(FeedbackType type)
        {
            Id = NullValue.NullInt32;
            EntryId = NullValue.NullInt32;
            FeedbackType = type;
            Status = FeedbackStatusFlag.None;
            DateCreated = NullValue.NullDateTime;
            DateModified = NullValue.NullDateTime;
            Author = string.Empty;
        }

        /// <summary>
        /// Gets or sets the blog id for this feedback item.
        /// You can usually get this via the entry, but not 
        /// for a comment left in the contact page.
        /// </summary>
        /// <value>The blog id.</value>
        public int BlogId { get; set; }

        /// <summary>
        /// Gets or sets the parent entry ID. Feedback must be associated with an entry, 
        /// except for Contact page inquiries.
        /// </summary>
        /// <value>The parent ID.</value>
        public int EntryId { get; set; }

        /// <summary>
        /// The Entry.
        /// </summary>
        public Entry Entry
        {
            get
            {
                if(_entry == null && EntryId != NullValue.NullInt32)
                {
                    if(!String.IsNullOrEmpty(_parentEntryName))
                    {
                        _entry = new Entry(PostType.BlogPost)
                        {
                            Id = EntryId,
                            EntryName = _parentEntryName,
                            DateCreated = _parentDateCreated,
                            DateSyndicated = _parentDateSyndicated
                        };
                    }
                }
                return _entry;
            }
            set
            {
                _entry = value;
                if(value != null)
                {
                    EntryId = value.Id;
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of the post.
        /// </summary>
        /// <value>The type of the post.</value>
        public FeedbackType FeedbackType { get; set; }

        /// <summary>
        /// Gets or sets the status of this feedback item.
        /// </summary>
        /// <value>The type of the post.</value>
        public FeedbackStatusFlag Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feedback was created via the CommentAPI.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [created via comment API]; otherwise, <c>false</c>.
        /// </value>
        public bool CreatedViaCommentApi { get; set; }

        /// <summary>
        /// Gets or sets the title of the feedback.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body of the Feedback.  This is the 
        /// main content of the entry.
        /// </summary>
        /// <value></value>
        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                _feedbackChecksumHash = string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the source URL.  For comments, this is the URL 
        /// to the comment form used if any. For trackbacks, this is the 
        /// url of the site making the trackback.
        /// the 
        /// </summary>
        /// <value>The source URL.</value>
        public Uri SourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the author name of the entry.  
        /// For comments, this is the name given by the commenter. 
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feedback was left by an author of the blog.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this feedback was left by a blog author; otherwise, <c>false</c>.
        /// </value>
        public bool IsBlogAuthor { get; set; }

        public string Email
        {
            get { return _email ?? string.Empty; }
            set { _email = value; }
        }

        public string Referrer
        {
            get { return _referrer ?? string.Empty; }
            set { _referrer = value; }
        }

        public IPAddress IpAddress { get; set; }

        public string UserAgent
        {
            get { return _userAgent ?? string.Empty; }
            set { _userAgent = value; }
        }

        /// <summary>
        /// Gets or sets the date this item was created.
        /// </summary>
        /// <value></value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date this entry was last updated.
        /// </summary>
        /// <value></value>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feedback is approved for display.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
        /// </value>
        public bool Approved
        {
            get { return IsStatusSet(FeedbackStatusFlag.Approved); }
            set { SetStatus(FeedbackStatusFlag.Approved, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this feedback is approved for display.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
        /// </value>
        public bool FlaggedAsSpam
        {
            get { return IsStatusSet(FeedbackStatusFlag.FlaggedAsSpam); }
            set { SetStatus(FeedbackStatusFlag.FlaggedAsSpam, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this feedback is approved for display.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
        /// </value>
        public bool ConfirmedSpam
        {
            get { return IsStatusSet(FeedbackStatusFlag.ConfirmedSpam); }
            set { SetStatus(FeedbackStatusFlag.ConfirmedSpam, value); }
        }

        /// <summary>
        /// Whether or not this entry needs moderator approval.
        /// </summary>
        public bool NeedsModeratorApproval
        {
            get { return FeedbackStatusFlag.NeedsModeration == Status; }
            set { SetStatus(FeedbackStatusFlag.NeedsModeration, value); }
        }

        /// <summary>
        /// Whether or not this entry is deleted (ie in the trash can).
        /// </summary>
        public bool Deleted
        {
            get { return IsStatusSet(FeedbackStatusFlag.Deleted); }
            set { SetStatus(FeedbackStatusFlag.Deleted, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this comment was approved by moderator.  
        /// </summary>
        /// <remarks>
        /// If ApprovedByModerator is true, then Approved must also be true.
        /// </remarks>
        /// <value><c>true</c> if [approved by moderator]; otherwise, <c>false</c>.</value>
        public bool ApprovedByModerator
        {
            get { return IsStatusSet(FeedbackStatusFlag.ApprovedByModerator); }
        }

        /// <summary>
        /// This is a checksum of the entry text combined with 
        /// a hash of the text like so "####.HASH". 
        /// </summary>
        /// <value></value>
        public string ChecksumHash
        {
            get
            {
                if(String.IsNullOrEmpty(_feedbackChecksumHash))
                {
                    _feedbackChecksumHash = string.Format("{0}.{1}", CalculateChecksum(Body), SecurityHelper.HashPassword(Body));
                }
                return _feedbackChecksumHash;
            }
            set { _feedbackChecksumHash = value; }
        }

        /// <summary>
        /// Gets or sets the name of the parent entry.
        /// </summary>
        /// <value>The name of the parent entry.</value>
        public string ParentEntryName
        {
            get
            {
                if(_parentEntryName == null)
                {
                    _parentEntryName = Entry != null ? Entry.EntryName : string.Empty;
                }
                return _parentEntryName;
            }
            set { _parentEntryName = value; }
        }

        /// <summary>
        /// Gets or sets the parent entry date created.
        /// </summary>
        /// <value>The parent date created.</value>
        public DateTime ParentDateCreated
        {
            get
            {
                if(_parentDateCreated == NullValue.NullDateTime)
                {
                    _parentDateCreated = Entry != null ? Entry.DateCreated : DateTime.MinValue;
                }
                return _parentDateCreated;
            }
            set { _parentDateCreated = value; }
        }

        /// <summary>
        /// Gets or sets the parent entry date created.
        /// </summary>
        /// <value>The parent date created.</value>
        public DateTime ParentDateSyndicated
        {
            get
            {
                if(_parentDateSyndicated == NullValue.NullDateTime)
                {
                    _parentDateSyndicated = Entry != null ? Entry.DateSyndicated : DateTime.MinValue;
                }
                return _parentDateSyndicated;
            }
            set { _parentDateSyndicated = value; }
        }

        DateTime _parentDateSyndicated;

        /// <summary>
        /// Gets or sets the ID for this feedback item.
        /// </summary>
        /// <value>The feedback ID.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets the specified feedback by id.
        /// </summary>
        /// <param name="feedbackId">The feedback id.</param>
        /// <returns></returns>
        public static FeedbackItem Get(int feedbackId)
        {
            return ObjectProvider.Instance().GetFeedback(feedbackId);
        }

        /// <summary>
        /// Gets the feedback counts for the various top level statuses.
        /// </summary>
        public static FeedbackCounts GetFeedbackCounts()
        {
            FeedbackCounts counts;
            ObjectProvider.Instance().GetFeedbackCounts(out counts.ApprovedCount, out counts.NeedsModerationCount,
                                                        out counts.FlaggedAsSpamCount, out counts.DeletedCount);
            return counts;
        }

        /// <summary>
        /// Returns the itemCount most recent active comments.
        /// </summary>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public static ICollection<FeedbackItem> GetRecentComments(int itemCount)
        {
            return ObjectProvider.Instance().GetPagedFeedback(0, itemCount, FeedbackStatusFlag.Approved,
                                                              FeedbackStatusFlag.None, FeedbackType.Comment);
        }

        /// <summary>
        /// Updates the specified entry in the data provider.
        /// </summary>
        /// <param name="feedbackItem">Entry.</param>
        /// <returns></returns>
        public static bool Update(FeedbackItem feedbackItem)
        {
            if(feedbackItem == null)
            {
                throw new ArgumentNullException("feedbackItem");
            }

            feedbackItem.DateModified = Config.CurrentBlog.TimeZone.Now;
            return ObjectProvider.Instance().Update(feedbackItem);
        }

        /// <summary>
        /// Approves the comment, and removes it from the SPAM folder or from the 
        /// Trash folder.
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="spamService"></param>
        /// <returns></returns>
        public static void Approve(FeedbackItem feedback, ICommentSpamService spamService)
        {
            if(feedback == null)
            {
                throw new ArgumentNullException("feedback");
            }

            feedback.SetStatus(FeedbackStatusFlag.Approved, true);
            feedback.SetStatus(FeedbackStatusFlag.Deleted, false);
            if(spamService != null)
            {
                spamService.SubmitGoodFeedback(feedback);
            }

            Update(feedback);
        }

        /// <summary>
        /// Confirms the feedback as spam and moves it to the trash.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        /// <param name="spamService"></param>
        public static void ConfirmSpam(FeedbackItem feedback, ICommentSpamService spamService)
        {
            if(feedback == null)
            {
                throw new ArgumentNullException("feedback");
            }

            feedback.SetStatus(FeedbackStatusFlag.Approved, false);
            feedback.SetStatus(FeedbackStatusFlag.ConfirmedSpam, true);

            if(spamService != null)
            {
                spamService.SubmitGoodFeedback(feedback);
            }

            Update(feedback);
        }

        /// <summary>
        /// Confirms the feedback as spam and moves it to the trash.
        /// </summary>
        /// <param name="feedback">The feedback.</param>
        public static void Delete(FeedbackItem feedback)
        {
            if(feedback == null)
            {
                throw new ArgumentNullException("feedback");
            }

            feedback.SetStatus(FeedbackStatusFlag.Approved, false);
            feedback.SetStatus(FeedbackStatusFlag.Deleted, true);

            Update(feedback);
        }


        /// <summary>
        /// Destroys all non-active emails that meet the status.
        /// </summary>
        /// <param name="feedbackStatus">The feedback.</param>
        public static void Destroy(FeedbackStatusFlag feedbackStatus)
        {
            if((feedbackStatus & FeedbackStatusFlag.Approved) == FeedbackStatusFlag.Approved)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_DestroyActiveComment);
            }

            ObjectProvider.Instance().DestroyFeedback(feedbackStatus);
        }

        /// <summary>
        /// Checks to see if the specified status bit is set.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        protected bool IsStatusSet(FeedbackStatusFlag status)
        {
            return (Status & status) == status;
        }

        /// <summary>
        /// Turns the specified status bit on or off depending on the setOn value.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="setOn"></param>
        protected void SetStatus(FeedbackStatusFlag status, bool setOn)
        {
            if(setOn)
            {
                Status = Status | status;
            }
            else
            {
                Status = Status & ~status;
            }
        }

        /// <summary>
        /// Calculates a simple checksum of the specified text.  
        /// This is used for comment filtering purposes. 
        /// Once deployed, this algorithm shouldn't change.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns></returns>
        public static int CalculateChecksum(string text)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }
            int checksum = 0;
            foreach(char c in text)
            {
                checksum += c;
            }
            return checksum;
        }
    }

    public struct FeedbackCounts
    {
        public int ApprovedCount;
        public int DeletedCount;
        public int FlaggedAsSpamCount;
        public int NeedsModerationCount;
    }

    /// <summary>
    /// Specifies the current status of a piece of feedback.
    /// </summary>
    [Flags]
    public enum FeedbackStatusFlag
    {
        None = 0,
        Approved = 1,
        NeedsModeration = 2,
        ApprovedByModerator = Approved | NeedsModeration,
        FlaggedAsSpam = 4,
        FalsePositive = FlaggedAsSpam | Approved,
        Deleted = 8,
        ConfirmedSpam = FlaggedAsSpam | Deleted,
    }
}