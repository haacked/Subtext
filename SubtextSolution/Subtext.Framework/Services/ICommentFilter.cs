using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
    public interface ICommentFilter
    {
        void FilterAfterPersist(FeedbackItem feedbackItem);
        void FilterBeforePersist(FeedbackItem feedback);
    }
}