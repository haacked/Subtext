#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace Subtext.Framework.Web.HttpModules
{
    /// <summary>
    /// Summary description for BasicAuthenticationModule
    /// </summary>
    public class FormToBasicAuthenticationModule : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.AuthenticateRequest += OnAuthenticateRequest;
            app.EndRequest += OnEndRequest;
        }

        public void Dispose()
        {
            // do nothing
        }

        private void OnEndRequest(Object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            HandleEndRequest(context);
        }

        public void HandleEndRequest(HttpContextBase context)
        {
            if (!String.IsNullOrEmpty(context.User.Identity.Name)
               || context.Request.IsStaticFileRequest()
               || !(context.Response.StatusCode == 302
                    &&
                    context.Response.RedirectLocation.IndexOf(FormsAuthentication.LoginUrl,
                                                              StringComparison.OrdinalIgnoreCase) == 0))
            {
                return;
            }

            if (!Regex.IsMatch(context.Request.Path, @"Rss\.axd"))
            {
                return;
            }

            string authHeader = context.Request.Headers["Authorization"];
            if (String.IsNullOrEmpty(authHeader))
            {
                SendAuthRequest(context);
            }
        }

        private static void SendAuthRequest(HttpContextBase context)
        {
            Debug.WriteLine("Auth");
            context.Response.StatusCode = 401;

            context.Response.AddHeader("WWW-Authenticate",
                                       String.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"",
                                                     Config.CurrentBlog.Title));
            //			context.ApplicationInstance.CompleteRequest();
        }

        public void AuthenticateRequest(Blog blog, HttpContextBase context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (String.IsNullOrEmpty(authHeader))
            {
                return;
            }

            if (authHeader.IndexOf("Basic ") == 0)
            {
                byte[] bytes = Convert.FromBase64String(authHeader.Remove(0, 6));

                string authString = Encoding.Default.GetString(bytes);
                string[] usernamepassword = authString.Split(':');

                if (context.Authenticate(blog, usernamepassword[0], usernamepassword[1], false))
                {
                    context.User = new GenericPrincipal(new GenericIdentity(usernamepassword[0]), null);
                }
                else
                {
                    SendAuthRequest(context);
                }
            }
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            AuthenticateRequest(Config.CurrentBlog, context);
        }
    }
}