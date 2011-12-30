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
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using log4net;
using Subtext.Framework.Logging;
using Subtext.Framework.Security;

namespace Subtext.Framework.Web.HttpModules
{
    /// <summary>pa
    /// Handles the AuthenticateRequest event of a request.  Decrypts the authentication 
    /// token and sets up the current user as a GeneralPrinciple, attaching its roles.  
    /// </summary>
    public class AuthenticationModule : IHttpModule
    {
        private readonly static ILog Log = new Log();

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        public void Dispose()
        {
            //Do Nothing...
        }

        public void AuthenticateRequest(HttpContextBase httpContext, BlogRequest blogRequest)
        {
            if (blogRequest.RequestLocation == RequestLocation.StaticFile)
            {
                return;
            }

            HttpCookie authCookie = httpContext.Request.SelectAuthenticationCookie(blogRequest.Blog);
            FormsAuthenticationTicket authTicket = GetFormsAuthenticationTicket(authCookie);
            HandleFormsAuthenticationTicket(blogRequest, httpContext, authTicket);
        }

        public void HandleFormsAuthenticationTicket(BlogRequest blogRequest, HttpContextBase httpContext, FormsAuthenticationTicket authTicket)
        {
            if (authTicket != null)
            {
                if (FormsAuthentication.SlidingExpiration)
                {
                    authTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
                }

                // When the ticket was created, the UserData property was assigned a
                // pipe delimited string of role names.
                SetHttpContextUser(httpContext, authTicket);
            }
        }

        public FormsAuthenticationTicket GetFormsAuthenticationTicket(HttpCookie authCookie)
        {
            if (null == authCookie)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("There is no authentication cookie.");
                }
                return null;
            }

            FormsAuthenticationTicket authTicket;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                Log.Error("Could not decrypt the authentication cookie.", ex);
                return null;
            }

            if (null == authTicket)
            {
                Log.Warn("Could not decrypt the authentication cookie. No exception was thrown.");
                return null;
            }

            if (authTicket.Expired)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("Authentication ticket expired.");
                }
                return null;
            }
            return authTicket;
        }

        private static void SetHttpContextUser(HttpContextBase httpContext, FormsAuthenticationTicket authTicket)
        {
            var roles = authTicket.UserData.Split(new[] { '|' });
            // Create an Identity object
            var id = new FormsIdentity(authTicket);

            // This principal will flow throughout the request.
            var principal = new GenericPrincipal(id, roles);
            // Attach the new principal object to the current HttpContext object
            httpContext.User = principal;
            if (Log.IsDebugEnabled)
            {
                Log.Debug(string.Format("Authentication succeeded. Current.User={0}; {1}", id.Name, authTicket.UserData));
            }
        }

        void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            AuthenticateRequest(context, BlogRequest.Current);
        }
    }
}