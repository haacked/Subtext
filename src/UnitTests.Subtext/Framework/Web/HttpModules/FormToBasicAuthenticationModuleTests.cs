using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Subtext.Framework.Web.HttpModules
{
    [TestClass]
    public class FormToBasicAuthenticationModuleTests
    {
        [TestMethod]
        public void HandleEndRequest_WithNullUser_DoesNotThrowException()
        {
            // Arrange
            var module = new FormToBasicAuthenticationModule();
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request.Url).Returns(new Uri("http://localhost/"));
            context.Setup(c => c.Response.StatusCode).Returns(200);

            // Act, Assert
            module.HandleEndRequest(context.Object);
        }
    }
}
