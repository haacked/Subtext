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

using System.Web;
using System.Xml;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Format;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Text;

namespace Subtext.Framework.Syndication
{
	// To Do: Should we validate the supplied XML? 
	// Not much that can be done if it is not correct.

	/// <summary>
	/// CommentAPI Implementation (http://wellformedweb.org/story/9)
	/// Accepts a posted XML document via HttpPost.
	/// </summary>
	public class CommentHandler : IHttpHandler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommentHandler"/> class.
		/// </summary>
		public CommentHandler()
		{
		}

		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			HttpRequest Request = context.Request;
			if(Request.RequestType == "POST" && Request.ContentType == "text/xml")
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Request.InputStream);

				FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
				comment.CreatedViaCommentAPI = true;
				string name = doc.SelectSingleNode("//item/author").InnerText;
				if(name.IndexOf("<") != -1)
				{
					name = name.Substring(0, name.IndexOf("<"));
				}
                comment.Author = name.Trim();
				comment.Body = doc.SelectSingleNode("//item/description").InnerText;
				comment.Title = doc.SelectSingleNode("//item/title").InnerText;
				comment.SourceUrl = HtmlHelper.CheckForUrl(doc.SelectSingleNode("//item/link").InnerText);
				comment.EntryId = UrlFormats.GetPostIDFromUrl(Request.Path);

                Blog blog = Config.CurrentBlog;

				// [ 1644691 ] Closing comments didn't stop the CommentAPI
                if (!Cacher.GetEntry(comment.EntryId, CacheDuration.Medium, ObjectProvider.Instance(), blog).CommentingClosed) {
                    var feedbackService = new AkismetSpamService(Config.CurrentBlog.FeedbackSpamServiceKey, blog, null, new UrlHelper(null, null));
                    FeedbackItem.Create(comment, new CommentFilter(new SubtextCache(HttpContext.Current.Cache), feedbackService, blog), blog);
                }
			}
		}
	
		/// <summary>
		/// Gets a value indicating whether another request can use
		/// the <see cref="T:System.Web.IHttpHandler"/>
		/// instance.
		/// </summary>
		/// <value></value>
		public bool IsReusable
		{
			get { return true; }
		}
	}
}

