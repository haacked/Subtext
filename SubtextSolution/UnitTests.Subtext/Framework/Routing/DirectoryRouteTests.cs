using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;
using System.Web.Routing;
using System;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class DirectoryRouteTests
    {
        [Test]
        public void GetVirtualPath_WithoutSubolder_ReturnsUrlWithoutSubfolder()
        {
            //arrange
            var route = new DirectoryRoute("admin");
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/posts/foo.aspx");
            var blogRequest = new BlogRequest("localhost", null, new Uri("http://localhost"), true);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var virtualPath = route.GetVirtualPath(requestContext
                , new RouteValueDictionary(new { pathInfo = "posts/foo.aspx" }));

            //assert
            Assert.IsNotNull(virtualPath);
            Assert.AreEqual("admin/posts/foo.aspx", virtualPath.VirtualPath);
        }

        [Test]
        public void GetVirtualPath_WithSubolder_ReturnsUrlWithSubfolder()
        {
            //arrange
            var route = new DirectoryRoute("admin");
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/admin/");
            var blogRequest = new BlogRequest("localhost", "subfolder", new Uri("http://localhost"), false);
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var virtualPath = route.GetVirtualPath(requestContext
                , new RouteValueDictionary(new { pathInfo = "posts/foo.aspx" }));

            //assert
            Assert.IsNotNull(virtualPath);
            Assert.AreEqual("subfolder/admin/posts/foo.aspx", virtualPath.VirtualPath);
        }

        [Test]
        public void Ctor_WithDirectoryNameArg_AppendsPathInfoCatchAll()
        { 
            //arrange, act
            DirectoryRoute route = new DirectoryRoute("dir");

            //assert
            Assert.AreEqual("dir/{*pathInfo}", route.Url);
        }

        [Test]
        public void Ctor_WithDirectoryNameArg_SetsDirectoryName()
        {
            //arrange, act
            DirectoryRoute route = new DirectoryRoute("dir");

            //assert
            Assert.AreEqual("dir", route.DirectoryName);
        }

        [Test]
        public void GetRouteData_MatchingTheImplicitSubfolderRoute_ReturnsParentDirectoryRoute() { 
            //arrange
            var route = new DirectoryRoute("dir");
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/subfolder/dir/foo.aspx");
            var blogRequest = new BlogRequest("localhost", "subfolder", new Uri("http://localhost"), false);
            
            //act
            var routeData = route.GetRouteData(httpContext.Object, blogRequest);

            //assert
            Assert.AreEqual(route, routeData.Route);
        }
    }
}
