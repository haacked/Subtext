using System;
using System.Security.Permissions;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Master page template for the host admin.
	/// </summary>
	[PrincipalPermission(SecurityAction.Demand, Role = "HostAdmins")]
	public partial class HostAdminTemplate : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				DataBind();
					
			if (hostAdminName != null && HostInfo.Instance != null)
			{
				hostAdminName.Text = HostInfo.Instance.HostUserName;
			}

			if (this.mnuImportStart != null)
			{
				this.mnuImportStart.Visible = (Config.ActiveBlogCount <= 0);
			}
		}
		
		/// <summary>
		/// Adds a control to the sidebar.
		/// </summary>
		/// <param name="control"></param>
		public void AddSidebarControl(Control control)
		{
			if(this.MPSidebar != null)
			{
				MPSidebar.Controls.Add(control);
			}
		}
	}
}
