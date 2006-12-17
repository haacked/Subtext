#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Data.SqlClient;
using System.Web.Security;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;
using Subtext.Framework.Properties;

namespace Subtext.Framework
{
	/// <summary>
	/// Represents the system that hosts the blogs within this 
	/// Subtext installation.  This is a Singleton.
	/// </summary>
	public sealed class HostInfo
	{
		static HostInfo _instance = LoadHost(true);
		
	    private HostInfo()
	    {
	    }
	    
		/// <summary>
		/// Returns an instance of <see cref="HostInfo"/> used to 
		/// describe this installation of Subtext.
		/// </summary>
		/// <returns></returns>
		public static HostInfo Instance
		{
			get
			{
				return _instance;
			}
		}

		/// <summary>
		/// Changes the password for the Host Owner.
		/// </summary>
		/// <param name="oldPassword">The old password.</param>
		/// <param name="newPassword">The new password.</param>
		public void SetPassword(string oldPassword, string newPassword)
		{
			Membership.Provider.ChangePassword(Owner.UserName, oldPassword, newPassword);
		}

		/// <summary>
		/// Gets the owner of the subtext installation. 
		/// This person is known as THE HostAdmin.
		/// </summary>
		/// <value>The owner.</value>
		public MembershipUser Owner
		{
			get
			{
				if(this.owner == null && ownerId != Guid.Empty)
				{
					using(MembershipApplicationScope.SetApplicationName("/"))
					{
						this.owner = Membership.GetUser(ownerId);
					}
				}
				return this.owner;
			}
		}

		MembershipUser owner;

		internal Guid ownerId = Guid.Empty;

		/// <summary>
		/// Gets a value indicating whether the HostInfo table exists.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if host info table exists; otherwise, <c>false</c>.
		/// </value>
		public static bool HostInfoTableExists
		{
			get
			{
				try
				{
					LoadHost(false);
					return true;
				}
				catch(HostDataDoesNotExistException)
				{
					return false;
				}
			}
		}

		/// <summary>
		/// The Membership Application ID for the Host.
		/// </summary>
		public Guid ApplicationId
		{
			get { return this.applicationId; }
			set { this.applicationId = value; }
		}

		Guid applicationId;

		/// <summary>
		/// Loads the host from the Object Provider.  This is provided 
		/// for those cases when we really need to hit the db.
		/// </summary>
		/// <param name="suppressException">If true, won't throw an exception.</param>
		/// <returns></returns>
		public static HostInfo LoadHost(bool suppressException)
		{
			try
			{
				return ObjectProvider.Instance().LoadHostInfo(new HostInfo());
			}
			catch(SqlException e)
			{
				if(e.Message.IndexOf("Invalid object name 'subtext_Host'") >= 0)
				{
					if(suppressException)
						return null;
					else
						throw new HostDataDoesNotExistException();
				}
				else
					throw;
			}
		}

		/// <summary>
		/// Creates the host in the persistent store.
		/// </summary>
		/// <returns></returns>
		public static bool CreateHost(string hostUserName, string hostPassword, string email)
		{
			if(!InstallationManager.HostInfoRecordNeeded)
				throw new InvalidOperationException(Resources.InvalidOperation_HostRecordExists);

			HostInfo host = new HostInfo();
			
			string passwordSalt = SecurityHelper.CreateRandomSalt();
			
			if (Membership.Provider.PasswordFormat == MembershipPasswordFormat.Hashed)
				hostPassword = SecurityHelper.HashPassword(hostPassword, passwordSalt);
			
			host = ObjectProvider.Instance().CreateHost(host, hostUserName, hostPassword, passwordSalt, email);
			
			using(IDisposable scope = MembershipApplicationScope.SetApplicationName("/"))
			{
				if(!Roles.RoleExists("HostAdmins"))
					Roles.CreateRole("HostAdmins");
				
				Roles.AddUserToRole(host.Owner.UserName, "HostAdmins");
				scope.Dispose(); //Just to make sure it stays alive.
			}
			
			return true;
		}

		/// <summary>
		/// Changes the Host Owner password.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="oldPassword">The old password.</param>
		/// <param name="newPassword">The new password.</param>
		public static void ChangePassword(HostInfo host, string oldPassword, string newPassword)
		{
            if (host == null)
            {
                throw new ArgumentNullException("host", Resources.ArgumentNull_Generic);
            }

            if (oldPassword == null)
            {
                throw new ArgumentNullException("oldPassword", Resources.ArgumentNull_String);
            }

            if (newPassword == null)
            {
                throw new ArgumentNullException("newPassword", Resources.ArgumentNull_String);
            }

            if (oldPassword.Length == 0)
            {
                throw new ArgumentException(Resources.Argument_StringZeroLength, "oldPassword");
            }

            if (newPassword.Length == 0)
            {
                throw new ArgumentException(Resources.Argument_StringZeroLength, "newPassword");
            }

            //Make sure we can grab the host admin.
			using (MembershipApplicationScope.SetApplicationName("/"))
			{
				Membership.Provider.ChangePassword(host.Owner.UserName, oldPassword, newPassword);
			}
		}

		/// <summary>
		/// Gets or sets the name of the host user.
		/// </summary>
		/// <value></value>
		public string HostUserName
		{
			get { return this.Owner.UserName; }
		}

		/// <summary>
		/// Gets or sets the date this record was created. 
		/// This is essentially the date that Subtext was 
		/// installed.
		/// </summary>
		/// <value></value>
		public DateTime DateCreated
		{
			get { return _dateCreated; }
			set { _dateCreated = value; }
		}

		DateTime _dateCreated;
	}
}
