using System;
using System.Web.UI.WebControls;

namespace Subtext.Web.HostAdmin.UserControls
{
	public partial class UserChooser : System.Web.UI.UserControl
	{
		protected override void OnInit(EventArgs e)
		{
			this.createUserControl.Cancelled += OnCancelCreateUser;
			this.createUserControl.SaveComplete += OnSaveUserComplete;
			base.OnInit(e);
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

		protected void OnSaveUserComplete(object sender, EventArgs e)
		{
			ToggleShowCreateUser(false);
			usernameLiteral.Text = this.createUserControl.CreatedUserName;
			currentOwnerUpdatePanel.Update();
		}

		protected void OnCancelCreateUser(object sender, EventArgs e)
		{
			ToggleShowCreateUser(false);
		}

		protected void ShowCreateUser(object sender, EventArgs e)
		{
			ToggleShowCreateUser(true);
		}

		private void ToggleShowCreateUser(bool showCreateUser)
		{
			selectUserPlaceholder.Visible = !showCreateUser;
			createUserPlaceholder.Visible = showCreateUser;
		}
	}
}