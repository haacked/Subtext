using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Framework.Routing;
using MbUnit.Framework;
using System.Web;
using Moq;
using System.Web.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class HttpRouteHandlerTests
    {
        [Test]
        public void RouteHandler_ConstructedWithHttpHandler_ReturnsHttpHandler() { 
            //arrange
            var httpHandler = new FakeHttpHandler();
            var routeHandler = new HttpRouteHandler<FakeHttpHandler>(httpHandler);
            
            //act
            var returnedHandler = routeHandler.GetHttpHandler(null);

            //assert
            Assert.AreEqual(httpHandler, returnedHandler);
        }

        [Test]
        public void RouteHandler_ConstructedWithTypeArgument_ReturnsHandlerOfSaidType()
        {
            //arrange
            var routeHandler = new HttpRouteHandler<FakeHttpHandler>();

            //act
            var returnedHandler = routeHandler.GetHttpHandler(null);

            //assert
            Assert.AreEqual(typeof(FakeHttpHandler), returnedHandler.GetType());
        }

        [Test]
        public void GetHandler_WithRoutableHandler_SetsRequestContext()
        {
            //arrange
            var routeData = new RouteData();
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var handler = new FakeRoutableHttpHandler();
            var routeHandler = new HttpRouteHandler<FakeRoutableHttpHandler>(handler);

            //act
            var returnedHandler = routeHandler.GetHttpHandler(requestContext) as IRoutableHandler;

            //assert
            Assert.AreEqual(requestContext, returnedHandler.RequestContext);
        }

    }

    internal class FakeHttpHandler : IHttpHandler {
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }

    internal class FakeRoutableHttpHandler : IHttpHandler, IRoutableHandler
    {
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public RequestContext RequestContext
        {
            get;
            set;
        }

        public UrlHelper Url
        {
            get { throw new NotImplementedException(); }
        }
    }
}
