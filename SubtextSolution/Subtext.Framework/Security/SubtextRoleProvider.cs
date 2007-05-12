using System;
using System.Configuration.Provider;
using System.Data;
using System.Web;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Properties;
using System.Globalization;

namespace Subtext.Framework.Security
{
	public class SubtextRoleProvider : System.Web.Security.RoleProvider
	{
		private string connectionString;

		public override void Initialize(string name, NameValueCollection config)
		{
			if (String.IsNullOrEmpty(name))
			{
				name = "SubtextMembershipProvider";
			}

			base.Initialize(name, config);

			string csn = config["connectionStringName"];
			if (string.IsNullOrEmpty(csn))
			{
				throw new HttpException(String.Format(CultureInfo.CurrentUICulture, Resources.HttpException_MissingAttribute, "connectionStringName"));
			}

			this.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[csn].ConnectionString;

            if (string.IsNullOrEmpty(this.connectionString))
            {
                throw new Exception(String.Format(CultureInfo.CurrentUICulture, Resources.Configuration_KeyNotFound, csn));
            }

			config.Remove("connectionStringName");
		}

		/// <summary>
		/// Gets or sets the name of the application to store 
		/// and retrieve role information for.
		/// </summary>
		/// <remarks>
		/// Since we support multiple "Applications" within a single Subtext 
		/// IIS application, we need to store the application name within 
		/// the HttpContext, so that one request does not interfere with another.
		/// </remarks>
		/// <value></value>
		/// <returns>The name of the application to store and 
		/// retrieve role information for.</returns>
		public override string ApplicationName
		{
			get
			{
				if (HttpContext.Current == null)
					return applicationName ?? "/";

				return (string)HttpContext.Current.Items["ApplicationName"] ?? "/";
			}
			set
			{
				if (HttpContext.Current != null)
					HttpContext.Current.Items["ApplicationName"] = value;
				applicationName = value;
			}
		}

		private string applicationName;

		/// <summary>
		/// Adds the specified user names to the specified roles for the configured applicationName.
		/// </summary>
		/// <param name="usernames">A string array of user names to be added to the specified roles.</param>
		/// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			SetUsersRoles("subtext_UsersInRoles_AddUsersToRoles", usernames, roleNames, true);
		}

		public override void CreateRole(string roleName)
		{
            if (roleName == null)
                throw new ArgumentNullException("roleName", "Role is null.");

			if (roleName.Length == 0)
                throw new ArgumentException("Cannot create an empty role name.", "roleName");

            if (roleName.Contains(","))
                throw new ArgumentException("Role cannot contain a comma.", "roleName");

            if (roleName.Length > 512)
                throw new ArgumentException("Role name is too long.", "roleName");

			int recordsAffected = SqlHelper.ExecuteNonQuery(this.connectionString, "subtext_Roles_CreateRole", ApplicationName, roleName);
			if (recordsAffected != 1)
			{
				throw new ProviderException(Resources.SecurityException_RoleExists);
			}
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			//no need to delete roles at this moment from within Subtext (v2) - RA
			throw new NotImplementedException(Resources.NotImplementedException_Generic);
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			//I've yet to figure out what the @!^&) this actually is for.
			throw new NotImplementedException(Resources.NotImplementedException_Generic);
		}

		/// <summary>
		/// Gets a list of all the roles for the configured applicationName.
		/// </summary>
		/// <returns>
		/// A string array containing the names of all the roles stored in the data source for the configured applicationName.
		/// </returns>
		public override string[] GetAllRoles()
		{
			using(SqlConnection conn = new SqlConnection(this.connectionString))
			using (SqlCommand cmd = new SqlCommand("subtext_Roles_GetAllRoles", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
				
				conn.Open();
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					StringCollection allRoles = new StringCollection();
					while (reader.Read())
					{
						allRoles.Add(reader.GetString(0));
					}
					string[] returnAllRoles = new string[allRoles.Count];
					allRoles.CopyTo(returnAllRoles, 0);
					return returnAllRoles;	
				}
			}
		}

