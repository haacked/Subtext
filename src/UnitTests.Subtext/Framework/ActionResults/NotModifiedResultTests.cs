using System.Web;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Moq.Stub;
using Subtext.Infrastructure.ActionResults;

namespace UnitTests.Subtext.Framework.ActionResults
{
    [TestFixture]
    public class NotModifiedResultTests
    {
        [Test]
        public void NotModifiedResultSends304StatusCodeAndSuppressesContent()
        {
            // arrange
            var result = new NotModifiedResult();
            var httpContext = new Mock<HttpContextBase>();

            httpContext.Stub(h => h.Response.StatusCode);
            httpContext.Stub(h => h.Response.SuppressContent);
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext.Object;

            // act
            result.ExecuteResult(controllerContext);

            // assert
            Assert.AreEqual(304, httpContext.Object.Response.StatusCode);
            Assert.IsTrue(httpContext.Object.Response.SuppressContent);
        }
    }
}