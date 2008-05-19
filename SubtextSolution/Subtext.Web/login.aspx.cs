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
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using log4net;
using Subtext.Framework.Security;
using DotNetOpenId.RelyingParty;

namespace Subtext.Web.Pages
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public partial class login : System.Web.UI.Page
	{
		private readonly static ILog log = new Framework.Logging.Log();
        private const string loginFailedMessage = "That&#8217;s not it<br />";

		#region Declared Controls
		#endregion
	
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			BlogInfo currentBlog = Config.CurrentBlog;
			string returnUrl = Request.QueryString["ReturnURL"];
			if(currentBlog == null || (returnUrl != null && StringHelper.Contains(returnUrl, "HostAdmin", StringComparison.InvariantCultureIgnoreCase)))
			{
				if(!AuthenticateHostAdmin())
				{
					log.Warn("HostAdmin login failure for " + tbUserName.Text);
					Message.Text = loginFailedMessage;
					return;
				}
				else
				{
					ReturnToUrl("~/HostAdmin/Default.aspx");
					return;
				}
			}
			else
			{
                if (SecurityHelper.Authenticate(tbUserName.Text, tbPassword.Text, chkRememberMe.Checked))
				{
					ReturnToUrl(currentBlog.AdminHomeVirtualUrl);
					return;
				}
				else
				{
					log.Warn("Admin login failure for " + tbUserName.Text);
                    Message.Text = loginFailedMessage;
				}
			}
		}

        protected void btnOpenIdLogin_LoggedIn(object sender, OpenIdEventArgs e)
        {
            e.Cancel = true;
            if (e.Response.Status == AuthenticationStatus.Authenticated &&
                SecurityHelper.Authenticate(e.ClaimedIdentifier, chkRememberMe.Checked))
            {
                ReturnToUrl(Config.CurrentBlog.AdminHomeVirtualUrl);
            }
        } 

		private void ReturnToUrl(string defaultReturnUrl)
		{
			if(Request.QueryString["ReturnURL"] != null && Request.QueryString["ReturnURL"].Length > 0)
			{
				log.Debug("redirecting to " + Request.QueryString["ReturnURL"]);
				Response.Redirect(Request.QueryString["ReturnURL"], false);
				return;
			}
			else
			{
				log.Debug("redirecting to " + defaultReturnUrl);
				Response.Redirect(defaultReturnUrl, false);
				return;
			}
		}

		private bool AuthenticateHostAdmin()
		{
            return SecurityHelper.AuthenticateHostAdmin(tbUserName.Text, tbPassword.Text, chkRememberMe.Checked);
		}
	}
}

