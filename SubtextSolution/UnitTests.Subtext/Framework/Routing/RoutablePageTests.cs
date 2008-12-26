using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Routing;
using System.Web.Routing;
using Moq;
using System.Web;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class RoutablePageTests
    {
        [Test]
        public void UrlHelper_PopulatedByRouteCollection_InCtor() {
            //arrange
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            var routes = new RouteCollection();
            var routeablePage = new RoutablePage(routes);
            routeablePage.RequestContext = requestContext;

            //act
            var urlHelper = routeablePage.Url;

            //assert
            Assert.AreEqual(routes, urlHelper.Routes);
        }
    }
}
