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
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI.ViewModels
{
    public class CommentViewModel : IIdentifiable
    {
        public CommentViewModel(FeedbackItem feedbackItem, ISubtextContext context)
        {
            Feedback = feedbackItem;
            SubtextContext = context;
        }

        protected FeedbackItem Feedback { get; private set; }

        protected BlogUrlHelper UrlHelper
        {
            get { return SubtextContext.UrlHelper; }
        }

        protected ISubtextContext SubtextContext { get; private set; }

        public int Id
        {
            get
            {
                return Feedback.Id;
            }
        }

        public string Title
        {
            get
            {
                return Feedback.Title;
            }
        }

        public string Author
        {
            get { return Feedback.Author; }
        }

        public string Body
        {
            get { return Feedback.Body; }
        }

        public string Email
        {
            get { return Feedback.Email; }
        }

        public bool IsBlogAuthor
        {
            get
            {
                return Feedback.IsBlogAuthor;
            }
        }

        public string DisplayUrl
        {
            get
            {
                return UrlHelper.FeedbackUrl(Feedback);
            }
        }

        public DateTime DateCreated
        {
            get
            {
                return Feedback.DateCreatedUtc;
            }

        }

        public DateTime DateModified
        {
            get
            {
                return Feedback.DateModifiedUtc;
            }

        }
    }
}
