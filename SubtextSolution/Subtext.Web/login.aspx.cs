using System;
using System.Web.UI;
using Subtext.Framework.Configuration;
using Subtext.Web.Controls;

namespace Subtext.Web
{
	public partial class Login : Page
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

			System.Web.UI.WebControls.Login lc = (System.Web.UI.WebControls.Login) ControlHelper.FindControlRecursively(this.Master, "loginControl");
			lc.DestinationPageUrl = Config.CurrentBlog.AdminDirectoryVirtualUrl;

			base.OnInit(e);
		}
	}
}
