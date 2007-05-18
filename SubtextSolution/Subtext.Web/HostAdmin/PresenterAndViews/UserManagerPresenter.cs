using System;
using System.Web.Security;
using Subtext.Web.HostAdmin.PresenterAndViews;
using Subtext.Web.UI;

namespace Subtext.Web.HostAdmin.Presenters
{
	public class UserManagerPresenter : PresenterBase<IUserManagerView>
	{
		private MembershipProvider provider;

		public UserManagerPresenter(IUserManagerView view, MembershipProvider provider) : base(view)
		{
			this.provider = provider;
		}

		/// <summary>
		/// Called by the Init method. Presenters that implement this 
		/// base class should subscribe to their view specific 
		/// events, if any, when overriding this method.
		/// </summary>
		protected override void SubscribeToViewEvents()	{}

		public void SetFilter(string filter)
		{
            if (filter == string.Empty || String.Equals(filter, "all", StringComparison.InvariantCultureIgnoreCase))
            {
                View.CurrentFilter = null;
            }
            else
            {
                View.CurrentFilter = filter;
            }

			View.SelectedIndex = -1;
		}

		protected override void OnPreRender(object sender, EventArgs e)
		{
            int totalRecords;
			if(!String.IsNullOrEmpty(View.CurrentFilter))
			{
				View.Users = provider.FindUsersByName(View.CurrentFilter + "%", 0, View.PageSize, out totalRecords);
			}
			else
			{
				View.Users = provider.GetAllUsers(0, View.PageSize, out totalRecords);
			}

			View.DataBind();
		}

		public void SetSelectedUserName(string username)
		{
			View.SelectedUserName = username;
		}

		/// <summary>
		/// Changing whether or not the user is a member of the role.
		/// </summary>
		/// <param name="role"></param>
		/// <param name="isMemberOf"></param>
		public void ChangeRole(string role, bool isMemberOf)
		{
            if (isMemberOf)
            {
                Roles.AddUserToRole(this.View.SelectedUserName, role);
            }
            else
            {
                Roles.RemoveUserFromRole(this.View.SelectedUserName, role);
            }
		}
	}
}
