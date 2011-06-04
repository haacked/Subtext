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
using System.Web;
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

        protected ISubtextContext SubtextContext { get; private set; }

        protected ObjectRepository Repository
        {
            get { return SubtextContext.Repository; }
        }

        protected ICommentFilter Filter { get; private set; }

        public int Create(FeedbackItem comment, bool runFilters)
        {
            Entry entry = Cacher.GetEntry(comment.EntryId, SubtextContext);
            if (entry != null && entry.CommentingClosed)
            {
                return NullValue.NullInt32;
            }

            ISubtextContext context = SubtextContext;
            HttpContextBase httpContext = context.HttpContext;

            if (httpContext != null && httpContext.Request != null)
            {
                comment.UserAgent = httpContext.Request.UserAgent;
                comment.IpAddress = HttpHelper.GetUserIpAddress(httpContext);
            }

            if (runFilters)
            {
                comment.FlaggedAsSpam = true; //We're going to start with this assumption.
            }
            comment.Author = HtmlHelper.SafeFormat(comment.Author, context.HttpContext.Server);
            comment.Body = HtmlHelper.ConvertUrlsToHyperLinks(HtmlHelper.ConvertToAllowedHtml(comment.Body));
            comment.Title = HtmlHelper.SafeFormat(comment.Title, context.HttpContext.Server);
            comment.Entry = entry;
            comment.DateCreatedUtc = comment.DateCreatedUtc.IsNull() ? DateTime.UtcNow : comment.DateCreatedUtc;
            comment.DateModifiedUtc = comment.DateModifiedUtc.IsNull() ? DateTime.UtcNow : comment.DateModifiedUtc;

            if (runFilters)
            {
                OnBeforeCreate(comment);
            }

            comment.Id = Repository.Create(comment);

            if (runFilters)
            {
                OnAfterCreate(comment);
            }

            return comment.Id;
        }

        protected virtual void OnBeforeCreate(FeedbackItem feedback)
        {
            if (Filter != null)
            {
                Filter.FilterBeforePersist(feedback);
            }
        }

        protected virtual void OnAfterCreate(FeedbackItem feedback)
        {
            if (Filter != null)
            {
                Filter.FilterAfterPersist(feedback);
            }
        }

        public FeedbackItem Get(int id)
        {
            return Repository.GetFeedback(id);
        }

        public void UpdateStatus(FeedbackItem comment, FeedbackStatusFlag status)
        {
            comment.Status = status;
            Repository.Update(comment);
        }

        public void Destroy(int id)
        {
            Repository.DestroyFeedback(id);
        }
    }
}