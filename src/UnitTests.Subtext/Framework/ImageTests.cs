using System;
using System.IO;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework
{
    /// <summary>
    /// Tests of the Images class.
    /// </summary>
    /// <remarks>
    /// All tests should use the TestDirectory directory. For example, to create that 
    /// directory, just do this: Directory.Create(TestDirectory);
    /// </remarks>
    [TestFixture]
    public class ImageTests
    {
        private const string TestDirectory = "unit-test-dir";

        static readonly Byte[] singlePixelBytes =
            Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");

        [Test]
        [RollBack2]
        public void CanUpdate()
        {
            UnitTestHelper.SetupBlog();

            Image image = CreateImageInstance();
            Assert.GreaterEqualThan(Config.CurrentBlog.Id, 0);
            Assert.AreEqual(Config.CurrentBlog.Id, image.BlogId);
            int imageId = Images.InsertImage(image, singlePixelBytes);

            Image saved = ObjectProvider.Instance().GetImage(imageId, true /* activeOnly */);
            Assert.AreEqual(Config.CurrentBlog.Id, saved.BlogId, "The blog id for the image does not match!");
            saved.LocalDirectoryPath = Path.GetFullPath(TestDirectory);
            Assert.AreEqual("Test Image", saved.Title);

            saved.Title = "A Better Title";
            Images.Update(saved, singlePixelBytes);

            Image loaded = ObjectProvider.Instance().GetImage(imageId, true /* activeOnly */);
            Assert.AreEqual(Config.CurrentBlog.Id, loaded.BlogId, "The blog id for the image does not match!");
            loaded.LocalDirectoryPath = Path.GetFullPath(TestDirectory);

            Assert.AreEqual("A Better Title", loaded.Title, "The title was not updated");
        }

        [Test]
        [RollBack2]
        public void CanGetImagesByCategoryId()
        {
            UnitTestHelper.SetupBlog();
            int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "UnitTestImages",
                                                           CategoryType.ImageCollection);

            Assert.AreEqual(0, Images.GetImagesByCategoryId(categoryId, true).Count);

            Image image = CreateImageInstance(Config.CurrentBlog, categoryId);
            image.IsActive = true;

            int imageId = Images.InsertImage(image, singlePixelBytes);

            ImageCollection images = Images.GetImagesByCategoryId(categoryId, true);
            Assert.AreEqual(1, images.Count, "Expected to get our one image.");
            Assert.AreEqual(imageId, images[0].ImageID);
        }

        [Test]
        [RollBack2]
        public void CanSaveImage()
        {
            string filePath = Path.GetFullPath(@TestDirectory + Path.DirectorySeparatorChar + "test.gif");

            Assert.IsTrue(Images.SaveImage(singlePixelBytes, filePath));
            FileAssert.Exists(filePath);
        }

        [Test]
        public void CanMakeAlbumImages()
        {
            var image = new Image();
            image.Title = "Test Image";
            image.Height = 1;
            image.Width = 1;
            image.IsActive = true;
            image.LocalDirectoryPath = Path.GetFullPath(TestDirectory);
            image.FileName = "test.gif";

            //Write original image.
            Images.SaveImage(singlePixelBytes, image.OriginalFilePath);
            FileAssert.Exists(image.OriginalFilePath);

            Images.MakeAlbumImages(image);
            FileAssert.Exists(image.ResizedFilePath);
            FileAssert.Exists(image.ThumbNailFilePath);
        }

        [Test]
        public void InsertImageReturnsFalseForExistingImage()
        {
            Image image = CreateStandaloneImageInstance();
            Images.SaveImage(singlePixelBytes, image.OriginalFilePath);

            Assert.AreEqual(NullValue.NullInt32, Images.InsertImage(image, singlePixelBytes));
        }

        [Test]
        [RollBack2]
        public void CanInsertAndDeleteImage()
        {
            int imageId = 0;
            Image image = CreateImageInstance();
            image.IsActive = true;
            Image loadedImage = null;
            try
            {
                imageId = Images.InsertImage(image, singlePixelBytes);
                loadedImage = ObjectProvider.Instance().GetImage(imageId, false /* activeOnly */);
                Assert.IsNotNull(loadedImage);
                Assert.AreEqual(image.CategoryID, loadedImage.CategoryID);
            }
            finally
            {
                if(loadedImage != null)
                {
                    Images.DeleteImage(loadedImage);
                }
                Assert.IsNull(ObjectProvider.Instance().GetImage(imageId, false /* activeOnly */));
            }
        }

        private static Image CreateStandaloneImageInstance()
        {
            var image = new Image();
            image.Title = "Test Image";
            image.Height = 1;
            image.Width = 1;
            image.IsActive = true;
            image.LocalDirectoryPath = Path.GetFullPath(TestDirectory);
            image.FileName = "test.gif";
            return image;
        }

        private static Image CreateImageInstance()
        {
            UnitTestHelper.SetupBlog();
            int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, TestDirectory);
            return CreateImageInstance(Config.CurrentBlog, categoryId);
        }

        private static Image CreateImageInstance(Blog currentBlog, int categoryId)
        {
            Image image = CreateStandaloneImageInstance();
            image.BlogId = currentBlog.Id;
            image.CategoryID = categoryId;
            return image;
        }

        [Test]
        public void SaveImageReturnsFalseForInvalidImageName()
        {
            Assert.IsFalse(Images.SaveImage(singlePixelBytes, "!"));
        }

        [Test]
        public void GalleryDirectoryPath_WithBlogAndCategoryId_ReturnPhysicalDirectoryPath()
        {
            // arrange
            BlogUrlHelper helper = UnitTestHelper.SetupUrlHelper("/Subtext.Web");
            Mock<HttpContextBase> httpContext = Mock.Get(helper.HttpContext);
            httpContext.Setup(c => c.Server.MapPath("/Subtext.Web/images/localhost/Subtext_Web/123/")).Returns(
                @"c:\123\");
            var blog = new Blog {Host = "localhost", Subfolder = ""};

            // act
            string path = helper.GalleryDirectoryPath(blog, 123);

            // assert
            Assert.AreEqual(@"c:\123\", path);
        }

        [Test]
        public void DeleteImageThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.DeleteImage(null));
        }

        [Test]
        public void InsertImageThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.InsertImage(null, new byte[0]));
        }

        [Test]
        public void MakeAlbumImagesThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.MakeAlbumImages(null));
        }

        [Test]
        public void SaveImageThrowsArgumentNullExceptionForNullBuffer()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.SaveImage(null, "x"));
        }

        [Test]
        public void SaveImageThrowsArgumentNullExceptionForNullFileName()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.SaveImage(new byte[0], null));
        }

        [Test]
        public void SaveImageThrowsArgumentExceptionForNullFileName()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.SaveImage(new byte[0], ""));
        }

        [Test]
        public void UpdateThrowsArgumentNullExceptionForNullImage()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.Update(null, new byte[0]));
        }

        [Test]
        public void UpdateThrowsArgumentNullExceptionForNullBuffer()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.Update(new Image(), null));
        }

        [Test]
        public void UpdateImageThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.UpdateImage(null));
        }

        [SetUp]
        public void SetUp()
        {
            if(Directory.Exists(TestDirectory))
            {
                Directory.Delete(TestDirectory, true);
            }
            if(Directory.Exists("image"))
            {
                Directory.Delete("image", true);
            }
        }

        [TearDown]
        public void TearDown()
        {
            SetUp();
        }
    }
}