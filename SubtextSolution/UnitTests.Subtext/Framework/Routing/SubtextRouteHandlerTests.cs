using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;
using System;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class SubtextRouteHandlerTests
    {
        [Test]
        public void GetHandler_WithSubtextPage_SetsControls() { 
            //arrange
            IEnumerable<string> controlNames = null;

            var routeData = new RouteData();
            routeData.DataTokens.Add("controls", new string[] {"SomeControl"});
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var subtextPage = new Mock<ISubtextPage>();
            subtextPage.Expect(p => p.SetControls(It.IsAny<IEnumerable<string>>())).Callback<IEnumerable<string>>(c => controlNames = c);
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(subtextPage.Object);
            IRouteHandler subtextRouteHandler = new PageRouteHandler("~/Dtp.aspx", pageBuilder.Object);

            //act
            var handler = subtextRouteHandler.GetHttpHandler(requestContext) as ISubtextPage;

            //assert.
            Assert.AreEqual("SomeControl.ascx", controlNames.First());
        }

        [Test]
        public void GetHandler_WithRoutableHandler_SetsRequestContext()
        {
            //arrange
            var routeData = new RouteData();
            routeData.DataTokens.Add("controls", new string[] { "SomeControl" });
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var routableHandler = new Mock<IRouteableHandler>();
            RequestContext setContext = null;
            routableHandler.ExpectSet(h => h.RequestContext).Callback(context => setContext = context);
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(routableHandler.Object);
            IRouteHandler subtextRouteHandler = new PageRouteHandler("~/Dtp.aspx", pageBuilder.Object);

            //act
            var handler = subtextRouteHandler.GetHttpHandler(requestContext) as ISubtextPage;

            //assert.
            Assert.AreEqual(setContext, requestContext);
        }
    }
}
