using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.SecurityHandling
{
	/// <summary>
	/// Summary description for SecurityTests.
	/// </summary>
	[TestFixture]
	public class SecurityTests
	{
		string _hostName;

		/// <summary>
		/// Makes sure that the UpdatePassword method hashes the password.
		/// </summary>
		[Test]
		[Rollback]
		public void UpdatePasswordHashesPassword()
		{
			Config.Settings.UseHashedPasswords = true;
			Config.CreateBlog("", "username", "thePassword", _hostName, "MyBlog");
			string password = Security.HashPassword("newPass");

			Security.UpdatePassword("newPass");
			BlogInfo info = Config.GetBlogInfo(_hostName, "MyBlog");
			Assert.AreEqual(password, info.Password);
		}

		/// <summary>
		/// Basically a regression test of the HashPasswordMethod.
		/// </summary>
		[Test]
		public void HashPasswordReturnsProperHash()
		{
			Config.CreateBlog("", "username", "thePassword", _hostName, "MyBlog");
			string password = "myPassword";
			string hashedPassword = "Bc5M0y93wXmtXNxwW6IJVA==";
			Assert.AreEqual(hashedPassword, Security.HashPassword(password));
		
			Config.CurrentBlog.IsPasswordHashed = true;
			Config.CurrentBlog.Password = hashedPassword;
			Assert.IsTrue(Security.IsValidPassword(password));

			Config.CurrentBlog.IsPasswordHashed = false;
			Config.CurrentBlog.Password = password;
			Assert.IsTrue(Security.IsValidPassword(password));
		}

		/// <summary>
		/// Ensures HashesPassword is case sensitive.
		/// </summary>
		[Test]
		public void HashPasswordIsCaseSensitive()
		{
			string lowercase = "password";
			string uppercase = "Password";
			Assert.AreNotEqual(Security.HashPassword(lowercase), Security.HashPassword(uppercase), "A lower cased and upper cased password should not be equivalent.");
			Assert.AreNotEqual(Security.HashPassword(lowercase), Security.HashPassword(uppercase.ToUpper(CultureInfo.InvariantCulture)), "A lower cased and a completely upper cased password should not be equivalent.");
		}

		/// <summary>
		/// Want to make sure that we still understand the old 
		/// bitconverter created password.
		/// </summary>
		[Test]
		public void OldBitConverterPasswordUnderstood()
		{
			Config.CreateBlog("", "username", "thePassword", _hostName, "MyBlog");
			string password = "myPassword";
			Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
			Byte[] hashedBytes = new MD5CryptoServiceProvider().ComputeHash(clearBytes);
			string bitConvertedPassword = BitConverter.ToString(hashedBytes);
		
			Config.CurrentBlog.IsPasswordHashed = true;
			Config.CurrentBlog.Password = bitConvertedPassword;
			
			Assert.IsTrue(Security.IsValidPassword(password));
		}

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.  It 
		/// essentially copies the App.config file to the 
		/// run directory.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}
		
		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueHost();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog");
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
