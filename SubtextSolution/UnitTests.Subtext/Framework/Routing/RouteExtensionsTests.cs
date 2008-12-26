using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.Web.Routing;
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

            //act
            routes.Ignore("url");

            //assert
            Assert.AreEqual(typeof(IgnoreRoute), routes[0].GetType());
        }

        [Test]
        public void MapControls_WithConstraints_AddsPageRouteWithConstraintsToCollection() { 
            //arrange
            var routes = new RouteCollection();

            //act
            routes.MapControls("url", new { constraint = "constraintvalue" }, new string[] { "controls" });

            //assert
            Assert.AreEqual("constraintvalue", ((PageRoute)routes[0]).Constraints["constraint"]);
        }

        [Test]
        public void MapControls_WithoutConstraints_AddsPageRouteWithConstraintsToCollection()
        {
            //arrange
            var routes = new RouteCollection();

            //act
            routes.MapControls("url", new string[] { "controls" });

            //assert
            Assert.AreEqual("url", ((PageRoute)routes[0]).Url);
        }
    }
}
