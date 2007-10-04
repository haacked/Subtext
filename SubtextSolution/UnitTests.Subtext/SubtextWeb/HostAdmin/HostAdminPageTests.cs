using System;
using MbUnit.Framework;
using Subtext.Web.HostAdmin;

namespace UnitTests.Subtext.SubtextWeb.HostAdmin
{
	[TestFixture]
	public class HostAdminPageTests
	{
		[Test]
		public void AdminSecureCreationTests()
		{
			UnitTestHelper.AssertSecureCreation<HostAdminPage>(new string[] { "HostAdmins" });
		}
	}
}
