using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class RouteExtensionsTests
    {
        [TestMethod]
        public void Ignore_AddsIgnoreRoute_ToRouteCollection()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);

            //act
            subtextRoutes.Ignore("url");

            //assert
            Assert.AreEqual(typeof(IgnoreRoute), routes[0].GetType());
        }

        [TestMethod]
        public void MapControls_WithConstraints_AddsPageRouteWithConstraintsToCollection()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);

            //act
            subtextRoutes.MapControls("url", new { constraint = "constraintvalue" }, new[] { "controls" });

            //assert
            Assert.AreEqual("constraintvalue", ((PageRoute)routes[0]).Constraints["constraint"]);
        }

        [TestMethod]
        public void MapControls_WithoutConstraints_AddsPageRouteWithConstraintsToCollection()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);

            //act
            subtextRoutes.MapControls("url", new[] { "controls" });

            //assert
            Assert.AreEqual("url", ((PageRoute)routes[0]).Url);
        }

        [TestMethod]
        public void MapSystemDirectory_SetsDirectoryRouteHandlerAndAddsPathInfoToRouteUrl()
        {
            //arrange
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);

            //act
            subtextRoutes.MapSystemDirectory("install");

            //assert
            var route = routes[0] as Route;
            Assert.AreEqual("install/{*pathInfo}", route.Url);
            Assert.AreEqual(typeof(DirectoryRouteHandler), route.RouteHandler.GetType());
        }
    }
}