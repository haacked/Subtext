using System;
using System.Globalization;
using NUnit.Framework;
using Subtext.Framework;

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
	}
}
