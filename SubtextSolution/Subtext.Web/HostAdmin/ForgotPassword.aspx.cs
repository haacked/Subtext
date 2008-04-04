using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using Subtext.Framework.Security;
using Subtext.Web.Controls;

namespace Subtext.Web.HostAdmin
{
	public partial class ForgotPassword : System.Web.UI.Page
	{
        MembershipApplicationScope scope;

		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
            PasswordRecovery pr = ControlHelper.FindControlRecursively(this.Master, "passwordRecovery") as PasswordRecovery;
            pr.VerifyingUser += new LoginCancelEventHandler(pr_VerifyingUser);
            pr.Disposed += new EventHandler(pr_Disposed);

			base.OnInit(e);
		}

        private void pr_Disposed(object sender, EventArgs e)
        {
            if (scope != null)
            {
                scope.Dispose();
            }
        }

        private void pr_VerifyingUser(object sender, LoginCancelEventArgs e)
        {
            scope = MembershipApplicationScope.SetApplicationName("/");
        }
	}
}
