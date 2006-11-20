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
    class SubtextRoleProvider : System.Web.Security.RoleProvider
    {
        private string _applicationName;
        private string _connectionString;
        private NameValueCollection _config;

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
                    return "Blog_" + BlogId.ToString();
                }
            }
            set
            {
                _applicationName = value;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            string cdUserNames = "";
            foreach (string s in usernames)
            {
                cdUserNames += s + ",";
            }
            cdUserNames = cdUserNames.Remove(cdUserNames.Length - 1);

            string cdRoleNames = "";
            foreach (string s in roleNames)
            {
                cdRoleNames += s + ",";
            }
            cdRoleNames = cdRoleNames.Remove(cdRoleNames.Length - 1);

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_UsersInRoles_AddUsersToRoles", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@UserNames", cdUserNames);
            cmd.Parameters.AddWithValue("@RoleNames", cdRoleNames);
            cmd.Parameters.AddWithValue("@CurrentTimeUtc", DateTime.UtcNow.ToUniversalTime());
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public override void CreateRole(string roleName)
        {

            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Roles_CreateRole", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@RoleName", roleName);
            using (conn)
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new Exception("Role exists");
                }
            }

        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            //no need to delete roles at this moment from within Subtext (v2) - RA
            throw new Exception("The method or operation is not implemented.");
        }
        
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            //I've yet to figure out what the @!^&) this actually is for.
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetAllRoles()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Roles_GetAllRoles", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            using (conn)
            {
                conn.Open(); SqlDataReader reader = cmd.ExecuteReader();
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

        public override string[] GetRolesForUser(string username)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_UsersInRoles_GetRolesForUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@UserName", username);
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                StringCollection userRoles = new StringCollection();
                while (reader.Read())
                {
                    userRoles.Add(reader.GetString(0));
                }
                string[] returnUserRoles = new string[userRoles.Count];
                userRoles.CopyTo(returnUserRoles, 0);

                return returnUserRoles;
            }

        }

        public override string[] GetUsersInRole(string roleName)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_UsersInRoles_GetUsersInRoles", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@RoleName", roleName);
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                StringCollection usersInRole = new StringCollection();
                while (reader.Read())
                {
                    usersInRole.Add(reader.GetString(0));
                }
                string[] returnUsersInRole = new string[usersInRole.Count];
                usersInRole.CopyTo(returnUsersInRole, 0);

                return returnUsersInRole;
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_UsersInRoles_IsUserInRole", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@RoleName", roleName);
            using (conn)
            {
                return ((int)cmd.ExecuteScalar() == 1);
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool RoleExists(string roleName)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand("subtext_Roles_RoleExists", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@RoleName", roleName);
            cmd.Parameters.Add("@return_value", SqlDbType.Int);
            cmd.Parameters["@return_value"].Direction = ParameterDirection.ReturnValue;

            using (conn)
            {
               conn.Open();
               cmd.ExecuteNonQuery();
               return (int)cmd.Parameters["@return_value"].Value == 1 ? true : false; 

            }
        }

        public bool CreateStandardRoles()
        {
            string[] rolenames = new string[] {"Adminstrators",
                "PowerUsers", "Authors", "VerifiedCommenters",
                "Anonymous" };
            foreach (string role in rolenames)
            {
                CreateRole(role);
            }
            return true;
        }
    }
}
