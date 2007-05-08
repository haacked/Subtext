using System;
using System.Web.Security;
using System.ComponentModel;


namespace Subtext.Web.Controls.DataSources
{
	/// <summary>
	/// Inherits MembershipUser for the purposes of being bound to an ObjectDataSource.
	/// </summary>
	public class SubtextMembershipUser : MembershipUser
	{
		/// <summary>
		/// This constructor is used to create a MembershipUserWrapper from a MembershipUser object.  MembershipUser is a default type used
		/// in the Membership API provided with ASP.NET 2.0
		/// </summary>
		/// <param name="user">The user.</param>
		public SubtextMembershipUser(MembershipUser user) 
			: base(user.ProviderName
				, user.UserName
				, user.ProviderUserKey
				, user.Email
				, user.PasswordQuestion
				, user.Comment
				, user.IsApproved
				, user.IsLockedOut
				, user.CreationDate
				, user.LastLoginDate
				, user.LastActivityDate
				, user.LastPasswordChangedDate
				, user.LastLockoutDate)
		{
		}

		[DataObjectField(true)]
		public override string UserName
		{
			get { return base.UserName; }
		}


		public SubtextMembershipUser(string comment
			, DateTime creationDate
			, string email
			, bool approved
			, DateTime lastActivityDate
			, DateTime lastLoginDate
			, string passwordQuestion
			, object providerUserKey
			, string userName
			, DateTime lastLockoutDate
			, string providerName)
			: base(providerName
				, userName
				, providerUserKey
				, email
				, passwordQuestion
				, comment
				, approved
				, false
				, creationDate
				, lastLoginDate
				, lastActivityDate
				, DateTime.Now
				, lastLockoutDate)
		{
		}

	}

}

