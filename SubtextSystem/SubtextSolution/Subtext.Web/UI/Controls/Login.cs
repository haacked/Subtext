using System;
using Subtext.Framework;

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

namespace Subtext.Web.UI.Controls
{
	using System;

	/// <summary>
	///		Summary description for Login.
	/// </summary>
	public class Login : BaseControl
	{
		protected System.Web.UI.WebControls.TextBox tbUserName;
		protected System.Web.UI.WebControls.TextBox tbPassword;
		protected System.Web.UI.WebControls.Button btnLogin;
		protected System.Web.UI.WebControls.Literal Message;
		protected System.Web.UI.WebControls.CheckBox RememberMe;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(Security.IsAdmin)
			{
				this.Controls.Clear();
				this.Visible = false;
			}

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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			if(Security.Authenticate(tbUserName.Text,tbPassword.Text,RememberMe.Checked))
			{
				Response.Redirect(Request.Path);
			}
				////			BlogConfig config = Config.CurrentBlog;
				//			if(tbUserName.Text == config.UserName && tbPassword.Text == config.Password)
				//			{
				//				FormsAuthentication.SetAuthCookie(config.BlogID.ToString(),RememberMe.Checked);
				//				Response.Redirect(Request.Path);
				//			}
			else
			{
				Message.Text = "That's not it";
			}

		}
	}
}

