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
using System.Globalization;
using System.Text;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

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
			UnitTestHelper.SetupBlog();
			string password = SecurityHelper.HashPassword("newPass");

			SecurityHelper.UpdatePassword("newPass");
			BlogInfo info = Config.GetBlogInfo(Config.CurrentBlog.Host, string.Empty);
			Assert.AreEqual(password, info.Password);
		}

		/// <summary>
		/// Basically a regression test of the HashPasswordMethod.
		/// </summary>
		[Test]
		[RollBack]
		public void HashPasswordReturnsProperHash()
		{
			UnitTestHelper.SetupBlogWithUserAndPassword("username", "thePassword", string.Empty);
			string password = "myPassword";
			string hashedPassword = "Bc5M0y93wXmtXNxwW6IJVA==";
			Assert.AreEqual(hashedPassword, SecurityHelper.HashPassword(password));
		
			Config.CurrentBlog.IsPasswordHashed = true;
			Config.CurrentBlog.Password = hashedPassword;
			Assert.IsTrue(SecurityHelper.IsValidPassword(password));

			Config.CurrentBlog.IsPasswordHashed = false;
			Config.CurrentBlog.Password = password;
			Assert.IsTrue(SecurityHelper.IsValidPassword(password));
		}

		/// <summary>
		/// Ensures HashesPassword is case sensitive.
		/// </summary>
		[Test]
		[RollBack]
		public void HashPasswordIsCaseSensitive()
		{
			string lowercase = "password";
			string uppercase = "Password";
			UnitTestHelper.AssertAreNotEqual(SecurityHelper.HashPassword(lowercase), SecurityHelper.HashPassword(uppercase), "A lower cased and upper cased password should not be equivalent.");
			UnitTestHelper.AssertAreNotEqual(SecurityHelper.HashPassword(lowercase), SecurityHelper.HashPassword(uppercase.ToUpper(CultureInfo.InvariantCulture)), "A lower cased and a completely upper cased password should not be equivalent.");
		}

		[Test]
		[RollBack]
		public void CanSetAuthenticationCookie()
		{
			UnitTestHelper.SetupBlog();
			SecurityHelper.SetAuthenticationTicket(Config.CurrentBlog.UserName, false, "Admins");
			HttpCookie cookie = SecurityHelper.SelectAuthenticationCookie();
			Assert.IsNotNull(cookie, "Could not get authentication cookie.");
		}

		[Test]
		[RollBack]
		public void CanAuthenticateAdmin()
		{
			UnitTestHelper.SetupBlogWithUserAndPassword("the-username", "thePassword", string.Empty);

			Assert.IsTrue(SecurityHelper.Authenticate("the-username", "thePassword", true), "We should be able to login.");
			HttpCookie cookie = SecurityHelper.SelectAuthenticationCookie();
			Assert.IsNotNull(cookie, "Could not get authentication cookie.");
		}
		
		[Test]
		public void CanGenerateSymmetricEncryptionKey()
		{
			byte[] key = SecurityHelper.GenerateSymmetricKey();
			Assert.IsTrue(key.Length > 0, "Expected a non-zero key.");
		}
		
		[Test]
		public void CanSymmetcricallyEncryptAndDecryptText()
		{
			string clearText = "Hello world!";
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

		[TearDown]
		public void TearDown()
		{
		}
	}
}
