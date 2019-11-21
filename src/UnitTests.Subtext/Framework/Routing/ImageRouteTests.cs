using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class ImageRouteTests
    {
        [TestMethod]
        public void GetRouteDataWithAnyRequest_ReturnsNull()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/", string.Empty /* subfolder */, "~/");
            var route = new ImageRoute("{*anything}");

            //act
            RouteData routeData = route.GetRouteData(httpContext.Object);

            //assert
            Assert.IsNull(routeData);
        }
    }
}