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
using System.Web.Compilation;

namespace Subtext.Framework.UrlManager 
{
	/// <summary>
	/// System.Web.UI.PageHandlerFactory is internal. We need the option to load our own 
	/// classes with this for the virtual mapping.  With the virtual mapping default 
	/// documents will not be loaded. if no page is found, we will use attempt to load 
	/// default.aspx in the current directory
	/// </summary>
	public static class PageHandlerFactory 
	{
		public static IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{
            Type t = BuildManager.GetCompiledType(url);
			return (IHttpHandler) Activator.CreateInstance(t);
		}
	}
}

