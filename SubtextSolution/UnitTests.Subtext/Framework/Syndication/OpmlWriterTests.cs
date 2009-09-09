using System;
using System.IO;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Routing;
using Subtext.Framework.Syndication;

namespace UnitTests.Subtext.Framework.Syndication
{
    [TestFixture]
    public class OpmlWriterTests
    {
        [Test]
        public void OpmlWriter_WithTwoBlogs_RendersCorrectIndentedOpml()
        {
            //arrange
            var blogs = new[]
            {
                new Blog {Id = 1, Host = "example.com", Subfolder = "blog1", Title = "example blog"},
                new Blog {Id = 2, Host = "haacked.com", Title = "You've Been Haacked"}
            };
            var writer = new StringWriter();
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.BlogUrl(blogs[0])).Returns("/blog1");
            urlHelper.Setup(u => u.RssUrl(blogs[0])).Returns(new Uri("http://example.com/blog1/Rss.aspx"));
            urlHelper.Setup(u => u.BlogUrl(blogs[1])).Returns("/");
            urlHelper.Setup(u => u.RssUrl(blogs[1])).Returns(new Uri("http://haacked.com/Rss.aspx"));
            var opml = new OpmlWriter();

            //act
            opml.Write(blogs, writer, urlHelper.Object);

            //assert
            string expected =
                @"<opml version=""1.0"">
	<body>
		<outline title=""example blog"" htmlUrl=""http://example.com/blog1"" xmlUrl=""http://example.com/blog1/Rss.aspx"" />
		<outline title=""You've Been Haacked"" htmlUrl=""http://haacked.com/"" xmlUrl=""http://haacked.com/Rss.aspx"" />
	</body>
</opml>";
            UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected, writer.ToString());
            Assert.AreEqual(expected, writer.ToString());
        }
    }
}