using System;
using System.IO;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
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
            var repository = new DatabaseObjectProvider();

            Image image = CreateImageInstance();
            Assert.GreaterEqualThan(Config.CurrentBlog.Id, 0);
            Assert.AreEqual(Config.CurrentBlog.Id, image.BlogId);
            int imageId = repository.Insert(image, singlePixelBytes);

            Image saved = repository.GetImage(imageId, true /* activeOnly */);
            Assert.AreEqual(Config.CurrentBlog.Id, saved.BlogId, "The blog id for the image does not match!");
            saved.LocalDirectoryPath = Path.GetFullPath(TestDirectory);
            Assert.AreEqual("Test Image", saved.Title);

            saved.Title = "A Better Title";
            repository.Update(saved, singlePixelBytes);

            Image loaded = repository.GetImage(imageId, true /* activeOnly */);
            Assert.AreEqual(Config.CurrentBlog.Id, loaded.BlogId, "The blog id for the image does not match!");
            loaded.LocalDirectoryPath = Path.GetFullPath(TestDirectory);

            Assert.AreEqual("A Better Title", loaded.Title, "The title was not updated");
        }

        [Test]
        [RollBack2]
        public void CanGetImagesByCategoryId()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "UnitTestImages",
                                                           CategoryType.ImageCollection);

            Assert.AreEqual(0, repository.GetImagesByCategory(categoryId, true).Count);

            Image image = CreateImageInstance(Config.CurrentBlog, categoryId);
            image.IsActive = true;

            int imageId = repository.Insert(image, singlePixelBytes);

            ImageCollection images = repository.GetImagesByCategory(categoryId, true);
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
            var repository = new DatabaseObjectProvider();
            Image image = CreateStandaloneImageInstance();
            Images.SaveImage(singlePixelBytes, image.OriginalFilePath);

            Assert.AreEqual(NullValue.NullInt32, repository.Insert(image, singlePixelBytes));
        }

        [Test]
        [RollBack2]
        public void CanInsertAndDeleteImage()
        {
            var repository = new DatabaseObjectProvider();
            int imageId = 0;
            Image image = CreateImageInstance();
            image.IsActive = true;
            Image loadedImage = null;
            try
            {
                imageId = repository.Insert(image, singlePixelBytes);
                loadedImage = repository.GetImage(imageId, false /* activeOnly */);
                Assert.IsNotNull(loadedImage);
                Assert.AreEqual(image.CategoryID, loadedImage.CategoryID);
            }
            finally
            {
                if (loadedImage != null)
                {
                    repository.Delete(loadedImage);
                }
                Assert.IsNull(repository.GetImage(imageId, false /* activeOnly */));
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
            var blog = new Blog { Host = "localhost", Subfolder = "" };

            // act
            string path = helper.GalleryDirectoryPath(blog, 123);

            // assert
            Assert.AreEqual(@"c:\123\", path);
        }

        [Test]
        public void DeleteImageThrowsArgumentNullException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Delete((Image)null));
        }

        [Test]
        public void InsertImageThrowsArgumentNullException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Insert(null, new byte[0]));
        }

        [Test]
        public void MakeAlbumImagesThrowsArgumentNullException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => Images.MakeAlbumImages(null));
        }

        [Test]
        public void SaveImageThrowsArgumentNullExceptionForNullBuffer()
        {
            var repository = new DatabaseObjectProvider();
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
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Update(null, new byte[0]));
        }

        [Test]
        public void UpdateThrowsArgumentNullExceptionForNullBuffer()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Update(new Image(), null));
        }

        [Test]
        public void UpdateImageThrowsArgumentNullException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Update((Image)null));
        }

        [SetUp]
        public void SetUp()
        {
            if (Directory.Exists(TestDirectory))
            {
                Directory.Delete(TestDirectory, true);
            }
            if (Directory.Exists("image"))
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