using MbUnit.Framework;
using Moq;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogML
{
    [TestFixture]
    public class BlogMLReaderTests
    {
        [Test]
        public void ReadBlogWithNullStreamThrowsException()
        {
            // arrange
            var reader = new BlogMLReader();

            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => reader.ReadBlog(new Mock<IBlogMlImportService>().Object, null));
        }
    }
}