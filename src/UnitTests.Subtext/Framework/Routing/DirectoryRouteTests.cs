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
    public class DirectoryRouteTests
    {
        [TestMethod]
        public void GetVirtualPath_WithoutSubolder_ReturnsUrlWithoutSubfolder()
        {
            //arrange
            var route = new DirectoryRoute("admin", new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/posts/foo.aspx");
            var blogRequest = new BlogRequest("localhost", null, new Uri("http://localhost"), true);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            VirtualPathData virtualPath = route.GetVirtualPath(requestContext
                                                               ,
                                                               new RouteValueDictionary(
                                                                   new { pathInfo = "posts/foo.aspx" }));

            //assert
            Assert.IsNotNull(virtualPath);
            Assert.AreEqual("admin/posts/foo.aspx", virtualPath.VirtualPath);
        }

        [TestMethod]
        public void GetVirtualPath_WithSubolder_ReturnsUrlWithSubfolder()
        {
            //arrange
            var route = new DirectoryRoute("admin", new Mock<IDependencyResolver>().Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/");
            var blogRequest = new BlogRequest("localhost", "subfolder", new Uri("http://localhost"), false);
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            VirtualPathData virtualPath = route.GetVirtualPath(requestContext
                                                               ,
                                                               new RouteValueDictionary(
                                                                   new { pathInfo = "posts/foo.aspx" }));

            //assert
            Assert.IsNotNull(virtualPath);
            Assert.AreEqual("subfolder/admin/posts/foo.aspx", virtualPath.VirtualPath);
        }

        [TestMethod]
        public void Ctor_WithDirectoryNameArg_AppendsPathInfoCatchAll()
        {
            //arrange, act
            var route = new DirectoryRoute("dir", new Mock<IDependencyResolver>().Object);
            ;

            //assert
            Assert.AreEqual("dir/{*pathInfo}", route.Url);
        }

        [TestMethod]
        public void Ctor_WithDirectoryNameArg_SetsDirectoryName()
        {
            //arrange, act
            var route = new DirectoryRoute("dir", new Mock<IDependencyResolver>().Object);
            ;

            //assert
            Assert.AreEqual("dir", route.DirectoryName);
        }

        [TestMethod]
        public void GetRouteData_MatchingTheImplicitSubfolderRoute_ReturnsParentDirectoryRoute()
        {
            //arrange
            var route = new DirectoryRoute("dir", new Mock<IDependencyResolver>().Object);
            ;
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/dir/foo.aspx");
            var blogRequest = new BlogRequest("localhost", "subfolder", new Uri("http://localhost"), false);

            //act
            RouteData routeData = route.GetRouteData(httpContext.Object, blogRequest);

            //assert
            Assert.AreEqual(route, routeData.Route);
        }
    }
}