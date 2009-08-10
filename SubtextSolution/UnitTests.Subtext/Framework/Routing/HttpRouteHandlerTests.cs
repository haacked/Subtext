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
using Ninject.Activation.Blocks;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class HttpRouteHandlerTests
    {
        [Test]
        public void RouteHandler_ConstructedWithType_InstantiatesNewHandlerEveryTime() 
        {
            // arrange
            var hook = new Hook(() => new FakeHttpHandler());
            var hooks = new[] { hook };
            var request = new Mock<IRequest>();
            var activationBlock = new Mock<IActivationBlock>();
            activationBlock.Setup(a => a.CreateRequest(It.IsAny<Type>(), It.IsAny<Func<IBindingMetadata, bool>>(), It.IsAny<IEnumerable<IParameter>>(), It.IsAny<bool>())).Returns(request.Object);
            activationBlock.Setup(a => a.Resolve(It.IsAny<IRequest>())).Returns(hooks);
            var kernel = new Mock<IKernel>();
            kernel.Setup(k => k.BeginBlock()).Returns(activationBlock.Object);
            
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