		/// <summary>
		/// Gets a list of the roles that a specified user is in for the 
		/// configured applicationName.
		/// </summary>
		/// <param name="username">The user to return a list of roles for.</param>
		/// <returns>
		/// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
		/// </returns>
		public override string[] GetRolesForUser(string username)
		{
			StringCollection userRoles = new StringCollection();

			using (IDataReader reader = SqlHelper.ExecuteReader(this.connectionString, "subtext_UsersInRoles_GetRolesForUser", ApplicationName, username))
			{
				while (reader.Read())
				{
					userRoles.Add(reader.GetString(0));
				}
			}

			if(ApplicationName != "/" 
				&& !userRoles.Contains(RoleNames.Administrators)
				&& String.Equals(username, Config.CurrentBlog.Owner.UserName, StringComparison.InvariantCultureIgnoreCase))
			{
				//A blog owner is considered in the "Administrators" role for a 
				//blog even if the owner isn't explicitly a member of the "Administrators" 
				//role.
				userRoles.Add(RoleNames.Administrators);
			}

			string[] returnUserRoles = new string[userRoles.Count];
			userRoles.CopyTo(returnUserRoles, 0);
			return returnUserRoles;
		}

		public override string[] GetUsersInRole(string roleName)
		{
			StringCollection usersInRole = new StringCollection();
			using (IDataReader reader = SqlHelper.ExecuteReader(this.connectionString, "subtext_UsersInRoles_GetUsersInRoles", ApplicationName, roleName))
			{
				while (reader.Read())
				{
					usersInRole.Add(reader.GetString(0));
				}
			}
			string[] returnUsersInRole = new string[usersInRole.Count];
			usersInRole.CopyTo(returnUsersInRole, 0);
			return returnUsersInRole;
		}

		/// <summary>
		/// Gets a value indicating whether the specified user is 
		/// in the specified role for the configured applicationName.
		/// </summary>
		/// <param name="username">The user name to search for.</param>
		/// <param name="roleName">The role to search in.</param>
		/// <returns>
		/// true if the specified user is in the specified role for the 
		/// configured applicationName; otherwise, false.
		/// </returns>
		public override bool IsUserInRole(string username, string roleName)
		{
			SqlParameter returnValue = DataHelper.MakeReturnValueParam();

			SqlHelper.ExecuteNonQuery(this.connectionString
									  , CommandType.StoredProcedure
									  , "subtext_UsersInRoles_IsUserInRole"
									  , new SqlParameter("ApplicationName", ApplicationName)
									  , new SqlParameter("@UserName", username)
									  , new SqlParameter("@RoleName", roleName)
									  , returnValue);
			
			bool success = (1 == (int)returnValue.Value);
			if(!success)
			{
				//A blog owner is considered in the "Administrators" role for a 
				//blog even if the owner isn't explicitly a member of the "Administrators" 
				//role.
				if(String.Equals(roleName, RoleNames.Administrators, StringComparison.InvariantCultureIgnoreCase) && ApplicationName != "/")
				{
					//Since users are unique to an installation, this is a safe check.
					success = String.Equals(Config.CurrentBlog.Owner.UserName, username, StringComparison.InvariantCultureIgnoreCase);
				}
			}
			return success;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			SetUsersRoles("subtext_UsersInRoles_RemoveUsersFromRoles", usernames, roleNames, false);
		}

		private void SetUsersRoles(string storedProcName, string[] usernames, string[] roleNames, bool includeTime)
		{
			string delimitedUserNames = String.Join(",", usernames);
			string delimitedRoles = String.Join(",", roleNames);

			using (SqlConnection conn = new SqlConnection(this.connectionString))
			using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
				cmd.Parameters.AddWithValue("@UserNames", delimitedUserNames);
				cmd.Parameters.AddWithValue("@RoleNames", delimitedRoles);
				if (includeTime)
					cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow.ToUniversalTime());

				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Gets a value indicating whether the specified role name 
		/// already exists in the role data source for the configured applicationName.
		/// </summary>
		/// <param name="roleName">The name of the role to search for in the data source.</param>
		/// <returns>
		/// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
		/// </returns>
		public override bool RoleExists(string roleName)
		{
			SqlParameter returnValue = DataHelper.MakeReturnValueParam();

			SqlHelper.ExecuteNonQuery(this.connectionString
									  , CommandType.StoredProcedure
									  , "subtext_Roles_RoleExists"
									  , new SqlParameter("ApplicationName", ApplicationName)
									  , new SqlParameter("@RoleName", roleName)
									  , returnValue);

			return 1 == (int)returnValue.Value;
		}
	}
}
