using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Ninject;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class RoutesTests
    {
        [Test]
        public void RequestWithSubfolderForBlogRoot_WithAggregateEnabled_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/foo.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);
            var routeHandler = routeData.RouteHandler as DirectoryRouteHandler;

            //assert.
            Assert.IsNotNull(routeHandler);
            Assert.AreEqual("foo.aspx", routeData.Values["pathInfo"]);
        }

        [Test]
        public void RequestWithSubfolderForBlogRoot_WithBlogHavingDifferentSubfolder_DoesNotMatch()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/foo.aspx", "not-subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData);
        }

        [Test]
        public void RequestWithSubfolder_ForAdminDirectory_UsesDirectoryRouteHandler()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/foo.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);
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
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/providers/foo.aspx");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);
            var routeHandler = routeData.RouteHandler as DirectoryRouteHandler;

            //assert.
            Assert.IsNotNull(routeHandler);
            Assert.AreEqual("foo.aspx", routeData.Values["pathInfo"]);
        }

        [Test]
        public void Request_ForBlogPost_ContainsControlsForBlogPost()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/2008/12/10/blog-post.aspx");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);
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
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/2008/1/10/blog-post.aspx");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData);
        }

        [Test]
        public void RequestWithSubfolders_ForBlogPost_ContainsControlsForBlogPost()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/archive/2008/12/10/blog-post.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);
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
        public void Request_ForDirectHttpHandlers_Matches(string url, string subfolder)
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest(url, subfolder);

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNotNull(routeData);
        }

        [Test]
        public void RequestWithSubfolderForCommentApiController_WithBlogHavingSubfolder_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/comments/123.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNotNull(routeData);
            Assert.AreEqual("CommentApi", routeData.Values["controller"]);
            Assert.AreEqual("Create", routeData.Values["action"]);
            Assert.AreEqual(routeData.RouteHandler.GetType(), typeof(MvcRouteHandler));
        }

        [Test]
        public void RequestWithSubfolderForAggregatorBug_WithBlogHavingSubfolder_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/aggbug/123.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNotNull(routeData);
            Assert.AreEqual("Statistics", routeData.Values["controller"]);
            Assert.AreEqual("RecordAggregatorView", routeData.Values["action"]);
            Assert.AreEqual(routeData.RouteHandler.GetType(), typeof(MvcRouteHandler));
        }

        [Test]
        public void RequestWithSubfolderForInstallDirectory_DoesNotMatch()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/install/default.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData);
        }

        [Test]
        public void RequestWithoutSubfolderForInstallDirectory_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/install/default.aspx", "");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNotNull(routeData);
        }

        [Test]
        public void GetRouteData_ForRequestForExportController_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IKernel>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/export.ashx", "");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNotNull(routeData);
            Assert.AreEqual("export", routeData.Values["controller"]);
            Assert.AreEqual("blogml", routeData.Values["action"]);
        }
    }
}