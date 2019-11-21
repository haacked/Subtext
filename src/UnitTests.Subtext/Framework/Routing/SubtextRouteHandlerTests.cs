using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using STRouting = Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class SubtextRouteHandlerTests
    {
        [TestMethod]
        public void GetHandler_WhichReturnsIPageWithControls_SetsControls()
        {
            //arrange
            IEnumerable<string> controlNames = null;
            var routeData = new RouteData();
            routeData.DataTokens.Add("controls", new[] { "SomeControl" });
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var pageWithControls = new Mock<STRouting.IPageWithControls>();
            pageWithControls.Setup(p => p.SetControls(It.IsAny<IEnumerable<string>>())).Callback<IEnumerable<string>>(
                c => controlNames = c);
            var pageBuilder = new Mock<STRouting.ISubtextPageBuilder>();
            pageBuilder.Setup(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(
                pageWithControls.Object);
            IRouteHandler subtextRouteHandler = new STRouting.PageRouteHandler("~/aspx/Dtp.aspx", pageBuilder.Object,
                                                                     new Mock<IDependencyResolver>().Object);

            //act
            subtextRouteHandler.GetHttpHandler(requestContext);

            //assert.
            Assert.AreEqual("SomeControl", controlNames.First());
        }
    }
}