using System;
using System.Threading;
using MbUnit.Framework;
using WatiN.Core;

namespace WatinTests.Tests.Admin
{
	[TestFixture(ApartmentState = ApartmentState.STA)]
	public class LoginTests
	{
		[Test]
		public void LoginRequiresCorrectUsernameAndPassword()
		{
			using (Browser browser = new Browser())
			{
				browser.GoToAdmin();
				Assert.IsTrue(browser.IsOnLoginPage);
				browser.Login("username", "not-password");
				Assert.IsTrue(browser.ContainsText("That’s not it"), "Expected an error message.");

				browser.Login("not-username", "password");
				Assert.IsTrue(browser.ContainsText("That’s not it"), "Expected an error message.");
			}
		}

		[Test]
		public void EmailPasswordFailsForUnknownUserName()
		{
			using(Browser browser = new Browser())
			{
				browser.GoToAdmin();
				browser.Login("not-username", "password");
				browser.Link(Find.ByText("Email me my password.")).Click();
				Assert.IsTrue(browser.ContainsText("I don't know you"));
			}
		}
	}
}
