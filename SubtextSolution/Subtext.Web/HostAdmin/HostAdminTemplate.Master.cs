using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.HostAdmin
{
	public partial class HostAdminTemplate : System.Web.UI.MasterPage
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
	}
}
