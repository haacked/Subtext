using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Security;
using Subtext.Web.Controllers;

namespace UnitTests.Subtext.SubtextWeb.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void Logout_LogsUserOut()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.UrlHelper.BlogUrl()).Returns("/blog");
            var accountService = new Mock<IAccountService>();
            accountService.Setup(s => s.Logout());
            var controller = new AccountController(context.Object.UrlHelper, accountService.Object);

            // act
            var result = controller.Logout() as RedirectResult;

            // assert
            accountService.Verify(s => s.Logout());
            Assert.AreEqual("/blog", result.Url);
        }
    }
}
