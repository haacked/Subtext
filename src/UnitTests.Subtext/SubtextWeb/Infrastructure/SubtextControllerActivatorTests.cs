using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Subtext.Web.Controllers;
using Subtext.Web.Infrastructure;

namespace UnitTests.Subtext.SubtextWeb.Infrastructure
{
    [TestFixture]
    public class SubtextControllerActivatorTests
    {
        [Test]
        public void Create_WithControllerType_CanCreateControllerOfThatType()
        {
            // arrange
            var serviceLocator = new Mock<IDependencyResolver>();
            serviceLocator.Setup(l => l.GetService(typeof(IdenticonController))).Returns(new IdenticonController());
            var factory = new SubtextControllerActivator(serviceLocator.Object);

            // act
            var controller = factory.Create(null, typeof(IdenticonController));

            // assert
            Assert.AreEqual(typeof(IdenticonController), controller.GetType());
        }

        [Test]
        public void Create_SetsEmptyTempDataProvider()
        {
            // arrange
            var serviceLocator = new Mock<IDependencyResolver>();
            serviceLocator.Setup(l => l.GetService(typeof(IdenticonController))).Returns(new IdenticonController());
            var factory = new SubtextControllerActivator(serviceLocator.Object);

            // act
            var controller = factory.Create(null, typeof(IdenticonController)) as Controller;

            // assert
            Assert.AreEqual(typeof(EmptyTempDataProvider), controller.TempDataProvider.GetType());
        }

    }
}
