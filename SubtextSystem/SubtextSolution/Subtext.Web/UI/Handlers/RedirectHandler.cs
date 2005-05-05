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
	/// Summary description for RedirectHandler.
	/// </summary>
	public class RedirectHandler : System.Web.IHttpHandler
	{
		public RedirectHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IHttpHandler Members

		public void ProcessRequest(HttpContext context)
		{
			
			string uri = context.Request.Path;
			string redirectUrl = null;

			//Needs to be better factored. But for now, we will just look for known updated urls
			//from the original .Text web implementation

			//Handle Archives
			if(Regex.IsMatch(uri,"/archive/",RegexOptions.IgnoreCase))
			{
				UrlFormats formats = Config.CurrentBlog(context).UrlFormats;

				string dateString = Subtext.Framework.Util.Globals.GetReqeustedFileName(uri);
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
			else if(Regex.IsMatch(uri,"/posts/|/story/",RegexOptions.IgnoreCase))
			{
				string entryName = WebPathStripper.GetReqeustedFileName(uri);
				Entry entry = null;
				if(WebPathStripper.IsNumeric(entryName))
				{
					entry = Cacher.SingleEntry(Int32.Parse(entryName),CacheTime.Short,context);
				}
				else
				{
					entry = Cacher.SingleEntry(entryName,CacheTime.Short,context);
				}
				
				if(entry != null)
				{
					redirectUrl = entry.Link;
				}
			}

			if(redirectUrl != null)
			{
				context.Response.StatusCode = 301;
				context.Response.Redirect(redirectUrl,true);
			}
			else
			{
				context.Response.Write("<h2>Sorry, the page you requested does not exist or the content has been removed.</h2>");
			}
		}

		public bool IsReusable
		{
			get
			{
				// TODO:  Add RedirectHandler.IsReusable getter implementation
				return false;
			}
		}

		#endregion
	}
}
