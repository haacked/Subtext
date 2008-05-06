using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Threading;

namespace WatinTests.Tests.HostAdmin
{
    [TestFixture(ApartmentState = ApartmentState.STA)]
    public class LoginTests
    {
        [Test]
        public void HostAdminLoginFailsWithIncorrectUsernameAndPassword()
        {
            using (Browser browser = new Browser())
            {
                browser.GoToHostAdmin();
                Assert.IsTrue(browser.IsOnLoginPage);
                browser.Login("username", "not-password");
                Assert.IsTrue(browser.ContainsText("That’s not it"), "Expected an error message.");

                browser.Login("not-username", "password");
                Assert.IsTrue(browser.ContainsText("That’s not it"), "Expected an error message.");
            }
        }

        [Test]
        public void CanLoginToHostAdmin()
        {
            using (Browser browser = new Browser())
            {
                browser.GoToHostAdmin();
                Assert.IsTrue(browser.IsOnLoginPage);
                browser.Login("username", "password");
                Assert.IsTrue(browser.ContainsText("Host Admin - Installed Blogs"), "Expected an error message.");
            }
        }
    }
}
