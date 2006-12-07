using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework.Data;

namespace Subtext.Framework.Security
{
    /// <summary>
    /// Custom Membership Provider for Subtext users.
    /// </summary>
    public class SubtextMembershipProvider : MembershipProvider
    {
        private string connectionString;
        private NameValueCollection _config;

		/// <summary>
		/// Initializes a new instance of the <see cref="SubtextMembershipProvider"/> class.
		/// </summary>
        public SubtextMembershipProvider() : base()
        {
        }

		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="name">The friendly name of the provider.</param>
		/// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
		/// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"></see> on a provider after the provider has already been initialized.</exception>
		/// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            _config = config;
            if (string.IsNullOrEmpty(name))
                name = "SubtextMembershipProvider";
            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (string.IsNullOrEmpty(connectionStringName))
                throw new HttpException("Missing attribute 'connectionStringName'");

            this.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            if (string.IsNullOrEmpty(this.connectionString))
                throw new InvalidOperationException("The connection string '" + connectionStringName + "' was not found");

            config.Remove("connectionStringName");
        }

		/// <summary>
		/// The name of the application using the custom membership provider.
		/// </summary>
		/// <value></value>
		/// <returns>The name of the application using the custom membership provider.</returns>
        public override string ApplicationName
        {
            get
            {
				return this.applicationName ?? "/";
			}
            set
            {
				this.applicationName = value;
            }
        }
		string applicationName;

		/// <summary>
		/// Processes a request to update the password for a membership user.
		/// </summary>
		/// <param name="username">The user to update the password for.</param>
		/// <param name="oldPassword">The current password for the specified user.</param>
		/// <param name="newPassword">The new password for the specified user.</param>
		/// <returns>
		/// true if the password was updated successfully; otherwise, false.
		/// </returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
			if(!ValidateUser(username, oldPassword))
				return false;
			
			string passwordSalt = SecurityHelper.CreateRandomSalt();
			if(PasswordFormat == MembershipPasswordFormat.Hashed)
			{
				newPassword = SecurityHelper.HashPassword(newPassword);
			}

