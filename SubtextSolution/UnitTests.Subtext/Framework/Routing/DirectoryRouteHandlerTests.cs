using System;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class DirectoryRouteHandlerTests
    {
        [Test]
        public void RequestContext_WithNonDirectoryRoute_CausesInvalidOperationException() { 
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            var routeData = new RouteData();
            routeData.Route = new Route("url", new DirectoryRouteHandler());
            var requestContext = new RequestContext(httpContext.Object, routeData);
            IRouteHandler routeHandler = new DirectoryRouteHandler();
            
            //act
            try
            {
                routeHandler.GetHttpHandler(requestContext);
            }
            catch (InvalidOperationException) {
                return;
            }

            //assert
            Assert.Fail();
        }

        [Test]
        public void RequestWithoutSubfolder_ForDirectory_GetsHandlerInPhysicalDirectory()
        {
            //arrange
            string virtualPath = string.Empty;
            RouteData routeData = new RouteData();
            routeData.Route = new DirectoryRoute("admin");
            routeData.Values.Add("pathinfo", "foo.aspx");
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            var httpHandler = new Mock<IHttpHandler>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(httpHandler.Object).Callback<string, Type>((vpath, type) => virtualPath = vpath);
            IRouteHandler routeHandler = new DirectoryRouteHandler(pageBuilder.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/foo.aspx");
            RequestContext requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var handler = routeHandler.GetHttpHandler(requestContext);

            //assert
            Assert.AreEqual("~/admin/foo.aspx", virtualPath);
        }

        [Test]
        public void DirectoryRouteHandler_InheritsRouteHandlerBase()
        {
            Assert.IsTrue(typeof(RouteHandlerBase).IsAssignableFrom(typeof(DirectoryRouteHandler)));
        }

        [Test]
        public void RequestWithoutSubfolder_ForAshxFileInDirectory_GetsHandlerInPhysicalDirectory()
        {
            //arrange
            string virtualPath = string.Empty;
            RouteData routeData = new RouteData();
            routeData.Route = new DirectoryRoute("admin");
            routeData.Values.Add("pathinfo", "foo.ashx");
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            var httpHandler = new Mock<IHttpHandler>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(httpHandler.Object).Callback<string, Type>((vpath, type) => virtualPath = vpath);
            IRouteHandler routeHandler = new DirectoryRouteHandler(pageBuilder.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/foo.ashx");
            RequestContext requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var handler = routeHandler.GetHttpHandler(requestContext);

            //assert
            Assert.AreEqual("~/admin/foo.ashx", virtualPath);
        }

        //TODO: Simplify this test.
        [Test]
        public void RequestWithoutSubfolder_ForDirectoryWithoutFile_AppendsDefaultFileToVirtualPath()
        {
            //arrange
            string virtualPath = string.Empty;
            RouteData routeData = new RouteData();
            routeData.Route = new DirectoryRoute("admin");
            routeData.Values.Add("pathinfo", "posts");
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            var httpHandler = new Mock<IHttpHandler>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(httpHandler.Object).Callback<string, Type>((vpath, type) => virtualPath = vpath);
            IRouteHandler routeHandler = new DirectoryRouteHandler(pageBuilder.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/posts/");
            RequestContext requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var handler = routeHandler.GetHttpHandler(requestContext);

            //assert
            Assert.AreEqual("~/admin/posts/Default.aspx", virtualPath);
        }

        [Test]
        public void RequestWithoutSubfolder_ForDirectoryWithoutFileAndWithouEndingSlash_AppendsDefaultFileToVirtualPath()
        {
            //arrange
            string virtualPath = string.Empty;
            RouteData routeData = new RouteData();
            routeData.Route = new DirectoryRoute("admin");
            routeData.Values.Add("pathinfo", "posts");
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            var httpHandler = new Mock<IHttpHandler>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(httpHandler.Object).Callback<string, Type>((vpath, type) => virtualPath = vpath);
            IRouteHandler routeHandler = new DirectoryRouteHandler(pageBuilder.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/admin/posts");
            RequestContext requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var handler = routeHandler.GetHttpHandler(requestContext);

            //assert
            Assert.AreEqual("~/admin/posts/Default.aspx", virtualPath);
        }

        [Test]
        public void RequestWithSubfolder_ForDirectory_GetsHandlerInPhysicalDirectory() { 
            //arrange
            string virtualPath = string.Empty;
            RouteData routeData = new RouteData();
            routeData.Route = new DirectoryRoute("admin");
            routeData.Values.Add("subfolder", "blogsubfolder");
            routeData.Values.Add("pathinfo", "foo.aspx");
            var pageBuilder = new Mock<ISubtextPageBuilder>();
            var httpHandler = new Mock<IHttpHandler>();
            pageBuilder.Expect(b => b.CreateInstanceFromVirtualPath(It.IsAny<string>(), It.IsAny<Type>())).Returns(httpHandler.Object).Callback<string, Type>((vpath, type) => virtualPath = vpath);
            IRouteHandler routeHandler = new DirectoryRouteHandler(pageBuilder.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/blogsubfolder/admin/foo.aspx", "blogsubfolder");
            RequestContext requestContext = new RequestContext(httpContext.Object, routeData);

            //act
            var handler = routeHandler.GetHttpHandler(requestContext);

            //assert
            Assert.AreEqual("~/admin/foo.aspx", virtualPath);
        }
    }
}
