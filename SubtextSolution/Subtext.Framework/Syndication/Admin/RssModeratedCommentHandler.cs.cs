using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Framework.Components;
using Subtext.Extensibility;
using Subtext.Framework.Web;
using Subtext.Framework.Configuration;
using System.Web;
using Subtext.Framework.Security;

namespace Subtext.Framework.Syndication.Admin
{
	public class RssModeratedCommentHandler:RssCommentHandler
	{
		public RssModeratedCommentHandler():base()
		{
		}

		protected override bool RequiresAdminRole
		{
			get
			{
				return true;
			}
		}
		protected override IList<Subtext.Framework.Components.FeedbackItem> GetFeedEntries()
		{

			BlogInfo blogInfo = Config.CurrentBlog;
			ParentEntry = new Entry(PostType.None);
			ParentEntry.AllowComments = true;
			ParentEntry.Title = "Comments requiring your approval.";
			ParentEntry.Url = "/Admin/Feedback.aspx?status=2";
			
			ParentEntry.Body = "The following items are waiting approval.";// = new Uri(blogInfo.RootUrl + "Admin/Feedback.aspx");
			//ParentEntry.FullyQualifiedUrl
			FeedbackCounts counts = FeedbackItem.GetFeedbackCounts();
			IList<FeedbackItem> moderatedFeedback = FeedbackItem.GetPagedFeedback(0, counts.NeedsModerationCount, FeedbackStatusFlag.NeedsModeration, FeedbackType.None);

			return moderatedFeedback;
		}
		protected override CommentRssWriter GetCommentWriter(IList<FeedbackItem> comments, Entry entry)
		{
			return new ModeratedCommentRssWriter(comments, entry);
		}
		protected override BaseSyndicationWriter SyndicationWriter
		{
			get
			{
				return new ModeratedCommentRssWriter(Comments,ParentEntry);
			}
		}
	}
}
