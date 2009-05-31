using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.Framework.Services
{
    public class CommentService : ICommentService
    {
        public CommentService(ISubtextContext context, ICommentFilter filter)
        {
            SubtextContext = context;
            Filter = filter;
        }

        protected ISubtextContext SubtextContext {
            get;
            private set;
        }

        protected ObjectProvider Repository {
            get {
                return SubtextContext.Repository;
            }
        }

        protected ICommentFilter Filter
        {
            get;
            private set;
        }

        public int Create(FeedbackItem comment) {
            //TODO: Make this a comment filter.
            var entry = Cacher.GetEntry(comment.EntryId, CacheDuration.Medium, SubtextContext);
            if (entry == null || entry.CommentingClosed) {
                return NullValue.NullInt32;
            }

            var context = SubtextContext;
            var httpContext = context.HttpContext;

            if (httpContext != null && httpContext.Request != null) {
                comment.UserAgent = httpContext.Request.UserAgent;
                comment.IpAddress = HttpHelper.GetUserIpAddress(httpContext);
            }

            comment.FlaggedAsSpam = true; //We're going to start with this assumption.
            comment.Author = HtmlHelper.SafeFormat(comment.Author, context.HttpContext.Server);
            comment.Body = HtmlHelper.ConvertUrlsToHyperLinks(HtmlHelper.ConvertToAllowedHtml(comment.Body));
            comment.Title = HtmlHelper.SafeFormat(comment.Title, context.HttpContext.Server);

            // If we are creating this feedback item as part of an import, we want to 
            // be sure to use the item's datetime, and not set it to the current time.
            if (NullValue.NullDateTime.Equals(comment.DateCreated))
            {
                comment.DateCreated = context.Blog.TimeZone.Now;
                comment.DateModified = comment.DateCreated;
            }
            else if (NullValue.NullDateTime.Equals(comment.DateModified))
            {
                comment.DateModified = comment.DateCreated;
            }

            OnBeforeCreate(comment);

            comment.Id = Repository.Create(comment);

            OnAfterCreate(comment);

            return comment.Id;
        }

        protected virtual void OnBeforeCreate(FeedbackItem feedback) {
            if (Filter != null) {
                Filter.FilterBeforePersist(feedback);
            }
        }

        protected virtual void OnAfterCreate(FeedbackItem feedback) {
            if (Filter != null) {
                Filter.FilterAfterPersist(feedback);
            }
        }
    }
}
