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
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;
using Subtext.TestLibrary;

namespace UnitTests.Subtext.Framework.SecurityHandling
{
	/// <summary>
	/// Summary description for SecurityTests.
	/// </summary>
	[TestFixture]
	public class SecurityTests
	{
		[Test]
		[RollBack2]
		public void CanGetExpiredCookie()
		{
			UnitTestHelper.SetupBlog();
			HttpCookie cookie = SecurityHelper.GetExpiredCookie();
			Assert.Greater(DateTime.Now.AddYears(-29), cookie.Expires);
		}

		[Test]
		[RollBack2]
		public void CanGetApplicationId()
		{
			HttpContext.Current = null;
			Assert.AreEqual("/", SecurityHelper.GetApplicationId());
			UnitTestHelper.SetupBlog();
			StringAssert.AreEqualIgnoreCase("Blog_" + Config.CurrentBlog.Id, SecurityHelper.GetApplicationId());
		}

		[Test]
		public void IsInRoleReturnsFalseForNullHttpContext()
		{
			HttpContext.Current = null;
			Thread.CurrentPrincipal = null;
			Assert.IsFalse(SecurityHelper.IsInRole(RoleNames.Administrators));

			using (new HttpSimulator().SimulateRequest())
			{
				Assert.IsFalse(SecurityHelper.IsInRole(RoleNames.Administrators));
			}
		}

		[Test]
		[RollBack2]
		public void IsHostAdminReturnsCorrectAnswer()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				Assert.IsFalse(SecurityHelper.IsHostAdmin);
				UnitTestHelper.SetupBlog();
				MembershipUser user = Config.CurrentBlog.Owner;
				using (MembershipApplicationScope.SetApplicationName("/"))
				{
					IPrincipal principal =
						new GenericPrincipal(new GenericIdentity(user.UserName), new string[] {"HostAdmins"});
					HttpContext.Current.User = principal;
					Assert.IsTrue(SecurityHelper.IsHostAdmin);
				}
			}
		}

		/// <summary>
		/// Ensures HashesPassword is case sensitive.
		/// </summary>
		[Test]
		[RollBack2]
		public void HashPasswordIsCaseSensitive()
		{
			string lowercase = "password";
			string uppercase = "Password";
			UnitTestHelper.AssertAreNotEqual(SecurityHelper.HashPassword(lowercase), SecurityHelper.HashPassword(uppercase), "A lower cased and upper cased password should not be equivalent.");
			UnitTestHelper.AssertAreNotEqual(SecurityHelper.HashPassword(lowercase), SecurityHelper.HashPassword(uppercase.ToUpper(CultureInfo.InvariantCulture)), "A lower cased and a completely upper cased password should not be equivalent.");
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
			string clearText = "Hello world!";
			byte[] key = SecurityHelper.GenerateSymmetricKey();
			byte[] iv = SecurityHelper.GenerateInitializationVector();

			string encrypted = SecurityHelper.EncryptString(clearText, Encoding.UTF8, key, iv);
			Assert.IsTrue(encrypted != clearText, "Encrypted text should not equal the clear text.");
			string unencrypted = SecurityHelper.DecryptString(encrypted, Encoding.UTF8, key, iv);
			Assert.AreEqual(clearText, unencrypted, "Round trip encrypt/decrypt failed to produce original string.");
		}

		[RowTest]
		[Row(null, true, ExpectedException = typeof(ArgumentNullException))]
		[Row("Test", false, ExpectedException = typeof(ArgumentNullException))]
		public void EncryptThrowsArgumentNullException(string clearText, bool includeEncoding)
		{
			byte[] key = SecurityHelper.GenerateSymmetricKey();
			byte[] iv = SecurityHelper.GenerateInitializationVector();
			SecurityHelper.EncryptString(clearText, includeEncoding ? Encoding.UTF8 : null, key, iv);
		}

		[RowTest]
		[Row(null, true, ExpectedException = typeof(ArgumentNullException))]
		[Row("Test", false, ExpectedException = typeof(ArgumentNullException))]
		public void DecryptThrowsArgumentNullException(string encryptedText, bool includeEncoding)
		{
			byte[] key = SecurityHelper.GenerateSymmetricKey();
			byte[] iv = SecurityHelper.GenerateInitializationVector();
			SecurityHelper.DecryptString(encryptedText, includeEncoding ? Encoding.UTF8 : null, key, iv);
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
