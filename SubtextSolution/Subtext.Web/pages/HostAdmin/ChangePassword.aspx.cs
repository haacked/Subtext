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
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace Subtext.Web.HostAdmin
{
    /// <summary>
    /// Allows the user to change the host admin password.
    /// </summary>
    public partial class ChangePassword : HostAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblSuccess.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                HostInfo.SetHostPassword(HostInfo.Instance, txtNewPassword.Text);
                HostInfo.UpdateHost(HostInfo.Instance);
                lblSuccess.Visible = true;
            }
        }

        private void vldCurrent_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string password = txtCurrentPassword.Text;
            if(Config.Settings.UseHashedPasswords)
            {
                password = SecurityHelper.HashPassword(password, HostInfo.Instance.Salt);
            }

            args.IsValid = password == HostInfo.Instance.Password;
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
            vldCurrent.ServerValidate +=
                new System.Web.UI.WebControls.ServerValidateEventHandler(vldCurrent_ServerValidate);
        }

        #endregion
    }
}