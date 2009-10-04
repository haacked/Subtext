using System;
using System.IO;
using MbUnit.Framework;
using Moq;
using Subtext.BlogML;

namespace UnitTests.Subtext.BlogML
{
    [TestFixture]
    public class BlogMLReaderTests
    {
        [Test]
        [ExpectedArgumentNullException]
        public void CreateReaderWithNoProviderThrowsArgumentNullException()
        {
            BlogMLReader.Create(null);
        }

        [Test]
        public void ReadBlogWithNullStreamThrowsException()
        {
            // arrange
            var provider = new Mock<BlogMLProvider>();
            BlogMLReader reader = BlogMLReader.Create(provider.Object);

            // act, assert
            UnitTestHelper.AssertThrows<ArgumentNullException>(() => reader.ReadBlog(null));
        }

        [Test]
        public void CanCreateReaderWithProvider()
        {
            var provider = new Mock<BlogMLProvider>();
            BlogMLReader.Create(provider.Object);
        }

        [Test]
        public void ImportCallsPreAndCompleteMethods()
        {
            // arrange
            var provider = new Mock<BlogMLProvider>();
            bool preImportCalled = false;
            bool importCompleteCalled = false;
            provider.Setup(p => p.PreImport()).Callback(() => preImportCalled = true);
            provider.Setup(p => p.ImportComplete()).Callback(() => importCompleteCalled = true);
            BlogMLReader reader = BlogMLReader.Create(provider.Object);

            // act
            using(Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml"))
            {
                reader.ReadBlog(stream);
            }

            //assert
            Assert.IsTrue(preImportCalled);
            Assert.IsTrue(importCompleteCalled);
        }
    }
}