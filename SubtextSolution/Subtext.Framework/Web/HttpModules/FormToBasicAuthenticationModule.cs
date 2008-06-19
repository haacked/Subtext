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
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;
using Subtext.Framework.Web;

namespace Subtext.Framework.Web.HttpModules
{
    /// <summary>
    /// Summary description for BasicAuthenticationModule
    /// </summary>
    public class FormToBasicAuthenticationModule : IHttpModule
    {
        public void Dispose()
        {
            // throw new Exception("The method or operation is not implemented.");
        }

        public void EndRequest(Object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            if (!String.IsNullOrEmpty(SecurityHelper.CurrentUserName)
                ||HttpHelper.IsStaticFileRequest()
                ||!(context.Response.StatusCode == 302
                    && context.Response.RedirectLocation.IndexOf(FormsAuthentication.LoginUrl) == 0))
                return;

            if(!Regex.IsMatch(context.Request.Path,@"Rss\.axd"))
                return;

            string authHeader = context.Request.Headers["Authorization"];
            if (String.IsNullOrEmpty(authHeader))
            {
                SendAuthRequest(context);
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
        	app.AuthenticateRequest += AuthenticateRequest;
            app.EndRequest += EndRequest;
        }

    	private void AuthenticateRequest(object sender, EventArgs e)
    	{
			HttpContext context = ((HttpApplication)sender).Context;
			string authHeader = context.Request.Headers["Authorization"];
			if(String.IsNullOrEmpty(authHeader))
				return;
			if (authHeader.IndexOf("Basic ") == 0)
			{
				byte[] bytes = Convert.FromBase64String(authHeader.Remove(0, 6));

				string authString = Encoding.Default.GetString(bytes);
				string[] usernamepassword = authString.Split(':');

				if (SecurityHelper.Authenticate(usernamepassword[0], usernamepassword[1], false))
				{
					context.User = new GenericPrincipal(new GenericIdentity(usernamepassword[0]), null); 

				}
				else
				{
					SendAuthRequest(context);
				}
			}
		}
    }
}