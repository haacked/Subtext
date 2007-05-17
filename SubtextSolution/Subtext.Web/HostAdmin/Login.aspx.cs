using System;
using System.Web.UI.WebControls;
using System.Web.Security;
using Subtext.Web.Controls;
using Subtext.Framework.Security;

namespace Subtext.Web.HostAdmin
{
	public partial class Login : System.Web.UI.Page
	{
        private MembershipApplicationScope scope;

		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
            System.Web.UI.WebControls.Login lc = ControlHelper.FindControlRecursively(this.Master, "loginControl") as System.Web.UI.WebControls.Login;
            lc.LoggingIn += new LoginCancelEventHandler(lc_LoggingIn);
            lc.Disposed += new EventHandler(lc_Disposed);

            base.OnInit(e);
		}

        void lc_Disposed(object sender, EventArgs e)
        {
            if (scope != null)
            {
                scope.Dispose();
            }
        }

        void lc_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            scope = MembershipApplicationScope.SetApplicationName("/");
        }
	}
}
