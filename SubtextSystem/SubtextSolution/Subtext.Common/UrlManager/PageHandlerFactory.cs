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
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

namespace Subtext.Common.UrlManager 
{
	/// <summary>
	/// System.Web.UI.PageHandlerFactory is internal. We need the option to load our own 
	/// classes with this for the virtual mapping.  With the virtual mapping default 
	/// documents will not be loaded. if no page is found, we will use attempt to load 
	/// default.aspx in the current directory
	/// </summary>
	public class PageHandlerFactory 
	{
		static PageHandlerFactory()
		{
			_stackwalk = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
		}

		public PageHandlerFactory(){}

		public static IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{
			StackWalk.Assert();

			if(!path.ToLower().EndsWith(".aspx"))
			{
				path = System.IO.Path.Combine(path,"default.aspx");
			}
			return PageParser.GetCompiledPageInstance(url, path, context);
		}

		private static IStackWalk _stackwalk = null;
		public static IStackWalk StackWalk
		{
			get
			{
				return _stackwalk;
			}
		}
		
	}
}

