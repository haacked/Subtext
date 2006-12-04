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

        public SubtextMembershipProvider() : base()
        {
        }

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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
			throw new NotImplementedException("The method or operation is not implemented.");
        }

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

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(username, false);

            if (u == null)
            {
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
						cmd.Parameters.AddWithValue("@PasswordQuestion", "");
						cmd.Parameters.AddWithValue("@PasswordAnswer", "");
						cmd.Parameters.AddWithValue("@IsApproved", true);
						cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.Now.ToShortTimeString());
						cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now.ToShortDateString());
						cmd.Parameters.AddWithValue("@UniqueEmail", 0);
						cmd.Parameters.AddWithValue("@PasswordFormat", 0);
						cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier);
						cmd.Parameters["@UserId"].Value = providerUserKey;
						cmd.Parameters["@UserId"].Direction = ParameterDirection.Output;

						conn.Open();
						if (cmd.ExecuteNonQuery() >= 1)
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
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Users_DeleteUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", "\\");
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@TablesToDeleteFrom", deleteAllRelatedData ? 15 : 1);
            cmd.Parameters.Add("@NumTablesDeletedFrom", SqlDbType.Int);
            cmd.Parameters["@NumTablesDeletedFrom"].Direction = ParameterDirection.Output;
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters["@NumTablesDeletedFrom"].Value) >= 1 ? true : false;
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {

                return Convert.ToBoolean(_config["EnablePasswordReset"]);
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {

                return Convert.ToBoolean(_config["EnablePasswordRetrieval"]);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection muc = new MembershipUserCollection();
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Membership_GetUsersByEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@Email", emailToMatch);
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MembershipUser mu = new MembershipUser(Name,
                        rdr.GetString(1), null, null, null, null, true,
                        false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                    muc.Add(mu);
                }
                totalRecords = rdr.RecordsAffected;
            }
            return muc;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection muc = new MembershipUserCollection();
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Membership_GetUsersByEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@UsernameToMatch", usernameToMatch);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MembershipUser mu = new MembershipUser(Name,
                        rdr.GetString(1), null, null, null, null, true,
                        false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                    muc.Add(mu);
                }
                totalRecords = rdr.RecordsAffected;
            }
            return muc;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection muc = new MembershipUserCollection();
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand("SELECT UserID, UserName from subtext_users where applicationId = '" + GetApplicationGuid() + "'", conn);
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    MembershipUser mu = new MembershipUser(Name,
                        rdr.GetString(1), null, null, null, null, true,
                        false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                    muc.Add(mu);
                }
                totalRecords = rdr.RecordsAffected;
            }
            return muc;
        }

        public override int GetNumberOfUsersOnline()
        {
            //@ApplicationName            nvarchar(256),
            //@MinutesSinceLastInActive   int,
            //@CurrentTimeUtc             datetime
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Membership_GetNumberOfUsersOnline", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            //Todo: Make time considered online configurable
            cmd.Parameters.AddWithValue("@MinutesSinceLastInActive", 20);
            cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow.ToLongTimeString());
            using (conn)
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
        	using(IDataReader reader = SqlHelper.ExecuteReader(this.connectionString, "subtext_Membership_GetUserByName", Membership.ApplicationName, username, DateTime.UtcNow, userIsOnline))
        	{
				return LoadUserFromReader(reader);
        	}
        }
    	
    	private MembershipUser LoadUserFromReader(IDataReader reader)
    	{
			if (!reader.Read())
				return null;
    		
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
				return LoadUserFromReader(reader);
			}
        }

        public override string GetUserNameByEmail(string email)
        {
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand("subtext_Membership_GetUserByEmail", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@ApplicationName", "/");
					cmd.Parameters.AddWithValue("@email", email);

					conn.Open();
					return (string)cmd.ExecuteScalar();
				}
			}
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {

                return Convert.ToInt32(_config["MaxInvalidPasswordAttempts"]);
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {

                return Convert.ToInt32(_config["MinRequiredNonAlphanumericCharacters"]);
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {

                return Convert.ToInt32(_config["MinRequiredPasswordLength"]);
            }
        }

        public override int PasswordAttemptWindow
        {
			get { throw new NotImplementedException("The method or operation is not implemented."); }
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
            //Subtext currently only uses SHA1 Hashing
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            //not implemented in first attempt - RA
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {

                return Convert.ToBoolean(_config["RequiresQuestionAndAnswer"]);
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return Convert.ToBoolean(_config["RequiresUniqueEmail"]);
            }
        }

        public override string ResetPassword(string username, string answer)
        {
			throw new NotImplementedException("The method or operation is not implemented.");
        }

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

        private string GetApplicationGuid()
        {
            string retVal;
             SqlConnection conn = new SqlConnection(this.connectionString);
             SqlCommand cmd = new SqlCommand("SELECT applicationId FROM " +
                 "subtext_applications WHERE LoweredApplicationName = '" +
                 ApplicationName.ToLower() + "'",
                 conn);
             using (conn)
             {
                 conn.Open();
                  retVal = cmd.ExecuteScalar().ToString();
             }
             return retVal;
        }
    }
}