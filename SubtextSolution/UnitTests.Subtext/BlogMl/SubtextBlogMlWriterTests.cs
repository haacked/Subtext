using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.BlogML;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;
using Subtext.ImportExport;
using System.Collections.ObjectModel;
using Subtext.Extensibility.Interfaces;

namespace UnitTests.Subtext.BlogML
{
	/// <summary>
	/// Unit tests of the BlogImportExport functionality.
	/// </summary>
	[TestFixture]
	public class SubtextBlogMlWriterTests
	{
		/// <summary>
		/// Make sure that when we export a post with a category, that we retain 
		/// the mapping between the post and category.
		/// </summary>
		[Test]
		public void CanWritePostWithCategory() {
            //arrange
            Blog blog = new Blog
            {
                Id = 1975,
                Title = "The Title Of This Blog",
                Author = "MasterChief",
                Host = "example.com"
            };

            var categories = new Collection<LinkCategory>();
            categories.Add(new LinkCategory { Id = 123, 
                BlogId = 1975,
                CategoryType = CategoryType.PostCollection,
                IsActive = true, 
                Description = "description of category",
                Title = "Test Category"
            });

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Repository.GetBlogById(1975)).Returns(blog);
            subtextContext.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false)).Returns(categories);
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<Entry>())).Returns("/whatever");
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.Blog).Returns(blog);
            TestBlogMlProvider provider = new TestBlogMlProvider(subtextContext.Object);
			BlogMLWriter writer = BlogMLWriter.Create(provider);
            StringWriter stringWriter = new StringWriter();

            //act
            using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
			{
				writer.Write(xmlWriter);
            }
            
            //assert
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stringWriter.ToString());

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/09/BlogML");

            Assert.AreEqual("1", xml.SelectSingleNode("/bml:blog/bml:categories/bml:category/@id", nsmgr).InnerText);
            Assert.AreEqual("Test Category", xml.SelectSingleNode("/bml:blog/bml:categories/bml:category/bml:title", nsmgr).InnerText);
		}

        //[Test]
        //public void WritingBlogMLWithEverythingWorks() {
        //    //arrange
        //    Blog blog = new Blog
        //    {
        //        Id = 1975,
        //        Title = "The Title Of This Blog",
        //        Author = "MasterChief",
        //        Host = "example.com"
        //    };

        //    var categories = new Collection<LinkCategory>();
        //    categories.Add(new LinkCategory {
        //        Id = 123,
        //        BlogId = 1975,
        //        CategoryType = CategoryType.PostCollection,
        //        IsActive = true,
        //        Description = "description of category",
        //        Title = "CategoryA"
        //    });

        //    Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication(blog, "phil", "blah blah", "full bodied goodness");
        //    entry.Categories.Add("CategoryA");
        //    FeedbackItem comment = UnitTestHelper.CreateCommentInstance(blog, entry.Id, "joe", "re: blah", UnitTestHelper.GenerateUniqueString(), DateTime.Now);
        //    comment.FeedbackType = FeedbackType.Comment;
        //    comment.Status = FeedbackStatusFlag.Approved;

        //    Trackback trackback = new Trackback(entry.Id, "blah", new Uri("http://example.com/"), "you", "your post is great" + UnitTestHelper.GenerateUniqueString(), DateTime.Now);
        //    trackback.BlogId = blog.Id;
        //    trackback.Status = FeedbackStatusFlag.Approved;

        //    var subtextContext = new Mock<ISubtextContext>();
        //    subtextContext.Setup(c => c.Repository.GetBlogById(1975)).Returns(blog);
        //    subtextContext.Setup(c => c.Repository.GetCategories(CategoryType.PostCollection, false)).Returns(categories);
        //    subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<Entry>())).Returns("/whatever");
        //    subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
        //    subtextContext.Setup(c => c.Blog).Returns(blog);
        //    TestBlogMlProvider provider = new TestBlogMlProvider(subtextContext.Object);
        //    BlogMLWriter writer = BlogMLWriter.Create(provider);
        //    StringWriter stringWriter = new StringWriter();

        //    //act
        //    using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter)) {
        //        writer.Write(xmlWriter);
        //    }
            
        //    //assert
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(stringWriter.ToString());
        //    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        //    nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/09/BlogML");

        //    XmlNode postNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']", nsmgr);
        //    Assert.IsNotNull(postNode, "The post node is null");

        //    XmlNode firstPostCategoryNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']/bml:categories/bml:category", nsmgr);
        //    Assert.IsNotNull(firstPostCategoryNode, "Expected a category for the first post");

        //    XmlNode firstPostCommentNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']/bml:comments/bml:comment", nsmgr);
        //    Assert.IsNotNull(firstPostCommentNode, "Expected a comment for the first post");

        //    XmlNode firstPostTrackbackNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']/bml:trackbacks/bml:trackback", nsmgr);
        //    Assert.IsNotNull(firstPostTrackbackNode, "Expected a trackback for the first post");
        //}


        //Temporary hack to get this test to pass while we refactor.
        internal class TestBlogMlProvider : SubtextBlogMLProvider {
            public TestBlogMlProvider(ISubtextContext context) : base("connection string", context) {
                BlogMLPosts = new PagedCollection<BlogMLPost>();
            }

            public override IPagedCollection<BlogMLPost> GetBlogPosts(string blogId, int pageIndex, int pageSize) {
                return BlogMLPosts;   
            }

            public IPagedCollection<BlogMLPost> BlogMLPosts {
                get;
                private set;
            }
        }
	}
}
