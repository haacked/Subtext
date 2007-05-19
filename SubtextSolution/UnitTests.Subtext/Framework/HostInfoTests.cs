using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class HostInfoTests
	{
		[Test]
		[RollBack]
		public void CanLoadHost()
		{
			SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString, CommandType.Text, "DELETE subtext_Host");

			if (HostInfo.Instance == null)
			{
				Assert.IsNull(HostInfo.Instance, "HostInfo should be Null");
				string username = UnitTestHelper.GenerateRandomString();
				MembershipUser user =
					Membership.CreateUser(username, "test", UnitTestHelper.GenerateRandomString() + "@example.com");
				HostInfo.CreateHost(user);
				Assert.AreEqual(username, HostInfo.Instance.Owner.UserName, "The owner was not set.");
			}

			Assert.IsNotNull(HostInfo.Instance, "Host should not be null.");
			
		}
	}
}
