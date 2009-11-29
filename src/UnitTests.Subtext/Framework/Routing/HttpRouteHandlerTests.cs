using System;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Ninject;
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
            IKernel kernel = UnitTestHelper.MockKernel(() => new[] {new FakeHttpHandler()});
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            IRouteHandler routeHandler = new HttpRouteHandler<FakeHttpHandler>(kernel);

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

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}