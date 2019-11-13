using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.Skins._System.Controls;

namespace UnitTests.Subtext.Framework.Skinning
{
    [TestFixture]
    public class ErrorUserControlTests
    {
        [Test]
        public void ShowErrorDetails_ForLocalHostNonAdmin_ReturnsTrue()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.HttpContext.Request.IsLocal).Returns(true);
            context.Setup(c => c.User.IsInRole("Admins")).Returns(false);
            var page = new SubtextPage {SubtextContext = context.Object};
            var control = new Error {Page = page};

            // act
            bool result = control.ShowErrorDetails;

            // assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShowErrorDetails_ForLocalHostAdmin_ReturnsTrue()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.HttpContext.Request.IsLocal).Returns(true);
            context.Setup(c => c.User.IsInRole("Admins")).Returns(true);
            var page = new SubtextPage { SubtextContext = context.Object };
            var control = new Error { Page = page };

            // act
            bool result = control.ShowErrorDetails;

            // assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ShowErrorDetails_ForNonLocalHost_ReturnsFalse()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.HttpContext.Request.IsLocal).Returns(false);
            context.Setup(c => c.User.IsInRole("Admins")).Returns(false);
            var page = new SubtextPage { SubtextContext = context.Object };
            var control = new Error { Page = page };

            // act
            bool result = control.ShowErrorDetails;

            // assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ShowErrorDetails_ForAdminNonLocalHost_ReturnsTrue()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.HttpContext.Request.IsLocal).Returns(false);
            context.Setup(c => c.User.IsInRole("Admins")).Returns(true);
            var page = new SubtextPage { SubtextContext = context.Object };
            var control = new Error { Page = page };

            // act
            bool result = control.ShowErrorDetails;

            // assert
            Assert.IsTrue(result);
        }
    }
}
