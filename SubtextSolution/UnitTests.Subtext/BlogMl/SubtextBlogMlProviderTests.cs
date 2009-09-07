using System.Collections.Generic;
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
    public class SubtextBlogMlProviderTests
    {
        [Test]
        public void GetTitleFromEntry_WithPostHavingNoTitle_CreatesUsesPostNameIfAvailable()
        {
            // arrange
            BlogMLPost post = new BlogMLPost { Title = null, PostName = "Hello World"};

            // act
            string title = SubtextBlogMLProvider.GetTitleFromPost(post);

            // assert
            Assert.AreEqual("Hello World", title);
        }

        [Test]
        public void GetTitleFromEntry_WithPostHavingNoTitleAndNoPostName_UsesPostId()
        {
            // arrange
            BlogMLPost post = new BlogMLPost { Title = null, PostName = null, ID = "87618298" };

            // act
            string title = SubtextBlogMLProvider.GetTitleFromPost(post);

            // assert
            Assert.AreEqual("Post #87618298", title);
        }

        [Test]
        public void CreateBlogPost_WithNoContent_UsesId()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 123 });
            var commentService = new Mock<ICommentService>();
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry publishedEntry = null;
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Callback<Entry>(e => publishedEntry = e);
            var provider = new SubtextBlogMLProvider("test", context.Object, commentService.Object, entryPublisher.Object);
            var blog = new BlogMLBlog();
            blog.Posts.Add(new BlogMLPost { Title = null, PostName = null, ID = "123", Content = new BlogMLContent { Text = "" } });

            // act
            provider.CreateBlogPost(blog, blog.Posts[0], null);

            // assert
            Assert.AreEqual("Post #123", publishedEntry.Title);
        }

        [Test]
        public void CreateEntryFromBlogMLBlogPost_WithNullPostNameButWithPostUrlContainingBlogSpotDotCom_UsesLastSegmentAsEntryName()
        {
            // arrange
            var post = new BlogMLPost { PostUrl = "http://example.blogspot.com/2003/07/the-last-segment.html" };
            var blog = new BlogMLBlog();

            // act
            var entry = SubtextBlogMLProvider.CreateEntryFromBlogMLBlogPost(blog, post, new Dictionary<string, string>());

            // assert
            Assert.AreEqual("the-last-segment", entry.EntryName);
        }

        [Test]
        public void CreateEntryFromBlogMLBlogPost_WithNullTitleNameButWithPostUrlContainingBlogSpotDotCom_UsesLastSegmentAsTitle()
        {
            // arrange
            var post = new BlogMLPost { PostUrl = "http://example.blogspot.com/2003/07/the-last-segment.html" };
            var blog = new BlogMLBlog();

            // act
            var entry = SubtextBlogMLProvider.CreateEntryFromBlogMLBlogPost(blog, post, new Dictionary<string, string>());

            // assert
            Assert.AreEqual("the last segment", entry.Title);
        }
    }
}
