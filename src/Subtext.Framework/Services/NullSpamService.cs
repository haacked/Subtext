using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
    public class NullSpamService : ICommentSpamService
    {
        public bool IsSpam(FeedbackItem feedback)
        {
            return false;
        }

        public void SubmitGoodFeedback(FeedbackItem feedback)
        {
        }

        public void SubmitSpam(FeedbackItem feedback)
        {
        }
    }
}
