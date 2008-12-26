using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Web;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework;
using Subtext.Framework.Routing;
using UnitTests.Subtext;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class RoutesTests
    {
        [Test]
        public void RequestWithSubfolder_ForAdminDirectory_UsesDirectoryRouteHandler() {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/foo.aspx", "subfolder");

            //act
            var routeData = routes.GetRouteData(httpContext.Object);
            var routeHandler = routeData.RouteHandler as DirectoryRouteHandler;

            //assert.
            Assert.IsNotNull(routeHandler);
            Assert.AreEqual("foo.aspx", routeData.Values["pathInfo"]);
        }

        [Test]
        public void RequestWithoutSubfolder_ForProvidersDirectory_UsesDirectoryRouteHandler()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/providers/foo.aspx");

            //act
            var routeData = routes.GetRouteData(httpContext.Object);
            var routeHandler = routeData.RouteHandler as DirectoryRouteHandler;

            //assert.
            Assert.IsNotNull(routeHandler);
            Assert.AreEqual("foo.aspx", routeData.Values["pathInfo"]);
        }

        [Test]
        public void Request_ForBlogPost_ContainsControlsForBlogPost() {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/2008/12/10/blog-post.aspx");

            //act
            var routeData = routes.GetRouteData(httpContext.Object);
            var controls = routeData.DataTokens["controls"] as IEnumerable<string>;
            //assert.

            Assert.IsTrue(controls.Contains("viewpost"));
            Assert.IsTrue(controls.Contains("comments"));
            Assert.IsTrue(controls.Contains("postcomment"));
            Assert.AreEqual("blog-post", routeData.Values["slug"]);
        }

        [Test]
        public void RequestUrlWithSingleDigitMonth_ForBlogPost_DoesNotMatchPageRoute()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/2008/1/10/blog-post.aspx");

            //act
            var routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData.Route as PageRoute);
        }

        [Test]
        public void RequestWithSubfolders_ForBlogPost_ContainsControlsForBlogPost()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/archive/2008/12/10/blog-post.aspx", "subfolder");

            //act
            var routeData = routes.GetRouteData(httpContext.Object);
            var controls = routeData.DataTokens["controls"] as IEnumerable<string>;
            //assert.

            Assert.IsTrue(controls.Contains("viewpost"));
            Assert.IsTrue(controls.Contains("comments"));
            Assert.IsTrue(controls.Contains("postcomment"));
            Assert.AreEqual("blog-post", routeData.Values["slug"]);
        }

        [RowTest]
        [Row("~/subfolder/sitemap.ashx", "subfolder")]
        [Row("~/sitemap.ashx", null)]
        [Row("~/subfolder/BrowserServices.ashx", "subfolder")]
        [Row("~/BrowserServices.ashx", null)]
        [Row("~/subfolder/admin/handlers/BlogMLExport.ashx", "subfolder")]
        [Row("~/admin/handlers/BlogMLExport.ashx", null)]
        [Row("~/subfolder/admin/FooRss.axd", "subfolder")]
        [Row("~/admin/FooRss.axd", null)]
        public void Request_ForDirectHttpHandlers_Matches(string url, string subfolder) {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest(url, subfolder);

            //act
            var routeData = routes.GetRouteData(httpContext.Object);
            
            //assert
            Assert.IsNotNull(routeData);
        }
    }
}
