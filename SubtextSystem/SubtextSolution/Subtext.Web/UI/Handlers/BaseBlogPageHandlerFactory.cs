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

namespace Subtext.Web.UI.Pages 
{
	/// <summary>
	/// Abstract base factory class for creating blog page handlers.
	/// </summary>
	public abstract class BaseBlogPageHandlerFactory :  IHttpHandlerFactory
	{
		/// <summary>
		/// Gets the page type.
		/// </summary>
		/// <value></value>
		public abstract Type PageType
		{
			get;
		}
		
		/// <summary>
		/// Creates and returns the handler based on the specifed 
		/// HttpContext, request type, url, and path.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="requestType">Request type.</param>
		/// <param name="url">URL.</param>
		/// <param name="path">Path.</param>
		/// <returns></returns>
		public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{
			return (IHttpHandler)Activator.CreateInstance(PageType);
		}

		/// <summary>
		/// Releases the handler. Currently does nothing.
		/// </summary>
		/// <param name="handler">Handler.</param>
		public virtual void ReleaseHandler(IHttpHandler handler) 
		{
		}
	}
}

