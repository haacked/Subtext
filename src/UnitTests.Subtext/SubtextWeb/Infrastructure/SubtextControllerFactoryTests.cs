using System;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Subtext.Infrastructure;
using Subtext.Web.Controllers;
using Subtext.Web.Infrastructure;

namespace UnitTests.Subtext.SubtextWeb.Infrastructure
{
    [TestFixture]
    public class SubtextControllerFactoryTests
    {
        [Test]
        public void CreateController_WithControllerName_CanCreateControllerCaseInsensitively()
        {
            // arrange
            var serviceLocator = new Mock<IDependencyResolver>();
            serviceLocator.Setup(l => l.GetService(typeof(IdenticonController))).Returns(new IdenticonController());
            var factory = new SubtextControllerFactory(serviceLocator.Object);

            // act
            var controller = factory.CreateController(null, "identicon");

            // assert
            Assert.AreEqual(typeof(IdenticonController), controller.GetType());
        }

        [Test]
        public void ReleaseController_WithDisposableController_CallsDisposeOnController()
        {
            // arrange
            var serviceLocator = new Mock<IDependencyResolver>();
            var factory = new SubtextControllerFactory(serviceLocator.Object);
            var controller = new Mock<IController>();
            var disposable = controller.As<IDisposable>();
            disposable.Setup(d => d.Dispose());
            
            // act
            factory.ReleaseController(controller.Object);

            // assert
            disposable.Verify(d => d.Dispose());
        }
    }
}
