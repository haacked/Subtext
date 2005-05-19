using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;

namespace Subtext.Framework
{
	/// <summary>
	/// Handles blog logins/passwords/tickets
	/// </summary>
	public sealed class Security
	{
		//Can not instantiate this object
		private Security(){}

		/// <summary>
		/// Check to see if the supplied credentials are valid for the current blog. 
		/// If so, Set the user's FormsAuthentication Ticket This method will handle 
		/// passwords for both hashed and non-hashed configurations
		/// </summary>
		/// <param name="username">Supplied UserName</param>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool indicating successful login</returns>
		public static bool Authenticate(string username, string password)
		{
			return Authenticate(username, password, false);
		}

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
			//if we don't match username, don't bother with password
			if(IsValidUser(username, password))
			{
					SetTicket(username, persist);
					return true;
			}
			return false;
		}

		//Maybe this method should be public?

		/// <summary>
		/// Private method to set FormsAuthentication Ticket. 
		/// </summary>
		/// <param name="username">Username for the ticket</param>
		/// <param name="persist">Should this ticket be persisted</param>
		private static void SetTicket(string username, bool persist)
		{
			FormsAuthentication.SetAuthCookie(username,persist);
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
		/// Validates if the supplied credentials match the current blog
		/// </summary>
		/// <param name="username">Supplied Username</param>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool value indicating if the user is valid.</returns>
		public static bool IsValidUser(string username, string password)
		{
			if(string.Compare(username, Config.CurrentBlog.UserName, true) == 0)
			{
				return IsValidPassword(password);
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
					hashedBytes[i] = byte.Parse(hashBytesStrings[i].ToString(), NumberStyles.HexNumber);
					storedPassword = Convert.ToBase64String(hashedBytes);
				}
			}
			return string.Compare(password, storedPassword, false) == 0;
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
		/// Updates the current users password to the supplied value. Handles hashing (or not hashing of the password)
		/// </summary>
		/// <param name="password">Supplied Password</param>
		public static void UpdatePassword(string password)
		{
			BlogConfig config = Config.CurrentBlog;
			if(Config.CurrentBlog.IsPasswordHashed)
			{
				config.Password = HashPassword(password);
			}
			else
			{
				config.Password = password;
			}
			//Save new password.
			Config.UpdateConfigData(config);
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
				return string.Compare(CurrentUserName, Config.CurrentBlog.UserName, true) == 0;
			}
		}

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
		/// If true, then the user is connecting to the blog via "localhost" 
		/// on the same server as this is installed.  In other words, we're 
		/// pretty sure the user is a developer.
		/// </summary>
		public static bool UserIsConnectingLocally
		{
			get
			{
				return StringHelper.AreEqualIgnoringCase(HttpContext.Current.Request.Url.Host, "localhost")
					&& HttpContext.Current.Request.UserHostAddress == HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]
					&& HttpContext.Current.Request.UserHostAddress == "127.0.0.1";
			}
		}

	}
}
