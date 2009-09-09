using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Ninject;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class SubtextRouteHandlerTests
    {
        [Test]
        public void GetHandler_WhichReturnsIPageWithControls_SetsControls()
        {
            //arrange
            IEnumerable<string> controlNames = null;
            var routeData = new RouteData();
            routeData.DataTokens.Add("controls", new[] {"SomeControl"});
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var pageWithControls = new Mock<IPageWithControls>();
            pageWithControls.Setup(p => p.SetControls(It.IsAny<IEnumerable<string>>())).Callback<IEnumerable<string>>(
                c => controlNames = c);
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            pageBuilder.Setup(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(
                pageWithControls.Object);
            IRouteHandler subtextRouteHandler = new PageRouteHandler("~/pages/Dtp.aspx", pageBuilder.Object,
                                                                     new Mock<IKernel>().Object);

            //act
            var handler = subtextRouteHandler.GetHttpHandler(requestContext) as ISubtextHandler;

            //assert.
            Assert.AreEqual("SomeControl.ascx", controlNames.First());
        }
    }
}