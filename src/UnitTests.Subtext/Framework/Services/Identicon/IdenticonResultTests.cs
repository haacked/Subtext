using System;
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

        [Test]
        public void ExecuteResult_WithNullEtag_DoesNotAddEtagToHeader()
        {
            // arrange
            var result = new IdenticonResult(123, 40, "");
            var context = new Mock<ControllerContext>();
            context.Setup(c => c.HttpContext.Response.AppendHeader("ETag", It.IsAny<string>())).Throws(new InvalidOperationException());
            context.Setup(c => c.HttpContext.Response.OutputStream).Returns(new MemoryStream());

            // act, assert
            result.ExecuteResult(context.Object);
        }

        [Test]
        public void ExecuteResult_SetsProperContentType()
        {
            // arrange
            var result = new IdenticonResult(123, 40, "");
            var context = new Mock<ControllerContext>();
            context.SetupSet(c => c.HttpContext.Response.ContentType, "image/png");
            context.Setup(c => c.HttpContext.Response.OutputStream).Returns(new MemoryStream());

            // act
            result.ExecuteResult(context.Object);

            // assert
            context.VerifySet(c => c.HttpContext.Response.ContentType, "image/png");
        }
        
        [Test]
        public void ExecuteResult_ClearsResponse()
        {
            // arrange
            var result = new IdenticonResult(123, 40, null);
            var context = new Mock<ControllerContext>();
            bool responseCleared = false;
            context.Setup(c => c.HttpContext.Response.Clear()).Callback(() => responseCleared = true);
            context.Setup(c => c.HttpContext.Response.OutputStream).Returns(new MemoryStream());

            // act
            result.ExecuteResult(context.Object);

            // assert
            Assert.IsTrue(responseCleared);
        }
    }
}
