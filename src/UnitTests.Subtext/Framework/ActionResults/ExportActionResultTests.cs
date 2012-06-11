using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.ImportExport;
using Subtext.Infrastructure.ActionResults;

namespace UnitTests.Subtext.Framework.ActionResults
{
    [TestFixture]
    public class ExportActionResultTests
    {
        [Test]
        public void Ctor_WithFileName_SetsFileDownloadName()
        {
            // arrange, act
            var result = new ExportActionResult(new Mock<IBlogMLWriter>().Object, "test");

            // assert
            Assert.AreEqual("test", result.FileDownloadName);
        }

        [Test]
        public void Ctor_SetsContentType_ToXml()
        {
            // arrange, act
            var result = new ExportActionResult(new Mock<IBlogMLWriter>().Object, "test");

            // assert
            Assert.AreEqual("text/xml", result.ContentType);
        }

        [Test]
        public void ExecuteResult_WritesToBlogMLWriter()
        {
            // arrange
            var stringWriter = new StringWriter();
            var writer = new Mock<IBlogMLWriter>();
            bool blogWritten = false;
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Response.Output).Returns(stringWriter);
            var controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            writer.Setup(w => w.Write(It.IsAny<XmlTextWriter>())).Callback(() => blogWritten = true);
            var result = new ExportActionResult(writer.Object, "test");

            // act
            result.ExecuteResult(controllerContext);

            // assert
            Assert.IsTrue(blogWritten);
        }
    }
}