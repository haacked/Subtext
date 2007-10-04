using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using Subtext.Framework;
using Subtext.Framework.Security;
using Subtext.Framework.Web;
using System.Web.Configuration;
using Subtext.Extensibility.Interfaces;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Subtext.Framework.Configuration;

namespace Subtext.Web.HttpModules
{
	/// <summary>
	/// Summary description for BasicAuthenticationModule
	/// </summary>
	public class FormToBasicAuthenticationModule : System.Web.IHttpModule
	{
		public void Dispose()
		{
			//		throw new Exception("The method or operation is not implemented.");
		}

		public void EndRequest(Object sender, EventArgs e)
		{
			HttpApplication app = ((HttpApplication)sender);
			HttpContext context = ((HttpApplication)sender).Context;

			if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)
				||HttpHelper.IsStaticFileRequest()
				||!(context.Response.StatusCode == 302
				&& context.Response.RedirectLocation.IndexOf(FormsAuthentication.LoginUrl) == 0))
				return;

			if(!Regex.IsMatch(context.Request.Path,@"Rss\.aspx"))
				return;

			string authHeader = context.Request.Headers["Authorization"];
			if (String.IsNullOrEmpty(authHeader))
			{
				SendAuthRequest(context);
			}
			else
			{
				if (authHeader.IndexOf("Basic ") == 0)
				{
					byte[] bytes = Convert.FromBase64String(authHeader.Remove(0, 6));

					string authString = Encoding.Default.GetString(bytes);
					string[] usernamepassword = authString.Split(':');

					if (Membership.Provider.ValidateUser(usernamepassword[0], usernamepassword[1]))
					{
						context.Response.Redirect(context.Request.Url.ToString());
					}
					else
					{
						SendAuthRequest(context);
					}
				}
				else
				{
					FormsAuthentication.RedirectToLoginPage();
				}
			}
		}

		private static void SendAuthRequest(HttpContext context)
		{
			Debug.WriteLine("Auth");
			context.Response.StatusCode = 401;

			context.Response.AddHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", Config.CurrentBlog.Title));
//			context.ApplicationInstance.CompleteRequest();
		}

		public void Init(HttpApplication app)
		{

			app.EndRequest += new EventHandler(EndRequest);
		}

	}
}