using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.ModelBinders;

namespace UnitTests.Subtext.Framework.ModelBinders
{
    [TestFixture]
    public class XmlModelBinderTests
    {
        [Test]
        public void BindModel_WithXmlInInputStream_ReturnsXmlDoc()
        {
            // arrange
            var binder = new XmlModelBinder();
            var controllerContext = new ControllerContext();
            var httpContext = new Mock<HttpContextBase>();
            Stream stream =
                @"<?xml version=""1.0""?>
                            <root><node>test</node></root>".ToStream();
            httpContext.Setup(h => h.Request.InputStream).Returns(stream);
            httpContext.Setup(h => h.Request.ContentType).Returns("text/xml");
            controllerContext.HttpContext = httpContext.Object;
            var bindingContext = new ModelBindingContext();

            // act
            var doc = binder.BindModel(controllerContext, bindingContext) as XmlDocument;

            // assert
            Assert.IsNotNull(doc);
            Assert.AreEqual("test", doc.SelectSingleNode("//node").InnerText);
        }

        [Test]
        public void BindModel_WithNonTextXmlContentType_ThrowsException()
        {
            // arrange
            var binder = new XmlModelBinder();
            var controllerContext = new ControllerContext();
            var httpContext = new Mock<HttpContextBase>();
            Stream stream = @"<?xml version=""1.0""?>
                            <root><node /></root>".ToStream();
            httpContext.Setup(h => h.Request.InputStream).Returns(stream);
            httpContext.Setup(h => h.Request.ContentType).Returns("text/html");
            controllerContext.HttpContext = httpContext.Object;
            var bindingContext = new ModelBindingContext();

            // act, assert
            UnitTestHelper.AssertThrows<InvalidOperationException>(() =>
                                                                   binder.BindModel(controllerContext, bindingContext));
        }
    }
}