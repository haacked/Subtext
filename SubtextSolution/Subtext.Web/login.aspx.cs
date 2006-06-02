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

namespace Subtext.Web.Pages
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public class login : System.Web.UI.Page
	{
		#region Declared Controls
		protected System.Web.UI.WebControls.Label Message;
		protected System.Web.UI.WebControls.TextBox tbUserName;
		protected System.Web.UI.WebControls.TextBox tbPassword;
		protected System.Web.UI.WebControls.CheckBox chkRemember;
		protected System.Web.UI.WebControls.Button btnLogin;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected MetaBuilders.WebControls.DefaultButtons DefaultButtons1;
		protected System.Web.UI.HtmlControls.HtmlImage headerLogoImg;
		protected System.Web.UI.WebControls.LinkButton lbSendPassword;
		#endregion
	
		private void Page_Load(object sender, System.EventArgs e)
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
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			this.lbSendPassword.Click += new System.EventHandler(this.lbSendPassword_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lbSendPassword_Click(object sender, System.EventArgs e)
		{
			BlogInfo info = Config.CurrentBlog;
			bool messageSent = false;
			string password;
			
			if(info != null)
			{
				//Try Admin login.
				if(StringHelper.AreEqualIgnoringCase(tbUserName.Text, info.UserName))
				{
					if(info.IsPasswordHashed)
					{
						password = Security.ResetPassword();
					}
					else
					{
						password = info.Password;
					}

					string message = "Here is your blog login information:\nUserName: {0}\nPassword: {1}\n\nPlease disregard this message if you did not request it.";
					EmailProvider mail = Subtext.Extensibility.Providers.EmailProvider.Instance();
			
					string To = info.Email;
					string From = mail.AdminEmail;
					string Subject = "Login Credentials";
					string Body = string.Format(message,info.UserName,password);
					mail.Send(To,From,Subject,Body);
					Message.Text = "Login Credentials Sent<br />";
					messageSent = true;
				}
			}
				
			if(StringHelper.AreEqualIgnoringCase(tbUserName.Text, HostInfo.Instance.HostUserName))
			{
				//Try Host Admin login.
				if(Config.Settings.UseHashedPasswords)
					password = Security.ResetHostAdminPassword();		
				else
					password = HostInfo.Instance.Password;

				string message = "Here is your Host Admin Login information:\nUserName: {0}\nPassword: {1}\n\nPlease disregard this message if you did not request it.";
				EmailProvider mail = Subtext.Extensibility.Providers.EmailProvider.Instance();
			
				string hostAdminEmail = ConfigurationSettings.AppSettings["HostEmailAddress"];
				if(hostAdminEmail == null || hostAdminEmail.Length == 0 || hostAdminEmail.IndexOf('@') <= 0) //Need better email validation. I know!
				{
					Message.Text = "Sorry, but I don&#8217;t know where to send the email.  Please specify a Host Email Address in Web.config. It is the AppSetting &#8220;HostEmailAddress&#8221;";
					return;
				}
				string To = hostAdminEmail;
				string From = mail.AdminEmail;
				string Subject = "Subtext Host Admin Login Credentials";
				string Body = string.Format(message, HostInfo.Instance.HostUserName, password);
				mail.Send(To, From, Subject, Body);
				Message.Text = "Login Credentials Sent<br />";
				messageSent = true;
			}
			
			if(!messageSent)
			{
				Message.Text = "I don't know you";
			}
		}

		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			BlogInfo currentBlog = Config.CurrentBlog;
			string returnUrl = Request.QueryString["ReturnURL"];
			if(currentBlog == null || (returnUrl != null && StringHelper.Contains(returnUrl, "HostAdmin", ComparisonType.CaseInsensitive)))
			{
				if(!AuthenticateHostAdmin())
				{
					Message.Text = "That&#8217;s not it<br />";
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
				if(Security.Authenticate(tbUserName.Text, tbPassword.Text, chkRemember.Checked))
				{
					ReturnToUrl(currentBlog.AdminHomeVirtualUrl);
					return;
				}
				else
				{
					Message.Text = "That&#8217;s not it<br />";
				}
			}
		}
		
		private void ReturnToUrl(string defaultReturnUrl)
		{
			if(Request.QueryString["ReturnURL"] != null && Request.QueryString["ReturnURL"].Length > 0)
			{
				Response.Redirect(Request.QueryString["ReturnURL"], false);
				return;
			}
			else
			{
				Response.Redirect(defaultReturnUrl, false);
				return;
			}
		}

		private bool AuthenticateHostAdmin()
		{
			return Security.AuthenticateHostAdmin(tbUserName.Text, tbPassword.Text, chkRemember.Checked);
		}
	}
}

