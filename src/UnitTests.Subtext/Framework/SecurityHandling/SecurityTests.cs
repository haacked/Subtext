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
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.SecurityHandling
{
    /// <summary>
    /// Summary description for SecurityTests.
    /// </summary>
    [TestFixture]
    public class SecurityTests
    {
        /// <summary>
        /// Makes sure that the UpdatePassword method hashes the password.
        /// </summary>
        [Test]
        [RollBack]
        public void UpdatePasswordHashesPassword()
        {
            string hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "MyBlog");

            Config.Settings.UseHashedPasswords = true;
            Config.CreateBlog("", "username", "thePassword", hostName, "MyBlog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "MyBlog");
            string password = SecurityHelper.HashPassword("newPass");

            SecurityHelper.UpdatePassword("newPass");
            Blog info = Config.GetBlog(hostName, "MyBlog");
            Assert.AreEqual(password, info.Password);
        }

        [Test]
        public void IsValidPassword_WithBlogHavingHashedPasswordMatchingGivenClearTextPassword_ReturnsTrue()
        {
            // arrange
            const string password = "myPassword";
            const string hashedPassword = "Bc5M0y93wXmtXNxwW6IJVA==";
            Assert.AreEqual(hashedPassword, SecurityHelper.HashPassword(password));
            var blog = new Blog {UserName = "username", Password = hashedPassword, IsPasswordHashed = true};

            // act
            bool isValidPassword = SecurityHelper.IsValidPassword(blog, password);

            // assert
            Assert.IsTrue(isValidPassword);
        }

        [Test]
        public void IsValidPassword_WithPasswordHashingEnabledAndGivenTheHashedPassword_ReturnsFalse()
        {
            // arrange
            const string password = "myPassword";
            const string hashedPassword = "Bc5M0y93wXmtXNxwW6IJVA==";
            Assert.AreEqual(hashedPassword, SecurityHelper.HashPassword(password));
            var blog = new Blog {UserName = "username", Password = hashedPassword, IsPasswordHashed = true};

            // act
            bool isValidPassword = SecurityHelper.IsValidPassword(blog, hashedPassword);

            // assert
            Assert.IsFalse(isValidPassword);
        }

        [Test]
        public void IsValidPassword_WithClearTextPasswordMatchingBlogPassword_ReturnsTrue()
        {
            // arrange
            const string password = "myPassword";
            var blog = new Blog {UserName = "username", Password = password, IsPasswordHashed = false};

            // act
            bool isValidPassword = SecurityHelper.IsValidPassword(blog, password);

            // assert
            Assert.IsTrue(isValidPassword);
        }

        /// <summary>
        /// Ensures HashesPassword is case sensitive.
        /// </summary>
        [Test]
        public void HashPasswordIsCaseSensitive()
        {
            const string lowercase = "password";
            const string uppercase = "Password";
            UnitTestHelper.AssertAreNotEqual(SecurityHelper.HashPassword(lowercase),
                                             SecurityHelper.HashPassword(uppercase),
                                             "A lower cased and upper cased password should not be equivalent.");
            UnitTestHelper.AssertAreNotEqual(SecurityHelper.HashPassword(lowercase),
                                             SecurityHelper.HashPassword(uppercase.ToUpper(CultureInfo.InvariantCulture)),
                                             "A lower cased and a completely upper cased password should not be equivalent.");
        }

        /// <summary>
        /// Want to make sure that we still understand the old 
        /// bitconverter created password.
        /// </summary>
        [Test]
        public void IsValidPassword_GivenValidPasswordHashedUsingOldBitConverterStyleHash_ReturnsTrue()
        {
            // arrange
            const string password = "myPassword";
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
            Byte[] hashedBytes = new MD5CryptoServiceProvider().ComputeHash(clearBytes);
            string bitConvertedPassword = BitConverter.ToString(hashedBytes);
            var blog = new Blog {UserName = "username", Password = bitConvertedPassword, IsPasswordHashed = true};

            // act
            bool isValid = SecurityHelper.IsValidPassword(blog, password);

            // assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void SelectAuthenticationCookie_WithCookieNameMatchingBlog_ReturnsThatCookie()
        {
            // arrange
            var cookies = new HttpCookieCollection
            {
                new HttpCookie("This Is Not The Cookie You're Looking For"),
                new HttpCookie(".ASPXAUTH.42") {Path = "/Subtext.Web"},
                new HttpCookie("Awful Cookie")
            };
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());
            request.Setup(r => r.Cookies).Returns(cookies);

            // act
            HttpCookie cookie = request.Object.SelectAuthenticationCookie(new Blog {Id = 42});

            // assert
            Assert.IsNotNull(cookie);
            Assert.AreEqual(".ASPXAUTH.42", cookie.Name);
            Assert.AreEqual("/Subtext.Web", cookie.Path);
        }

        [Test]
        public void GetFullCookieName_WithBlog_ReturnsCookieNameWithBlogId()
        {
            // arrange
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());
            var blog = new Blog {Id = 42};

            // act
            string cookieName = request.Object.GetFullCookieName(blog);

            // assert
            Assert.AreEqual(".ASPXAUTH.42", cookieName);
        }

        [Test]
        public void GetFullCookieName_WithNullBlog_ReturnsCookieNameWithHostAdminMarker()
        {
            // arrange
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());

            // act
            string cookieName = request.Object.GetFullCookieName(null);

            // assert
            Assert.AreEqual(".ASPXAUTH.HA.null", cookieName);
        }

        [Test]
        public void GetFullCookieName_WithAggregateBlog_ReturnsCookieNameWithHostAdminMarker()
        {
            // arrange
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());

            // act
            string cookieName = request.Object.GetFullCookieName(new Blog(true /*isAggregateBlog*/));

            // assert
            Assert.AreEqual(".ASPXAUTH.HA.null", cookieName);
        }

        [Test]
        public void GetFullCookieName_WithReturnUrlPointingToHostAdmin_ReturnsCookieNameWithBlogIdAndHostAdminInitials()
        {
            // arrange
            var request = new Mock<HttpRequestBase>();
            var queryStringParams = new NameValueCollection {{"ReturnUrl", "/HostAdmin"}};
            request.Setup(r => r.QueryString).Returns(queryStringParams);
            var blog = new Blog {Id = 42};

            // act
            string cookieName = request.Object.GetFullCookieName(blog, false);

            // assert
            Assert.AreEqual(".ASPXAUTH.HA.42", cookieName);
        }

        [Test]
        public void GetFullCookieName_WithForceHostAdminTrueAndNullBlog_ReturnsCookieNameWithHostAdminInitials()
        {
            // arrange
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());

            // act
            string cookieName = request.Object.GetFullCookieName(null, true);

            // assert
            Assert.AreEqual(".ASPXAUTH.HA.null", cookieName);
        }

        [Test]
        public void CanAuthenticateAdmin()
        {
            // arrange
            var cookies = new HttpCookieCollection();
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.Path).Returns("/whatever");
            request.Setup(r => r.Cookies).Returns(cookies);
            request.Setup(r => r.QueryString).Returns(new NameValueCollection());
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(request.Object);
            httpContext.Setup(c => c.Response.Cookies).Returns(cookies);
            var blog = new Blog {UserName = "the-username", Password = "thePassword", IsPasswordHashed = false};

            // act
            bool authenticated = httpContext.Object.Authenticate(blog, "the-username", "thePassword", true);

            // assert
            Assert.IsTrue(authenticated);
            HttpCookie cookie = request.Object.SelectAuthenticationCookie(blog);
            Assert.IsNotNull(cookie);
        }

        [Test]
        public void CanGenerateSymmetricEncryptionKey()
        {
            byte[] key = SecurityHelper.GenerateSymmetricKey();
            Assert.IsTrue(key.Length > 0, "Expected a non-zero key.");
        }

        [Test]
        public void CanSymmetricallyEncryptAndDecryptText()
        {
            const string clearText = "Hello world!";
            byte[] key = SecurityHelper.GenerateSymmetricKey();
            byte[] iv = SecurityHelper.GenerateInitializationVector();

            string encrypted = SecurityHelper.EncryptString(clearText, Encoding.UTF8, key, iv);
            Assert.IsTrue(encrypted != clearText, "Encrypted text should not equal the clear text.");
            string unencrypted = SecurityHelper.DecryptString(encrypted, Encoding.UTF8, key, iv);
            Assert.AreEqual(clearText, unencrypted, "Round trip encrypt/decrypt failed to produce original string.");
        }

        /// <summary>
        /// Sets the up test fixture.  This is called once for 
        /// this test fixture before all the tests run.
        /// </summary>
        [TestFixtureSetUp]
        public void SetUpTestFixture()
        {
            //Confirm app settings
            UnitTestHelper.AssertAppSettings();
        }
    }
}