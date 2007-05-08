using System;
using System.Web.UI.WebControls;

namespace Subtext.Web.HostAdmin.UserControls
{
	public partial class UserChooser : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public string UserName
		{
			get { return (string)ViewState["UserName"] ?? string.Empty; }
			set { this.ViewState["UserName"] = value; }
		}

		protected void OnUserSelected(object sender, EventArgs e)
		{
			GridViewRow row = this.usersGrid.Rows[this.usersGrid.SelectedIndex];
			this.UserName = row.Cells[2].Text;
			if (String.IsNullOrEmpty(this.UserName))
				throw new InvalidOperationException(
					"The username is empty. Most likely the cell index is off within the 'OnUserSelected' method.");

			usernameLiteral.Text = this.UserName;
			currentOwnerUpdatePanel.Update();
		}
	}
}