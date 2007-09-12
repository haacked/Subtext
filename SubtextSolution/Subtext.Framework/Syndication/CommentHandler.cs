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
using System.Web;
using System.Xml;
using Subtext.Extensibility;
using Subtext.Extensibility.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Format;
using Subtext.Framework.Text;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Syndication
{
	// TODO: Should we validate the supplied XML? 
	// Not much that can be done if it is not correct.

	/// <summary>
	/// CommentAPI Implementation (http://wellformedweb.org/story/9)
	/// Accepts a posted XML document via HttpPost.
	/// </summary>
	public class CommentHandler : BaseHttpHandler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommentHandler"/> class.
		/// </summary>
		public CommentHandler()
		{
		}

		/// <summary>
		/// Handles an XML POST Request that conforms to the Comment API. 
		/// Creates a comment in the system as a result.
		/// </summary>
		/// <param name="context">Context.</param>
		public override void HandleRequest(HttpContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context", Resources.ArgumentNull_Generic);
			}

			HttpRequest Request = context.Request;
			if (Request.RequestType == "POST" && Request.ContentType == "text/xml")
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Request.InputStream);

				FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
				comment.CreatedViaCommentAPI = true;
				string name = doc.SelectSingleNode("//item/author").InnerText;
				if (name.IndexOf("<") != -1)
				{
					name = name.Substring(0, name.IndexOf("<"));
				}
				comment.Author = name.Trim();
				comment.Body = doc.SelectSingleNode("//item/description").InnerText;
				comment.Title = doc.SelectSingleNode("//item/title").InnerText;
				comment.SourceUrl = HtmlHelper.CheckForUrl(doc.SelectSingleNode("//item/link").InnerText);
				comment.EntryId = UrlFormats.GetPostIDFromUrl(Request.Path);

				// [ 1644691 ] Closing comments didn't stop the CommentAPI
				if (!Subtext.Framework.Data.Cacher.GetEntry(comment.EntryId, CacheDuration.Medium).CommentingClosed)
					FeedbackItem.Create(comment, new CommentFilter(HttpContext.Current.Cache));
			}
		}

		/// <summary>
		/// Validates the parameters.  Inheriting classes must
		/// implement this and return true if the parameters are
		/// valid, otherwise false.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns><c>true</c> if the parameters are valid,
		/// otherwise <c>false</c></returns>
		public override bool ValidateParameters(HttpContext context)
		{
			return true;
		}

		/// <summary>
		/// Gets a value indicating whether this handler
		/// requires users to be authenticated.
		/// </summary>
		/// <value>
		///    <c>true</c> if authentication is required
		///    otherwise, <c>false</c>.
		/// </value>
		public override bool RequiresAuthentication
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the content MIME type.
		/// </summary>
		/// <value></value>
		public override string ContentMimeType
		{
			get { return "text"; }
		}
	}
}

