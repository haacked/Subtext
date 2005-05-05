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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Handlers 
{
	/// <summary>
	/// Most pages in WebUI do not exist and are loaded via Activator.CreateInstance(Page page). However, the admin pages
	/// exist and must be parsed. When in BlogRequestType.Multiple the admin directory likely exists in a different 
	/// directory than request. This Handler will edit the path property and reset it to the actual path.
	/// </summary>
	public class BlogExistingPageHandler :  IHttpHandlerFactory
	{
		public BlogExistingPageHandler(){}

		public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{

			BlogConfig config = Config.CurrentBlog(context);
			if(config.IsVirtual)
			{
				string app = config.Application.ToLower();
				app = app.Substring(1,app.Length - 2);
				url = url.ToLower().Remove(url.ToLower().LastIndexOf(app),(app.Length) + 1);
				path = path.ToLower().Remove(path.ToLower().LastIndexOf(app),(app.Length + 1));

				if(!Regex.IsMatch(path,"\\.\\w+$"))
				{
					path = System.IO.Path.Combine(path,"default.aspx");

				}
			}

			return PageParser.GetCompiledPageInstance(url, path, context);

		}

		public  void ReleaseHandler(IHttpHandler handler) 
		{

		}
	}
}

