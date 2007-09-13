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
using System.Diagnostics;
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
		private static readonly HostInfo _instance = new HostInfo();
		
		static HostInfo()
		{
			LoadHost(true);
		}

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
				Debug.Assert(_instance != null, "An assertion failed. The HostInfo singleton should never be null.");
				return _instance;
			}
		}

        MembershipUser owner;
        internal Guid _ownerId = Guid.Empty;
        /// <summary>
		/// Gets the owner of the subtext installation. 
		/// This person is known as THE HostAdmin.
		/// </summary>
		/// <value>The owner.</value>
		public MembershipUser Owner
		{
			get
			{
				if(owner == null && _ownerId != Guid.Empty)
				{
					using(MembershipApplicationScope.SetApplicationName("/"))
					{
						owner = Membership.GetUser(_ownerId);
					}
				}
				return owner;
			}
		}

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

        Guid _applicationId;
		/// <summary>
		/// The Membership Application ID for the Host.
		/// </summary>
		public Guid ApplicationId
		{
			get { return _applicationId; }
			set { _applicationId = value; }
		}

		/// <summary>
		/// Loads the host from the Object Provider.  This is provided 
		/// for those cases when we really need to hit the db.
		/// </summary>
		/// <param name="suppressException">If true, won't throw an exception.</param>
		/// <returns></returns>
		public static void LoadHost(bool suppressException)
		{
			try
			{
				ObjectProvider.Instance().LoadHostInfo(_instance);
			}
			catch(SqlException e)
			{
                if (e.Message.IndexOf("Invalid object name 'subtext_Host'") >= 0)
                {
                    if (suppressException)
                    {
                    	return;
                    }
                    else
                    {
                        throw new HostDataDoesNotExistException();
                    }
                }
                else
                {
                    throw;
                }
			}
		}

		/// <summary>
		/// Creates the host in the persistent store.
		/// </summary>
		/// <returns></returns>
		public static void CreateHost(MembershipUser owner)
		{
            if (!InstallationManager.HostInfoRecordNeeded)
                throw new InvalidOperationException(Resources.InvalidOperation_HostRecordExists);

			ObjectProvider.Instance().CreateHost(owner, _instance);

			using (MembershipApplicationScope.SetApplicationName("/"))
			{
				if (!Roles.RoleExists(RoleNames.HostAdmins))
				{
					Roles.CreateRole(RoleNames.HostAdmins);
				}
				Roles.AddUserToRole(owner.UserName, RoleNames.HostAdmins);
			}
		}

		/// <summary>
		/// Gets or sets the name of the host user.
		/// </summary>
		/// <value></value>
		public string HostUserName
		{
			get { return Owner.UserName; }
		}


        DateTime _dateCreated = NullValue.NullDateTime;
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
	}
}
