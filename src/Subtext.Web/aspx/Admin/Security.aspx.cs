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
using Ninject;
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

        [Inject]
        public IAccountService AccountService { get; set; }

        protected override void BindLocalUI()
        {
            if (!String.IsNullOrEmpty(Blog.OpenIdUrl))
            {
                tbOpenIDURL.Text = Blog.OpenIdUrl;
            }

            tbOpenIDServer.Text = Blog.OpenIdServer;
            tbOpenIDDelegate.Text = Blog.OpenIdDelegate;

            base.BindLocalUI();
        }

        protected void btnSaveOptions_Click(object sender, EventArgs e)
        {
            string openIdUrl = tbOpenIDURL.Text == "http://" ? string.Empty : tbOpenIDURL.Text;
            Blog.OpenIdUrl = openIdUrl;

            Repository.UpdateConfigData(Blog);
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string failureMessage = Resources.Security_PasswordNotUpdated;
            if (Page.IsValid)
            {
                if (SecurityHelper.IsValidPassword(SubtextContext.Blog, tbCurrent.Text))
                {
                    if (tbPassword.Text == tbPasswordConfirm.Text)
                    {
                        AccountService.UpdatePassword(tbPassword.Text);

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

        protected void OnSavePassthroughClick(object sender, EventArgs e)
        {
            Blog.OpenIdServer = tbOpenIDServer.Text;
            Blog.OpenIdDelegate = tbOpenIDDelegate.Text;

            Repository.UpdateConfigData(Blog);
        }
    }
}