using Subtext.Framework.Components;

namespace Subtext.Framework.Email
{
    public interface IEmailService
    {
        void EmailCommentToBlogAuthor(FeedbackItem comment);
    }
}