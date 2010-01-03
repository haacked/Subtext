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
        protected override void OnLoad(EventArgs e)
        {
            txtEmail.Text = HostInfo.Instance.Email;
            lblSuccess.Visible = false;
            emailChangedLabel.Visible = false;
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

        protected void OnChangeEmailButtonClick(object sender, EventArgs e)
        {
            emailChangedLabel.Visible = true;
            HostInfo.Instance.Email = txtEmail.Text;
            HostInfo.UpdateHost(HostInfo.Instance);
        }

        private void ValidatePassword(object source, ServerValidateEventArgs args)
        {
            string password = txtCurrentPassword.Text;
            if(Config.Settings.UseHashedPasswords)
            {
                password = SecurityHelper.HashPassword(password, HostInfo.Instance.Salt);
            }

            args.IsValid = password == HostInfo.Instance.Password;
        }

        override protected void OnInit(EventArgs e)
        {
            vldCurrent.ServerValidate += ValidatePassword;
            base.OnInit(e);
        }
    }
}