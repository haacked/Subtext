using System;
using System.Web.Security;
using MbUnit.Framework;

namespace UnitTests.Subtext.Framework.SecurityTests
{
	[TestFixture]
	public class SubtextRoleProviderTests
	{
		[Test]
		[RollBack]
		public void IsUserInRoleReturnsCorrectResult()
		{
			UnitTestHelper.SetupBlog();
			Console.WriteLine(Membership.ApplicationName);
			Assert.IsTrue(Roles.IsUserInRole(UnitTestHelper.MembershipTestUsername, "Administrators"), "our fake user should be a member of administrators");
			Assert.IsFalse(Roles.IsUserInRole(UnitTestHelper.MembershipTestUsername, "Commenters"), "our fake user should not be a member of commenters");
		}
	}
}
