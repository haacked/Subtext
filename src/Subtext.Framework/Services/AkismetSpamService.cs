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
using System.Globalization;
using System.Net;
using log4net;
using Subtext.Akismet;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;
using Subtext.Framework.Routing;
using Subtext.Framework.Web;

namespace Subtext.Framework.Services
{
    [Serializable]
    public class AkismetSpamService : ICommentSpamService
    {
        private readonly static ILog Log = new Log();
        readonly AkismetClient _akismet;
        readonly Blog _blog;
        readonly BlogUrlHelper _urlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AkismetSpamService"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="blog">The blog.</param>
        public AkismetSpamService(string apiKey, Blog blog)
            : this(apiKey, blog, null, null)
        {
        }

        public AkismetSpamService(string apiKey, Blog blog, AkismetClient akismetClient, BlogUrlHelper urlHelper)
        {
            _blog = blog;
            _akismet = akismetClient ?? new AkismetClient(apiKey, urlHelper.BlogUrl().ToFullyQualifiedUrl(blog));
            IWebProxy proxy = HttpHelper.GetProxy();
            if (proxy != null)
            {
                _akismet.Proxy = proxy;
            }
            _urlHelper = urlHelper ?? new BlogUrlHelper(null, null);
        }

        /// <summary>
        /// Examines the item and determines whether or not it is spam.
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public bool IsSpam(FeedbackItem feedback)
        {
            Comment comment = ConvertToAkismetItem(feedback);

            try
            {
                if (_akismet.CheckCommentForSpam(comment))
                {
                    _akismet.SubmitSpam(comment);
                    return true;
                }
            }
            catch (InvalidResponseException e)
            {
                Log.Error(e.Message, e);
            }
            return false;
        }

        /// <summary>
        /// Submits the item to the service as a false positive. 
        /// Something that should not have been marked as spam.
        /// </summary>
        /// <param name="feedback"></param>
        public void SubmitGoodFeedback(FeedbackItem feedback)
        {
            Comment comment = ConvertToAkismetItem(feedback);
            _akismet.SubmitHam(comment);
        }

        /// <summary>
        /// Submits the item to the service as a piece of SPAM that got through 
        /// the filter. Something that should've been marked as SPAM.
        /// </summary>
        /// <param name="feedback"></param>
        public void SubmitSpam(FeedbackItem feedback)
        {
            Comment comment = ConvertToAkismetItem(feedback);
            _akismet.SubmitSpam(comment);
        }

        /// <summary>
        /// Verifies the api key.
        /// </summary>
        /// <returns></returns>
        public bool VerifyApiKey()
        {
            try
            {
                return _akismet.VerifyApiKey();
            }
            catch (WebException e)
            {
                Log.Error("Error occured while verifying Akismet.", e);
                return false;
            }
        }

        public Comment ConvertToAkismetItem(FeedbackItem feedback)
        {
            var comment = new Comment(feedback.IpAddress, feedback.UserAgent) { Author = feedback.Author ?? string.Empty, AuthorEmail = feedback.Email };
            if (feedback.SourceUrl != null)
            {
                comment.AuthorUrl = feedback.SourceUrl;
            }
            comment.Content = feedback.Body;
            comment.Referrer = feedback.Referrer;

            var feedbackUrl = _urlHelper.FeedbackUrl(feedback);
            if (feedbackUrl != null)
            {
                Uri permalink = feedbackUrl.ToFullyQualifiedUrl(_blog);
                if (permalink != null)
                {
                    comment.Permalink = permalink;
                }
            }

            comment.CommentType = feedback.FeedbackType.ToString().ToLower(CultureInfo.InvariantCulture);
            return comment;
        }
    }
}