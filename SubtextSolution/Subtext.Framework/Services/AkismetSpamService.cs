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
using Subkismet;
using log4net;
using Subtext.Framework.Components;

namespace Subtext.Framework.Services
{
	[Serializable]
	public class AkismetSpamService : Akismet<FeedbackItem>, IFeedbackSpamService
	{
		private readonly static ILog log = new Logging.Log();

		/// <summary>
		/// Initializes a new instance of the <see cref="AkismetSpamService"/> class.
		/// </summary>
		/// <param name="apiKey">The API key.</param>
		/// <param name="blog">The blog.</param>
		public AkismetSpamService(string apiKey, IBlogInfo blog) : this(new AkismetClient(apiKey, blog.RootUrl))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AkismetSpamService"/> class.
		/// </summary>
		/// <param name="akismetClient">The akismet client.</param>
		public AkismetSpamService(IAkismetClient akismetClient)
			: base(akismetClient, delegate(FeedbackItem item) { return ConvertToAkismetItem(item); })
		{
		}

		/// <summary>
		/// Verifies the api key.
		/// </summary>
		/// <returns></returns>
		public override bool VerifyApiKey()
		{
			try
			{
				return base.VerifyApiKey();
			}
			catch (WebException e)
			{
				log.Error("Unexpected error occured while verifying Akismet.", e);
				return false;
			}
		}

		/// <summary>
		/// Examines the item and determines whether or not it is spam.
		/// </summary>
		/// <param name="feedback"></param>
		/// <returns></returns>
		public override bool IsSpam(FeedbackItem feedback)
		{
			try
			{
				return base.IsSpam(feedback);
			}
			catch (InvalidResponseException e)
			{
				log.Error(e.Message, e);
			}
			return false;
		}

		private static Comment ConvertToAkismetItem(FeedbackItem feedback)
		{
			Comment comment = new Comment(feedback.IpAddress, feedback.UserAgent);
			comment.Author = feedback.Author;
			comment.AuthorEmail = feedback.Email;
			if (feedback.SourceUrl != null)
			{
				comment.AuthorUrl = feedback.SourceUrl;
			}
			comment.Content = feedback.Body;
			comment.Referrer = feedback.Referrer;
			if (feedback.DisplayUrl != null)
			{
				comment.Permalink = feedback.DisplayUrl;
			}

			comment.CommentType = feedback.FeedbackType.ToString().ToLower(CultureInfo.InvariantCulture);
			return comment;
		}
	}
}
