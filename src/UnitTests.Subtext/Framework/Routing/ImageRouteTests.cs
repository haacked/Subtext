using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class ImageRouteTests
    {
        [Test]
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