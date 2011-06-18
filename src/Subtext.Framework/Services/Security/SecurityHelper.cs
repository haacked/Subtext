#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;
using log4net;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;

namespace Subtext.Framework.Security
{
    /// <summary>
    /// Handles blog logins/passwords/tickets
    /// </summary>
    public static class SecurityHelper
    {
        private readonly static ILog Log = new Log();

        /// <summary>
        /// Gets a value indicating whether the current user is a 
        /// Host Admin for the entire installation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [is host admin]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsHostAdministrator(this IPrincipal user)
        {
            return user.IsInRole("HostAdmins");
        }

        /// <summary>
        /// Check to see if the supplied credentials are valid for the current blog. If so, 
        /// Set the user's FormsAuthentication Ticket This method will handle passwords for 
        /// both hashed and non-hashed configurations
        /// </summary>
        public static bool Authenticate(this HttpContextBase httpContext, Blog blog, string username, string password,
                                        bool persist)
        {
            if (!IsValidUser(blog, username, password))
            {
                return false;
            }

            httpContext.SetAuthenticationTicket(blog, username, persist, "Admins");
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
        public static bool Authenticate(string claimedIdentifier, bool persist)
        {
            Blog currentBlog = Config.CurrentBlog;
            if (currentBlog == null)
            {
                return false;
            }

            //If the current blog doesn't have a valid OpenID URI, must fail
            if (!Uri.IsWellFormedUriString(currentBlog.OpenIdUrl, UriKind.Absolute))
            {
                return false;
            }

            //If the cliamed identifier isn't a valid OpenID URI, must fail
            if (!Uri.IsWellFormedUriString(claimedIdentifier, UriKind.Absolute))
            {
                return false;
            }

            var currentBlogClaimUri = new Uri(currentBlog.OpenIdUrl);
            var claimedUri = new Uri(claimedIdentifier);

            if (claimedUri.Host != currentBlogClaimUri.Host ||
               claimedUri.AbsolutePath != currentBlogClaimUri.AbsolutePath ||
               claimedUri.Query != currentBlogClaimUri.Query)
            {
                return false;
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("SetAuthenticationTicket-Admins via OpenID for " + currentBlog.UserName);
            }
            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            httpContext.SetAuthenticationTicket(currentBlog, currentBlog.UserName, persist, "Admins");
            return true;
        }

        public static bool ValidateHostAdminPassword(this HostInfo host, string username, string password)
        {
            if (!String.Equals(username, host.HostUserName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (Config.Settings.UseHashedPasswords)
            {
                password = HashPassword(password, host.Salt);
            }

            return String.Equals(host.Password, password, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Authenticates the host admin.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="persist">if set to <c>true</c> [persist].</param>
        /// <returns></returns>
        public static bool AuthenticateHostAdmin(this HostInfo host, string username, string password, bool persist)
        {
            if (!host.ValidateHostAdminPassword(username, password))
            {
                return false;
            }

            if (Log.IsDebugEnabled)
            {
                Log.Debug("SetAuthenticationTicket-HostAdmins for " + username);
            }
            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
            httpContext.SetAuthenticationTicket(null, username, persist, true, "HostAdmins");

            return true;
        }

        /// <summary>
        /// Used to remove a cookie from the client.
        /// </summary>
        /// <returns>a correctly named cookie with Expires date set 30 years ago</returns>
        public static HttpCookie GetExpiredCookie(this HttpRequestBase request, Blog blog)
        {
            var expiredCookie = new HttpCookie(request.GetFullCookieName(blog)) { Expires = DateTime.UtcNow.AddYears(-30) };
            return expiredCookie;
        }

        /// <summary>
        /// Obtains the correct cookie for the current blog
        /// </summary>
        /// <returns>null if correct cookie was not found</returns>
        public static HttpCookie SelectAuthenticationCookie(this HttpRequestBase request, Blog blog)
        {
            HttpCookie authCookie = null;
            HttpCookie c;
            int count = request.Cookies.Count;

            for (int i = 0; i < count; i++)
            {
                c = request.Cookies[i];

                if (c.Name == request.GetFullCookieName(blog))
                {
                    authCookie = c;
                    break;
                }
            }
            return authCookie;
        }

        /// <summary>
        /// Identifies cookies by unique BlogHost names (rather than a single
        /// name for all cookies in multiblog setups as the old code did).
        /// </summary>
        /// <returns></returns>
        public static string GetFullCookieName(this HttpRequestBase request, Blog blog)
        {
            return request.GetFullCookieName(blog, forceHostAdmin: (blog == null || blog.IsAggregateBlog));
        }

        public static string GetFullCookieName(this HttpRequestBase request, Blog blog, bool forceHostAdmin)
        {
            var name = new StringBuilder(FormsAuthentication.FormsCookieName);
            name.Append(".");

            //See if we need to authenticate the HostAdmin
            string path = request.Path;
            string returnUrl = request.QueryString["ReturnURL"];
            if (forceHostAdmin
               || (path + returnUrl).Contains("HostAdmin", StringComparison.OrdinalIgnoreCase))
            {
                name.Append("HA.");
            }

            if (!forceHostAdmin && blog != null)
            {
                name.Append(blog.Id.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                name.Append("null");
            }
            if (Log.IsDebugEnabled)
            {
                Log.Debug("GetFullCookieName selected cookie named " + name);
            }
            return name.ToString();
        }

        public static void SetAuthenticationTicket(this HttpContextBase httpContext, Blog blog, string username,
                                                   bool persist, params string[] roles)
        {
            httpContext.SetAuthenticationTicket(blog, username, persist, false, roles);
        }

        /// <summary>
        /// Used by methods in this class plus Install.Step02_ConfigureHost
        /// </summary>
        public static void SetAuthenticationTicket(this HttpContextBase httpContext, Blog blog, string username,
                                                   bool persist, bool forceHostAdmin, params string[] roles)
        {
            //Getting a cookie this way and using a temp auth ticket 
            //allows us to access the timeout value from web.config in partial trust.
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(username, persist);
            FormsAuthenticationTicket tempTicket = FormsAuthentication.Decrypt(authCookie.Value);
            string userData = string.Join("|", roles);

            var authTicket = new FormsAuthenticationTicket(
                tempTicket.Version,
                tempTicket.Name,
                tempTicket.IssueDate,
                tempTicket.Expiration, //this is how we access the configured timeout value
                persist,
                //the configured persistence value in web.config is not used. We use the checkbox value on the login page.
                userData, //roles
                tempTicket.CookiePath);
            authCookie.Value = FormsAuthentication.Encrypt(authTicket);
            authCookie.Name = httpContext.Request.GetFullCookieName(blog, forceHostAdmin);
            //prevents login problems with some multiblog setups

            httpContext.Response.Cookies.Add(authCookie);
        }

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
        public static bool IsValidUser(this Blog blog, string username, string password)
        {
            if (String.Equals(username, blog.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return IsValidPassword(blog, password);
            }
            Log.DebugFormat("The supplied username '{0}' does not equal the configured username of '{1}'.", username,
                            blog.UserName);
            return false;
        }

        /// <summary>
        /// Check to see if the supplied password matches the password 
        /// for the current blog. This method will check the 
        /// BlogConfigurationSettings to see if the password should be 
        /// Encrypted/Hashed
        /// </summary>
        public static bool IsValidPassword(Blog blog, string password)
        {
            if (blog.IsPasswordHashed)
            {
                password = HashPassword(password);
            }
            string storedPassword = blog.Password;

            if (storedPassword.IndexOf('-') > 0)
            {
                // NOTE: This is necessary because I want to change how 
                // we store the password.  Maybe changing the password 
                // storage is dumb.  Let me know. -Phil
                //	This is an old password created from BitConverter 
                // string.  Converting to a Base64 hash.
                string[] hashBytesStrings = storedPassword.Split('-');
                var hashedBytes = new byte[hashBytesStrings.Length];
                for (int i = 0; i < hashBytesStrings.Length; i++)
                {
                    hashedBytes[i] = byte.Parse(hashBytesStrings[i].ToString(CultureInfo.InvariantCulture),
                                                NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    storedPassword = Convert.ToBase64String(hashedBytes);
                }
            }

            return String.Equals(password, storedPassword, StringComparison.Ordinal);
        }

        public static bool IsAdministrator(this IPrincipal user)
        {
            if (user == null)
            {
                return false;
            }
            return user.IsInRole("Admins");
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
                return false;
            }
            return HttpContext.Current.User.IsInRole(role);
        }

        /// <summary>
        /// Generates the symmetric key.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSymmetricKey()
        {
            SymmetricAlgorithm rijaendel = Rijndael.Create();
            rijaendel.GenerateKey();
            return rijaendel.Key;
        }

        /// <summary>
        /// Generates the symmetric key.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateInitializationVector()
        {
            SymmetricAlgorithm rijaendel = Rijndael.Create();
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
            SymmetricAlgorithm rijaendel = Rijndael.Create();
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
        public static string DecryptString(string encryptedBase64EncodedString, Encoding encoding, byte[] key,
                                           byte[] initializationVendor)
        {
            SymmetricAlgorithm rijaendel = Rijndael.Create();
            ICryptoTransform decryptor = rijaendel.CreateDecryptor(key, initializationVendor);
            byte[] encrypted = Convert.FromBase64String(encryptedBase64EncodedString);
            byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
            return encoding.GetString(decrypted);
        }
    }
}