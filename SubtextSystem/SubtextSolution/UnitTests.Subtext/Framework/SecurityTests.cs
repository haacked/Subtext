using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using NUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	/// <summary>
	/// Unit tests of the Subtext.Framework.Security class methods.
	/// </summary>
	[TestFixture]
	public class SecurityTests
	{
		/// <summary>
		/// Basically a regression test of the HashPasswordMethod.
		/// </summary>
		[Test]
		public void HashPasswordReturnsProperHash()
		{
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
			string password = "myPassword";
			Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
			Byte[] hashedBytes = new MD5CryptoServiceProvider().ComputeHash(clearBytes);
			string bitConvertedPassword = BitConverter.ToString(hashedBytes);
		
			Config.CurrentBlog.IsPasswordHashed = true;
			Config.CurrentBlog.Password = bitConvertedPassword;
			
			Assert.IsTrue(Security.IsValidPassword(password));
		}

		[SetUp]
		public void SetUp()
		{
			Config.ConfigurationProvider = new UnitTestConfigProvider();

			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
