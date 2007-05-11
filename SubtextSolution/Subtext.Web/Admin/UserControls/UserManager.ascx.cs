using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using Subtext.Web.HostAdmin.PresenterAndViews;
using Subtext.Web.HostAdmin.Presenters;

namespace Subtext.Web.Admin.UserControls
{
	public partial class UserManager : System.Web.UI.UserControl, IUserManagerView
	{
		private UserManagerPresenter presenter;

		public UserManager()
		{
			this.presenter = new UserManagerPresenter(this, Membership.Provider);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public string Title
		{
			get { return (string)this.ViewState["Title"] ?? "Roles"; }
			set { this.ViewState["Title"] = value; }
		}

		/// <summary>
		/// The current letter filter, if any, used to filter 
		/// users names.
		/// </summary>
		public string CurrentFilter
		{
			get { return this.currentFilter; }
			set { this.currentFilter = value; }
		}

		private string currentFilter;

		/// <summary>
		/// The users to display in the grid.
		/// </summary>
		public MembershipUserCollection Users
		{
			get { return this.users; }
			set { this.users = value; }
		}

		private MembershipUserCollection users;

		/// <summary>
		/// Size of the page of users.
		/// </summary>
		public int PageSize
		{
			get { return this.pageSize; }
			set { this.pageSize = value; }
		}

		/// <summary>
		/// Current selected index of the grid.
		/// </summary>
		public int SelectedIndex
		{
			get { return this.usersGrid.SelectedIndex; }
			set { this.usersGrid.SelectedIndex = value; }
		}

		/// <summary>
		/// The provider id for the selected user .
		/// </summary>
		public string SelectedUserName
		{
			get { return (string)ViewState["SelectedUserName"]; }
			set { ViewState["SelectedUserName"] = value; }
		}

		private int pageSize = 10; //Default.

		public override void DataBind()
		{
			this.usersGrid.DataSource = this.users;
			this.usersGrid.DataBind();
			BindAlphabet();

			if (SelectedUserName != null)
			{
				this.rolesRepeater.DataSource = Roles.GetAllRoles();
				this.userInfo.DataBind();
			}
		}

		/// <summary>
		/// True if the validation controls all return valid.
		/// </summary>
		public bool IsValid
		{
			get { return this.Page.IsValid; }
		}

		//Probably should go in a resource.
		private static string[] alphabet = { "A", "B", "C", "D", "E", "F"
			, "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R"
			, "S", "T", "U", "V", "W", "X", "Y", "Z", "All" };

		void BindAlphabet()
		{
			this.alphabetRepeater.DataSource = alphabet;
			this.alphabetRepeater.DataBind();
		}

		public void OnLetterClick(object sender, RepeaterCommandEventArgs e)
		{
			string letter = e.CommandArgument.ToString();
			this.presenter.SetFilter(letter);
		}

		protected void OnSearchClick(object sender, EventArgs e)
		{
			Page.Validate("ValGroupSearchByTerm");
			//TODO:
		}

		protected void OnViewUserDetailClick(object sender, EventArgs e)
		{
			LinkButton button = (LinkButton)sender;
			GridViewRow item = (GridViewRow)button.Parent.Parent;
			usersGrid.SelectedIndex = item.DataItemIndex;
			string username = button.CommandArgument;
			presenter.SetSelectedUserName(username);
		}

		protected void OnRoleMembershipChanged(object sender, EventArgs e)
		{
			CheckBox roleCheckBox = (CheckBox)sender;
			if (roleCheckBox == null)
				throw new InvalidOperationException("We need a checkbox named 'roleCheckBox' on this page.");

			this.presenter.ChangeRole(roleCheckBox.Text, roleCheckBox.Checked);
		}

		protected void OnResetPasswordClick(object sender, EventArgs e)
		{
		}

		protected void OnDeleteUserClick(object sender, EventArgs e)
		{
		}
	}
}