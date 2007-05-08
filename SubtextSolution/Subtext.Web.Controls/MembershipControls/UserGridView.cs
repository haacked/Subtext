using System;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls.MembershipControls
{
	public class UserGridView : GridView
	{
		protected override void OnInit(EventArgs e)
		{
			this.AutoGenerateColumns = false;
			this.AllowPaging = true;
			this.DataKeyNames = new string[] {"ProviderUserKey"};
			this.AlternatingRowStyle.CssClass = "alt";
			this.PagerStyle.CssClass = "gridPagerStyle";
			this.PagerStyle.HorizontalAlign = HorizontalAlign.Right;
			this.HeaderStyle.CssClass = "header";
			this.SelectedRowStyle.CssClass = "selected";
			base.OnInit(e);
		}
	}
}
