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
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;

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
		/// Loads the host from the Object Provider. This is provided for 
		/// those cases when we really need to hit the data strore. Calling this
		/// method will also reload the HostInfo.Instance from the data store.
		/// </summary>
		/// <param name="suppressException">If true, won't throw an exception.</param>
		/// <returns></returns>
		public static HostInfo LoadHost(bool suppressException)
		{
			try
			{
                _instance = ObjectProvider.Instance().LoadHostInfo(new HostInfo());
			    return _instance;
			}
			catch(SqlException e)
			{
				// LoadHostInfo now executes the stored proc subtext_GetHost, instead of checking the table subtext_Host 
				if (e.Message.IndexOf("Invalid object name 'subtext_Host'") >= 0 || e.Message.IndexOf("Could not find stored procedure 'subtext_GetHost'") >= 0)
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
		/// Updates the host in the persistent store.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public static bool UpdateHost(HostInfo host)
		{
			if(ObjectProvider.Instance().UpdateHost(host))
			{
				_instance = host;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Creates the host in the persistent store.
		/// </summary>
		/// <returns></returns>
		public static bool CreateHost(string hostUserName, string hostPassword)
		{
			if(!InstallationManager.HostInfoRecordNeeded)
				throw new InvalidOperationException("Cannot create a Host record.  One already exists.");

			HostInfo host = new HostInfo();
			host.HostUserName = hostUserName;

			SetHostPassword(host, hostPassword);

		    return UpdateHost(host);
		}

		public static void SetHostPassword(HostInfo host, string newPassword)
		{
			host.Salt = SecurityHelper.CreateRandomSalt();
			if(Config.Settings.UseHashedPasswords)
			{
				string hashedPassword = SecurityHelper.HashPassword(newPassword, host.Salt);
				host.Password = hashedPassword;
			}
			else
				host.Password = newPassword;
		}

		/// <summary>
		/// Gets or sets the name of the host user.
		/// </summary>
		/// <value></value>
		public string HostUserName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the host password.
		/// </summary>
		/// <value></value>
		public string Password
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the salt.
		/// </summary>
		/// <value></value>
		public string Salt
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the date this record was created. 
		/// This is essentially the date that Subtext was 
		/// installed.
		/// </summary>
		/// <value></value>
		public DateTime DateCreated
		{
			get;
			set;
		}
	}
}
