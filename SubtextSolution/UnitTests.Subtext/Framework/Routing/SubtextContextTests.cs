using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class SubtextContextTests
    {
        [Test]
        public void Ctor_WithAllNonNullArgs_SetsProperties() { 
            //arrange
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            var urlHelper = new UrlHelper(requestContext, new RouteCollection());
            var objectRepository = new Mock<ObjectProvider>().Object;
            var blog = new Blog();

            //act
            var subtextContext = new SubtextContext(blog, requestContext, urlHelper, objectRepository);

            //assert
            Assert.AreEqual(blog, subtextContext.Blog);
            Assert.AreEqual(urlHelper, subtextContext.UrlHelper);
            Assert.AreEqual(requestContext, subtextContext.RequestContext);
            Assert.AreEqual(objectRepository, subtextContext.Repository);
        }
    }
}
