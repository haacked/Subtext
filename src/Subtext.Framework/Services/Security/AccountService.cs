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
using System.Security.Cryptography;
using System.Web;

namespace Subtext.Framework.Security
{
    public class AccountService : IAccountService
    {
        public AccountService(ISubtextContext context)
        {
            SubtextContext = context;
        }

        protected ISubtextContext SubtextContext
        {
            get;
            private set;
        }

        public void Logout()
        {
            var request = SubtextContext.HttpContext.Request;
            var response = SubtextContext.HttpContext.Response;

            LogoutCookie(response, request, SubtextContext.Blog);
            LogoutCookie(response, request, null /* Host Admin */);
        }

        private static void LogoutCookie(HttpResponseBase response, HttpRequestBase request, Blog blog)
        {
            var cookieName = request.GetFullCookieName(blog);
            if (request.Cookies[cookieName] != null)
            {
                var authCookie = new HttpCookie(cookieName)
                                 {
                                     HttpOnly = true,
                                     Expires = DateTime.UtcNow.AddYears(-30),
                                     Value =
                                         request.Browser == null ||
                                         request.Browser["supportsEmptyStringInCookieValue"] == "false"
                                             ? "Empty"
                                             : String.Empty
                                 };
                request.Cookies.Remove(cookieName);
                response.Cookies.Add(authCookie);
            }
        }

        public virtual void UpdatePassword(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }
            var blog = SubtextContext.Blog;
            blog.Password = blog.IsPasswordHashed ? SecurityHelper.HashPassword(password) : password;
            SubtextContext.Repository.UpdateBlog(blog);
        }

        public string ResetPassword()
        {
            string password = GenerateRandomPassword();
            UpdatePassword(password);
            return password;
        }

        private string GenerateRandomPassword()
        {
            byte[] data = new byte[16];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        protected bool IsValidPassword(string password, string storedPassword, bool hashed, string salt)
        {
            return storedPassword == (hashed ? SecurityHelper.HashPassword(password, salt) : password);
        }
    }
}
