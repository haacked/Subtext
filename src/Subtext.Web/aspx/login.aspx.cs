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
using System.Linq;
using System.Web;
using DotNetOpenAuth.OpenId.RelyingParty;
using log4net;
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Logging;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.Properties;

namespace Subtext.Web.Pages
{
    /// <summary>
    /// Summary description for login.
    /// </summary>
    public partial class login : SubtextPage
    {
        private readonly static ILog Log = new Log();
        private static readonly string LoginFailedMessage = Resources.Login_Failed + "<br />";

        public HostInfo HostInfo
        {
            get { return Host.Value; }
        }

        [Inject]
        public LazyNotNull<HostInfo> Host { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["__OpenIdUrl__"];
                if (cookie != null)
                {
                    btnOpenIdLogin.Text = cookie.Value;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string returnUrl = Request.QueryString["ReturnURL"];
            string username = tbUserName.Text;
            string password = tbPassword.Text;
            bool persist = chkRememberMe.Checked;

            bool isAdmin = false;

            var blog = Blog;
            if (blog == null || blog.Password == null)
            {
                blog = null;
            }

            if (blog != null)
            {
                isAdmin = blog.IsValidUser(username, password);
                returnUrl = String.IsNullOrEmpty(returnUrl) ? (string)AdminUrl.Home() : returnUrl;
            }
            else
            {
                returnUrl = String.IsNullOrEmpty(returnUrl) ? "~/HostAdmin/Default.aspx" : returnUrl;
            }

            bool isHostAdmin = HostInfo.ValidateHostAdminPassword(username, password);

            if (!isAdmin && !isHostAdmin)
            {
                Message.Text = LoginFailedMessage;
                return;
            }

            var roles = new string[2];
            if (isAdmin)
            {
                roles[0] = "Admins";
                SubtextContext.HttpContext.SetAuthenticationTicket(blog, username, persist, roles.Where(s => s != null).ToArray());

            }
            if (isHostAdmin)
            {
                roles[1] = "HostAdmins";
                SubtextContext.HttpContext.SetAuthenticationTicket(null, username, persist, forceHostAdmin: true, roles: roles.Where(s => s != null).ToArray());
            }

            ReturnToUrl(returnUrl);
        }

        protected void btnOpenIdLogin_LoggingIn(object sender, OpenIdEventArgs e)
        {
            if (btnOpenIdLogin.RememberMe)
            {
                var openIdCookie = new HttpCookie("__OpenIdUrl__", btnOpenIdLogin.Text) { Expires = DateTime.UtcNow.AddDays(14) };
                Response.Cookies.Add(openIdCookie);
            }
        }

        protected void btnOpenIdLogin_LoggedIn(object sender, OpenIdEventArgs e)
        {
            e.Cancel = true;
            if (e.Response.Status == AuthenticationStatus.Authenticated &&
               SecurityHelper.Authenticate(e.ClaimedIdentifier, btnOpenIdLogin.RememberMe))
            {
                ReturnToUrl(AdminUrl.Home());
            }
            else
            {
                openIdMessage.Text = Resources.Login_AuthenticationFailed;
            }
        }

        private void ReturnToUrl(string defaultReturnUrl)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ReturnURL"]))
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug(string.Format("redirecting to {0}", Request.QueryString["ReturnURL"]));
                }
                Response.Redirect(Request.QueryString["ReturnURL"], false);
                return;
            }
            if (Log.IsDebugEnabled)
            {
                Log.Debug(string.Format("redirecting to {0}", defaultReturnUrl));
            }
            Response.Redirect(defaultReturnUrl, false);
            return;
        }

        private bool AuthenticateHostAdmin()
        {
            return HostInfo.AuthenticateHostAdmin(tbUserName.Text, tbPassword.Text, chkRememberMe.Checked);
        }
    }
}