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
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using log4net;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.Framework.Security
{
	/// <summary>
	/// Handles blog logins/passwords/tickets
	/// </summary>
	public static class SecurityHelper
	{
		private readonly static ILog log = new Log();

		/// <summary>
		/// Check to see if the supplied credentials are valid for the current blog. If so, 
		/// Set the user's FormsAuthentication Ticket This method will handle passwords for 
		/// both hashed and non-hashed configurations
		/// </summary>
		/// <param name="username">Supplied UserName</param>
		/// <param name="password">Supplied Password</param>
		/// <param name="persist">If valid, should we persist the login</param>
		/// <returns>bool indicating successful login</returns>
		public static bool Authenticate(string username, string password, bool persist)
		{
			if (!IsValidUser(username, password))
			{
				return false;
			}

			log.Debug("SetAuthenticationTicket-Admins for " + username);
			SetAuthenticationTicket(username, persist, "Admins");
			return true;
		}

        /// <summary>
        /// Check to see if the supplied OpenID claim is valid for the current blog. If so, 
        /// Set the user's FormsAuthentication Ticket This method will handle passwords for 
        /// both hashed and non-hashed configurations
        /// We're comparing URI objects rather than using simple string compare because
        /// functionally equivalent URI's may not pass string comparaisons, e.g.
        /// such as http://example.myopenid.com/ and http://example.myopenid.com (trailing /)
        /// </summary>
        /// <param name="claimedIdenftifier">OpenID claimed identifier URI</param>
        /// <param name="persist">If valid, should we persist the login</param>
        /// <returns>bool indicating successful login</returns>
        public static bool Authenticate(string claimedIdentifier, bool persist)
        {
            BlogInfo currentBlog = Config.CurrentBlog;
            //If the current blog doesn't have a valid OpenID URI, must fail
            if(!Uri.IsWellFormedUriString(currentBlog.OpenIDUrl, UriKind.Absolute))
                return false;

            //If the cliamed identifier isn't a valid OpenID URI, must fail
            if (!Uri.IsWellFormedUriString(claimedIdentifier, UriKind.Absolute))
                return false;

            Uri currentBlogClaimUri = new Uri(currentBlog.OpenIDUrl);
            Uri claimedUri = new Uri(claimedIdentifier);

            if (claimedUri.Host != currentBlogClaimUri.Host ||
                claimedUri.AbsolutePath != currentBlogClaimUri.AbsolutePath ||
                claimedUri.Query != currentBlogClaimUri.Query)
                return false;

            log.Debug("SetAuthenticationTicket-Admins via OpenID for " + currentBlog.UserName);
            SetAuthenticationTicket(currentBlog.UserName, persist, "Admins");
            return true;
        }

		/// <summary>
		/// Authenticates the host admin.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="persist">if set to <c>true</c> [persist].</param>
		/// <returns></returns>
		public static bool AuthenticateHostAdmin(string username, string password, bool persist)
		{
			if (!String.Equals(username, HostInfo.Instance.HostUserName, StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}
			
			if(Config.Settings.UseHashedPasswords)
			{
				password = HashPassword(password, HostInfo.Instance.Salt);
			}

			if (!String.Equals(HostInfo.Instance.Password, password, StringComparison.InvariantCultureIgnoreCase))
			{
				return false;
			}
			
			log.Debug("SetAuthenticationTicket-HostAdmins for " + username);
			SetAuthenticationTicket(username, persist, true, "HostAdmins");
			
			return true;
		}

		/// <summary>
		/// Used to remove a cookie from the client.
		/// </summary>
		/// <returns>a correctly named cookie with Expires date set 30 years ago</returns>
		public static HttpCookie GetExpiredCookie()
		{
			HttpCookie expiredCookie = new HttpCookie(GetFullCookieName());
			expiredCookie.Expires = DateTime.Now.AddYears(-30);
			return expiredCookie;
		}

		/// <summary>
		/// Obtains the correct cookie for the current blog
		/// </summary>
		/// <returns>null if correct cookie was not found</returns>
		public static HttpCookie SelectAuthenticationCookie()
		{
			HttpCookie authCookie = null;
            HttpCookie c;
		    int count = HttpContext.Current.Request.Cookies.Count;
		    
			log.Debug("cookie count = " + count);
		    for (int i = 0; i < count; i++)
			{
                c = HttpContext.Current.Request.Cookies[i];
				#region Logging
				if (log.IsDebugEnabled)
				{
					if (c == null)
					{
						log.Debug("cookie was null");
						continue;
					}
					if (c.Value == null)
					{
						log.Debug("cookie value was null");
					}
					else if (c.Value == "")
					{
						log.Debug("cookie value was empty string");
					}
					if (c.Name == null)
					{
						log.Debug("cookie name was null");//not a valid Subtext cookie
						continue;
					}
					log.DebugFormat("Cookie named '{0}' found", c.Name);
				}
				#endregion

				if (c.Name == GetFullCookieName())
				{
					authCookie = c;
					log.Debug("Cookie selected = " + authCookie.Name);
#if !DEBUG
					//if in DEBUG, the loop does not break so all cookies can be logged
					break;
#endif
				}
			}
			return authCookie;
		}

		/// <summary>
		/// Identifies cookies by unique BlogHost names (rather than a single
		/// name for all cookies in multiblog setups as the old code did).
		/// </summary>
		/// <returns></returns>
		public static string GetFullCookieName()
		{
			return GetFullCookieName(false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="forceHostAdmin">true if the name shall be forced to comply with the HostAdmin cookie</param>
		/// <returns></returns>
		private static string GetFullCookieName(bool forceHostAdmin)
		{
			StringBuilder name = new StringBuilder(FormsAuthentication.FormsCookieName);
			name.Append(".");
			
			//See if we need to authenticate the HostAdmin
			string path = HttpContext.Current.Request.Path;
			string returnUrl = HttpContext.Current.Request.QueryString.ToString(); //["ReturnURL"];
			if (forceHostAdmin
				|| StringHelper.Contains(path + returnUrl, "HostAdmin", 
			    StringComparison.InvariantCultureIgnoreCase))
			{
			    name.Append("HA.");
			}

		    try
		    {
		    	try 
		    	{
		    		//Need to clean this up. Either this should return null, or throw an exception,
		    		//but not both.
					if (!forceHostAdmin && Config.CurrentBlog != null)
						name.Append(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture));
		    		else
						name.Append("null");
		    	}
		    	catch(BlogDoesNotExistException)
		    	{
					name.Append("null");
		    	}
		    }
            catch (SqlException sqlExc)
		    {
                if (sqlExc.Number == (int)SqlErrorMessage.CouldNotFindStoredProcedure 
                    && sqlExc.Message.IndexOf("'subtext_GetConfig'") > 0)
                {
                    // must not have the db installed.
                    log.Debug("The database must not be installed.");
                }
                else throw;
		    }
			log.Debug("GetFullCookieName selected cookie named " + name.ToString());
			return name.ToString();           
		}

        public static void SetAuthenticationTicket(string username, bool persist, params string[] roles)
        {
            SetAuthenticationTicket(username, persist, false, roles);
        }
		
		/// <summary>
		/// Used by methods in this class plus Install.Step02_ConfigureHost
		/// </summary>
		/// <param name="username">Username for the ticket</param>
		/// <param name="persist">Should this ticket be persisted</param>
        public static void SetAuthenticationTicket(string username, bool persist, bool forceHostAdmin, params string[] roles)
		{
			//Getting a cookie this way and using a temp auth ticket 
			//allows us to access the timeout value from web.config in partial trust.
			HttpCookie authCookie = FormsAuthentication.GetAuthCookie(username, persist);
			FormsAuthenticationTicket tempTicket = FormsAuthentication.Decrypt(authCookie.Value);
			string userData = string.Join("|", roles);

			FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
				tempTicket.Version,
				tempTicket.Name,
				tempTicket.IssueDate,
				tempTicket.Expiration,//this is how we access the configured timeout value
				persist,//the configured persistence value in web.config is not used. We use the checkbox value on the login page.
				userData,//roles
				tempTicket.CookiePath);
			authCookie.Value = FormsAuthentication.Encrypt(authTicket);
			authCookie.Name = GetFullCookieName(forceHostAdmin);//prevents login problems with some multiblog setups

			HttpContext.Current.Response.Cookies.Add(authCookie);
			#region Logging
			if (log.IsDebugEnabled)
			{
				log.Debug("the code must call a redirect after this");
				log.DebugFormat("cookie '{3}' added to response for '{0}'; expires {1} and contains roles: {2}",
					username, authCookie.Expires, authTicket.UserData, authCookie.Name);
			} 
			#endregion
		}

				
		/// <summary>
		/// Logs the user off the system.
		/// </summary>
		public static void LogOut()
		{
			HttpCookie authCookie = new HttpCookie(GetFullCookieName());
			authCookie.Expires = DateTime.Now.AddYears(-30); //setting an expired cookie forces client to remove it
			HttpContext.Current.Response.Cookies.Add(authCookie);
			#region Logging
			if (log.IsDebugEnabled)
			{
				string username = HttpContext.Current.User.Identity.Name;
				log.Debug("Logging out " + username);
				log.Debug("the code MUST call a redirect after this");
			} 
			#endregion
			FormsAuthentication.SignOut();
		}

		//From Forums Source Code
		
		/// <summary>
		/// Get MD5 hashed/encrypted representation of the password and 
		/// returns a Base64 encoded string of the hash.
		/// This is a one-way hash.
		/// </summary>
		/// <remarks>
		/// Passwords are case sensitive now. Before they weren't.
		/// </remarks>
		/// <param name="password">Supplied Password</param>
		/// <returns>Encrypted (Hashed) value</returns>
		public static string HashPassword(string password) 
		{
			Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
			Byte[] hashedBytes = new MD5CryptoServiceProvider().ComputeHash(clearBytes);
			
			return Convert.ToBase64String(hashedBytes);
		}

		/// <summary>
		/// Get MD5 hashed/encrypted representation of the password and a 
		/// salt value combined in the proper manner.  
		/// Returns a Base64 encoded string of the hash.
		/// This is a one-way hash.
		/// </summary>
		/// <remarks>
		/// Passwords are case sensitive now. Before they weren't.
		/// </remarks>
		/// <param name="password">Supplied Password</param>
		/// <returns>Encrypted (Hashed) value</returns>
		public static string HashPassword(string password, string salt)
		{
			string preHash = CombinePasswordAndSalt(password, salt);
			return HashPassword(preHash);
		}

		/// <summary>
		/// Creates a random salt value.
		/// </summary>
		/// <returns></returns>
		public static string CreateRandomSalt()
		{
			return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
		}

		/// <summary>
		/// Returns a string with a password and salt combined.
		/// </summary>
		/// <param name="password">Password.</param>
		/// <param name="salt">Salt.</param>
		/// <returns></returns>
		public static string CombinePasswordAndSalt(string password, string salt)
		{
			//TODO: return salt + "." + password;
			//We're not ready to do this yet till we do it to the blog_content table too.
			return password;
		}

		/// <summary>
		/// Validates if the supplied credentials match the current blog
		/// </summary>
		/// <param name="username">Supplied Username</param>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool value indicating if the user is valid.</returns>
		public static bool IsValidUser(string username, string password)
		{
			if (String.Equals(username, Config.CurrentBlog.UserName, StringComparison.InvariantCultureIgnoreCase))
			{
				return IsValidPassword(password);
			}
			else
			{
				log.DebugFormat("The supplied username '{0}' does not equal the configured username of '{1}'.", username, Config.CurrentBlog.UserName);
			}
			return false;
		}

		/// <summary>
		/// Check to see if the supplied password matches the password 
		/// for the current blog. This method will check the 
		/// BlogConfigurationSettings to see if the password should be 
		/// Encrypted/Hashed
		/// </summary>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool value indicating if the supplied password matches the current blog's password</returns>
		public static bool IsValidPassword(string password)
		{
			if(Config.CurrentBlog.IsPasswordHashed)
			{
				password = HashPassword(password);
			}
			string storedPassword = Config.CurrentBlog.Password;
			
			if(storedPassword.IndexOf('-') > 0)
			{
				// NOTE: This is necessary because I want to change how 
				// we store the password.  Mayb changing the password 
				// storage is dumb.  Let me know. -Phil
				//	This is an old password created from BitConverter 
				// string.  Converting to a Base64 hash.
				string[] hashBytesStrings = storedPassword.Split('-');
				byte[] hashedBytes = new byte[hashBytesStrings.Length];
				for(int i = 0; i < hashBytesStrings.Length; i++)
				{
					hashedBytes[i] = byte.Parse(hashBytesStrings[i].ToString(CultureInfo.InvariantCulture), NumberStyles.HexNumber);
					storedPassword = Convert.ToBase64String(hashedBytes);
				}
			}
			
			bool areEqual = String.Equals(password, storedPassword, StringComparison.InvariantCulture);
			if (!areEqual)
			{
				log.Debug("The supplied password is incorrect.");
			}
			return areEqual;
		}

		/// <summary>
		/// When we Encrypt/Hash the password, we can not un-Encrypt/Hash the password. If user's need to retrieve this value, all we can
		/// do is reset the passowrd to a new value and send it.
		/// </summary>
		/// <returns>A New Password</returns>
		public static string ResetPassword()
		{
			string password = RandomPassword();
			
			UpdatePassword(password);

			return password;
		}

		/// <summary>
		/// Updates the current users password to the supplied value. 
		/// Handles hashing (or not hashing of the password)
		/// </summary>
		/// <param name="password">Supplied Password</param>
		public static void UpdatePassword(string password)
		{
			BlogInfo info = Config.CurrentBlog;
			if(Config.CurrentBlog.IsPasswordHashed)
			{
				info.Password = HashPassword(password);
			}
			else
			{
				info.Password = password;
			}
			//Save new password.
			Config.UpdateConfigData(info);
		}

		public static void UpdateHostAdminPassword(string password)
		{
			HostInfo hostInfo = HostInfo.Instance;
			if(Config.Settings.UseHashedPasswords)
			{
				hostInfo.Password = HashPassword(password, HostInfo.Instance.Salt);
			}
			else
			{
				hostInfo.Password = password;
			}
			HostInfo.UpdateHost(hostInfo);
		}

		public static string ResetHostAdminPassword()
		{
			string password = RandomPassword();
			UpdateHostAdminPassword(password);
			return password;
		}

		/// <summary>
		/// Generates a "Random Enough" password. :)
		/// </summary>
		/// <returns></returns>
		public static string RandomPassword()
		{
			return Guid.NewGuid().ToString().Substring(0,8);
		}

		/// <summary>
		/// Gets a value indicating whether the current 
		/// user is the admin for the current blog.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [is admin]; otherwise, <c>false</c>.
		/// </value>
		public static bool IsAdmin
		{
			get
			{
				bool areNamesEqual = String.Equals(CurrentUserName, Config.CurrentBlog.UserName, StringComparison.InvariantCultureIgnoreCase);
				#region temp logging code
				if (!areNamesEqual)
				{
					log.DebugFormat("CurrentUserName '{0}'is not equal to Config.CurrentBlog.UserName '{1}'", CurrentUserName, Config.CurrentBlog.UserName);
				}
				#endregion
				//TODO: Eventually just check for admin role.
				return IsInRole("Admins") && areNamesEqual;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current user is a 
		/// Host Admin for the entire installation.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [is host admin]; otherwise, <c>false</c>.
		/// </value>
		public static bool IsHostAdmin
		{
			get
			{
				//TODO: Remove the second check when we have better security model.
				return IsInRole("HostAdmins");
			}
		}

		/// <summary>
		/// Gets the name of the current user.
		/// </summary>
		/// <value></value>
		public static string CurrentUserName
		{
			get
			{
				if(HttpContext.Current.Request.IsAuthenticated)
				{
					try
					{
						return HttpContext.Current.User.Identity.Name;
					}
					catch{}
				}
				return null;
			}
		}

		/// <summary>
		/// Returns true if the user is in the specified role.
		/// It's a wrapper to calling the IsInRole method of 
		/// IPrincipal.
		/// </summary>
		/// <param name="role">Role.</param>
		/// <returns></returns>
		public static bool IsInRole(string role)
		{
			if (HttpContext.Current.User == null)
			{
				log.Debug("HttpContext.Current.User == null");
                return false;
			}
			bool isInRole = HttpContext.Current.User.IsInRole(role);
			if (!isInRole)
			{
				log.Debug(HttpContext.Current.User.Identity.Name + " is not in role " + role);
			}
			return isInRole;
		}

		/// <summary>
		/// If true, then the user is connecting to the blog via "localhost" 
		/// on the same server as this is installed.  In other words, we're 
		/// pretty sure the user is a developer.
		/// </summary>
		public static bool UserIsConnectingLocally
		{
			get
			{
				return String.Equals(HttpContext.Current.Request.Url.Host, "localhost", StringComparison.InvariantCultureIgnoreCase)
					&& HttpHelper.GetUserIpAddress(HttpContext.Current).ToString() == HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]
					&& HttpHelper.GetUserIpAddress(HttpContext.Current).ToString() == "127.0.0.1";
			}
		}

		/// <summary>
		/// Generates the symmetric key.
		/// </summary>
		/// <returns></returns>
		public static byte[] GenerateSymmetricKey()
		{
			SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			rijaendel.GenerateKey();
			return rijaendel.Key;
		}

		/// <summary>
		/// Generates the symmetric key.
		/// </summary>
		/// <returns></returns>
		public static byte[] GenerateInitializationVector()
		{
			SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			rijaendel.GenerateIV();
			return rijaendel.IV;
		}

		/// <summary>
		/// Generates the symmetric key.
		/// </summary>
		/// <param name="clearText">The clear text.</param>
		/// <param name="encoding">The encoding.</param>
		/// <param name="key">The key.</param>
		/// <param name="initializationVendor">The initialization vendor.</param>
		/// <returns></returns>
		public static string EncryptString(string clearText, Encoding encoding, byte[] key, byte[] initializationVendor)
		{
			SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			ICryptoTransform encryptor = rijaendel.CreateEncryptor(key, initializationVendor);
			byte[] clearTextBytes = encoding.GetBytes(clearText);
			byte[] encrypted = encryptor.TransformFinalBlock(clearTextBytes, 0, clearTextBytes.Length);
			return Convert.ToBase64String(encrypted);
		}

		/// <summary>
		/// Decrypts the string.
		/// </summary>
		/// <param name="encryptedBase64EncodedString">The encrypted base64 encoded string.</param>
		/// <param name="encoding">The encoding.</param>
		/// <param name="key">The key.</param>
		/// <param name="initializationVendor">The initialization vendor.</param>
		/// <returns></returns>
		public static string DecryptString(string encryptedBase64EncodedString, Encoding encoding, byte[] key, byte[] initializationVendor)
		{
			SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			ICryptoTransform decryptor = rijaendel.CreateDecryptor(key, initializationVendor);
			byte[] encrypted = Convert.FromBase64String(encryptedBase64EncodedString);
			byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
			return encoding.GetString(decrypted);
		}
	}
}
