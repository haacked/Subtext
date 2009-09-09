using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ImageTests
    {
        [Test]
        [RollBack]
        public void CanGetRecentImages()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            ObjectProvider provider = ObjectProvider.Instance();
            var category = new LinkCategory
            {
                BlogId = Config.CurrentBlog.Id,
                Description = "Whatever",
                IsActive = true,
                Title = "Whatever"
            };
            int categoryId = provider.CreateLinkCategory(category);

            var image = new Image
            {
                Title = "Title",
                CategoryID = categoryId,
                BlogId = Config.CurrentBlog.Id,
                FileName = "Foo",
                Height = 10,
                Width = 10,
                IsActive = true,
            };
            int imageId = provider.InsertImage(image);

            //act
            ICollection<Image> images = provider.GetImages(Config.CurrentBlog.Host, null, 10);

            //assert
            Assert.AreEqual(1, images.Count);
            Assert.AreEqual(imageId, images.First().ImageID);
        }

        [Test]
        public void CanGetAndSetSimpleProperties()
        {
            var image = new Image();

            image.BlogId = 123;
            Assert.AreEqual(123, image.BlogId);

            image.CategoryID = 321;
            Assert.AreEqual(321, image.CategoryID);

            image.FileName = "Test.jpg";
            Assert.AreEqual("Test.jpg", image.FileName);

            image.Height = 300;
            Assert.AreEqual(300, image.Height);

            image.ImageID = 999;
            Assert.AreEqual(999, image.ImageID);

            image.IsActive = true;
            Assert.IsTrue(image.IsActive);

            image.LocalDirectoryPath = @"d:\";
            Assert.AreEqual(@"d:\", image.LocalDirectoryPath);

            image.Title = "Testing";
            Assert.AreEqual("Testing", image.Title);

            image.Width = 312;
            Assert.AreEqual(312, image.Width);
        }

        [Test]
        public void CanGetFilePath()
        {
            var image = new Image();
            image.LocalDirectoryPath = @"c:\";
            image.FileName = @"Test.jpg";
            Assert.AreEqual(@"c:\Test.jpg", image.FilePath);
        }

        [Test]
        public void GetOriginalFileNamePrependsLetterOWithUnderscore()
        {
            var image = new Image();
            image.LocalDirectoryPath = @"c:\";
            image.FileName = @"Test.jpg";
            Assert.AreEqual(@"o_Test.jpg", image.OriginalFile);
            Assert.AreEqual(@"c:\o_Test.jpg", image.OriginalFilePath);
        }

        [Test]
        public void GetOriginalThumbNailFileNamePrependsLetterTWithUnderscore()
        {
            var image = new Image();
            image.LocalDirectoryPath = @"c:\";
            image.FileName = @"Test.jpg";
            Assert.AreEqual(@"t_Test.jpg", image.ThumbNailFile);
            Assert.AreEqual(@"c:\t_Test.jpg", image.ThumbNailFilePath);
        }

        [Test]
        public void GetResizedFileNamePrependsLetterTWithUnderscore()
        {
            var image = new Image();
            image.FileName = @"Test.jpg";
            image.LocalDirectoryPath = @"c:\";
            Assert.AreEqual(@"r_Test.jpg", image.ResizedFile);
            Assert.AreEqual(@"c:\r_Test.jpg", image.ResizedFilePath);
        }
    }
}