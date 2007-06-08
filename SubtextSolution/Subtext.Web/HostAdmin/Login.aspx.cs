using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Security;
using Subtext.Web.Controls;

namespace Subtext.Web.HostAdmin
{
	public partial class Login : Page
	{
        private MembershipApplicationScope scope;

		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			System.Web.UI.WebControls.Login lc = (System.Web.UI.WebControls.Login) ControlHelper.FindControlRecursively(this.Master, "loginControl");
            lc.LoggingIn += new LoginCancelEventHandler(lc_LoggingIn);
			lc.DestinationPageUrl = "~/HostAdmin/Default.aspx";
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
