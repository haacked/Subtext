using System;
using System.Web.Security;

namespace Subtext.Web.HostAdmin
{
	public partial class Login : System.Web.UI.Page
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			Membership.ApplicationName = Roles.ApplicationName = "/";
			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Write(Page.User.Identity.Name + "<br />");
			Response.Write(Page.User.IsInRole("Administrators") + "<br />");
			Response.Write(Page.User.IsInRole("HostAdmins") + "<br />");
			Response.Write(Membership.ApplicationName + "<br />");
		}
	}
}
