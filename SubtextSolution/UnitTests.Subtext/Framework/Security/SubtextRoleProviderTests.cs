using System;
using System.Configuration.Provider;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace UnitTests.Subtext.Framework.SecurityTests
{
	[TestFixture]
	public class SubtextRoleProviderTests
	{
		/// <summary>
		/// When making a user the owner of a blog, we want to special case 
		/// the IsInRole check to return true when checking if the user is 
		/// an administrator if the user isn't explicitely a member of 
		/// the Administrators role for that blog.
		/// </summary>
		[Test]
		[RollBack2]
		public void OwnerOfBlogIsSpecialCasedAsBeingInAdministratorsRole()
		{
			UnitTestHelper.SetupBlog();
			MembershipUser oldOwner = Config.CurrentBlog.Owner;
			MembershipUser newOwner = Membership.CreateUser(UnitTestHelper.MembershipTestUsername, "test", UnitTestHelper.MembershipTestEmail);
			using(MembershipApplicationScope.SetApplicationName(Config.CurrentBlog.ApplicationName))
			{
				string[] roles = Roles.GetRolesForUser(newOwner.UserName);
				CollectionAssert.DoesNotContain(roles, RoleNames.Administrators, "The user should not have Administrators as one of its asserts");
				Assert.IsFalse(Roles.IsUserInRole(newOwner.UserName, RoleNames.Administrators), "This user should not yet be an admin of this blog.");

				Config.CurrentBlog.Owner = newOwner;
				Config.UpdateConfigData(Config.CurrentBlog);
				Assert.IsTrue(Roles.IsUserInRole(newOwner.UserName, RoleNames.Administrators), "Now that this user is the owner of the blog, we should return true when asked if the user is an admin.");
				roles = Roles.GetRolesForUser(newOwner.UserName);
				CollectionAssert.Contains(roles, RoleNames.Administrators, "The user should now be an Administrator");
				
				Config.CurrentBlog.Owner = oldOwner;
				Config.UpdateConfigData(Config.CurrentBlog);
				Assert.IsFalse(Roles.IsUserInRole(newOwner.UserName, RoleNames.Administrators), "Since we never explicitely added the user to the admin role, this user should no longer be an admin.");
				roles = Roles.GetRolesForUser(newOwner.UserName);
				CollectionAssert.DoesNotContain(roles, RoleNames.Administrators, "The user should no longer be an administrator");
			}
		}

		[Test]
		[RollBack2]
		public void IsUserInRoleReturnsCorrectResult()
		{
		    string username = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.SetupBlogWithUserAndPassword(username, "password");
            using (MembershipApplicationScope.SetApplicationName(Config.CurrentBlog.ApplicationName))
            {
				Assert.IsTrue(Roles.IsUserInRole(username, RoleNames.Administrators), "our fake user should be a member of administrators");
                Assert.IsFalse(Roles.IsUserInRole(username, RoleNames.Commenters), "our fake user should not be a member of commenters");
            }
		}

        [Test]
        [RollBack2]
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
		[RollBack2]
		public void CanRemoveUserFromRole()
		{
			string username = UnitTestHelper.MembershipTestUsername;
			MembershipUser user = Membership.CreateUser(username, "abc123@#$#$!", UnitTestHelper.MembershipTestEmail);
			using (MembershipApplicationScope.SetApplicationName("/"))
			{
				Assert.IsNotNull(user);
				Roles.CreateRole("ATestRole");
				Roles.AddUsersToRoles(new string[] { username }, new string[] { "ATestRole" });
				Assert.IsTrue(Roles.IsUserInRole(username, "ATestRole"));
				Roles.RemoveUserFromRole(username, "ATestRole");
				Assert.IsFalse(Roles.IsUserInRole(username, "ATestRole"));

				Roles.AddUsersToRole(new string[] { username }, "ATestRole");
				Assert.IsTrue(Roles.IsUserInRole(username, "ATestRole"));
				Roles.RemoveUsersFromRole(new string[] { username }, "ATestRole");
				Assert.IsFalse(Roles.IsUserInRole(username, "ATestRole"));
			}
		}

		[Test]
        [RollBack2]
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

        [Test]
        [RollBack2]
        [ExpectedException(typeof(NotImplementedException))]
        public void CanDeleteRole()
        {
            //DeleteRole has not been implemented yet.
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.CreateRole("ATestRole");
                string[] roles = Roles.GetAllRoles();
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "ATestRole"; })), "Could not find the role 'ATestRole'");
                Roles.DeleteRole("ATestRole");
                roles = Roles.GetAllRoles();
                Assert.IsNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "ATestRole"; })), "Could not find TestRole1");
            }
        }

        [Test]
        [RollBack2]
        [ExpectedException(typeof(NotImplementedException))]
        public void CanFindUsersInRole()
        {
            string username = "x" + UnitTestHelper.MembershipTestUsername;
            //DeleteRole has not been implemented yet.
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.CreateRole("ATestRole");
                string[] roles = Roles.GetAllRoles();
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "ATestRole"; })), "Could not find the role 'ATestRole'");
                Membership.CreateUser(username, UnitTestHelper.MembershipTestPassword, "test");
                Roles.AddUserToRole(username, "ATestRole");
                string[] usernames = Roles.FindUsersInRole("ATestRole", "x");
                Assert.GreaterEqualThan(usernames.Length, 1, "Should have found at least one user.");
                Assert.AreEqual(username, Array.Find(usernames, new Predicate<string>(delegate(string value) { return value == username; })), "Could not find the user '" + username + "'");
            }
        }

        [Test]
        [RollBack2]
        public void CanGetUsersInRole()
        {
            string username1 = UnitTestHelper.MembershipTestUsername;
            string username2 = UnitTestHelper.MembershipTestUsername;

            Membership.CreateUser(username1, UnitTestHelper.MembershipTestPassword, UnitTestHelper.MembershipTestEmail);
            Membership.CreateUser(username2, UnitTestHelper.MembershipTestPassword, UnitTestHelper.MembershipTestEmail);

            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.CreateRole("AnotherTestRole");
                Roles.AddUserToRole(username1, "AnotherTestRole");
                Roles.AddUserToRole(username2, "AnotherTestRole");
                string[] usernames = Roles.GetUsersInRole("AnotherTestRole");
                Assert.GreaterEqualThan(usernames.Length, 2, "Should have found at least two users.");
                Assert.AreEqual(username1, Array.Find(usernames, new Predicate<string>(delegate(string value) { return value == username1; })), "Could not find the user '" + username1 + "'");
                Assert.AreEqual(username2, Array.Find(usernames, new Predicate<string>(delegate(string value) { return value == username2; })), "Could not find the user '" + username2 + "'");
            }
        }

	    [Test]
        [RollBack2]
        public void CanGetRolesForUser()
        {
            string username = UnitTestHelper.MembershipTestUsername;
            //DeleteRole has not been implemented yet.
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Membership.CreateUser(username, UnitTestHelper.MembershipTestPassword, "test");
                Roles.CreateRole("ATestRole1");
                Roles.CreateRole("ATestRole2");
                Roles.CreateRole("ATestRole3");
                Roles.AddUserToRole(username, "ATestRole1");
                Roles.AddUserToRole(username, "ATestRole2");
                Roles.AddUserToRole(username, "ATestRole3");

                string[] roles = Roles.GetRolesForUser(username);
                Assert.AreEqual(3, roles.Length, "Expected the user to be in three roles.");
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "ATestRole1"; })), "Could not find the role 'ATestRole1'");
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "ATestRole2"; })), "Could not find the role 'ATestRole2'");
                Assert.IsNotNull(Array.Find(roles, new Predicate<string>(delegate(string value) { return value == "ATestRole2"; })), "Could not find the role 'ATestRole3'");
            }
        }

        #region Exception Tests
        [Test]
        [ExpectedArgumentNullException]
        [RollBack2]
        public void CreateRoleThrowsArgumentNullException()
        {
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.Provider.CreateRole(null);
            }
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        [RollBack2]
        public void CreateRoleThrowsProviderExceptionForExistingRole()
        {
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.Provider.CreateRole("DuplicateRole");
                Roles.Provider.CreateRole("DuplicateRole");
            }
        }

        [RowTest]
        [Row("", ExpectedException = typeof(ArgumentException))]
        [Row("HasA,Comma", ExpectedException = typeof(ArgumentException))]
        [RollBack2]
        public void CreateRoleThrowsArgumentException(string rolename)
        {
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.Provider.CreateRole(rolename);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        [RollBack2]
        public void CreateRoleThrowsArgumentExceptionForTooLongRoleName()
        {
            string rolename = new string('a', 513);
            using (MembershipApplicationScope.SetApplicationName("/"))
            {
                Roles.Provider.CreateRole(rolename);
            }
        }
        #endregion
    }
}
