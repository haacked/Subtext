using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class RoutesTests
    {
        [TestMethod]
        public void RequestWithSubfolderForBlogRoot_WithAggregateEnabled_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void RequestWithSubfolderForBlogRoot_WithBlogHavingDifferentSubfolder_DoesNotMatch()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/foo.aspx", "not-subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData);
        }

        [TestMethod]
        public void RequestWithSubfolder_ForAdminDirectory_UsesDirectoryRouteHandler()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void RequestWithoutSubfolder_ForProvidersDirectory_UsesDirectoryRouteHandler()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void Request_ForBlogPost_ContainsControlsForBlogPost()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void RequestUrlWithSingleDigitMonth_ForBlogPost_DoesNotMatchPageRoute()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/2008/1/10/blog-post.aspx");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData);
        }

        [TestMethod]
        public void RequestWithSubfolders_ForBlogPost_ContainsControlsForBlogPost()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [DataTestMethod]
        [DataRow("~/subfolder/sitemap.ashx", "subfolder")]
        [DataRow("~/sitemap.ashx", null)]
        [DataRow("~/subfolder/BrowserServices.ashx", "subfolder")]
        [DataRow("~/BrowserServices.ashx", null)]
        [DataRow("~/subfolder/admin/handlers/BlogMLExport.ashx", "subfolder")]
        [DataRow("~/admin/handlers/BlogMLExport.ashx", null)]
        [DataRow("~/subfolder/admin/FooRss.axd", "subfolder")]
        [DataRow("~/admin/FooRss.axd", null)]
        public void Request_ForDirectHttpHandlers_Matches(string url, string subfolder)
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest(url, subfolder);

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNotNull(routeData);
        }

        [TestMethod]
        public void RequestWithSubfolderForCommentApiController_WithBlogHavingSubfolder_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void RequestWithSubfolderForAggregatorBug_WithBlogHavingSubfolder_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void RequestWithSubfolderForInstallDirectory_DoesNotMatch()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/install/default.aspx", "subfolder");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNull(routeData);
        }

        [TestMethod]
        public void RequestWithoutSubfolderForInstallDirectory_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/install/default.aspx", "");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNotNull(routeData);
        }

        [TestMethod]
        public void GetRouteData_ForRequestForExportController_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
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

        [TestMethod]
        public void GetRouteData_ForRequestForEntryAdminController_Matches()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/comments/destroy.ashx", "");

            //act
            RouteData routeData = routes.GetRouteData(httpContext.Object);

            //assert.
            Assert.IsNotNull(routeData);
            Assert.AreEqual("comment", routeData.Values["controller"]);
            Assert.AreEqual("destroy", routeData.Values["action"]);
        }
    }
}