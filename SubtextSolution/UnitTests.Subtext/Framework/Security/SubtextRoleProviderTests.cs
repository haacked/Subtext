using System;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
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
		    string username = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.SetupBlogWithUserAndPassword(username, "password");
            using (MembershipApplicationScope.SetApplicationName(Config.CurrentBlog.ApplicationName))
            {
                Assert.IsTrue(Roles.IsUserInRole(username, "Administrators"), "our fake user should be a member of administrators");
                Assert.IsFalse(Roles.IsUserInRole(username, "Commenters"), "our fake user should not be a member of commenters");
            }
		}

        [Test]
        [RollBack]
        public void CanAddUserToRole()
        {
            string username = UnitTestHelper.MembershipTestUsername;
            MembershipUser user = Membership.CreateUser(username, "abc123@#$#$!", UnitTestHelper.MembershipTestEmail);
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Assert.IsNotNull(user);
                Roles.CreateRole("ATestRole");
                Roles.AddUserToRole(username, "ATestRole");
                Assert.IsTrue(Roles.IsUserInRole(username, "ATestRole"));
            }
        }

        [Test]
        [RollBack]
        public void CanCreateRoles()
        {
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.CreateRole("TestRole1");
                Roles.CreateRole("TestRole2");
                string[] roles = Roles.GetAllRoles();
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "TestRole1"; })), "Could not find TestRole1");
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "TestRole2"; })), "Could not find TestRole2");
            }
        }
	}
}
