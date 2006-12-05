using System;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.SecurityTests
{
	[TestFixture]
	public class SubtextMembershipProviderTests
	{
		[Test]
		[RollBack]
		public void CanUpdateUser()
		{
			UnitTestHelper.SetupBlog();
			Config.CurrentBlog.Owner.Email = "blah@example.com";
			Membership.UpdateUser(Config.CurrentBlog.Owner);
			MembershipUser owner = Membership.GetUser(Config.CurrentBlog.Owner.ProviderUserKey);
			Assert.AreEqual("blah@example.com", owner.Email);
		}

		[Test]
		[RollBack]
		public void CanFindUsersByEmail()
		{
			UnitTestHelper.SetupBlog();
			string email = UnitTestHelper.GenerateRandomString() + "@example.com";
			Membership.CreateUser("anothertestuser0", "whatever-password", email);
			Membership.CreateUser("anothertestuser1", "another-password", email);
			Membership.UpdateUser(Config.CurrentBlog.Owner);

			MembershipUserCollection foundUsers = Membership.FindUsersByEmail(email);
			Assert.AreEqual(2, foundUsers.Count, "Expected to find two users");
			
			foreach(MembershipUser user in foundUsers)
			{
				Assert.AreEqual(email, user.Email, "Hey, we found the wrong user!");
			}
		}

		[Test]
		[RollBack]
		public void CanFindUsersByName()
		{
			UnitTestHelper.SetupBlog();
			string name = UnitTestHelper.GenerateRandomString();
			Membership.CreateUser(name, "whatever-password");
			Membership.CreateUser(name + "blah", "secret-password");

			MembershipUserCollection foundUsers = Membership.FindUsersByName(name);
			Assert.AreEqual(2, foundUsers.Count, "Expected to find two users");

			foreach (MembershipUser user in foundUsers)
			{
				Assert.IsTrue(user.UserName.IndexOf(name) >= 0, "Hey, we found the wrong user!");
			}
		}

		[Test]
		[RollBack]
		public void CanGetUserNameByEmail()
		{
			UnitTestHelper.SetupBlog();
			string userName = Membership.GetUserNameByEmail(Config.CurrentBlog.Owner.Email);
			Assert.AreEqual(Config.CurrentBlog.Owner.UserName, userName, "Usernames match.");
		}

		[Test]
		[RollBack]
		public void CanGetAllUsers()
		{
			UnitTestHelper.SetupBlog();
			string name = UnitTestHelper.GenerateRandomString();
			Membership.CreateUser(name, "whatever-password");
			Membership.CreateUser(name + "blah", "secret-password");

			MembershipUserCollection allUsers = Membership.GetAllUsers();
			Assert.AreEqual(3, allUsers.Count, "Expected to find three users");
		}
		
		[Test]
		[RollBack]
		public void CanGetNumberOfUsersOnline()
		{
			UnitTestHelper.SetupBlog();
			Config.CurrentBlog.Owner.LastActivityDate = DateTime.Now;
			Membership.UpdateUser(Config.CurrentBlog.Owner);
			Assert.AreEqual(1, Membership.GetNumberOfUsersOnline());
		}

		[Test, Ignore("Need to get this to work later.")]
		[RollBack]
		public void CanGetDeleteUser()
		{
			UnitTestHelper.SetupBlog();
			MembershipUser user = Membership.CreateUser("anothertestuser012", "whatever-password");
			string userName = user.UserName;
			Assert.AreNotEqual(Config.CurrentBlog.Owner.ProviderUserKey, user.ProviderUserKey);
			Assert.IsNotNull(Membership.GetUser(userName));
			Assert.IsTrue(Membership.DeleteUser(userName, true));
			Assert.IsNull(Membership.GetUser(userName));
		}

		[Test]
		[RollBack]
		public void CanChangePassword()
		{
			UnitTestHelper.SetupBlog();
			Assert.IsTrue(Membership.Provider.ValidateUser(UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestPassword));
			Membership.Provider.ChangePassword(UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestPassword, "NewPassword");
			Assert.IsTrue(Membership.Provider.ValidateUser(UnitTestHelper.MembershipTestUsername, "NewPassword"), "Could not validate user with new password.");
		}
		

		#region ... Exception Cases ...
		[Test]
		[RollBack]
		[ExpectedArgumentNullException]
		public void FindUsersByEmailThrowsArgumentNullException()
		{
			Membership.FindUsersByEmail(null);
		}

		[Test]
		[RollBack]
		[ExpectedArgumentNullException]
		public void FindUsersByNameThrowsArgumentNullException()
		{
			Membership.FindUsersByName(null);
		}
		#endregion

	}
}
