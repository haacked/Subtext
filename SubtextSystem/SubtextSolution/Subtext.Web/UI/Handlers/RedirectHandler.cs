using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Common.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Util;

namespace Subtext.Web.UI.Handlers
{
	/// <summary>
	/// Handles redirects of incoming requests to the appropriate control.
	/// </summary>
	public class RedirectHandler : System.Web.IHttpHandler
	{
		#region IHttpHandler Members

		/// <summary>
		/// Enables processing of HTTP Web requests by a custom
		/// <see langword="HttpHandler "/>
		/// that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <remarks>
		/// Parses the incoming request url to determine where the request 
		/// should be redirected.
		/// </remarks>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, <see langword="Request"/>, <see langword="Response"/>, <see langword="Session"/>, and <see langword="Server"/>)<see langword=""/> used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			string uri = context.Request.Path;
			string redirectUrl = null;

			//Needs to be better factored. But for now, we will just look for known updated urls
			//from the original .Text web implementation

			//Handle Archives
			if(Regex.IsMatch(uri,"/archive/",RegexOptions.IgnoreCase))
			{
				UrlFormats formats = Config.CurrentBlog.UrlFormats;

				string dateString = UrlFormats.GetRequestedFileName(uri);
				try
				{
					if(dateString.Length == 8)
					{
						DateTime dt = DateTime.ParseExact(dateString,"MMddyyyy",new CultureInfo("en-US"));
						redirectUrl = formats.DayUrl(dt);
					}
					else
					{
						DateTime dt = DateTime.ParseExact(dateString,"MMyyyy",new CultureInfo("en-US"));
						redirectUrl = formats.MonthUrl(dt);
					}
				}
				catch(System.FormatException)
				{
					context.Response.StatusCode = 404;
					context.Response.Redirect("~/SystemMessages/FileNotFound.aspx");
				}
			}
			else if(Regex.IsMatch(uri,"/posts/|/story/",RegexOptions.IgnoreCase))
			{
				string entryName = WebPathStripper.GetRequestedFileName(uri);
				Entry entry = null;
				if(WebPathStripper.IsNumeric(entryName))
				{
					entry = Cacher.GetSingleEntry(Int32.Parse(entryName), CacheTime.Short, context);
				}
				else
				{
					//This is why EntryName must be unique.
					entry = Cacher.GetSingleEntry(entryName, CacheTime.Short, context);
				}
				
				if(entry != null)
				{
					redirectUrl = entry.Link;
				}
			}

			if(redirectUrl != null)
			{
				context.Response.StatusCode = 301;
				context.Response.Redirect(redirectUrl, true);
			}
			else
			{
				context.Response.Write("<h2>Sorry, the page you requested does not exist or the content has been removed.</h2>");
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
			get
			{
				return true;
			}
		}

		#endregion
	}
}
