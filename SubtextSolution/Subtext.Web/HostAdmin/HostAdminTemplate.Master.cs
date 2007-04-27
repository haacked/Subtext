using System;
using Subtext.Framework.Configuration;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Master page template for the host admin.
	/// </summary>
	public partial class HostAdminTemplate : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				DataBind();
					
			if (this.mnuImportStart != null)
			{
				this.mnuImportStart.Visible = (Config.ActiveBlogCount <= 0);
			}
		}
	}
}
