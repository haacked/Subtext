using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
    [TestClass]
    public class FileHelperTests
    {
        private const string TestDirectory = "unit-test-dir";

        [TestMethod]
        public void IsValidImageFileName_WithImageFileName_ReturnsTrue()
        {
            // arrange
            string fileName = @"abc-ae323340-eghe-23423423.jpg";

            // act
            bool isValid = FileHelper.IsValidImageFilePath(fileName);

            // assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidImageFileName_WithTextFileName_ReturnsFalse()
        {
            // arrange
            string fileName = @"abc-ae323340-eghe-23423423.txt";

            // act
            bool isValid = FileHelper.IsValidImageFilePath(fileName);

            // assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsValidFileName_WithValidChars_ReturnsTrue()
        {
            // arrange
            string fileName = @"abc-ae323340-eghe-23423423.jpg";

            // act
            bool isValid = FileHelper.IsValidFilePath(fileName);

            // assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidFileName_WithBadFileName_ReturnsFalse()
        {
            // arrange
            string fileName = @"abc-*:\0-|\/:.txt";

            // act
            bool isValid = FileHelper.IsValidFilePath(fileName);

            // assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void CanCheckDirectory()
        {
            string dir = Path.GetFullPath(TestDirectory);
            FileHelper.EnsureDirectory(dir);
            Assert.IsTrue(Directory.Exists(dir));
        }

        [TestMethod]
        public void CheckDirectoryThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => FileHelper.EnsureDirectory(null));
            UnitTestHelper.AssertThrowsArgumentNullException(() => FileHelper.EnsureDirectory(string.Empty));
        }

        [TestMethod]
        public void WriteBytesToFile_WithNullDestination_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => FileHelper.WriteBytesToFile(null, new byte[0]));
            UnitTestHelper.AssertThrowsArgumentNullException(
                () => FileHelper.WriteBytesToFile(string.Empty, new byte[0]));
        }

        [TestMethod]
        public void WriteBytesToFile_WithInvalidFilePath_ThrowsInvailidOperationException()
        {
            UnitTestHelper.AssertThrows<InvalidOperationException>(
                () => FileHelper.WriteBytesToFile("c:\\foo\\#$3211|.jpg", new byte[0]));
        }

        private void DeleteTestFolders()
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

        [TestInitialize]
        public void TestInitialize()
        {
            DeleteTestFolders();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteTestFolders();
        }
    }
}