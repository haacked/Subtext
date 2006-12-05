using System;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Security;

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
			
			using(MembershipApplicationScope.SetApplicationName("/"))
			{
				Assert.IsTrue(Roles.IsUserInRole(HostInfo.Instance.Owner.UserName, "HostAdmins"), "HostAdmin should be a member of HostAdmins.");
			}
		}
	}
}
