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
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Format;
using Subtext.Framework.Text;

namespace Subtext.Common.Syndication
{
	// To Do: Should we validate the supplied XML? 
	// Not much that can be done if it is not correct.

	/// <summary>
	/// Implementation of http://wellformedweb.org/story/9
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

		public void ProcessRequest(HttpContext context)
		{
			HttpRequest Request = context.Request;
			if(Request.RequestType == "POST" && Request.ContentType == "text/xml")
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Request.InputStream);
				Entry entry = new Entry(PostType.Comment);

				string name = doc.SelectSingleNode("//item/author").InnerText;
				if(name.IndexOf("<") != -1)
				{
					name = name.Substring(0,name.IndexOf("<"));
				}
                entry.Author = name.Trim();

				entry.Body = doc.SelectSingleNode("//item/description").InnerText;
			
				entry.Title = doc.SelectSingleNode("//item/title").InnerText;
				entry.AlternativeTitleUrl = HtmlHelper.CheckForUrl(doc.SelectSingleNode("//item/link").InnerText);

				entry.ParentId = UrlFormats.GetPostIDFromUrl(Request.Path);

				Entries.InsertComment(entry);
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

