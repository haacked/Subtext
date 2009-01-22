using System.Collections.Generic;
using System.IO;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Web.SiteMap;
using System;
using System.Xml;
using System.Globalization;
using UnitTests.Subtext;

namespace UnitTests.Subtext.SubtextWeb
{
    [TestFixture]
    public class SitemapHandlerTests
    {
        [Test]
        public void ProcessRequest_WithSingleBlogPost_ProducesSitemapWithBlogPostNode()
        {
            //arrange
            var entries = new List<Entry>();
            entries.Add(new Entry(PostType.BlogPost) { Id = 123, DateModified = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture)});
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetPostCountsByMonth()).Returns(new List<ArchiveCount>());
            repository.Setup(r => r.GetEntries(It.IsAny<int>(), PostType.BlogPost, It.IsAny<PostConfig>(), It.IsAny<bool>())).Returns(entries);
            repository.Setup(r => r.GetEntries(It.IsAny<int>(), PostType.Story, It.IsAny<PostConfig>(), It.IsAny<bool>())).Returns(new List<Entry>());
            repository.Setup(r => r.GetCategories(CategoryType.PostCollection, true)).Returns(new List<LinkCategory>());

            Mock<ISubtextContext> subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSitemapHandlerRequest(repository);
            var handler = new SiteMapHttpHandler();
            handler.SubtextContext = subtextContext.Object;

            //act
            handler.ProcessRequest(subtextContext.Object);
            
            //assert
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(writer.ToString());
            var entryUrlNode = doc.DocumentElement.ChildNodes[1];
            Assert.AreEqual("http://localhost/some-blogpost-with-id-of-123", entryUrlNode.ChildNodes[0].InnerText);
            Assert.AreEqual("2008-01-23", entryUrlNode.ChildNodes[1].InnerText);
        }

        [Test]
        public void ProcessRequest_WithSingleArticle_ProducesSitemapWithArticleNode()
        {
            //arrange
            var entries = new List<Entry>();
            entries.Add(new Entry(PostType.Story) { Id = 321, DateModified = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture) });
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetPostCountsByMonth()).Returns(new List<ArchiveCount>());
            repository.Setup(r => r.GetEntries(It.IsAny<int>(), PostType.BlogPost, It.IsAny<PostConfig>(), It.IsAny<bool>())).Returns(new List<Entry>());
            repository.Setup(r => r.GetEntries(It.IsAny<int>(), PostType.Story, It.IsAny<PostConfig>(), It.IsAny<bool>())).Returns(entries);
            repository.Setup(r => r.GetCategories(CategoryType.PostCollection, true)).Returns(new List<LinkCategory>());

            Mock<ISubtextContext> subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSitemapHandlerRequest(repository);
            var handler = new SiteMapHttpHandler();
            handler.SubtextContext = subtextContext.Object;

            //act
            handler.ProcessRequest(subtextContext.Object);

            //assert
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(writer.ToString());
            var entryUrlNode = doc.DocumentElement.ChildNodes[1];
            Assert.AreEqual("http://localhost/some-article-with-id-of-321", entryUrlNode.ChildNodes[0].InnerText);
            Assert.AreEqual("2008-01-23", entryUrlNode.ChildNodes[1].InnerText);
        }
    }
}
