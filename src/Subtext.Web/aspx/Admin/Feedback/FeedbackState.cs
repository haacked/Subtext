using System;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;

namespace Subtext.Web.Admin.Feedback
{
    public class FeedbackState
    {
        public bool Approvable = true;
        public string ApproveText = Resources.Label_Approve;
        public bool Deletable = true;
        public string DeleteToolTip = string.Empty;
        public bool Destroyable;
        public bool Emptyable;
        public string EmptyToolTip = string.Empty;
        public string HeaderText = string.Empty;
        public string NoCommentsHtml = Resources.Label_NoApprovedComments;
        public bool Spammable = true;

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
                    throw new InvalidOperationException(String.Format(Resources.InvalidOperation_InvalidFeedbackStatus,
                                                                      status));
            }
        }
    }

    public class ApprovedFeedbackState : FeedbackState
    {
        public ApprovedFeedbackState()
        {
            HeaderText = Resources.Label_Comments;
            Approvable = false;
        }
    }

    public class NeedsModerationState : FeedbackState
    {
        public NeedsModerationState()
        {
            HeaderText = Resources.Label_CommentsPendingModeratorApproval;
            NoCommentsHtml = Resources.Label_NoCommentsNeedModeration;
        }
    }

    public class FlaggedAsSpamState : FeedbackState
    {
        public FlaggedAsSpamState()
        {
            HeaderText = Resources.Label_CommentsFlaggedAsSpam;
            DeleteToolTip = Resources.Label_TrashesSpam;
            Spammable = false;
            Emptyable = true;
            EmptyToolTip = Resources.Label_DestroySpamTooltip;
            NoCommentsHtml = Resources.Label_NoCommentsFlaggedAsSpam;
        }
    }

    public class DeletedState : FeedbackState
    {
        public DeletedState()
        {
            HeaderText = Resources.Label_CommentsInTrash;
            Spammable = false;
            Deletable = false;
            DeleteToolTip = Resources.Label_TrashesSpam;
            Destroyable = true;
            Emptyable = true;
            EmptyToolTip = Resources.Label_DestroyTrashTooltip;
            ApproveText = Resources.Label_Undelete;
            NoCommentsHtml = Resources.Label_NoCommentsInTrash;
        }
    }
}