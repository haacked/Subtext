using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;
using System.Web.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class RootRouteTests
    {
        [Test]
        public void GetRouteDataWithRequestForAppRoot_WhenAggregationEnabled_MatchesAndReturnsAggDefault() { 
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/", string.Empty /* subfolder */, "~/");
            var route = new RootRoute(true);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/AggDefault.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
            Assert.IsFalse(routeData.DataTokens.ContainsKey(PageRoute.ControlNamesKey));
        }

        [Test]
        public void GetRouteDataWithRequestForAppRoot_WhenAggregationDisabled_MatchesAndReturnsDtp()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/", string.Empty /* subfolder */, "~/");
            var route = new RootRoute(false);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/Dtp.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
            Assert.IsTrue(routeData.DataTokens.ContainsKey(PageRoute.ControlNamesKey));
        }

        [Test]
        public void GetRouteDataWithRequestForSubfolder_WhenAggregationEnabled_MatchesRequestAndReturnsDtp() {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder", "subfolder" /* subfolder */, "~/");
            var route = new RootRoute(true);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/Dtp.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
        }

        [Test]
        public void GetRouteDataWithRequestForSubfolder_WhenAggregationDisabled_MatchesRequestAndReturnsDtp()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder", "subfolder" /* subfolder */, "~/");
            var route = new RootRoute(false);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/Dtp.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
        }

        [Test]
        public void GetRouteDataWithRequestWithSubfolder_WhenAggregationEnabledAndBlogDoesNotHaveSubfolder_DoesNotMatch()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/foo", string.Empty /* subfolder */, "~/");
            var route = new RootRoute(true);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNull(routeData);
        }

        [Test]
        public void GetRouteDataWithRequestWithSubfolder_WhenAggregationDisabledAndBlogDoesNotHaveSubfolder_DoesNotMatch()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/foo", string.Empty /* subfolder */, "~/");
            var route = new RootRoute(false);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNull(routeData);
        }

        [Test]
        public void GetRouteDataWithRequestWithSubfolder_WhenAggregationEnabledAndSubfolderDoesNotMatchBlogSubfolder_DoesNotMatch()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/foo", "bar" /* subfolder */, "~/");
            var route = new RootRoute(true);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNull(routeData);
        }

        [Test]
        public void GetRouteDataWithRequestWithSubfolder_WhenAggregationDisabledAndSubfolderDoesNotMatchBlogSubfolder_DoesNotMatch()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/foo", "bar" /* subfolder */, "~/");
            var route = new RootRoute(false);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNull(routeData);
        }

        [Test]
        public void GetRouteDataWithRequestForDefault_WhenAggregationEnabled_MatchesAndReturnsAggDefault()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/Default.aspx", string.Empty /* subfolder */, "~/");
            var route = new RootRoute(true);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/AggDefault.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
        }

        [Test]
        public void GetRouteDataWithRequestForDefault_WhenAggregationDisabled_MatchesAndReturnsDtp()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/Default.aspx", string.Empty /* subfolder */, "~/");
            var route = new RootRoute(false);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/Dtp.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
        }

        [Test]
        public void GetRouteDataWithRequestForDefaultInSubfolder_WhenAggregationEnabled_MatchesRequestAndReturnsDtp()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/default.aspx", "subfolder" /* subfolder */, "~/");
            var route = new RootRoute(true);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/Dtp.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
        }

        [Test]
        public void GetRouteDataWithRequestForDefaultInSubfolder_WhenAggregationDisabled_MatchesRequestAndReturnsDtp()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/default.aspx", "subfolder" /* subfolder */, "~/");
            var route = new RootRoute(false);

            //act
            var routeData = route.GetRouteData(httpContext.Object);

            //assert
            var routeHandler = routeData.RouteHandler as PageRouteHandler;
            Assert.AreEqual("~/Dtp.aspx", routeHandler.VirtualPath);
            Assert.AreSame(route, routeData.Route);
        }

        [Test]
        public void GetVirtualPath_WhenAggregationEnabledAndNoSubfolderInRouteData_ReturnsRoot()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/default.aspx", string.Empty /* subfolder */, "~/");
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var route = new RootRoute(true);
            var routeValues = new RouteValueDictionary();

            //act
            var virtualPathInfo = route.GetVirtualPath(requestContext, routeValues);

            //assert
            Assert.AreEqual(string.Empty, virtualPathInfo.VirtualPath);
        }

        [Test]
        public void GetVirtualPath_WhenAggregationEnabledWithSubfolderInRouteData_ReturnsSubfolder()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/default.aspx", "subfolder" /* subfolder */, "~/");
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var route = new RootRoute(true);
            var routeValues = new RouteValueDictionary();

            //act
            var virtualPathInfo = route.GetVirtualPath(requestContext, routeValues);

            //assert
            Assert.AreEqual("subfolder", virtualPathInfo.VirtualPath);
        }

        [Test]
        public void GetVirtualPath_WhenAggregationEnabledWithSubfolderInRouteValues_ReturnsSubfolder()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/default.aspx", "subfolder" /* subfolder */, "~/");
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var route = new RootRoute(true);
            var routeValues = new RouteValueDictionary(new { subfolder = "subfolder"});

            //act
            var virtualPathInfo = route.GetVirtualPath(requestContext, routeValues);

            //assert
            Assert.AreEqual("subfolder", virtualPathInfo.VirtualPath);
        }

        [Test]
        public void GetVirtualPath_WhenSupplyingRouteValues_ReturnsNull()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/default.aspx", string.Empty /* subfolder */, "~/");
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var route = new RootRoute(true);
            var routeValues = new RouteValueDictionary(new { foo = "bar" });

            //act
            var virtualPathInfo = route.GetVirtualPath(requestContext, routeValues);

            //assert
            Assert.IsNull(virtualPathInfo);
        }
    }
}
