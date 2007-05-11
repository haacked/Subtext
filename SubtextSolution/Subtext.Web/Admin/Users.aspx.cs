using System;
using System.Web.Security;
using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin
{
	public partial class Users : AdminOptionsPage
	{
		public Users()
		{
			this.TabSectionId = "Users";
		}

		protected void OnAddUserClick(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				string username = Membership.GetUserNameByEmail(emailTextBox.Text);
				if(String.IsNullOrEmpty(username))
				{
					emailExistsValidator.IsValid = false;
					emailExistsValidator.Validate();
					return;
				}

				foreach(string role in this.roleChooser.SelectedRoles)
				{
					Roles.AddUserToRole(username, role);
				}
			}
		}
	}
}
