using System;
using System.IO;
using MbUnit.Framework;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
    [TestFixture]
    public class FileHelperTests
    {
        private const string TestDirectory = "unit-test-dir";

        [Test]
        public void IsValidImageFileName_WithImageFileName_ReturnsTrue()
        {
            // arrange
            string fileName = @"abc-ae323340-eghe-23423423.jpg";

            // act
            bool isValid = FileHelper.IsValidImageFilePath(fileName);

            // assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidImageFileName_WithTextFileName_ReturnsFalse()
        {
            // arrange
            string fileName = @"abc-ae323340-eghe-23423423.txt";

            // act
            bool isValid = FileHelper.IsValidImageFilePath(fileName);

            // assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValidFileName_WithValidChars_ReturnsTrue()
        {
            // arrange
            string fileName = @"abc-ae323340-eghe-23423423.jpg";

            // act
            bool isValid = FileHelper.IsValidFilePath(fileName);

            // assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValidFileName_WithBadFileName_ReturnsFalse()
        {
            // arrange
            string fileName = @"abc-*:\0-|\/:.txt";

            // act
            bool isValid = FileHelper.IsValidFilePath(fileName);

            // assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void CanCheckDirectory()
        {
            string dir = Path.GetFullPath(TestDirectory);
            FileHelper.EnsureDirectory(dir);
            Assert.IsTrue(Directory.Exists(dir));
        }

        [Test]
        public void CheckDirectoryThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrows<ArgumentNullException>(() => FileHelper.EnsureDirectory(null));
            UnitTestHelper.AssertThrows<ArgumentNullException>(() => FileHelper.EnsureDirectory(string.Empty));
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
