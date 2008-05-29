using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ImageTests
    {
        [Test]
        public void CanGetAndSetSimpleProperties()
        {
            Image image = new Image();
            
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
            Image image = new Image();
            image.LocalDirectoryPath = @"c:\";
            image.FileName = @"Test.jpg";
            Assert.AreEqual(@"c:\Test.jpg", image.FilePath);
        }

        [Test]
        public void GetOriginalFileNamePrependsLetterOWithUnderscore()
        {
            Image image = new Image();
            image.LocalDirectoryPath = @"c:\";
            image.FileName = @"Test.jpg";
            Assert.AreEqual(@"o_Test.jpg", image.OriginalFile);
            Assert.AreEqual(@"c:\o_Test.jpg", image.OriginalFilePath);
        }

        [Test]
        public void GetOriginalThumbNailFileNamePrependsLetterTWithUnderscore()
        {
            Image image = new Image();
            image.LocalDirectoryPath = @"c:\";
            image.FileName = @"Test.jpg";
            Assert.AreEqual(@"t_Test.jpg", image.ThumbNailFile);
            Assert.AreEqual(@"c:\t_Test.jpg", image.ThumbNailFilePath);
        }

        [Test]
        public void GetResizedFileNamePrependsLetterTWithUnderscore()
        {
            Image image = new Image();
            image.FileName = @"Test.jpg";
            image.LocalDirectoryPath = @"c:\";
            Assert.AreEqual(@"r_Test.jpg", image.ResizedFile);
            Assert.AreEqual(@"c:\r_Test.jpg", image.ResizedFilePath);
        }

    }
}
