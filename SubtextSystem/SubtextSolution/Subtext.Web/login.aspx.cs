#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
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
		protected System.Web.UI.WebControls.Label Message;
		protected System.Web.UI.WebControls.TextBox tbUserName;
		protected System.Web.UI.WebControls.TextBox tbPassword;
		protected System.Web.UI.WebControls.CheckBox chkRemember;
		protected System.Web.UI.WebControls.Button btnLogin;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.HtmlControls.HtmlAnchor aspnetLink;
		protected MetaBuilders.WebControls.DefaultButtons DefaultButtons1;
		protected System.Web.UI.WebControls.LinkButton lbSendPassword;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			#if WANRelease
					lbSendPassword.Visible = false;
					aspnetLink.Visible = true;					
			#endif
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
			string password = null;
			
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
				Message.Text = "Login Credentials Sent<br>";
				messageSent = true;
			}
			
			if(StringHelper.AreEqualIgnoringCase(tbUserName.Text, HostInfo.Instance.HostUserName))
			{
				if(Config.Settings.UseHashedPasswords)
					password = Security.ResetHostAdminPassword();		
				else
					password = HostInfo.Instance.Password;

				string message = "Here is your Host Admin Login information:\nUserName: {0}\nPassword: {1}\n\nPlease disregard this message if you did not request it.";
				EmailProvider mail = Subtext.Extensibility.Providers.EmailProvider.Instance();
			
				string To = info.Email;
				string From = mail.AdminEmail;
				string Subject = "Subtext Host Admin Login Credentials";
				string Body = string.Format(message, HostInfo.Instance.HostUserName, password);
				mail.Send(To, From, Subject, Body);
				Message.Text = "Login Credentials Sent<br>";
				messageSent = true;
			}
			
			if(!messageSent)
				Message.Text = "I don't know you";		
		}

		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			string returnUrl = Request.QueryString["ReturnURL"];
			const bool caseSensitive = true;
			if(returnUrl != null && StringHelper.Contains(returnUrl, "HostAdmin", !caseSensitive))
			{
				AuthenticateHostAdmin();
			}
			else
			{

				BlogInfo info = Config.CurrentBlog;
				if(Security.Authenticate(tbUserName.Text, tbPassword.Text, chkRemember.Checked))
				{
					if(returnUrl != null)
					{
						Response.Redirect(returnUrl);
					}
					else
					{
						Response.Redirect(info.RootUrl + "admin/default.aspx");
					}
				}
				else
				{
					Message.Text = "That's not it<br>";
				}
			}
		}

		private void AuthenticateHostAdmin()
		{
			if(StringHelper.AreEqualIgnoringCase(tbUserName.Text, HostInfo.Instance.HostUserName))
			{
				string password = tbPassword.Text;
				if(Config.Settings.UseHashedPasswords)
					password = Security.HashPassword(tbPassword.Text, HostInfo.Instance.Salt);
				if(StringHelper.AreEqualIgnoringCase(HostInfo.Instance.Password, password))
				{
					System.Web.Security.FormsAuthentication.SetAuthCookie("HostAdmin", chkRemember.Checked);
					if(Request.QueryString["ReturnURL"] != null)
					{
						Response.Redirect(Request.QueryString["ReturnURL"]);
					}
					else
					{
						Response.Redirect("~/HostAdmin/default.aspx");
					}
				}
			}
		}
	}
}

