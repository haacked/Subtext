using System;
using System.Globalization;
using System.Net;
using Subtext.Akismet;
using log4net;
using Subtext.Framework.Components;
using Subtext.Framework.Web;

namespace Subtext.Framework.Services
{
	[Serializable]
	public class AkismetSpamService : IFeedbackSpamService
	{
		private readonly static ILog log = new Subtext.Framework.Logging.Log();
		AkismetClient akismet;

		/// <summary>
		/// Initializes a new instance of the <see cref="AkismetSpamService"/> class.
		/// </summary>
		/// <param name="apiKey">The API key.</param>
		/// <param name="blog">The blog.</param>
		public AkismetSpamService(string apiKey, BlogInfo blog)
		{
			this.akismet = new AkismetClient(apiKey, blog.RootUrl);
			IWebProxy proxy = HttpHelper.GetProxy();
			if(proxy != null)
				this.akismet.Proxy = proxy;
		}

		/// <summary>
		/// Verifies the api key.
		/// </summary>
		/// <returns></returns>
		public bool VerifyApiKey()
		{
			try
			{
				return this.akismet.VerifyApiKey();
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
				
				if (this.akismet.CheckCommentForSpam(comment))
				{
					this.akismet.SubmitSpam(comment);
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
			this.akismet.SubmitHam(comment);
		}

		/// <summary>
		/// Submits the item to the service as a piece of SPAM that got through 
		/// the filter. Something that should've been marked as SPAM.
		/// </summary>
		/// <param name="feedback"></param>
		public void SubmitSpam(FeedbackItem feedback)
		{
			Comment comment = ConvertToAkismetItem(feedback);
			this.akismet.SubmitSpam(comment);
		}
		
		private Comment ConvertToAkismetItem(FeedbackItem feedback)
		{
			Comment comment = new Comment(feedback.IpAddress, feedback.UserAgent);
			comment.Author = feedback.Author;
			comment.AuthorEmail = feedback.Email;
			if (feedback.SourceUrl != null)
				comment.AuthorUrl = feedback.SourceUrl;
			comment.Content = feedback.Body;
			comment.Referer = feedback.Referrer;
			if (feedback.DisplayUrl != null)
				comment.Permalink = feedback.DisplayUrl;

			comment.CommentType = feedback.FeedbackType.ToString().ToLower(CultureInfo.InvariantCulture);
			return comment;
		}
	}
}
