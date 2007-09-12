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
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using System.Data.SqlClient;
using SubSonic;
using Subtext.Data;
using Subtext.Framework.Data;
using System.Globalization;
using Subtext.Framework.Properties;
using Subtext.Framework.Configuration;

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
		/// Initializes the provider.
		/// </summary>
		/// <param name="name">The friendly name of the provider.</param>
		/// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
		/// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"></see> on a provider after the provider has already been initialized.</exception>
		/// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
		/// <exception cref="ConfigurationErrorsException">The 'connectionStringName' attribute is missing or the connection string is not found.</exception>
		public override void Initialize(string name, NameValueCollection config)
		{
			if (config == null)
				throw new ArgumentNullException("config", Resources.ArgumentNull_Collection);

			_config = config;
			if (String.IsNullOrEmpty(name))
				name = "SubtextMembershipProvider";

			base.Initialize(name, config);

			string connectionStringName = config["connectionStringName"];
			if (string.IsNullOrEmpty(connectionStringName))
				throw new ConfigurationErrorsException("Missing attribute 'connectionStringName'");

			connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

			if (string.IsNullOrEmpty(this.connectionString))
				throw new ConfigurationErrorsException("The connection string '" + connectionStringName + "' was not found");

			config.Remove("connectionStringName");
		}

		/// <summary>
		/// The name of the application using the custom membership provider.
		/// </summary>
		/// <remarks>
        /// Since we support multiple "Applications" within a single Subtext 
        /// IIS application, we need to handle the presidence correctly.
        /// First, we check the HttpContext (in order to keep changes here
        /// local to the request). Second, we check a private member variable.
        /// Our final fallback is the current blog application name.
        /// </remarks>
		/// <value></value>
		/// <returns>The name of the application using the custom membership provider.</returns>
        public override string ApplicationName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Items.Contains(SecurityHelper.ApplicationNameContextId))
                    {
                        return (string)HttpContext.Current.Items[SecurityHelper.ApplicationNameContextId];
                    }
                    else
                    {
                        if (Config.CurrentBlog != null)
                        {
                            return Config.CurrentBlog.ApplicationName;
                        }
                        else
                        {
                            return applicationName;
                        }
                    }
                }
                else
                {
                    return applicationName;
                }
            }
            set
            {
				//TODO: Please review
				if (HttpContext.Current != null)
					HttpContext.Current.Items[SecurityHelper.ApplicationNameContextId] = value;
				else
					applicationName = value;
            } 
		}

        private string applicationName = "/";

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
            if (!ValidateUser(username, oldPassword))
            {
                return false;
            }

			string passwordSalt = SecurityHelper.CreateRandomSalt();
			if (PasswordFormat == MembershipPasswordFormat.Hashed)
			{
				newPassword = SecurityHelper.HashPassword(newPassword);
			}

			int recordsAffected = StoredProcedures.MembershipSetPassword(username, newPassword, passwordSalt, DateTime.UtcNow, (int)Membership.Provider.PasswordFormat).Execute();
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
			throw new NotImplementedException(Resources.NotImplementedException_Generic);
			//TODO: return 0 < StoredProcedures.MembershipChangePasswordQuestionAndAnswer(username, newPasswordQuestion, newPasswordAnswer).Execute();
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
			if (PasswordFormat == MembershipPasswordFormat.Hashed)
				password = SecurityHelper.HashPassword(password, salt);

			int recordsAffected = StoredProcedures.MembershipCreateUser(username
				, password
				, salt
				, email
				, passwordQuestion
				, passwordAnswer
				, true
				, DateTime.UtcNow
				, DateTime.UtcNow
				, RequiresUniqueEmail
				, (int)PasswordFormat
				, (Guid)providerUserKey).Execute();

			if (recordsAffected >= 1) //Records affected.
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
			StoredProcedure proc = StoredProcedures.UsersDeleteUser(username, deleteAllRelatedData ? 15 : 1, 0);
			proc.Execute();
			return (int) proc.OutputValues[0] >= 1;
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
				return Convert.ToBoolean(_config["enablePasswordReset"], CultureInfo.InvariantCulture);
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
				return Convert.ToBoolean(_config["enablePasswordRetrieval"], CultureInfo.InvariantCulture);
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
			if (emailToMatch == null)
				throw new ArgumentNullException("emailToMatch", Resources.ArgumentNull_String);

			if (emailToMatch.Length == 0)
				throw new ArgumentException(Resources.Argument_StringZeroLength, "emailToMatch");

			return FindUsersByNameOrEmail(null, emailToMatch, pageIndex, pageSize, out totalRecords);
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
			if (usernameToMatch == null)
				throw new ArgumentNullException("usernameToMatch", Resources.ArgumentNull_String);

			if (usernameToMatch.Length == 0)
				throw new ArgumentException(Resources.Argument_StringZeroLength, "usernameToMatch");

			return FindUsersByNameOrEmail(usernameToMatch, null, pageIndex, pageSize, out totalRecords);
		}

		private MembershipUserCollection FindUsersByNameOrEmail(string userName, string email, int pageIndex, int pageSize, out int totalRecords)
		{
			if (email == null && userName == null)
				throw new ArgumentNullException("email and userName", Resources.ArgumentNull_String);

			if (email != null && userName != null)
				throw new ArgumentException("Can only search on email or username. Not both at the same time.");

			MembershipUserCollection foundUsers = new MembershipUserCollection();

			string application = String.IsNullOrEmpty(ApplicationName) || ApplicationName == "/" ? null : ApplicationName;
			StoredProcedure proc = StoredProcedures.MembershipFindUsersByNameOrEmail(application, userName, email, pageIndex, pageSize, 0);
			using (IDataReader reader = proc.GetReader())
			{
				while (reader.Read())
				{
					foundUsers.Add(LoadUserFromReader(reader));
				}
				reader.Close();
				totalRecords = (int)(proc.OutputValues[0] ?? 0);
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
				string application = String.IsNullOrEmpty(ApplicationName) || ApplicationName == "/" ? null : ApplicationName;

				//TODO: If the Application is not "/", we should only list users 
				//		who are a member of any role within the current blog application.
				using (SqlCommand command = new SqlCommand("subtext_Membership_GetAllUsers", conn))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@ApplicationName", application));
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
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("subtext_Membership_GetNumberOfUsersOnline", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@MinutesSinceLastInActive", 20);
					//cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow.ToLongTimeString());
               cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.Now);
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
			throw new NotImplementedException(Resources.NotImplementedException_Generic);
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
			using (IDataReader reader = StoredProcedures.MembershipGetUserByName(username, DateTime.UtcNow, null).GetReader())
			{
				if (!reader.Read())
					return null;
				MembershipUser user = LoadUserFromReader(reader);
				return user;
			}
		}

		private static MembershipUser LoadUserFromReader(IDataReader reader)
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
			using (IDataReader reader = StoredProcedures.MembershipGetUserByUserId((Guid)providerUserKey, DateTime.UtcNow, userIsOnline).GetReader())
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
			if (email == null)
				throw new ArgumentNullException("emailToMatch", Resources.ArgumentNull_String);

			if (email.Length == 0)
				throw new ArgumentException(Resources.Argument_StringZeroLength, "emailToMatch");

			return (string)StoredProcedures.MembershipGetUserByEmail(email).ExecuteScalar();
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
				return Convert.ToInt32(_config["maxInvalidPasswordAttempts"], NumberFormatInfo.InvariantInfo);
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
				return Convert.ToInt32(_config["minRequiredNonAlphanumericCharacters"], NumberFormatInfo.InvariantInfo);
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
				return Convert.ToInt32(_config["minRequiredPasswordLength"], NumberFormatInfo.InvariantInfo);
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
				return Convert.ToInt32(_config["passwordAttemptWindow"], NumberFormatInfo.InvariantInfo);
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
			get { return _config["passwordStrengthRegularExpression"]; }
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
				return Convert.ToBoolean(_config["requiresQuestionAndAnswer"], CultureInfo.InvariantCulture);
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
				return Convert.ToBoolean(_config["requiresUniqueEmail"], CultureInfo.InvariantCulture);
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
				throw new ArgumentNullException("username", Resources.ArgumentNull_String);

			if (answer == null)
				throw new ArgumentNullException("answer", Resources.ArgumentNull_String);

			if (username.Length == 0)
				throw new ArgumentException(Resources.Argument_StringZeroLength, "username");

			if (answer.Length == 0)
				throw new ArgumentException(Resources.Argument_StringZeroLength, "answer");

			MembershipUser user = GetUser(username, true);
			if (user == null)
				throw new InvalidOperationException("Cannot reset the password for a null user.");

			string passwordSalt = SecurityHelper.CreateRandomSalt();
			string newClearPassword = SecurityHelper.RandomPassword();

			string newPassword = newClearPassword;
			if (PasswordFormat == MembershipPasswordFormat.Hashed)
				newPassword = SecurityHelper.HashPassword(newClearPassword, passwordSalt);

			int recordsAffected = StoredProcedures.MembershipResetPassword(username
			                                                               , newPassword
			                                                               , MaxInvalidPasswordAttempts
			                                                               , PasswordAttemptWindow
			                                                               , passwordSalt
			                                                               , DateTime.UtcNow
			                                                               , (int) PasswordFormat
			                                                               , answer).Execute();

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
			throw new NotImplementedException(Resources.NotImplementedException_Generic);
		}

		/// <summary>
		/// Updates information about a user in the data source.
		/// </summary>
		/// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"></see> object that represents the user to update and the updated information for the user.</param>
		public override void UpdateUser(MembershipUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user", Resources.ArgumentNull_Obj);
			}

			StoredProcedures.MembershipUpdateUser(user.UserName
			                                      , user.Email
			                                      , user.Comment
			                                      , user.IsApproved
			                                      , user.LastLoginDate
			                                      , user.LastActivityDate
			                                      , 0
			                                      , DateTime.UtcNow).Execute();
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

			using (IDataReader reader = StoredProcedures.MembershipGetPasswordWithFormat(username, false, DateTime.UtcNow).GetReader())
			{
				if (!reader.Read()) return false;

				passwordFormat = (MembershipPasswordFormat)DataHelper.ReadInt32(reader, "PasswordFormat");
				storedPassword = DataHelper.ReadString(reader, "Password");
				salt = DataHelper.ReadString(reader, "PasswordSalt");
			}

			string comparePassword = (passwordFormat == MembershipPasswordFormat.Hashed ? SecurityHelper.HashPassword(password, salt) : password);
			return String.Equals(comparePassword, storedPassword, StringComparison.Ordinal);
		}
	}
}