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
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using log4net;
using Subtext.Data;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Security
{
	/// <summary>
	/// Handles blog logins/passwords/tickets
	/// </summary>
	public static class SecurityHelper
	{
		private readonly static ILog log = new Log();
        internal const string ApplicationNameContextId = "ApplicationName";

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
			int count = HttpContext.Current.Request.Cookies.Count;

			log.Debug("cookie count = " + count);
			for (int i = 0; i < count; i++)
			{
				HttpCookie c = HttpContext.Current.Request.Cookies[i];
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
					else if (String.IsNullOrEmpty(c.Value))
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
					if (Config.CurrentBlog != null)
						name.Append(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture));
					else
						name.Append("null");
				}
				catch (BlogDoesNotExistException)
				{
					name.Append("null");
				}
			}
			catch (SqlException sqlExc)
			{
				if (sqlExc.Number == (int)SqlErrorMessage.CouldNotFindStoredProcedure
					&& sqlExc.Message.IndexOf("'subtext_GetBlog'") > 0)
				{
					// must not have the db installed.
					log.Debug("The database must not be installed.");
				}
				else throw;
			}
			log.Debug("GetFullCookieName selected cookie named " + name);
			return name.ToString();
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
		/// <param name="salt">Salt for hashing the password</param>
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
		/// Generates a "Random Enough" password. :)
		/// </summary>
		/// <returns></returns>
		public static string RandomPassword()
		{
			return Guid.NewGuid().ToString().Substring(0, 8);
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
				return IsInRole(RoleNames.Administrators);
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
				return IsInRole(RoleNames.HostAdmins);
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
			IPrincipal currentPrincipal = null;
			if (HttpContext.Current != null)
				currentPrincipal = HttpContext.Current.User;

			currentPrincipal = currentPrincipal ?? Thread.CurrentPrincipal;

			if (currentPrincipal == null)
			{
				log.Debug("The Current User is (checked both HttpContext and Thread.CurrentPrincipal) null");
				return false;
			}
			bool isInRole = currentPrincipal.IsInRole(role);
			if (!isInRole)
			{
				log.Debug(currentPrincipal.Identity.Name + " is not in role " + role);
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
			if (clearText == null)
				throw new ArgumentNullException("clearText", Resources.ArgumentNull_Obj);

            if (encoding == null)
                throw new ArgumentNullException("encoding", Resources.ArgumentNull_Obj);

			SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			ICryptoTransform encryptor = rijaendel.CreateEncryptor(key, initializationVendor);

			byte[] clearTextBytes = encoding.GetBytes(clearText);
			byte[] encrypted = encryptor.TransformFinalBlock(clearTextBytes, 0, clearTextBytes.Length);
			return Convert.ToBase64String(encrypted);
		}

		/// <summary>
		/// Decrypts the string.
		/// </summary>
		/// <param name="encryptedData">The encrypted base64 encoded string.</param>
		/// <param name="encoding">The encoding.</param>
		/// <param name="key">The key.</param>
		/// <param name="initializationVendor">The initialization vendor.</param>
		/// <returns></returns>
		public static string DecryptString(string encryptedData, Encoding encoding, byte[] key, byte[] initializationVendor)
		{
			if (encryptedData == null)
				throw new ArgumentNullException("clearText", Resources.ArgumentNull_Obj);

			if (encoding == null)
				throw new ArgumentNullException("encoding", Resources.ArgumentNull_Obj);

            SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			ICryptoTransform decryptor = rijaendel.CreateDecryptor(key, initializationVendor);
			
            byte[] encrypted = Convert.FromBase64String(encryptedData);
			byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
			return encoding.GetString(decrypted);
		}

		/// <summary>
		/// Gets the application id for the current blog.
		/// </summary>
		/// <returns></returns>
		public static string GetApplicationId()
		{
			if (Config.CurrentBlog == null)
				return "/";

			return String.Format(CultureInfo.InvariantCulture, "Blog_{0}", Config.CurrentBlog.Id);
		}
	}
}
