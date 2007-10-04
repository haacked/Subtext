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
using System.Globalization;
using System.Net;
using System.Web;
using log4net;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Threading;
using Subtext.Framework.Web;
using Subtext.Framework.Security;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for Entry.
	/// </summary>
	[Serializable]
	public class FeedbackItem : IIdentifiable
	{
		private readonly static ILog log = new Log();

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
			ObjectProvider.Instance().GetFeedbackCounts(out counts.ApprovedCount, out counts.NeedsModerationCount, out counts.FlaggedAsSpamCount, out counts.DeletedCount);
			return counts;
		}

		/// <summary>
		/// Returns a pageable collection of comments.
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="status">A flag for the status types to return.</param>
		/// <param name="type">The type of feedback to return.</param>
		/// <returns></returns>
		public static IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlags status, FeedbackType type)
		{
			return ObjectProvider.Instance().GetPagedFeedback(pageIndex, pageSize, status, FeedbackStatusFlags.None, type);
		}

		/// <summary>
		/// Returns a pageable collection of comments.
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="status">A flag for the status types to return.</param>
		/// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
		/// <param name="type">The type of feedback to return.</param>
		/// <returns></returns>
		public static IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlags status, FeedbackStatusFlags excludeStatusMask, FeedbackType type)
		{
			return ObjectProvider.Instance().GetPagedFeedback(pageIndex, pageSize, status, excludeStatusMask, type);
		}

		/// <summary>
		/// Creates a feedback item in the database.
		/// </summary>
		/// <param name="feedback">The feedback.</param>
		/// <param name="filter">Spam filter.</param>
		/// <returns></returns>
		public static int Create(FeedbackItem feedback, CommentFilter filter)
		{
			if (HttpContext.Current != null && HttpContext.Current.Request != null)
			{
				feedback.UserAgent = HttpContext.Current.Request.UserAgent;
				feedback.IpAddress = HttpHelper.GetUserIpAddress(HttpContext.Current);
			}
			
			feedback.FlaggedAsSpam = true; //We're going to start with this assumption.
			feedback.Author = HtmlHelper.SafeFormat(feedback.Author);
			feedback.Body = HtmlHelper.ConvertUrlsToHyperLinks(HtmlHelper.ConvertToAllowedHtml(feedback.Body));
			feedback.Title = HtmlHelper.SafeFormat(feedback.Title);
		    
		    // If we are creating this feedback item as part of an import, we want to 
		    // be sure to use the item's datetime, and not set it to the current time.
            if (NullValue.NullDateTime.Equals(feedback.DateCreated))
            {
                feedback.DateCreated = Config.CurrentBlog.TimeZone.Now;
                feedback.DateModified = feedback.DateCreated;
            }
            else if (NullValue.NullDateTime.Equals(feedback.DateModified))
            {
                feedback.DateModified = feedback.DateCreated;
            }
            
            if (filter != null)
                filter.FilterBeforePersist(feedback);

            feedback.Id = ObjectProvider.Instance().CreateFeedback(feedback);

            if (filter != null)
                CommentFilter.FilterAfterPersist(feedback);

            // if it's not the administrator commenting and it's not a trackback and notification is enabled.
            if (!SecurityHelper.IsAdmin && !String.IsNullOrEmpty(Config.CurrentBlog.Owner.Email) && 
                feedback.FeedbackType != Extensibility.FeedbackType.PingTrack && Config.CurrentBlog.CommentNoficationEnabled)
            {
                //In order to make this async, we need to pass the HttpContext.Current 
                //several layers deep. Instead, we should create our own context.
                EmailCommentToAdmin(feedback, Config.CurrentBlog);
            }

            // if it's a trackback and notification is enabled.
            if (!String.IsNullOrEmpty(Config.CurrentBlog.Owner.Email) &&
                feedback.FeedbackType == Extensibility.FeedbackType.PingTrack && Config.CurrentBlog.TrackbackNoficationEnabled)
            {
                //In order to make this async, we need to pass the HttpContext.Current 
                //several layers deep. Instead, we should create our own context.
                EmailCommentToAdmin(feedback, Config.CurrentBlog);
            }
            
            return feedback.Id;
        }

		/// <summary>
		/// Returns the itemCount most recent active comments.
		/// </summary>
		/// <param name="itemCount"></param>
		/// <returns></returns>
		public static IList<FeedbackItem> GetRecentComments(int itemCount)
		{
			return ObjectProvider.Instance().GetPagedFeedback(0, itemCount, FeedbackStatusFlags.Approved, FeedbackStatusFlags.None, FeedbackType.Comment);
		}

		/// <summary>
		/// Updates the specified entry in the data provider.
		/// </summary>
		/// <param name="feedbackItem">Entry.</param>
		/// <returns></returns>
		public static void Update(FeedbackItem feedbackItem)
		{
			if (feedbackItem == null)
				throw new ArgumentNullException("feedbackItem", Resources.ArgumentNull_Generic);

			feedbackItem.DateModified = Config.CurrentBlog.TimeZone.Now;
			ObjectProvider.Instance().Update(feedbackItem);
		}

		private static void EmailCommentToAdmin(FeedbackItem comment, BlogInfo currentBlog)
		{
			string blogTitle = currentBlog.Title;

			// create and format an email to the site admin with comment details
			EmailProvider im = EmailProvider.Instance();

			string fromEmail = comment.Email;
			if (String.IsNullOrEmpty(fromEmail))
				fromEmail = null;

			string to = currentBlog.Owner.Email;
			string from = im.AdminEmail;
            		string subject = String.Format(CultureInfo.InvariantCulture, "{2}: {0} (via {1})", comment.Title, blogTitle, comment.FeedbackType == FeedbackType.Comment ? "Comment" : "Trackback/Pingback");
            		if (comment.FlaggedAsSpam)
                		subject = "[SPAM Flagged] " + subject;

           		string commenterUrl = "none given";
			if(comment.SourceUrl != null)
				commenterUrl = comment.SourceUrl.ToString();
			

			string bodyFormat = "{8}{7} from {0}" + Environment.NewLine

								+ "----------------------------------------------------" + Environment.NewLine
								+ "From:\t{1} <{2}>" + Environment.NewLine
								+ "Url:\t{3}" + Environment.NewLine
								+ "IP:\t{4}" + Environment.NewLine
								+ "====================================================" + Environment.NewLine + Environment.NewLine
								+ "{5}" + Environment.NewLine + Environment.NewLine
								+ "Source: {6}";

			string body = string.Format(CultureInfo.InvariantCulture, bodyFormat,
										blogTitle,
										comment.Author,
										fromEmail ?? "no email given",
										commenterUrl,
										comment.IpAddress,
				// we're sending plain text email by default, but body includes <br />s for crlf
										comment.Body.Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;", Environment.NewLine),

										currentBlog.UrlFormats.FeedbackFullyQualifiedUrl(comment.EntryId, comment.parentEntryName, comment.ParentDateCreated, comment),
                                        comment.FeedbackType == FeedbackType.Comment ? "Comment" : "Trackback/Pingback",
					comment.FlaggedAsSpam ? "Spam Flagged " : string.Empty);

			try
			{
				SendEmailDelegate sendEmail = im.Send;
				AsyncHelper.FireAndForget(sendEmail, to, from, fromEmail, subject, body);
			}
			catch(Exception e)
			{
				log.Warn("Could not email comment to admin", e);
			}
		}

		delegate bool SendEmailDelegate(string toAddress, string fromAddress, string replyTo, string subject, string body);
	
		/// <summary>
		/// Approves the comment, and removes it from the SPAM folder or from the 
		/// Trash folder.
		/// </summary>
		/// <param name="feedback"></param>
		/// <returns></returns>
		public static void Approve(FeedbackItem feedback)
		{
			if (feedback == null)
				throw new ArgumentNullException("feedback", Resources.ArgumentNull_Generic);

			feedback.SetStatus(FeedbackStatusFlags.Approved, true);
			feedback.SetStatus(FeedbackStatusFlags.Deleted, false);
			if(Config.CurrentBlog.FeedbackSpamService != null)
			{
				Config.CurrentBlog.FeedbackSpamService.SubmitGoodFeedback(feedback);
			}

			Update(feedback);
		}

		/// <summary>
		/// Confirms the feedback as spam and moves it to the trash.
		/// </summary>
		/// <param name="feedback">The feedback.</param>
		public static void ConfirmSpam(FeedbackItem feedback)
		{
			if (feedback == null)
				throw new ArgumentNullException("feedback", Resources.ArgumentNull_Generic);

			feedback.SetStatus(FeedbackStatusFlags.Approved, false);
			feedback.SetStatus(FeedbackStatusFlags.ConfirmedSpam, true);

			if (Config.CurrentBlog.FeedbackSpamService != null)
			{
				Config.CurrentBlog.FeedbackSpamService.SubmitGoodFeedback(feedback);
			}

			Update(feedback);
		}

		/// <summary>
		/// Confirms the feedback as spam and moves it to the trash.
		/// </summary>
		/// <param name="feedback">The feedback.</param>
		public static void Delete(FeedbackItem feedback)
		{
			if (feedback == null)
				throw new ArgumentNullException("feedback", Resources.ArgumentNull_Generic);

			feedback.SetStatus(FeedbackStatusFlags.Approved, false);
			feedback.SetStatus(FeedbackStatusFlags.Deleted, true);

			Update(feedback);
		}

		/// <summary>
		/// Confirms the feedback as spam and moves it to the trash.
		/// </summary>
		/// <param name="feedback">The feedback.</param>
		public static void Destroy(FeedbackItem feedback)
		{
			if (feedback == null)
				throw new ArgumentNullException("feedback", Resources.ArgumentNull_Generic);

			if (feedback.Approved)
				throw new InvalidOperationException(Resources.InvalidOperation_DestroyApprovedComment);
			
			ObjectProvider.Instance().DestroyFeedback(feedback.Id);
		}

		/// <summary>
		/// Destroys all non-active emails that meet the status.
		/// </summary>
		/// <param name="feedbackStatus">The feedback.</param>
		public static void Destroy(FeedbackStatusFlags feedbackStatus)
		{
			if ((feedbackStatus & FeedbackStatusFlags.Approved) == FeedbackStatusFlags.Approved)
				throw new InvalidOperationException(Resources.InvalidOperation_DestroyActiveComment);

			ObjectProvider.Instance().DestroyFeedback(feedbackStatus);
		}

		/// <summary>
		/// Creates a new <see cref="FeedbackItem"/> instance.
		/// </summary>
		/// <param name="type">Ptype.</param>
		public FeedbackItem(FeedbackType type)
		{
			this.feedbackType = type;
		}

		/// <summary>
		/// Gets or sets the ID for this feedback item.
		/// </summary>
		/// <value>The feedback ID.</value>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}
		private int _id = NullValue.NullInt32;

		/// <summary>
		/// Gets or sets the blog id for this feedback item.
		/// You can usually get this via the entry, but not 
		/// for a comment left in the contact page.
		/// </summary>
		/// <value>The blog id.</value>
		public int BlogId
		{
			get { return this.blogId; }
			set { this.blogId = value; }
		}

		int blogId;
		
		/// <summary>
		/// Gets or sets the parent entry ID. Feedback must be associated with an entry, 
		/// except for Contact page inquiries.
		/// </summary>
		/// <value>The parent ID.</value>
		public int EntryId
		{
			get
			{
				return this._entryId;
			}
			set
			{
				this._entryId = value;
			}
		}
		private int _entryId = NullValue.NullInt32;

		/// <summary>
		/// The Entry.
		/// </summary>
		public Entry Entry
		{
			get
			{
				if (this.entry == null && _entryId != NullValue.NullInt32)
					this.entry = Entries.GetEntry(_entryId, PostConfig.None, false);
				return this.entry;
			}
			set { this.entry = value; }
		}

		private Entry entry;
		
		/// <summary>
		/// Gets or sets the type of the post.
		/// </summary>
		/// <value>The type of the post.</value>
		public FeedbackType FeedbackType
		{
			get
			{
				return this.feedbackType;
			}
			set
			{
				this.feedbackType = value;
			}
		}
		private FeedbackType feedbackType = FeedbackType.None;

		/// <summary>
		/// Gets or sets the status of this feedback item.
		/// </summary>
		/// <value>The type of the post.</value>
		public FeedbackStatusFlags Status
		{
			get
			{
				return this.statusFlag;
			}
			set
			{
				this.statusFlag = value;
			}
		}
		private FeedbackStatusFlags statusFlag = FeedbackStatusFlags.None;

		/// <summary>
		/// Gets or sets a value indicating whether this feedback was created via the CommentAPI.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [created via comment API]; otherwise, <c>false</c>.
		/// </value>
		internal bool CreatedViaCommentAPI
		{
			get { return this.createdViaCommentAPI; }
			set { this.createdViaCommentAPI = value; }
		}

		bool createdViaCommentAPI;

		/// <summary>
		/// Gets or sets the title of the feedback.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		private string _title;
		
		/// <summary>
		/// Gets or sets the body of the Feedback.  This is the 
		/// main content of the entry.
		/// </summary>
		/// <value></value>
		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				_body = value;
				this._feedbackChecksumHash = string.Empty;
			}
		}
		private string _body;

		/// <summary>
		/// Gets or sets the source URL.  For comments, this is the URL 
		/// to the comment form used if any. For trackbacks, this is the 
		/// url of the site making the trackback.
		/// the 
		/// </summary>
		/// <value>The source URL.</value>
		public Uri SourceUrl
		{
			get { return _sourceurl; }
			set { _sourceurl = value; }
		}
		private Uri _sourceurl;
	
		/// <summary>
		/// Returns the URL to view the specific comment 
		/// within the blog.
		/// </summary>
		public Uri DisplayUrl
		{
			get
			{
				return Config.CurrentBlog.UrlFormats.FeedbackFullyQualifiedUrl(EntryId, ParentEntryName, ParentDateCreated, this);
			}
		}

		/// <summary>
		/// Gets or sets the author name of the entry.  
		/// For comments, this is the name given by the commenter. 
		/// </summary>
		/// <value>The author.</value>
		public string Author
		{
			get { return _author ?? string.Empty; }
			set { _author = value; }
		}
		private string _author;

		/// <summary>
		/// Gets or sets a value indicating whether this feedback was left by an author of the blog.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this feedback was left by a blog author; otherwise, <c>false</c>.
		/// </value>
		public bool IsBlogAuthor
		{
			get { return this.isBlogAuthor; }
			set { this.isBlogAuthor = value; }
		}

		bool isBlogAuthor;

		public string Email
		{
			get { return _email ?? string.Empty; }
			set { _email = value; }
		}
		private string _email;

		public string Referrer
		{
			get { return this.referrer ?? string.Empty; }
			set { this.referrer = value; }
		}

		string referrer;

		public IPAddress IpAddress
		{
			get { return this.ipAddress; }
			set { this.ipAddress = value; }
		}

		IPAddress ipAddress;

		public string UserAgent
		{
			get { return this.userAgent ?? string.Empty; }
			set { this.userAgent = value; }
		}

		string userAgent;

		private DateTime _datecreated = NullValue.NullDateTime;
		/// <summary>
		/// Gets or sets the date this item was created.
		/// </summary>
		/// <value></value>
		public DateTime DateCreated
		{
			get
			{
				return _datecreated;
			}
			set { _datecreated = value; }
		}

		private DateTime _dateupated = NullValue.NullDateTime;
		/// <summary>
		/// Gets or sets the date this entry was last updated.
		/// </summary>
		/// <value></value>
		public DateTime DateModified
		{
			get
			{
				return _dateupated;
			}
			set { _dateupated = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this feedback is approved for display.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
		/// </value>
		public bool Approved
		{
			get { return IsStatusSet(FeedbackStatusFlags.Approved); }
			set { SetStatus(FeedbackStatusFlags.Approved, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this feedback is approved for display.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
		/// </value>
		public bool FlaggedAsSpam
		{
			get { return IsStatusSet(FeedbackStatusFlags.FlaggedAsSpam); }
			set { SetStatus(FeedbackStatusFlags.FlaggedAsSpam, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this feedback is approved for display.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
		/// </value>
		public bool ConfirmedSpam
		{
			get { return IsStatusSet(FeedbackStatusFlags.ConfirmedSpam); }
			set { SetStatus(FeedbackStatusFlags.ConfirmedSpam, value); }
		}

		/// <summary>
		/// Whether or not this entry needs moderator approval.
		/// </summary>
		public bool NeedsModeratorApproval
		{
			get { return FeedbackStatusFlags.NeedsModeration == statusFlag; }
			set { SetStatus(FeedbackStatusFlags.NeedsModeration, value); }
		}

		/// <summary>
		/// Whether or not this entry is deleted (ie in the trash can).
		/// </summary>
		public bool Deleted
		{
			get { return IsStatusSet(FeedbackStatusFlags.Deleted); }
			set { SetStatus(FeedbackStatusFlags.Deleted, value); }
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
			get { return IsStatusSet(FeedbackStatusFlags.ApprovedByModerator); }
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
				if (String.IsNullOrEmpty(this._feedbackChecksumHash))
				{
					this._feedbackChecksumHash = CalculateChecksum(this.Body) + "." + SecurityHelper.HashPassword(this.Body);
				}
				return this._feedbackChecksumHash;
			}
			set { this._feedbackChecksumHash = value; }
		}

		string _feedbackChecksumHash = string.Empty;

		/// <summary>
		/// Checks to see if the specified status bit is set.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns></returns>
		protected bool IsStatusSet(FeedbackStatusFlags status)
		{
			return (this.Status & status) == status;
		}

		/// <summary>
		/// Turns the specified status bit on or off depending on the setOn value.
		/// </summary>
		/// <param name="status"></param>
		/// <param name="setOn"></param>
		protected void SetStatus(FeedbackStatusFlags status, bool setOn)
		{
			if (setOn)
			{
				this.Status = Status | status;
			}
			else
			{
				this.Status = Status & ~status;
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
			if (text == null)
				throw new ArgumentNullException("text", Resources.ArgumentNull_String);
			int checksum = 0;
			foreach (char c in text)
			{
				checksum += c;
			}
			return checksum;
		}

		/// <summary>
		/// Gets or sets the name of the parent entry.
		/// </summary>
		/// <value>The name of the parent entry.</value>
		public string ParentEntryName
		{
			get
			{
				if (this.parentEntryName == null)
					this.parentEntryName = Entry != null ? Entry.EntryName : string.Empty;
				return this.parentEntryName;
			}
			set { this.parentEntryName = value; }
		}

		string parentEntryName;

		/// <summary>
		/// Gets or sets the parent entry date created.
		/// </summary>
		/// <value>The parent date created.</value>
		public DateTime ParentDateCreated
		{
			get
			{
				if (this.parentDateCreated == NullValue.NullDateTime)
					this.parentDateCreated = Entry != null ? Entry.DateCreated : DateTime.MinValue;
				return this.parentDateCreated;
			}
			set { this.parentDateCreated = value; }
		}

		DateTime parentDateCreated = NullValue.NullDateTime;
	}
}
