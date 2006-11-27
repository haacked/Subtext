using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Subtext.Framework.Security
{
    /// <summary>
    /// Summary description for MembershipProvider
    /// </summary>
   
    public class SubtextMembershipProvider : System.Web.Security.MembershipProvider
    {
        private string _applicationName;
        private string _connectionString;
        private NameValueCollection _config;

        public SubtextMembershipProvider()
        {

        }

        public override void Initialize(string name, NameValueCollection config)
        {

            _config = config;
            if (string.IsNullOrEmpty(name))
                name = "SubtextMembershipProvider";
            base.Initialize(name, config);

            string csn = config["connectionStringName"];
            if (string.IsNullOrEmpty(csn))
                throw new HttpException("Missing attribute 'connectionStringName'");

            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[csn].ConnectionString;

            if (string.IsNullOrEmpty(_connectionString))
                throw new Exception("The connection string " + csn + "was not found");

            config.Remove("connectionStringName");
        }

        public override string ApplicationName
        {
            get
            {
                if (_applicationName == null)
                {
                    int BlogId;
                    try
                    {
                        BlogId = Subtext.Framework.Configuration.Config.CurrentBlog.Id;
                    }
                    catch (NullReferenceException)
                    {
                        BlogId = -1;
                    }
                    if (BlogId <= 0)
                    {
                        return "/";
                    }
                    else
                    {
                        return "blog_" + BlogId.ToString();
                    }
                }
                else
                {
                    return _applicationName;
                }
            }
            set
            {
                _applicationName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

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
                DateTime createDate = DateTime.Now;

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
                string salt = Subtext.Framework.Security.SecurityHelper.CreateRandomSalt();

                SqlConnection conn = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("subtext_Membership_CreateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", Subtext.Framework.Security.SecurityHelper.HashPassword(password, salt));
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
                cmd.Parameters["@UserId"].Direction = ParameterDirection.Output;
                using (conn)
                {
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
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
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
            SqlConnection conn = new SqlConnection(_connectionString);
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
            SqlConnection conn = new SqlConnection(_connectionString);
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
            SqlConnection conn = new SqlConnection(_connectionString);
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
            SqlConnection conn = new SqlConnection(_connectionString);
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
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Membership_GetUserByName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@UpdateLastActivity", 0);
            cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.Now.ToShortTimeString());
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                if (rdr.HasRows)
                {
                    MembershipUser retVal = new MembershipUser(Membership.Provider.Name,
                        username,
                        rdr.GetValue(8),
                        rdr.IsDBNull(0) ? "" : rdr.GetString(0),
                        rdr.IsDBNull(1) ? "" : rdr.GetString(1),
                        rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                        rdr.GetBoolean(3),
                        rdr.GetBoolean(9),
                        rdr.GetDateTime(4),
                        rdr.GetDateTime(5),
                        rdr.GetDateTime(6),
                        rdr.GetDateTime(7),
                        rdr.GetDateTime(10));
                    return retVal;
                }
                else
                {
                    return null;
                }
            }

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetUserNameByEmail(string email)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Membership_GetUserByEmail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", "/");
            cmd.Parameters.AddWithValue("@email", email);
            using (conn)
            {
                conn.Open();
                DataRow dr = (DataRow)cmd.ExecuteScalar();
                if (dr == null)
                {
                    return null;

                }
                else
                {
                    return (string)dr["UserName"];
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
            get { return Convert.ToInt32(_config["PasswordAttemptWindow"]); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            //Subtext currently only uses SHA1 Hashing
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            //not implemented in first attempt - RA
            get { throw new Exception("The method or operation is not implemented."); }
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
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool UnlockUser(string userName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool ValidateUser(string username, string password)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Membership_GetPasswordWithFormat", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@UpdateLastLoginActivityDate", 0);
            cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.Now.ToShortTimeString());
            using (conn)
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                if (rdr.FieldCount == 0) return false;
                string storedPassword = (string)rdr.GetValue(0);
                string salt = (string)rdr.GetValue(2);
                rdr.Close();
                string hashedTestPwd = Subtext.Framework.Security.SecurityHelper.HashPassword(password, salt);

                return String.Compare(hashedTestPwd, storedPassword) == 0;
            }
        }

        private string GetApplicationGuid()
        {
            string retVal;
             SqlConnection conn = new SqlConnection(_connectionString);
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