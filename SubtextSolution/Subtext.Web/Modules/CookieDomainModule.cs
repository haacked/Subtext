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
using System.Configuration;
using System.Web;
using System.Web.Security;

namespace Subtext.Web.Modules
{
	public class CookieDomainModule : System.Web.IHttpModule
	{
		public CookieDomainModule(){}

		public void Dispose(){}

		public void Init(System.Web.HttpApplication context)
		{
			context.EndRequest +=new EventHandler(cookie_Edit_EndRequest);
		}

		private void cookie_Edit_EndRequest(object sender, EventArgs e)
		{
			HttpContext context  = ((HttpApplication)sender).Context;
			if(context.Request.IsAuthenticated)
			{
				//should be a value like ".asp.net"
                string domain = ConfigurationManager.AppSettings["CookieDomain"] as string;

				if(domain != null)
				{
					string ticketName =  FormsAuthentication.FormsCookieName;
					
					HttpCookie cookie = context.Response.Cookies[ticketName];
					if(cookie != null)
					{
						cookie.Domain = domain;
						
					}
				}
			}
		}
	}
}
