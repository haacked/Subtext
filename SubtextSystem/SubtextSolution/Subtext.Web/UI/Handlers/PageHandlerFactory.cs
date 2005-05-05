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
using System.Web.UI;

namespace Subtext.Web.UI.Pages 
{
	/// <summary>
	/// System.Web.UI.PageHandlerFactory is internal. We need the option to load our own classes with this for the virtual mapping.
	/// With the virtual mapping default documents will not be loaded. if no page is found, we will use attempt to load default.aspx in the current
	/// directory
	/// </summary>
	[Obsolete("This class is now moved to the Framework Project")]
	public class PageHandlerFactory :  IHttpHandlerFactory
	{
		public PageHandlerFactory(){}

		public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{
			if(!path.ToLower().EndsWith(".aspx"))
			{
				path = System.IO.Path.Combine(path,"default.aspx");
			}
			return PageParser.GetCompiledPageInstance(url, path, context);
		}

		public  void ReleaseHandler(IHttpHandler handler) 
		{
			
		}
		
	}
}

