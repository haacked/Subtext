using System;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class HttpRouteHandlerTests
    {
        [Test]
        public void RouteHandler_ConstructedWithType_InstantiatesNewHandlerEveryTime()
        {
            // arrange
            var serviceLocator = new Mock<IServiceLocator>();
            serviceLocator.Setup(l => l.GetService<FakeHttpHandler>()).Returns(() => new FakeHttpHandler());
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            IRouteHandler routeHandler = new HttpRouteHandler<FakeHttpHandler>(serviceLocator.Object);

            // act
            IHttpHandler returnedHandler = routeHandler.GetHttpHandler(requestContext);
            IHttpHandler secondHandler = routeHandler.GetHttpHandler(requestContext);

            // assert
            Assert.AreNotSame(returnedHandler, secondHandler);
        }
    }

    internal class FakeHttpHandler : IHttpHandler
    {
        private static int _instanceId;

        public FakeHttpHandler()
        {
            InstanceId = ++_instanceId;
        }

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