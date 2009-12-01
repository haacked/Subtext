using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Moq;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Subtext.Framework.Infrastructure;

namespace UnitTests.Subtext.Framework.Infrastructure
{
    [TestFixture]
    public class NinjectServiceLocatorTests
    {
        [Test]
        public void GetService_WithKernel_DelegatesToKernel()
        {
            // arrange
            Func<IEnumerable<object>> returnFunc = () => new[] {new TestService()};
            var request = new Mock<IRequest>();
            var kernel = new Mock<IKernel>();
            kernel.Setup(
                k =>
                k.CreateRequest(typeof(ITestService), It.IsAny<Func<IBindingMetadata, bool>>(),
                                It.IsAny<IEnumerable<IParameter>>(), It.IsAny<bool>())).Returns(request.Object);
            kernel.Setup(k => k.Resolve(It.IsAny<IRequest>())).Returns(returnFunc);

            var serviceLocator = new NinjectServiceLocator(kernel.Object);

            // act
            var service = serviceLocator.GetService<ITestService>();

            // assert
            Assert.IsNotNull(service);
            Assert.AreEqual(typeof(TestService), service.GetType());
        }

        [Test]
        public void GetService_WithServiceTypeAndKernel_DelegatesToKernel()
        {
            // arrange
            Func<IEnumerable<object>> returnFunc = () => new[] { new TestService() };
            var request = new Mock<IRequest>();
            var kernel = new Mock<IKernel>();
            kernel.Setup(
                k =>
                k.CreateRequest(typeof(ITestService), It.IsAny<Func<IBindingMetadata, bool>>(),
                                It.IsAny<IEnumerable<IParameter>>(), It.IsAny<bool>())).Returns(request.Object);
            kernel.Setup(k => k.Resolve(It.IsAny<IRequest>())).Returns(returnFunc);

            var serviceLocator = new NinjectServiceLocator(kernel.Object);

            // act
            var service = serviceLocator.GetService(typeof(ITestService));

            // assert
            Assert.IsNotNull(service);
            Assert.AreEqual(typeof(TestService), service.GetType());
        }
    }

    public interface ITestService
    {
        
    }

    public interface IDerivedService : ITestService {}

    public class TestService : IDerivedService {}
}
