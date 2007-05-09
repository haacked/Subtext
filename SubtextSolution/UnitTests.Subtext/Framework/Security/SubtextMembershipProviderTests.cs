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
		public void RequiresQuestionAndAnswerIsTrue()
		{
			Assert.IsTrue(Membership.Provider.RequiresQuestionAndAnswer, "Expected RequiresQuestionAndAnswer to be true.");
		}
		
		[Test]
		public void EnablePasswordResetIsTrue()
		{
			Assert.IsTrue(Membership.Provider.EnablePasswordReset, "Expect enablePasswordReset to be true");
		}
		
		[Test]
		public void EnablePasswordRetrievalIsFalse()
		{
			Assert.IsFalse(Membership.Provider.EnablePasswordRetrieval, "Expect enablePasswordRetrieval to be false");
		}
		

		[Test]
		[RollBack]
		public void CanFindUsersByEmail()
		{
			UnitTestHelper.SetupBlog();
			string email = UnitTestHelper.GenerateRandomString() + "@example.com";
			Membership.CreateUser("anothertestuser1", "another-password", email);
			Membership.UpdateUser(Config.CurrentBlog.Owner);

			MembershipUserCollection foundUsers = Membership.FindUsersByEmail(email);
			Assert.AreEqual(1, foundUsers.Count, "Expected to find two users");
			
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
			Membership.CreateUser(name, "whatever-password",  UnitTestHelper.GenerateRandomString() + "@example.com");
			Membership.CreateUser(name + "blah", "secret-password",  UnitTestHelper.GenerateRandomString() + "@example.com");

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
			Membership.CreateUser(name, "whatever-password", UnitTestHelper.GenerateRandomString() + "@example.com");
			Membership.CreateUser(name + "blah", "secret-password", UnitTestHelper.GenerateRandomString() + "@example.com");

			MembershipUserCollection allUsers = Membership.GetAllUsers();
			Assert.GreaterEqualThan(allUsers.Count, 3, "Expected to find at least three users");
		    bool foundFirst = false;
		    bool foundSecond = false;
            foreach(MembershipUser user in allUsers)
            {
                if (user.UserName == name)
                    foundFirst = true;
                if (user.UserName == name + "blah")
                    foundSecond = true;
            }
		    Assert.IsTrue(foundFirst && foundSecond, "Did not find both users we created.");
		}
		
		[Test]
      [RollBack]
		public void CanGetNumberOfUsersOnline()
		{
			UnitTestHelper.SetupBlog();
         Config.CurrentBlog.Owner.LastActivityDate = DateTime.Now;
			Membership.UpdateUser(Config.CurrentBlog.Owner);
			Assert.GreaterEqualThan(Membership.GetNumberOfUsersOnline(), 1);
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
		    string username = UnitTestHelper.MembershipTestUsername;
            string password = UnitTestHelper.MembershipTestPassword;
            UnitTestHelper.SetupBlogWithUserAndPassword(username, password);
            Assert.IsTrue(Membership.Provider.ValidateUser(username, password));
            Assert.IsTrue(Membership.Provider.ChangePassword(username, UnitTestHelper.MembershipTestPassword, "NewPassword"));
            Assert.IsTrue(Membership.Provider.ValidateUser(username, "NewPassword"), "Could not validate user with new password.");
		}

		[Test]
		[RollBack]
		public void CanResetPassword()
		{
            string username = UnitTestHelper.MembershipTestUsername;
		    string password = UnitTestHelper.MembershipTestPassword;
            UnitTestHelper.SetupBlogWithUserAndPassword(username, password);
			Assert.IsTrue(Membership.Provider.ValidateUser(username, password), "Couldn't validate user.");
			string newPassword = Membership.Provider.ResetPassword(username, "It's Subtext Time!");
			Assert.IsFalse(Membership.Provider.ValidateUser(username, password), "Shouldn't be able to validate with old password.");
			Assert.IsNotNull(newPassword, "New password is null, indicating a problem occurred while resetting the password.");
            Assert.IsTrue(Membership.Provider.ValidateUser(username, newPassword), "Could not validate user with new password '" + newPassword + "'.");
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
