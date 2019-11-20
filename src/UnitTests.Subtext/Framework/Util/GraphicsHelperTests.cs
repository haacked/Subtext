using System.Drawing;
using System.IO;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Util;
using Subtext.Framework.Web;

namespace UnitTests.Subtext.Framework.Util
{
    [TestClass]
    public class GraphicsHelperTests
    {
        [TestMethod]
        public void GetFileStreamReturnsNullForNullPostedFile()
        {
            // arrange
            HttpPostedFile postedFile = null;

            // act
            byte[] fileStream = postedFile.GetFileStream();

            // assert
            Assert.IsNull(fileStream);
        }

        [TestMethod]
        public void FromFilePathAsUnindexedImage_WithFilePath_ReturnsImage()
        {
            // arrange
            string filePath = UnitTestHelper.UnpackEmbeddedBinaryResource("Framework.pb.jpg", "pb.jpg");

            // act
            Size imageSize;
            using(Image image = GraphicsHelper.FromFilePathAsUnindexedImage(filePath))
            {
                imageSize = image.Size;
            }

            // assert
            Assert.AreEqual(150, imageSize.Width);
            Assert.AreEqual(113, imageSize.Height);
        }

        [TestMethod]
        public void GetResizedImage_WithImage_ReturnsResizedImage()
        {
            // arrange
            string filePath = UnitTestHelper.UnpackEmbeddedBinaryResource("Framework.pb.jpg", "pb.jpg");

            // act
            Size imageSize;
            using(Image image = GraphicsHelper.FromFilePathAsUnindexedImage(filePath))
            {
                using(Image resized = image.GetResizedImage(new Size(100, 50)))
                {
                    imageSize = resized.Size;
                }
            }

            // assert
            Assert.AreEqual(100, imageSize.Width);
            Assert.AreEqual(50, imageSize.Height);
        }

        private void DeleteTestImage()
        {
            string imagePath = UnitTestHelper.GetPathInExecutingAssemblyLocation("pb.jpg");
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            DeleteTestImage();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteTestImage();
        }
    }
}