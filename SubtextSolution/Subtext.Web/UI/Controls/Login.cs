using System;
using Subtext.Framework;

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

namespace Subtext.Web.UI.Controls
{
	using System;
    using Subtext.Framework.Security;

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
			if(SecurityHelper.IsAdmin)
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
			if(SecurityHelper.Authenticate(tbUserName.Text,tbPassword.Text,RememberMe.Checked))
			{
				Response.Redirect(Request.Path);
			}
				////			BlogConfig config = Config.CurrentBlog;
				//			if(tbUserName.Text == config.UserName && tbPassword.Text == config.Password)
				//			{
				//				FormsAuthentication.SetAuthCookie(config.BlogId.ToString(),RememberMe.Checked);
				//				Response.Redirect(Request.Path);
				//			}
			else
			{
				Message.Text = "That's not it";
			}

		}
	}
}

