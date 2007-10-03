using System;
using System.Web.Security;
using MbUnit.Framework;
using SubSonic;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class HostInfoTests
	{
		[Test]
		[RollBack2]
		public void CanLoadHost()
		{
			SetupHostInfo();
			Assert.IsTrue(HostInfo.HostInfoTableExists);
			Assert.IsNotNull(HostInfo.Instance, "Host should not be null.");
			Assert.IsNotNull(HostInfo.Instance.Owner);
			Assert.Greater(HostInfo.Instance.DateCreated, NullValue.NullDateTime);
			StringAssert.IsNonEmpty(HostInfo.Instance.HostUserName);
		}

		[Test]
		[RollBack2]
		public void CanSetApplicationId()
		{
			SetupHostInfo();
			Guid guid = Guid.NewGuid();
			HostInfo.Instance.ApplicationId = guid;
			Assert.AreEqual(guid, HostInfo.Instance.ApplicationId);
		}

		private static void SetupHostInfo()
		{
			QueryCommand command = new QueryCommand("DELETE subtext_Host");
			DataService.ExecuteQuery(command);

			if (HostInfo.Instance == null || HostInfo.Instance.ApplicationId == Guid.Empty)
			{
				string username = UnitTestHelper.GenerateRandomString();
				MembershipUser user =
					Membership.CreateUser(username, "test", UnitTestHelper.GenerateRandomString() + "@example.com");
				HostInfo.CreateHost(user);
				Assert.AreEqual(username, HostInfo.Instance.Owner.UserName, "The owner was not set.");
			}
		}
	}
}
