using System;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Providers.BlogEntryEditor.FCKeditor;

namespace UnitTests.Subtext.BlogEntryProvider
{
    [TestFixture]
    public class FileBrowserConnectorTests
    {
        [Test]
        public void FileBrowserConnector_WithNonAdmin_SetsUnauthorizedStatusCode()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();

            subtextContext.Setup(c => c.User.IsInRole("Admins")).Returns(false);
            subtextContext.SetupSet(c => c.HttpContext.Response.StatusCode, 401);
            subtextContext.Setup(c => c.HttpContext.Response.End());

            var page = new FileBrowserConnector();
            page.SubtextContext = subtextContext.Object;

            // act
            ReflectionHelper.InvokeNonPublicMethod(page, "OnInit", new object[] {EventArgs.Empty});

            //assert
            subtextContext.VerifySet(c => c.HttpContext.Response.StatusCode, 401);
        }

        [Test]
        public void FileBrowserConnector_WithAdmin_DoesNotSetUnauthorizedStatusCode()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();

            subtextContext.Setup(c => c.User.IsInRole("Admins")).Returns(true);
            subtextContext.SetupSet(c => c.HttpContext.Response.StatusCode, 401).Throws(new Exception("Failed!"));
            subtextContext.Setup(c => c.HttpContext.Response.End());

            var page = new FileBrowserConnector();
            page.SubtextContext = subtextContext.Object;

            // act, assert
            ReflectionHelper.InvokeNonPublicMethod(page, "OnInit", new object[] {EventArgs.Empty});
        }
    }
}