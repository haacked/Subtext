using System;

namespace Subtext.Web
{
	public partial class Login : System.Web.UI.Page
	{
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
		///</summary>
		///<param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			if (Request.QueryString != null && Request.QueryString["ReturnUrl"] != null && Request.QueryString["ReturnUrl"].IndexOf("HostAdmin") >= 0)
			{
				Response.Redirect("~/HostAdmin/Login.aspx");
				return;
			}

			Master.DestinationUrl = "~/Admin/";
			base.OnInit(e);
		}
	}
}
