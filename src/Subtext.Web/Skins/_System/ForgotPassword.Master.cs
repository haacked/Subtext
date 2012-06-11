using System;
using System.Globalization;
using System.Web.UI;
using Ninject;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;
using Subtext.Web.Properties;

namespace Subtext.Web.Skins._System
{
    public partial class ForgotPassword : MasterPage
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            defaultInstructions.Visible = true;
            if (Config.CurrentBlog == null)
            {
                ResetForm.Visible = defaultInstructions.Visible = false;
                FailureText.Text = Resources.ForgotPasswordMaster_DoesNotWorkForHostAdmins;
            }
            base.OnLoad(e);
        }

        protected void OnForgotButtonClick(object sender, EventArgs args)
        {
            Blog currentBlog = Config.CurrentBlog;
            if (currentBlog == null)
            {
                FailureText.Text = Resources.ForgotPasswordMaster_DoesNotWorkForHostAdmins;
            }
            else
            {
                ResetAdminPassword(currentBlog);
            }
        }

        private void ResetAdminPassword(Blog currentBlog)
        {
            if (String.IsNullOrEmpty(currentBlog.Email) || currentBlog.Email != emailTextBox.Text ||
               currentBlog.UserName != usernameTextBox.Text)
            {
                Message.Visible = false;
                FailureText.Visible = true;
                FailureText.Text = Resources.ForgotPassword_UsernameAndPasswordDoNotMatch;
            }
            else
            {
                defaultInstructions.Visible = false;
                Message.Visible = true;
                FailureText.Visible = false;

                string newPassword = AccountService.ResetPassword();
                EmailProvider.Instance().Send(currentBlog.Email, currentBlog.Email, Resources.ForgotPassword_NewPassword
                                              ,
                                              String.Format(CultureInfo.InvariantCulture,
                                                            Resources.ForgotPaswword_HereIsNewPassword, newPassword));

                Message.Text = Resources.ForgotPassword_NewPasswordSent;
            }
        }
    }
}