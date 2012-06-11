using System.Drawing;
using System.IO;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework.Util;
using Subtext.Framework.Web;

namespace UnitTests.Subtext.Framework.Util
{
    [TestFixture]
    public class GraphicsHelperTests
    {
        [Test]
        public void GetFileStreamReturnsNullForNullPostedFile()
        {
            // arrange
            HttpPostedFile postedFile = null;

            // act
            byte[] fileStream = postedFile.GetFileStream();

            // assert
            Assert.IsNull(fileStream);
        }

        [Test]
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

        [Test]
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

        [SetUp]
        public void SetUp()
        {
            TearDown();
        }

        [TearDown]
        public void TearDown()
        {
            string imagePath = UnitTestHelper.GetPathInExecutingAssemblyLocation("pb.jpg");
            if(File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}