#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Handlers
{
	/// <summary>
	/// HTTP Handler for rendering a CSS stylesheet.
	/// </summary>
	public class BlogSecondaryCssHandler : IHttpHandler
	{
		/// <summary>
		/// Processes the request.
		/// </summary>
		/// <param name="context">Context.</param>
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentEncoding = System.Text.Encoding.UTF8;
			context.Response.ContentType = "text/css";
			context.Response.Write(Config.CurrentBlog(context).Skin.SkinCssText);
		}

		/// <summary>
		/// Gets a value indicating whether this handler is reusable.
		/// </summary>
		/// <remarks>
		/// This handler is not reusable.
		/// </remarks>
		/// <value>
		/// 	<c>true</c> if is reusable; otherwise, <c>false</c>.
		/// </value>
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}

