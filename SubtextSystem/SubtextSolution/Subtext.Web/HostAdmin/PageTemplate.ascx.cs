using System;
using Subtext.Framework;

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
		protected System.Web.UI.WebControls.Literal hostAdminName;

		private void InitializeComponent()
		{
		
		}

		protected override void OnLoad(EventArgs e)
		{
			if(!IsPostBack)
				DataBind();
			base.OnLoad (e);
		}

		/// <summary>
		/// Gets the name of the host user.
		/// </summary>
		/// <value>The name of the host user.</value>
		protected string HostUserName
		{
			get
			{
				return HostInfo.Instance.HostUserName;
			}
		}

	}
}
