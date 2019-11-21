using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class SubtextContextTests
    {
        [TestMethod]
        public void Ctor_WithAllNonNullArgs_SetsProperties()
        {
            //arrange
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, new RouteData());
            var urlHelper = new BlogUrlHelper(requestContext, new RouteCollection());
            ObjectRepository objectRepository = new Mock<ObjectRepository>().Object;
            var blog = new Blog();

            //act
            var subtextContext = new SubtextContext(blog, requestContext, urlHelper, objectRepository, null, null, null);

            //assert
            Assert.AreEqual(blog, subtextContext.Blog);
            Assert.AreEqual(urlHelper, subtextContext.UrlHelper);
            Assert.AreEqual(requestContext, subtextContext.RequestContext);
            Assert.AreEqual(objectRepository, subtextContext.Repository);
        }
    }
}