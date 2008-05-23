using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Feedback
{
    public class FeedbackState
    {
        public static FeedbackState GetUiState(FeedbackStatusFlag status)
        {
            //We've reduced multiple switch statements to one, 
            //that's good enough in my book.
            switch (status) 
            { 
                case FeedbackStatusFlag.Approved:
                    return new ApprovedFeedbackState();

                case FeedbackStatusFlag.NeedsModeration:
                    return new NeedsModerationState();

                case FeedbackStatusFlag.FlaggedAsSpam:
                    return new FlaggedAsSpamState();

                case FeedbackStatusFlag.Deleted:
                    return new DeletedState();

                default:
                    throw new InvalidOperationException("Invalid Feedback Status supplied '" + status + "'");
            }    
        }

        public string HeaderText = string.Empty ;
        public bool Approvable = true;
        public string ApproveText = "Approve";
        public bool Destroyable = false;
        public bool Deletable = true;
        public string DeleteToolTip = string.Empty;
        public bool Spammable = true;
        public bool Emptyable = false;
        public string EmptyToolTip = string.Empty;
        public string NoCommentsHtml = "<em>There are no approved comments to display.</em>";
    }

    public class ApprovedFeedbackState : FeedbackState
    {
        public ApprovedFeedbackState()
        {
            HeaderText = "Comments";
            Approvable = false;
        }
    }

    public class NeedsModerationState : FeedbackState
    {
        public NeedsModerationState() : base()
        {
            HeaderText = "Comments Pending Moderator Approval";
            NoCommentsHtml = "<em>No Entries Need Moderation.</em>";
        }
    }

    public class FlaggedAsSpamState : FeedbackState
    {
        public FlaggedAsSpamState()
        {
            HeaderText = "Comments Flagged as SPAM";
            DeleteToolTip = "Trashes checked spam";
            Spammable = false;
            Emptyable = true;
            EmptyToolTip = "Destroy all spam, not just checked";
            NoCommentsHtml = "<em>No Entries Flagged as SPAM.</em>";
        }
    }

    public class DeletedState : FeedbackState
    {
        public DeletedState()
        {
            HeaderText = "Comments In The Trash Bin";
            Spammable = false;
            Deletable = false;
            DeleteToolTip = "Trashes checked spam";
            Destroyable = true;
            Emptyable = true;
            EmptyToolTip = "Destroy all trash, not just checked";
            ApproveText = "Undelete";
            NoCommentsHtml = "<em>No Entries in the Trash.</em>";
        }
    }
}