			int recordsAffected = SqlHelper.ExecuteNonQuery(this.connectionString, "subtext_Membership_SetPassword", applicationName, username, newPassword, passwordSalt, DateTime.UtcNow, Membership.Provider.PasswordFormat);
			return recordsAffected > 0;
        }

		/// <summary>
		/// Processes a request to update the password question and answer for a membership user.
		/// </summary>
		/// <param name="username">The user to change the password question and answer for.</param>
		/// <param name="password">The password for the specified user.</param>
		/// <param name="newPasswordQuestion">The new password question for the specified user.</param>
		/// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
		/// <returns>
		/// true if the password question and answer are updated successfully; otherwise, false.
		/// </returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
			throw new NotImplementedException("The method or operation is not implemented.");
        }

		/// <summary>
		/// Adds a new membership user to the data source.
		/// </summary>
		/// <param name="username">The user name for the new user.</param>
		/// <param name="password">The password for the new user, in cleartext.</param>
		/// <param name="email">The e-mail address for the new user.</param>
		/// <param name="passwordQuestion">The password question for the new user.</param>
		/// <param name="passwordAnswer">The password answer for the new user</param>
		/// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
		/// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
		/// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus"></see> enumeration value indicating whether the user was created successfully.</param>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the information for the newly created user.
		/// </returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && !String.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

			if (GetUser(username, false) != null)
            {
            	status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (providerUserKey == null)
            {
                providerUserKey = Guid.NewGuid();
            }
            else
            {
                if (!(providerUserKey is Guid))
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return null;
                }
            }
            
        	string salt = SecurityHelper.CreateRandomSalt();
			if(PasswordFormat == MembershipPasswordFormat.Hashed)
        		password = SecurityHelper.HashPassword(password, salt);
        	
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("subtext_Membership_CreateUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
					cmd.Parameters.AddWithValue("@UserName", username);
					cmd.Parameters.AddWithValue("@Password", password);
					cmd.Parameters.AddWithValue("@PasswordSalt", salt);
					cmd.Parameters.AddWithValue("@Email", email);
					cmd.Parameters.AddWithValue("@PasswordQuestion", passwordQuestion);
					cmd.Parameters.AddWithValue("@PasswordAnswer", passwordAnswer);
					cmd.Parameters.AddWithValue("@IsApproved", true);
					cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow);
					cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
					cmd.Parameters.AddWithValue("@UniqueEmail", RequiresUniqueEmail);
					cmd.Parameters.AddWithValue("@PasswordFormat", PasswordFormat);
					cmd.Parameters.Add(DataHelper.MakeOutParam("@UserId", SqlDbType.UniqueIdentifier, 4));
					cmd.Parameters["@UserId"].Value = providerUserKey;

					conn.Open();
					if (cmd.ExecuteNonQuery() >= 1) //Records affected.
					{
						status = MembershipCreateStatus.Success;
						return GetUser(username, true);
					}
					else
					{
						status = MembershipCreateStatus.ProviderError;
						return null;
					}
				}
			}
        }

		/// <summary>
		/// Removes a user from the membership data source.
		/// </summary>
		/// <param name="username">The name of the user to delete.</param>
		/// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
		/// <returns>
		/// true if the user was successfully deleted; otherwise, false.
		/// </returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("subtext_Users_DeleteUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@ApplicationName", "\\");
					cmd.Parameters.AddWithValue("@UserName", username);
					cmd.Parameters.AddWithValue("@TablesToDeleteFrom", deleteAllRelatedData ? 15 : 1);
					cmd.Parameters.Add(DataHelper.MakeOutParam("@NumTablesDeletedFrom", SqlDbType.Int, 4));

					conn.Open();
					cmd.ExecuteNonQuery();
					return (int)cmd.Parameters["@NumTablesDeletedFrom"].Value >= 1;
				}
			}
        }

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to reset their passwords.
		/// </summary>
		/// <value></value>
		/// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        public override bool EnablePasswordReset
        {
            get
            {
                return Convert.ToBoolean(_config["enablePasswordReset"]);
            }
        }

		/// <summary>
		/// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
		/// </summary>
		/// <value></value>
		/// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return Convert.ToBoolean(_config["enablePasswordRetrieval"]);
            }
        }

		/// <summary>
		/// Gets a collection of membership users where the e-mail address 
		/// contains the specified e-mail address to match.
		/// </summary>
		/// <param name="emailToMatch">The e-mail address to search for.</param>
		/// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">The total number of matched users.</param>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.
		/// </returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
			if (String.IsNullOrEmpty(emailToMatch))
				throw new ArgumentNullException("emailToMatch", "Must specify an email to match.");

			MembershipUserCollection foundUsers = new MembershipUserCollection();

			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				conn.Open();
				SqlParameter totalCountParam = DataHelper.MakeOutParam("@TotalCount", SqlDbType.Int, 4);

				using (SqlCommand command = new SqlCommand("subtext_Membership_FindUsersByEmail", conn))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@ApplicationName", ApplicationName));
					command.Parameters.Add(new SqlParameter("@EmailToMatch", emailToMatch));
					command.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
					command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
					command.Parameters.Add(totalCountParam);
					
					using (IDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							foundUsers.Add(LoadUserFromReader(reader));
						}
						reader.Close();
						totalRecords = (int)command.Parameters["@TotalCount"].Value;
					}
				}
			}
			
			return foundUsers;
        }

		/// <summary>
		/// Gets a collection of membership users where the user name contains the specified user name to match.
		/// </summary>
		/// <param name="usernameToMatch">The user name to search for.</param>
		/// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">The total number of matched users.</param>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.
		/// </returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
			if (String.IsNullOrEmpty(usernameToMatch))
				throw new ArgumentNullException("usernameToMatch", "Must specify a username to match.");

			MembershipUserCollection foundUsers = new MembershipUserCollection();
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				conn.Open();
				SqlParameter totalCountParam = DataHelper.MakeOutParam("@TotalCount", SqlDbType.Int, 4);

				using (SqlCommand command = new SqlCommand("subtext_Membership_FindUsersByName", conn))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@ApplicationName", ApplicationName));
					command.Parameters.Add(new SqlParameter("@UserNameToMatch", usernameToMatch));
					command.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
					command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
					command.Parameters.Add(totalCountParam);

					using (IDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							foundUsers.Add(LoadUserFromReader(reader));
						}
						reader.Close();
						totalRecords = (int)command.Parameters["@TotalCount"].Value;
					}
				}
			}
			return foundUsers;
        }

		/// <summary>
		/// Gets a collection of all the users in the data source in pages of data.
		/// </summary>
		/// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
		/// <param name="pageSize">The size of the page of results to return.</param>
		/// <param name="totalRecords">The total number of matched users.</param>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUserCollection"></see> collection that contains a page of pageSize<see cref="T:System.Web.Security.MembershipUser"></see> objects beginning at the page specified by pageIndex.
		/// </returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection foundUsers = new MembershipUserCollection();
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				conn.Open();
				SqlParameter totalCountParam = DataHelper.MakeOutParam("@TotalCount", SqlDbType.Int, 4);

				using (SqlCommand command = new SqlCommand("subtext_Membership_GetAllUsers", conn))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@ApplicationName", ApplicationName));
					command.Parameters.Add(new SqlParameter("@PageIndex", pageIndex));
					command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
					command.Parameters.Add(totalCountParam);

					using (IDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							foundUsers.Add(LoadUserFromReader(reader));
						}
						reader.Close();
						totalRecords = (int)command.Parameters["@TotalCount"].Value;
					}
				}
			}
			return foundUsers;
        }

		/// <summary>
		/// Gets the number of users currently accessing the application.
		/// </summary>
		/// <returns>
		/// The number of users currently accessing the application.
		/// </returns>
        public override int GetNumberOfUsersOnline()
        {
			SqlParameter totalCountParam = DataHelper.MakeOutParam("@OnlineUserCount", SqlDbType.Int, 4);
			
			//Todo: Make time considered online configurable
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("subtext_Membership_GetNumberOfUsersOnline", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
					cmd.Parameters.AddWithValue("@MinutesSinceLastInActive", 20);
					cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow.ToLongTimeString());
					cmd.Parameters.Add(totalCountParam);

					conn.Open();
					cmd.ExecuteNonQuery();

					return (int)cmd.Parameters["@OnlineUserCount"].Value;
				}
			}
        }

		/// <summary>
		/// Gets the password for the specified user name from the data source.
		/// </summary>
		/// <param name="username">The user to retrieve the password for.</param>
		/// <param name="answer">The password answer for the user.</param>
		/// <returns>
		/// The password for the specified user name.
		/// </returns>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

		/// <summary>
		/// Gets information from the data source for a user. 
		/// Provides an option to update the last-activity date/time stamp for the user.
		/// </summary>
		/// <param name="username">The name of the user to get information for.</param>
		/// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the specified user's information from the data source.
		/// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
        	using(IDataReader reader = SqlHelper.ExecuteReader(this.connectionString
        	                                                   , "subtext_Membership_GetUserByName"
        	                                                   , Membership.ApplicationName
        	                                                   , username
        	                                                   , DateTime.UtcNow, userIsOnline))
        	{
				if(!reader.Read())
					return null;
				MembershipUser user = LoadUserFromReader(reader);
        		return user;
        	}
        }
    	
    	private MembershipUser LoadUserFromReader(IDataReader reader)
    	{
			return new MembershipUser(
				Membership.Provider.Name
				, DataHelper.ReadString(reader, "UserName")
				, DataHelper.ReadGuid(reader, "UserId")
				, DataHelper.ReadString(reader, "Email")
				, DataHelper.ReadString(reader, "PasswordQuestion")
				, DataHelper.ReadString(reader, "Comment")
				, DataHelper.ReadBoolean(reader, "IsApproved")
				, DataHelper.ReadBoolean(reader, "IsLockedOut")
				, DataHelper.ReadDate(reader, "CreateDate")
				, DataHelper.ReadDate(reader, "LastLoginDate")
				, DataHelper.ReadDate(reader, "LastActivityDate")
				, DataHelper.ReadDate(reader, "LastPasswordChangedDate")
				, DataHelper.ReadDate(reader, "LastLockoutDate")
			);
    	}

		/// <summary>
		/// Gets information from the data source for a user based on the unique identifier 
		/// for the membership user. Provides an option to update the last-activity date/time 
		/// stamp for the user.
		/// </summary>
		/// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
		/// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; 
		/// false to return user information without updating the last-activity date/time stamp for the 
		/// user.</param>
		/// <returns>
		/// A <see cref="T:System.Web.Security.MembershipUser"></see> object populated with the specified user's information from the data source.
		/// </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
			using(IDataReader reader = SqlHelper.ExecuteReader(this.connectionString, "subtext_Membership_GetUserByUserId", providerUserKey, DateTime.UtcNow, userIsOnline))
			{
				if (!reader.Read())
					return null;
				return LoadUserFromReader(reader);
			}
        }

		/// <summary>
		/// Gets the user name associated with the specified 
		/// e-mail address.
		/// </summary>
		/// <param name="email">The e-mail address to search for.</param>
		/// <returns>
		/// The user name associated with the specified e-mail address. If no match is found, return null.
		/// </returns>
        public override string GetUserNameByEmail(string email)
        {
			return (string)SqlHelper.ExecuteScalar(this.connectionString, "subtext_Membership_GetUserByEmail", ApplicationName, email ?? string.Empty);
        }

		/// <summary>
		/// Gets the number of invalid password or password-answer attempts 
		/// allowed before the membership user is locked out.
		/// </summary>
		/// <value></value>
		/// <returns>The number of invalid password or password-answer 
		/// attempts allowed before the membership user is locked out.</returns>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return Convert.ToInt32(_config["maxInvalidPasswordAttempts"]);
            }
        }

		/// <summary>
		/// Gets the minimum number of special characters that 
		/// must be present in a valid password.
		/// </summary>
		/// <value></value>
		/// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return Convert.ToInt32(_config["minRequiredNonAlphanumericCharacters"]);
            }
        }

		/// <summary>
		/// Gets the minimum length required for a password.
		/// </summary>
		/// <value></value>
		/// <returns>The minimum length required for a password. </returns>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return Convert.ToInt32(_config["minRequiredPasswordLength"]);
            }
        }

		/// <summary>
		/// Gets the number of minutes in which a maximum number of invalid 
		/// password or password-answer attempts are allowed before the membership 
		/// user is locked out.
		/// </summary>
		/// <value></value>
		/// <returns>The number of minutes in which a maximum number of invalid 
		/// password or password-answer attempts are allowed before the membership 
		/// user is locked out.</returns>
        public override int PasswordAttemptWindow
        {
			get
			{
				return Convert.ToInt32(_config["passwordAttemptWindow"]);
			}
        }

		/// <summary>
		/// Gets a value indicating the format for 
		/// storing passwords in the membership data store.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"></see> values 
		/// indicating the format for storing passwords in the data store.</returns>
        public override MembershipPasswordFormat PasswordFormat
        {
			get { return (MembershipPasswordFormat)Enum.Parse(typeof(MembershipPasswordFormat), _config["passwordFormat"]); }
        }

		/// <summary>
		/// Gets the regular expression used to evaluate a password.
		/// </summary>
		/// <value></value>
		/// <returns>A regular expression used to evaluate a password.</returns>
        public override string PasswordStrengthRegularExpression
        {
			get { return _config["passwordStrengthRegularExpression"];  }
        }

		/// <summary>
		/// Gets a value indicating whether the membership provider 
		/// is configured to require the user to answer a password 
		/// question for password reset and retrieval.
		/// </summary>
		/// <value></value>
		/// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return Convert.ToBoolean(_config["requiresQuestionAndAnswer"]);
            }
        }

		/// <summary>
		/// Gets a value indicating whether the membership provider is configured 
		/// to require a unique e-mail address for each user name.
		/// </summary>
		/// <value></value>
		/// <returns>true if the membership provider requires a unique e-mail address; 
		/// otherwise, false. The default is true.</returns>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return Convert.ToBoolean(_config["requiresUniqueEmail"]);
            }
        }

		/// <summary>
		/// Resets a user's password to a new, automatically generated password.
		/// </summary>
		/// <param name="username">The user to reset the password for.</param>
		/// <param name="answer">The password answer for the specified user.</param>
		/// <returns>The new password for the specified user.</returns>
        public override string ResetPassword(string username, string answer)
        {
			if (username == null)
				throw new ArgumentNullException("username", "Username cannot be null");

			if (answer == null)
				throw new ArgumentNullException("answer", "Must provide some answer");

			MembershipUser user = GetUser(username, true);
			if (user == null)
				throw new InvalidOperationException("Cannot reset the password for a null user.");
			
			string passwordSalt = SecurityHelper.CreateRandomSalt();
			string newClearPassword = SecurityHelper.RandomPassword();

			string newPassword = newClearPassword;
			if(PasswordFormat == MembershipPasswordFormat.Hashed)
				newPassword = SecurityHelper.HashPassword(newClearPassword, passwordSalt);
			
			int recordsAffected = SqlHelper.ExecuteNonQuery(this.connectionString
			                                          , "subtext_Membership_ResetPassword"
			                                          , ApplicationName
			                                          , username
			                                          , newPassword
			                                          , MaxInvalidPasswordAttempts
			                                          , PasswordAttemptWindow
			                                          , passwordSalt
			                                          , DateTime.UtcNow
			                                          , PasswordFormat
			                                          , answer);
			
			//TODO: How do we report more information properly?
			if (recordsAffected == 0)
				return null;
			
			return newClearPassword;
        }

		/// <summary>
		/// Clears a lock so that the membership user can be validated.
		/// </summary>
		/// <param name="userName">The membership user to clear the lock status for.</param>
		/// <returns>
		/// true if the membership user was successfully unlocked; otherwise, false.
		/// </returns>
        public override bool UnlockUser(string userName)
        {
			throw new NotImplementedException("The method or operation is not implemented.");
        }

		/// <summary>
		/// Updates information about a user in the data source.
		/// </summary>
		/// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"></see> object that represents the user to update and the updated information for the user.</param>
        public override void UpdateUser(MembershipUser user)
        {
			SqlHelper.ExecuteNonQuery(this.connectionString, "subtext_Membership_UpdateUser"
			                          , ApplicationName
			                          , user.UserName
			                          , user.Email
			                          , user.Comment
			                          , user.IsApproved
			                          , user.LastLoginDate
			                          , user.LastActivityDate
			                          , 0
			                          , DateTime.UtcNow);
        }

		/// <summary>
		/// Verifies that the specified user name and password exist in the data source.
		/// </summary>
		/// <param name="username">The name of the user to validate.</param>
		/// <param name="password">The password for the specified user.</param>
		/// <returns>
		/// true if the specified username and password are valid; otherwise, false.
		/// </returns>
        public override bool ValidateUser(string username, string password)
        {
			MembershipPasswordFormat passwordFormat;
			string storedPassword;
			string salt;
        	
        	using(SqlConnection conn = new SqlConnection(this.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("subtext_Membership_GetPasswordWithFormat",  conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
					cmd.Parameters.AddWithValue("@UserName", username);
					cmd.Parameters.AddWithValue("@UpdateLastLoginActivityDate", 0);
					cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.Now.ToShortTimeString());

					conn.Open();
					using (IDataReader reader = cmd.ExecuteReader())
					{
						if(!reader.Read()) return false;

						passwordFormat = (MembershipPasswordFormat)DataHelper.ReadInt32(reader, "PasswordFormat");
						storedPassword = DataHelper.ReadString(reader, "Password");
						salt = DataHelper.ReadString(reader, "PasswordSalt");
					}
				}
            }
        	
        	string comparePassword = (passwordFormat == MembershipPasswordFormat.Hashed ? SecurityHelper.HashPassword(password, salt) : password);
			return String.Equals(comparePassword, storedPassword, StringComparison.Ordinal);
        }
    }
}