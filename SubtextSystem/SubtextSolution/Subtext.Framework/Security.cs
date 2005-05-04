using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Subtext.Framework.Configuration;

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
		/// Check to see if the supplied credentials are valid for the current blog. If so, Set the user's FormsAuthentication Ticket
		/// This method will handle passwords for both hashed and non-hashed configurations
		/// </summary>
		/// <param name="username">Supplied UserName</param>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool indicating successful login</returns>
		public static bool Authenticate(string username, string password)
		{
			return Authenticate(username,password,false);
		}

		/// <summary>
		/// Check to see if the supplied credentials are valid for the current blog. If so, Set the user's FormsAuthentication Ticket
		/// This method will handle passwords for both hashed and non-hashed configurations
		/// </summary>
		/// <param name="username">Supplied UserName</param>
		/// <param name="password">Supplied Password</param>
		/// <param name="persist">If valid, should we persist the login</param>
		/// <returns>bool indicating successful login</returns>
		public static bool Authenticate(string username, string password,bool persist)
		{
			//if we don't match username, don't bother with password
			if(IsValidUser(username,password))
			{
					SetTicket(username,persist);
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
		/// Get hashed/encrypted representation of the password. This is a one-way hash.
		/// </summary>
		/// <param name="password">Supplied Password</param>
		/// <returns>Encrypted (Hashed) value</returns>
		public static string Encrypt(string password) 
		{
			// Force the string to lower case
			//
			password = password.ToLower();

			Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
			Byte[] hashedBytes = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

			return BitConverter.ToString(hashedBytes);
		}

		/// <summary>
		/// Validates if the supplied credentials match the current blog
		/// </summary>
		/// <param name="username">Supplied Username</param>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool value indicating if the user is valid.</returns>
		public static bool IsValidUser(string username, string password)
		{
			if(string.Compare(username,Config.CurrentBlog().UserName,true)==0)
			{
				return IsValidPassword(password);
			}
			return false;
		}

		/// <summary>
		/// Check to see if the supplied password matches the password for the current blog. This method will check the BlogConfigurationSettings
		/// to see if the password should be Encrypted/Hashed
		/// </summary>
		/// <param name="password">Supplied Password</param>
		/// <returns>bool value indicating if the supplied password matches the current blog's password</returns>
		public static bool IsValidPassword(string password)
		{
			if(Config.CurrentBlog().IsPasswordHashed)
			{
				password = Encrypt(password);
			}
			return string.Compare(password,Config.CurrentBlog().Password,false)==0;
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
			BlogConfig config = Config.CurrentBlog();
			if(Config.CurrentBlog().IsPasswordHashed)
			{
				config.Password = Encrypt(password);
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

		public static bool IsAdmin
		{
			get
			{
				return string.Compare(GetCurrentUserName,Config.CurrentBlog().UserName,true) == 0;
			}
		}

		public static string GetCurrentUserName
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

	}
}
