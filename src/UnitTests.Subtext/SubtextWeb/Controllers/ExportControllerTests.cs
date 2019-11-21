using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework;
using Subtext.ImportExport;
using Subtext.Infrastructure.ActionResults;
using Subtext.Web.Controllers;

namespace UnitTests.Subtext.SubtextWeb.Controllers
{
    [TestClass]
    public class ExportControllerTests
    {
        [TestMethod]
        public void Ctor_WithBlogMLSource_SetsSource()
        {
            // arrange
            var source = new Mock<IBlogMLSource>();
            
            // act
            var controller = new ExportController(source.Object, new Blog {Title = "whatever"});

            // assert
            Assert.AreEqual(source.Object, controller.Source);
        }

        [TestMethod]
        public void ExportBlogML_WithEmbedAttachmetsTrue_ReturnsExportActionResultWithEmbedTrue()
        {
            // arrange
            var source = new Mock<IBlogMLSource>();
            var controller = new ExportController(source.Object, new Blog { Title = "whatever" });

            // act
            var result = controller.BlogML(true /*embedAttachments*/) as ExportActionResult;

            // assert
            var writer = result.BlogMLWriter as BlogMLWriter;
            Assert.IsTrue(writer.EmbedAttachments);
        }

        [TestMethod]
        public void ExportBlogML_WithEmbedAttachmentsFalse_ReturnsExportActionResultWithEmbedFalse()
        {
            // arrange
            var source = new Mock<IBlogMLSource>();
            var controller = new ExportController(source.Object, new Blog { Title = "whatever" });

            // act
            var result = controller.BlogML(false /*embedAttachments*/) as ExportActionResult;

            // assert
            var writer = result.BlogMLWriter as BlogMLWriter;
            Assert.IsFalse(writer.EmbedAttachments);
        }

        [TestMethod]
        public void ExportBlogML_WithBlogTitle_SetsFileDownloadNameToTitle()
        {
            // arrange
            var source = new Mock<IBlogMLSource>();
            var controller = new ExportController(source.Object, new Blog { Title = "whatever" });

            // act
            var result = controller.BlogML(false /*embedAttachments*/) as ExportActionResult;

            // assert
            Assert.AreEqual("whatever-Export.xml", result.FileDownloadName);
        }

        [TestMethod]
        public void ExportBlogML_WithBlogTitleHavingIllegalFileNameCharacters_RemovesThoseCharactersFromFileDownloadName()
        {
            // arrange
            var source = new Mock<IBlogMLSource>();
            var controller = new ExportController(source.Object, new Blog { Title = @"whatever \|/ you say" });

            // act
            var result = controller.BlogML(false /*embedAttachments*/) as ExportActionResult;

            // assert
            Assert.AreEqual("whatever  you say-Export.xml", result.FileDownloadName);
        }
    }
}
