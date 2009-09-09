using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
    public interface ICommentService
    {
        int Create(FeedbackItem feedback);
    }
}