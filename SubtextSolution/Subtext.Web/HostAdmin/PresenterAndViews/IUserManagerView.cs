using System;
using System.Web.Security;
using Subtext.Web.UI;

namespace Subtext.Web.HostAdmin.PresenterAndViews
{
	public interface IUserManagerView : IView
	{
		/// <summary>
		/// The current letter filter, if any, used to filter 
		/// users names.
		/// </summary>
		string CurrentFilter { get; set; }

		/// <summary>
		/// The users to display in the grid.
		/// </summary>
		MembershipUserCollection Users { get; set;}

		/// <summary>
		/// Size of the page of users.
		/// </summary>
		int PageSize { get; set;}

		/// <summary>
		/// Current selected index of the grid.
		/// </summary>
		int SelectedIndex { get; set; }

		/// <summary>
		/// The username for the selected user .
		/// </summary>
		string SelectedUserName { get; set;}
	}

	public class FilterClickEventArgs : EventArgs
	{		
		public FilterClickEventArgs(string filter)
		{
			this.filter = filter;
		}

		public string Filter
		{
			get { return this.filter; }
		}

		private string filter;
	}
}
