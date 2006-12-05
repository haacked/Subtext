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
using MbUnit.Framework;
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
