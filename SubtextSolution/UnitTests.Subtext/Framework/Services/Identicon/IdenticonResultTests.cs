using System.IO;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Services.Identicon;

namespace UnitTests.Subtext.Framework.Services.Identicon
{
    [TestFixture]
    public class IdenticonResultTests
    {
        [Test]
        public void ExecuteResult_WithEtag_AddsEtagToHeader()
        {
            // arrange
            var result = new IdenticonResult(123, 40, "some-etag-value");
            var context = new Mock<ControllerContext>();
            string etag = null;
            context.Setup(c => c.HttpContext.Response.AppendHeader("ETag", It.IsAny<string>())).Callback<string, string>((key, value) => etag = value);
            context.Setup(c => c.HttpContext.Response.OutputStream).Returns(new MemoryStream());

            // act
            result.ExecuteResult(context.Object);

            // assert
            Assert.AreEqual("some-etag-value", etag);
        }
    }
}
