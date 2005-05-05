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

namespace Subtext.Common.UrlManager
{
	/// <summary>
	/// Class responisble for figuring out which Subtext page to load. 
	/// By default will load an array of Subtext.UrlManager.HttpHandlder 
	/// from the blog.config file. This contains a list of Regex patterns 
	/// to match the current request to. It also allows caching of the 
	/// Regex's and Types.
	/// </summary>
	public class UrlReWriteHandlerFactory:  IHttpHandlerFactory
	{
		public UrlReWriteHandlerFactory(){} //Nothing to do in the cnstr
		
		protected virtual HttpHandler[] GetHttpHandlers(HttpContext context)
		{
			return HandlerConfiguration.Instance().HttpHandlers;
		}

		/// <summary>
		/// Implementation of IHttpHandlerFactory. By default, it will load an array 
		/// of <see cref="HttpHandler"/>s from the blog.config. This can be changed, 
		/// by overrideing the GetHttpHandlers(HttpContext context) method. 
		/// </summary>
		/// <param name="context">Current HttpContext</param>
		/// <param name="requestType">Request Type (Passed along to other IHttpHandlerFactory's)</param>
		/// <param name="url">The current requested url. (Passed along to other IHttpHandlerFactory's)</param>
		/// <param name="path">The physical path of the current request. Is not gaurenteed 
		/// to exist (Passed along to other IHttpHandlerFactory's)</param>
		/// <returns>
		/// Returns an Instance of IHttpHandler either by loading an instance of IHttpHandler or by returning an other
		/// IHttpHandlerFactory.GetHanlder(HttpContext context, string requestType, string url, string path) method
		/// </returns>
		public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string path)
		{
			//Get the Handlers to process. By defualt, we grab them from the blog.config
			HttpHandler[] items = GetHttpHandlers(context);
			
			//Do we have any?
			if(items != null)
			{
				int count = items.Length;

				for(int i = 0; i < count; i++)
				{
					//We should use our own cached Regex. This should limit the number of Regex's created
					//and allows us to take advantage of RegexOptons.Compiled 
					if(items[i].IsMatch(context.Request.Path))
					{
						switch(items[i].HandlerType)
						{
							case HandlerType.Page:
								return ProccessHandlerTypePage(items[i],context,requestType,url);
							case HandlerType.Direct:
								HandlerConfiguration.SetControls(context,items[i].BlogControls);
								return (IHttpHandler)items[i].Instance();
							case HandlerType.Factory:
							//Pass a long the request to a custom IHttpHandlerFactory
								return ((IHttpHandlerFactory)items[i].Instance()).GetHandler(context,requestType,url,path);
							default:
								throw new Exception("Invalid HandlerType: Unknown");
						}
					}
				}
			}
			//If we do not find the page, just let ASP.NET take over
			return PageHandlerFactory.GetHandler(context,requestType,url, path);
		}


		private IHttpHandler ProccessHandlerTypePage(HttpHandler item, HttpContext context, string requestType, string url)
		{
			string pagepath = item.FullPageLocation;
			if(pagepath == null)
			{
				pagepath = HandlerConfiguration.Instance().FullPageLocation;
			}
			HandlerConfiguration.SetControls(context,item.BlogControls);
			return PageParser.GetCompiledPageInstance(url,pagepath,context);
		}


		public virtual void ReleaseHandler(IHttpHandler handler) 
		{

		}
	}
}
