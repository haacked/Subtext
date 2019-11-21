using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class PageRouteTests
    {
        [TestMethod]
        public void GetVirtualPath_WithoutSubolder_ReturnsUrlWithoutSubfolder()
        {
            //arrange
            var route = new PageRoute("archive/{slug}.aspx", "~/aspx/Dtp.aspx", null,
                                      new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/slug.aspx");
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            VirtualPathData virtualPath = route.GetVirtualPath(requestContext,
                                                               new RouteValueDictionary(new { slug = "test-slug" }));

            //assert
            Assert.IsNotNull(virtualPath);
            Assert.AreEqual("archive/test-slug.aspx", virtualPath.VirtualPath);
        }

        [TestMethod]
        public void GetVirtualPath_WithSubolder_ReturnsUrlWithSubfolder()
        {
            //arrange
            var route = new PageRoute("archive/{slug}.aspx", "~/aspx/Dtp.aspx", null,
                                      new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/slug.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            VirtualPathData virtualPath = route.GetVirtualPath(requestContext,
                                                               new RouteValueDictionary(new { slug = "test-slug" }));

            //assert
            Assert.IsNotNull(virtualPath);
            Assert.AreEqual("subfolder/archive/test-slug.aspx", virtualPath.VirtualPath);
        }

        [TestMethod]
        public void Request_ForPageRouteWithConstraints_MatchesWhenConstraintsAreSatisfied()
        {
            //arrange
            var route = new PageRoute("archive/{year}/{month}/{day}/{slug}.aspx", "~/aspx/Dtp.aspx", null,
                                      new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object) { Constraints = new RouteValueDictionary(new { year = @"\d{4}" }) };
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/2008/01/23/slug.aspx");
            var blogRequest = new BlogRequest("localhost", null, new Uri("http://localhost"), false);

            //act
            RouteData routeData = route.GetRouteData(httpContext.Object, blogRequest);

            //assert
            Assert.IsNotNull(routeData);
        }

        [TestMethod]
        public void RequestWithSubfolder_ForBlogPostWithSubfolder_Matches()
        {
            //arrange
            var subtextRoute = new PageRoute("archive/{slug}", "~/aspx/Dtp.aspx", null,
                                             new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/blogsubfolder/archive/blog-post");
            //This info is provided by the BlogRequestModule.
            var blogRequest = new BlogRequest("localhost", "blogsubfolder",
                                              new Uri("http://localhost/blogsubfolder/archive/blog-post"), true);

            //act
            RouteData routeData = subtextRoute.GetRouteData(httpContext.Object, blogRequest);

            //assert.
            Assert.IsNotNull(routeData);
            Assert.AreEqual("blog-post", routeData.Values["slug"]);
            Assert.AreEqual("blogsubfolder", routeData.Values["subfolder"]);
        }

        [TestMethod]
        public void RequestWithoutSubfolder_ForBlogPostWithSubfolder_DoesNotMatch()
        {
            //arrange
            var subtextRoute = new PageRoute("archive/{slug}", "~/aspx/Dtp.aspx", null,
                                             new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/blog-post");
            //This info is provided by the BlogRequestModule.
            var blogRequest = new BlogRequest("localhost", "subfolder", new Uri("http://localhost/archive/blog-post"),
                                              true);

            //act
            RouteData routeData = subtextRoute.GetRouteData(httpContext.Object, blogRequest);

            //assert.
            Assert.IsNull(routeData);
        }

        [TestMethod]
        public void RequestWithoutSubfolder_ForBlogPostWithoutSubfolder_Matches()
        {
            //arrange
            var subtextRoute = new PageRoute("archive/{slug}", "~/aspx/Dtp.aspx", null,
                                             new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/blog-post");
            //This info is provided by the BlogRequestModule.
            var blogRequest = new BlogRequest("localhost", string.Empty, new Uri("http://localhost/archive/blog-post"),
                                              true);

            //act
            RouteData routeData = subtextRoute.GetRouteData(httpContext.Object, blogRequest);

            //assert.
            Assert.IsNotNull(routeData);
            Assert.AreEqual("blog-post", routeData.Values["slug"]);
            Assert.AreEqual(string.Empty, routeData.Values["subfolder"]);
        }

        [TestMethod]
        public void RequestWithSubfolder_ForBlogPostWithoutSubfolder_DoesNotMatch()
        {
            //arrange
            var subtextRoute = new PageRoute("archive/{slug}", "~/aspx/Dtp.aspx", null,
                                             new Mock<ISubtextPageBuilder>().Object, new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/archive/blog-post");
            //This info is provided by the BlogRequestModule.
            var blogRequest = new BlogRequest("localhost", string.Empty,
                                              new Uri("http://localhost/subfolder/archive/blog-post"), true);

            //act
            RouteData routeData = subtextRoute.GetRouteData(httpContext.Object, blogRequest);

            //assert.
            Assert.IsNull(routeData);
        }

        [TestMethod]
        public void GetRouteData_MatchingTheImplicitSubfolderRoute_ReturnsParentDirectoryRoute()
        {
            //arrange
            var route = new PageRoute("url", "~/aspx/Dtp.aspx", new[] { "foo" }, new Mock<ISubtextPageBuilder>().Object,
                                      new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/url");
            var blogRequest = new BlogRequest("localhost", "subfolder", new Uri("http://localhost"), false);

            //act
            RouteData routeData = route.GetRouteData(httpContext.Object, blogRequest);

            //assert
            Assert.AreEqual(route, routeData.Route);
        }
    }
}