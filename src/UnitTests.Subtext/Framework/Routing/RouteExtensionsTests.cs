using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class RouteExtensionsTests
    {
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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