using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Security;

namespace Subtext.Web.Controls.DataSources
{
	[DataObject(true)]
	public static class MembershipDataLayer
	{
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static IList<SubtextMembershipUser> Select(int pageIndex, int pageSize)
		{
			int totalRecords;
			MembershipUserCollection users = Membership.GetAllUsers(pageIndex, pageSize, out totalRecords);
			
			//Store this sucker so we can pull it from the SelectCount method.
			HttpContext.Current.Items["rowCount"] = totalRecords;
			List<SubtextMembershipUser> subtextUsers = new List<SubtextMembershipUser>();

			foreach (MembershipUser user in users)
			{
				subtextUsers.Add(new SubtextMembershipUser(user));
			}
			return subtextUsers;
		}

		/// <summary>
		/// Returns the total count of records.
		/// </summary>
		/// <returns></returns>
		public static int SelectCount()
		{
			return (int)HttpContext.Current.Items["rowCount"];
		}

		/// <summary>
		/// Inserts a new user.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="approved">if set to <c>true</c> [is approved].</param>
		/// <param name="lastLockoutDate">The last lockout date.</param>
		/// <param name="creationDate">The creation date.</param>
		/// <param name="email">The email.</param>
		/// <param name="lastActivityDate">The last activity date.</param>
		/// <param name="providerName">Name of the provider.</param>
		/// <param name="isLockedOut">if set to <c>true</c> [is locked out].</param>
		/// <param name="lastLoginDate">The last login date.</param>
		/// <param name="isOnline">if set to <c>true</c> [is online].</param>
		/// <param name="lastPasswordChangedDate">The last password changed date.</param>
		/// <param name="password">The password.</param>
		[DataObjectMethod(DataObjectMethodType.Insert, true)]
		static public void Insert(string userName
			, bool approved
			, DateTime lastLockoutDate
			, DateTime creationDate
			, string email
			, DateTime lastActivityDate
			, string providerName
			, bool isLockedOut
			, DateTime lastLoginDate
			, bool isOnline
			, DateTime lastPasswordChangedDate
			, string password)
		{

			MembershipCreateStatus status;
			Membership.CreateUser(userName, password, email, null, null, approved, out status);

			if (status != MembershipCreateStatus.Success)
				throw new MembershipCreateUserException(status);
		}

		/// <summary>
		/// Deletes the specified user.
		/// </summary>
		/// <param name="userName"></param>
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		static public void Delete(string userName)
		{
			Membership.DeleteUser(userName, true);
		}

		/// <summary>
		/// Updates the user by userName.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="email">The email.</param>
		/// <param name="approved">if set to <c>true</c> [is approved].</param>
		/// <param name="lastActivityDate">The last activity date.</param>
		/// <param name="lastLoginDate">The last login date.</param>
		[DataObjectMethod(DataObjectMethodType.Update, true)]
		static public void Update(string userName
			, string email
			, bool approved
			, DateTime lastActivityDate
			, DateTime lastLoginDate)
		{
			bool isDirty = false;

			MembershipUser user = Membership.GetUser(userName);

			if (!String.Equals(user.Email, email, StringComparison.InvariantCultureIgnoreCase))
			{
				isDirty = true;
				user.Email = email;
			}

			if (user.IsApproved != approved)
			{
				isDirty = true;
				user.IsApproved = approved;
			}

			if (isDirty)
				Membership.UpdateUser(user);
		}

		/// <summary>
		/// Toggles approved status.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="approved">if set to <c>true</c> [approved].</param>
		[DataObjectMethod(DataObjectMethodType.Update, false)]
		static public void Update(string userName, bool approved)
		{
			MembershipUser user = Membership.GetUser(userName);

			if (user.IsApproved != approved)
			{
				user.IsApproved = approved;
				Membership.UpdateUser(user);
			}
		}
	}
}
