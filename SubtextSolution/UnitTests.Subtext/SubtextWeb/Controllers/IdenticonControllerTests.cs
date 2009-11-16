#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Specialized;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Services.Identicon;
using Subtext.Identicon;
using Subtext.Web.Controllers;
using Subtext.Infrastructure.ActionResults;

namespace UnitTests.Subtext.SubtextWeb.Controllers
{
    [TestFixture]
    public class IdenticonControllerTests
    {
        [Test]
        public void Image_WithCode_ReturnsResultWithCorrectCodeAndContentTypeAndDefaultSize()
        {
            // arrange
            var controller = new IdenticonController();

            // act
            var result = controller.Image(123) as IdenticonResult;

            // assert
            Assert.AreEqual(123, result.Code);
            Assert.AreEqual(40, result.Size);
            Assert.AreEqual("image/png", result.ContentType);
        }

        [Test]
        public void Image_WithSize_ReturnsResultWithSpecifiedSize()
        {
            // arrange
            var controller = new IdenticonController(new NameValueCollection{{"IdenticonSize", "80"}});

            // act
            var result = controller.Image(0) as IdenticonResult;

            // assert
            Assert.AreEqual(80, result.Size);
        }


        [Test]
        public void Image_WithoutCode_ReturnsCodeBasedOnIpAddress()
        {
            // arrange
            var controller = new IdenticonController();
            var controllerContext = new Mock<ControllerContext>();
            controller.ControllerContext = controllerContext.Object;
            controllerContext.Setup(c => c.HttpContext.Request.UserHostAddress).Returns("123.234.245.255");

            // act
            var result = controller.Image(null) as IdenticonResult;

            // assert
            Assert.AreEqual(-315619697, result.Code);
        }

        [Test]
        public void Image_WithRequestMatchingEtag_ReturnsNotModifiedResult()
        {
            // arrange
            string etag = IdenticonUtil.ETag(-1234, 40);
            var controller = new IdenticonController();
            var controllerContext = new Mock<ControllerContext>();
            controller.ControllerContext = controllerContext.Object;
            controllerContext.Setup(c => c.HttpContext.Request.Headers).Returns(new NameValueCollection {{"If-None-Match", etag}});

            // act
            var result = controller.Image(-1234) as NotModifiedResult;

            // assert
            Assert.IsNotNull(result);
        }

        public IdenticonControllerTests()
        {
            IdenticonUtil.Salt = "and pepper";
        }
    }
}
