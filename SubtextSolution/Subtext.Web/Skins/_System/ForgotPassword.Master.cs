using System;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace Subtext.Web.Skins._System
{
	public partial class ForgotPassword : System.Web.UI.MasterPage
	{
        protected override void OnLoad(EventArgs e)
        {
            defaultInstructions.Visible = true;
            if (Config.CurrentBlog == null) {
                ResetForm.Visible = defaultInstructions.Visible = false;
                FailureText.Text = "Sorry, but forgot password does not work for Host admins. Please read <a href=\"http://subtextproject.com/Home/FAQ/tabid/113/Default.aspx\" title=\"Subtext FAQ\">the FAQ</a> to reset your password.";
            }
            base.OnLoad(e);
        }

        protected void OnForgotButtonClick(object sender, EventArgs args) {
            Blog currentBlog = Config.CurrentBlog;
            if (currentBlog == null)
            {
                FailureText.Text = "Sorry, but forgot password does not work for Host admins. Please read <a href=\"http://subtextproject.com/Home/FAQ/tabid/113/Default.aspx\" title=\"Subtext FAQ\">the FAQ</a> to reset your password.";
            }
            else
            {
                ResetAdminPassword(currentBlog);
            }
        }

        private void ResetAdminPassword(Blog currentBlog)
        {
            if (String.IsNullOrEmpty(currentBlog.Email) || currentBlog.Email != emailTextBox.Text || currentBlog.UserName != usernameTextBox.Text)
            {
                Message.Visible = false;
                FailureText.Visible = true;
                FailureText.Text = "Sorry, but the username and email provided did not match our records";
            }
            else
            {
                defaultInstructions.Visible = false;
                Message.Visible = true;
                FailureText.Visible = false;

                string newPassword = SecurityHelper.ResetPassword();
                EmailProvider.Instance().Send(currentBlog.Email, currentBlog.Email, "Your new password", "I've been told that you forgot your password. Here it is" +
                    Environment.NewLine + "\t" + newPassword);
                
                Message.Text = "Password was reset and sent to your email address.";
            }
        }
	}
}
