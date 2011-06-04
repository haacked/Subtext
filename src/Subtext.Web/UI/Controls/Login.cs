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
using System.Web.UI.WebControls;
using Subtext.Framework.Security;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for Login.
    /// </summary>
    public class Login : BaseControl
    {
        protected Button btnLogin;
        protected Literal Message;
        protected CheckBox RememberMe;
        protected TextBox tbPassword;
        protected TextBox tbUserName;

        private void Page_Load(object sender, EventArgs e)
        {
            if (User.IsAdministrator())
            {
                Controls.Clear();
                Visible = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (SubtextContext.HttpContext.Authenticate(Blog, tbUserName.Text, tbPassword.Text, RememberMe.Checked))
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
    }
}