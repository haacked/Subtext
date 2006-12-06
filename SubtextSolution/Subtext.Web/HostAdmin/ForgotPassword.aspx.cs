using System;
using System.Web.Security;

namespace Subtext.Web.HostAdmin
{
	public partial class ForgotPassword : System.Web.UI.Page
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			//TODO: Shouldn't have to do this.
			Membership.ApplicationName = Roles.ApplicationName = "/";
			base.OnInit(e);
		}
	}
}
