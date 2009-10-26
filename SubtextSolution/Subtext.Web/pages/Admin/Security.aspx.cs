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
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Summary description for Password.
    /// </summary>
    public partial class Security : AdminOptionsPage
    {
        protected Label Message;
        protected ValidationSummary ValidationSummary1;

        protected override void BindLocalUI()
        {
            tbOpenIDURL.Text = Config.CurrentBlog.OpenIdUrl;
            base.BindLocalUI();
        }

        protected void btnSaveOptions_Click(object sender, EventArgs e)
        {
            Blog info = Config.CurrentBlog;
            info.OpenIdUrl = tbOpenIDURL.Text;
            Repository.UpdateConfigData(info);
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string failureMessage = Resources.Security_PasswordNotUpdated;
            if(Page.IsValid)
            {
                if(SecurityHelper.IsValidPassword(SubtextContext.Blog, tbCurrent.Text))
                {
                    if(tbPassword.Text == tbPasswordConfirm.Text)
                    {
                        SecurityHelper.UpdatePassword(tbPassword.Text);

                        Messages.ShowMessage(Resources.Security_PasswordUpdated);
                    }
                    else
                    {
                        Messages.ShowError(failureMessage);
                    }
                }
                else
                {
                    Messages.ShowError(failureMessage);
                }
            }
            else
            {
                Messages.ShowError(failureMessage);
            }
        }
    }
}