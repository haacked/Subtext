using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class SubtextBlogMlServiceTests
    {
        [Test]
        public void GetTitleFromEntry_WithPostHavingNoTitle_CreatesUsesPostNameIfAvailable()
        {
            // arrange
            var post = new BlogMLPost {Title = null, PostName = "Hello World"};

            // act
            string title = SubtextBlogMlImportService.GetTitleFromPost(post);

            // assert
            Assert.AreEqual("Hello World", title);
        }

        [Test]
        public void GetTitleFromEntry_WithPostHavingNoTitleAndNoPostName_UsesPostId()
        {
            // arrange
            var post = new BlogMLPost {Title = null, PostName = null, ID = "87618298"};

            // act
            string title = SubtextBlogMlImportService.GetTitleFromPost(post);

            // assert
            Assert.AreEqual("Post #87618298", title);
        }

        [Test]
        public void CreateBlogPost_WithNoContent_UsesId()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog {Id = 123});
            var commentService = new Mock<ICommentService>();
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var provider = new SubtextBlogMlImportService(context.Object, commentService.Object,
                                                     entryPublisher.Object);
            var blog = new BlogMLBlog();
            blog.Posts.Add(new BlogMLPost
            {Title = null, PostName = null, ID = "123", Content = new BlogMLContent {Text = ""}});

            // act
            provider.CreateBlogPost(blog, blog.Posts[0], null);

            // assert
            Assert.AreEqual("Post #123", publishedEntry.Title);
        }

        [Test]
        public void
            CreateEntryFromBlogMLBlogPost_WithNullPostNameButWithPostUrlContainingBlogSpotDotCom_UsesLastSegmentAsEntryName
            ()
        {
            // arrange
            var post = new BlogMLPost {PostUrl = "http://example.blogspot.com/2003/07/the-last-segment.html"};
            var blog = new BlogMLBlog();

            // act
            Entry entry = SubtextBlogMlImportService.CreateEntryFromBlogMLBlogPost(blog, post,
                                                                              new Dictionary<string, string>());

            // assert
            Assert.AreEqual("the-last-segment", entry.EntryName);
        }

        [Test]
        public void
            CreateEntryFromBlogMLBlogPost_WithNullTitleNameButWithPostUrlContainingBlogSpotDotCom_UsesLastSegmentAsTitle
            ()
        {
            // arrange
            var post = new BlogMLPost {PostUrl = "http://example.blogspot.com/2003/07/the-last-segment.html"};
            var blog = new BlogMLBlog();

            // act
            Entry entry = SubtextBlogMlImportService.CreateEntryFromBlogMLBlogPost(blog, post,
                                                                              new Dictionary<string, string>());

            // assert
            Assert.AreEqual("the last segment", entry.Title);
        }

        [Test]
        public void CreateEntryFromBlogMLBlogPost_WithPostHavingExcerpt_SetsEntryDescription()
        {
            // arrange
            var post = new BlogMLPost
            {HasExcerpt = true, Excerpt = new BlogMLContent {Text = "This is a story about a 3 hour voyage"}};
            var blog = new BlogMLBlog();

            // act
            Entry entry = SubtextBlogMlImportService.CreateEntryFromBlogMLBlogPost(blog, post,
                                                                              new Dictionary<string, string>());

            // assert
            Assert.AreEqual("This is a story about a 3 hour voyage", entry.Description);
        }

        [Test]
        public void ImportBlog_WithBlogNotAllowingDuplicateComments_TurnsOffCommentsTemporarily()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog{Host = "localhost", DuplicateCommentsEnabled = false});
            var duplicateCommentsEnabledValues = new List<bool>();
            context.Setup(c => c.Repository.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>(blog => duplicateCommentsEnabledValues.Add(blog.DuplicateCommentsEnabled));
            var commentService = new Mock<ICommentService>();
            var entryPublisher = new Mock<IEntryPublisher>();
            var importService = new SubtextBlogMlImportService(context.Object, commentService.Object, entryPublisher.Object);
            var reader = new Mock<BlogMLReader>();
            reader.Setup(r => r.ReadBlog(importService, It.IsAny<Stream>()));

            // act
            importService.ImportBlog(reader.Object, new MemoryStream());

            //assert
            Assert.IsTrue(duplicateCommentsEnabledValues.First());
            Assert.IsFalse(duplicateCommentsEnabledValues.ElementAt(1));
        }

    }
}