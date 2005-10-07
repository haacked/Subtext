using System;
using Subtext.Framework;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Summary description for PageTemplate.
	/// </summary>
	public class PageTemplate : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Literal hostAdminName;

		protected override void OnLoad(EventArgs e)
		{
			hostAdminName.Text = HostInfo.Instance.HostUserName;
			base.OnLoad (e);
		}

	}
}
