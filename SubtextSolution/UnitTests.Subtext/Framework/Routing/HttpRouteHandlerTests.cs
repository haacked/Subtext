using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class HttpRouteHandlerTests
    {
        [Test]
        public void RouteHandler_ConstructedWithType_InstantiatesNewHandlerEveryTime() 
        {
            // arrange
            var request = new Mock<IRequest>();
            var kernel = new Mock<IKernel>();
            kernel.Setup(k => k.CreateRequest(It.IsAny<Type>(), It.IsAny<Func<IBindingMetadata, bool>>(), It.IsAny<IEnumerable<IParameter>>(), It.IsAny<bool>())).Returns(request.Object);
            kernel.Setup(k => k.Resolve(It.IsAny<IRequest>())).Returns(() => new [] {new FakeHttpHandler()});

            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            IRouteHandler routeHandler = new HttpRouteHandler<FakeHttpHandler>(kernel.Object);

            // act
            var returnedHandler = routeHandler.GetHttpHandler(requestContext);
            var secondHandler = routeHandler.GetHttpHandler(requestContext);

            // assert
            Assert.AreNotSame(returnedHandler, secondHandler);
        }
    }

    internal class FakeHttpHandler : IHttpHandler {
        public FakeHttpHandler() { 
            InstanceId = ++_instanceId;
        }

        private static int _instanceId = 0;
        public int InstanceId { get; private set; }
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
