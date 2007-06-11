using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace UnitTests.Subtext.Framework.SecurityTests
{
	[TestFixture]
	public class SubtextMembershipProviderTests
	{
		[Test]
		[ExpectedException(typeof(NotImplementedException))]
		public void GetPasswordNotImplementedOnPurpose()
		{
			Membership.Provider.GetPassword("test", "test");
		}

		[Test]
		public void InitializeSetsNameIfNull()
		{
			SubtextMembershipProvider provider = new SubtextMembershipProvider();
			try
			{
				provider.Initialize(null, new NameValueCollection());
			}
			catch(ConfigurationException)
			{
			}
			Assert.AreEqual("SubtextMembershipProvider", provider.Name);
		}

		[Test]
		[RollBack2]
		public void CanUpdateUser()
		{
			UnitTestHelper.SetupBlog();
			Config.CurrentBlog.Owner.Email = "blah@example.com";
			Membership.UpdateUser(Config.CurrentBlog.Owner);
			MembershipUser owner = Membership.GetUser(Config.CurrentBlog.Owner.ProviderUserKey);
			Assert.AreEqual("blah@example.com", owner.Email);
		}
		
		[Test]
		public void CanGetMembershipProviderPropertyValues()
		{
			Assert.AreEqual(null, Membership.Provider.PasswordStrengthRegularExpression);
			Assert.AreEqual(4, Membership.Provider.MinRequiredPasswordLength);
			Assert.AreEqual(0, Membership.Provider.MinRequiredNonAlphanumericCharacters);
			Assert.IsTrue(Membership.Provider.RequiresQuestionAndAnswer, "Expected RequiresQuestionAndAnswer to be true.");
			Assert.IsTrue(Membership.Provider.EnablePasswordReset, "Expect enablePasswordReset to be true");
			Assert.IsFalse(Membership.Provider.EnablePasswordRetrieval, "Expect enablePasswordRetrieval to be false");
		}

		[Test]
		[RollBack2]
		public void CanFindUsersByEmail()
		{
			UnitTestHelper.SetupBlog();
			string email = UnitTestHelper.GenerateRandomString() + "@example.com";
			Membership.CreateUser("anothertestuser1", "another-password", email);
			Membership.UpdateUser(Config.CurrentBlog.Owner);

			MembershipUserCollection foundUsers;
			using (MembershipApplicationScope.SetApplicationName("/"))
			{
				foundUsers = Membership.FindUsersByEmail(email);
			}
			Assert.AreEqual(1, foundUsers.Count, "Expected to find two users");
			
            foreach(MembershipUser user in foundUsers)
			{
				Assert.AreEqual(email, user.Email, "Hey, we found the wrong user!");
			}
		}

		[Test]
		[RollBack2]
		public void CanFindUsersByName()
		{
			UnitTestHelper.SetupBlog();
			string name = UnitTestHelper.GenerateRandomString();
			Membership.CreateUser(name, "whatever-password",  UnitTestHelper.GenerateRandomString() + "@example.com");
			Membership.CreateUser(name + "blah", "secret-password",  UnitTestHelper.GenerateRandomString() + "@example.com");

			MembershipUserCollection foundUsers;
			using (MembershipApplicationScope.SetApplicationName("/"))
			{
				foundUsers = Membership.FindUsersByName(name);
			}
			Assert.AreEqual(2, foundUsers.Count, "Expected to find two users");

			foreach (MembershipUser user in foundUsers)
			{
				Assert.IsTrue(user.UserName.IndexOf(name) >= 0, "Hey, we found the wrong user!");
			}
		}
		
		[Test]
		[RollBack2]
		public void CanGetUserNameByEmail()
		{
			UnitTestHelper.SetupBlog();
			string userName = Membership.GetUserNameByEmail(Config.CurrentBlog.Owner.Email);
			Assert.AreEqual(Config.CurrentBlog.Owner.UserName, userName, "Usernames match.");
		}

		[Test]
		[RollBack2]
		public void CanFindUsersSpecifcToBlog()
		{
			string name = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetupBlogWithUserAndPassword("aaa" + name, "test");
			Membership.CreateUser("z0000" + name, "whatever-password", "z0000" + UnitTestHelper.MembershipTestEmail);
			
			using (MembershipApplicationScope.SetApplicationName(Config.CurrentBlog.ApplicationName))
			{
				MembershipUserCollection users = Membership.FindUsersByName("z0000");
				CollectionAssert.AreCountEqual(0, users);
				users = Membership.FindUsersByEmail("z0000");
				CollectionAssert.AreCountEqual(0, users);

				Roles.AddUserToRole("z0000" + name, RoleNames.Authors);
				users = Membership.FindUsersByEmail("z0000");
				CollectionAssert.AreCountEqual(1, users);
				users = Membership.FindUsersByName("z0000");
				CollectionAssert.AreCountEqual(1, users);
			}
		}

		[Test]
		public void CanGetUserReturnsNull()
		{
			Assert.IsNull(Membership.GetUser(Guid.NewGuid(), false));
		}

		[Test]
		[RollBack2]
		public void CanGetAllUsers()
		{
			UnitTestHelper.SetupBlog();
			string name = UnitTestHelper.GenerateRandomString();
			Membership.CreateUser(name, "whatever-password", UnitTestHelper.GenerateRandomString() + "@example.com");
			Membership.CreateUser(name + "blah", "secret-password", UnitTestHelper.GenerateRandomString() + "@example.com");

			MembershipUserCollection allUsers;
			using(MembershipApplicationScope.SetApplicationName("/"))
			{
				allUsers = Membership.GetAllUsers();
			}
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
		[RollBack2]
		public void CanGetAllUsersForABlog()
		{
			UnitTestHelper.SetupBlog();
            using (MembershipApplicationScope.SetApplicationName(Config.CurrentBlog.ApplicationName))
            {
                string name = UnitTestHelper.GenerateRandomString();
			    MembershipUser user = Membership.CreateUser(name, "whatever-password", UnitTestHelper.MembershipTestEmail);
			    Membership.CreateUser(name + "blah", "secret-password", UnitTestHelper.MembershipTestEmail);

				MembershipUserCollection allUsers = Membership.GetAllUsers();
				Assert.AreEqual(allUsers.Count, 1, "Expected to only find the owner of the blog");
				Roles.AddUserToRole(user.UserName, RoleNames.Anonymous);
				allUsers = Membership.GetAllUsers();
				Assert.AreEqual(allUsers.Count, 2, "Expected to find two users");
			}
		}
		
		[Test]
		[RollBack2]
		public void CanGetNumberOfUsersOnline()
		{
			UnitTestHelper.SetupBlog();
			Config.CurrentBlog.Owner.LastActivityDate = DateTime.Now;
			Membership.UpdateUser(Config.CurrentBlog.Owner);
			Assert.GreaterEqualThan(Membership.GetNumberOfUsersOnline(), 1);
		}

		[Test, Ignore("Need to get this to work later.")]
		[RollBack2]
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
		[RollBack2]
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
		[RollBack2]
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
		[ExpectedArgumentNullException]
		public void InitializeThrowsArgumentNullException()
		{
			SubtextMembershipProvider provider = new SubtextMembershipProvider();
			provider.Initialize("test", null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void FindUsersByEmailThrowsArgumentNullException()
		{
			int total;
			Membership.Provider.FindUsersByEmail(null, 0, 10, out total);
		}

		[Test]
		[ExpectedArgumentException]
		public void FindUsersByEmailThrowsArgumentException()
		{
			int total;
			Membership.Provider.FindUsersByEmail(string.Empty, 0, 10, out total);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void FindUsersByNameThrowsArgumentNullException()
		{
			int total;
			Membership.Provider.FindUsersByName(null, 0, 10, out total);
		}

		[Test]
		[ExpectedArgumentException]
		public void FindUsersByNameThrowsArgumentException()
		{
			int total;
			Membership.Provider.FindUsersByName(string.Empty, 0, 10, out total);
		}

		[Test]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void InitializeThrowsConfigurationException()
		{
			SubtextMembershipProvider provider = new SubtextMembershipProvider();
			provider.Initialize("test", new NameValueCollection());
		}

		[Test]
		[ExpectedException(typeof(ConfigurationErrorsException))]
		public void InitializeThrowsConfigurationExceptionForMissingConnectionString()
		{
			SubtextMembershipProvider provider = new SubtextMembershipProvider();
			NameValueCollection config = new NameValueCollection();
			config.Add("connectionStringName", String.Empty);
			provider.Initialize("test", config);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void UpdateUserThrowsArgumentNullException()
		{
			Membership.UpdateUser(null);
		}

		[RowTest]
		[Row(null, ExpectedException = typeof(ArgumentNullException))]
		[Row("", ExpectedException = typeof(ArgumentException))]
		[RollBack2]
		public void GetUserNameByEmailThrowsArgumentNullException(string email)
		{
			Membership.GetUserNameByEmail(email);
		}

		[RowTest]
		[Row(null, ExpectedException = typeof(ArgumentNullException))]
		[Row("", ExpectedException = typeof(ArgumentException))]
		[RollBack2]
		public void FindUsersByEmailThrowsArgumentNullException(string email)
		{
			Membership.FindUsersByEmail(email);
		}

		[RowTest]
		[Row(null, ExpectedException = typeof(ArgumentNullException))]
		[Row("", ExpectedException = typeof(ArgumentException))]
		[RollBack2]
		public void FindUsersByNameThrowsArgumentNullException(string username)
		{
			Membership.FindUsersByName(username);
		}
		#endregion

	}
}
