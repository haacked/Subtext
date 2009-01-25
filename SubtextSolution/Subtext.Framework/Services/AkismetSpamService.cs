#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
using Subtext.Framework.Routing;
using Subtext.Framework.Web;

namespace Subtext.Framework.Services
{
	[Serializable]
	public class AkismetSpamService : IFeedbackSpamService
	{
		private readonly static ILog log = new Subtext.Framework.Logging.Log();
		AkismetClient _akismet;
        UrlHelper _urlHelper;
        Blog _blog;

		/// <summary>
		/// Initializes a new instance of the <see cref="AkismetSpamService"/> class.
		/// </summary>
		/// <param name="apiKey">The API key.</param>
		/// <param name="blog">The blog.</param>
		public AkismetSpamService(string apiKey, Blog blog) : this(apiKey, blog, null, null)
		{
		}

        public AkismetSpamService(string apiKey, Blog blog, AkismetClient akismetClient, UrlHelper urlHelper) {
            _blog = blog;
            _akismet = akismetClient ?? new AkismetClient(apiKey, urlHelper.BlogUrl().ToFullyQualifiedUrl(blog));
            IWebProxy proxy = HttpHelper.GetProxy();
            if (proxy != null) {
                _akismet.Proxy = proxy;
            }
            _urlHelper = urlHelper ?? new UrlHelper(null, null);
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
				log.Error("Error occured while verifying Akismet.", e);
				return false;
			}
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
			catch(InvalidResponseException e)
			{
				log.Error(e.Message, e);
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
		
		private Comment ConvertToAkismetItem(FeedbackItem feedback)
		{
			Comment comment = new Comment(feedback.IpAddress, feedback.UserAgent);
			comment.Author = feedback.Author ?? string.Empty;
			comment.AuthorEmail = feedback.Email;
			if (feedback.SourceUrl != null)
				comment.AuthorUrl = feedback.SourceUrl;
			comment.Content = feedback.Body;
			comment.Referer = feedback.Referrer;

            Uri permalink = _urlHelper.FeedbackUrl(feedback).ToFullyQualifiedUrl(_blog);
            if (permalink != null)
				comment.Permalink = permalink;

			comment.CommentType = feedback.FeedbackType.ToString().ToLower(CultureInfo.InvariantCulture);
			return comment;
		}
	}
}
