using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Routing;
using Subtext.Framework.Syndication;

namespace UnitTests.Subtext.Framework.Syndication
{
    [TestClass]
    public class OpmlWriterTests
    {
        [TestMethod]
        public void OpmlWriter_WithTwoBlogs_RendersCorrectIndentedOpml()
        {
            //arrange
            var blogs = new[]
            {
                new Blog {Id = 1, Host = "example.com", Subfolder = "blog1", Title = "example blog"},
                new Blog {Id = 2, Host = "haacked.com", Title = "You've Been Haacked"}
            };
            var writer = new StringWriter();
            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.RssUrl(blogs[0])).Returns(new Uri("http://example.com/blog1/Rss.aspx"));
            urlHelper.Setup(u => u.RssUrl(blogs[1])).Returns(new Uri("http://haacked.com/Rss.aspx"));
            var opml = new OpmlWriter();

            //act
            opml.Write(blogs, writer, urlHelper.Object);

            //assert
            const string expected =
                @"<opml version=""1.0"">
	<head>
		<title>A Subtext Community</title>
	</head>
	<body>
		<outline text=""A Subtext Community Feeds"">
			<outline type=""rss"" text=""example blog"" xmlUrl=""http://example.com/blog1/Rss.aspx"" />
			<outline type=""rss"" text=""You've Been Haacked"" xmlUrl=""http://haacked.com/Rss.aspx"" />
		</outline>
	</body>
</opml>";

            UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected, writer.ToString());
            Assert.AreEqual(expected, writer.ToString());
        }
    }
}