using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Summary description for PageTemplate.
	/// </summary>
	public class PageTemplate : System.Web.UI.UserControl
	{
		protected Subtext.Web.Controls.ScriptTag scrHelpTipJavascript;
		protected Subtext.Web.Controls.ContentRegion MPSectionTitle;
		protected Subtext.Web.Controls.MenuItem Menuitem1;
		protected Subtext.Web.Controls.MenuItem Menuitem2;
		protected Subtext.Web.Controls.ContentRegion MPSidebar;
		protected Subtext.Web.Controls.ContentRegion MPContent;
		protected System.Web.UI.HtmlControls.HtmlAnchor logoutLink;
		protected Subtext.Web.Controls.MenuItem Menuitem3;
		protected System.Web.UI.WebControls.Literal hostAdminName;

		protected override void OnLoad(EventArgs e)
		{
			if(!IsPostBack)
				DataBind();
			base.OnLoad (e);
			hostAdminName.Text = HostInfo.Instance.HostUserName;

			Menuitem2.Visible = (Config.ActiveBlogCount <= 0);
		}
	}
}
