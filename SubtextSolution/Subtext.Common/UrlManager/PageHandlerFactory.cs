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
	public sealed class PageHandlerFactory 
	{
		private static IStackWalk _stackwalk = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		private PageHandlerFactory(){}

		public static IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{
			StackWalk.Assert();

			if(!path.ToLower(System.Globalization.CultureInfo.InvariantCulture).EndsWith(".aspx"))
			{
				path = System.IO.Path.Combine(path, "default.aspx");
			}
			return PageParser.GetCompiledPageInstance(url, path, context);
		}

		public static IStackWalk StackWalk
		{
			get
			{
				return _stackwalk;
			}
		}
		
	}
}

