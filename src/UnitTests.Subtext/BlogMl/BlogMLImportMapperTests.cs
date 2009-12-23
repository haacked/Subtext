using System;
using System.Globalization;
using System.Linq;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Components;
using Subtext.ImportExport;
using Subtext.Extensibility;
using Subtext.Framework;
using BlogML;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogMLImportMapperTests
    {
        [Test]
        public void ConvertBlogPost_WithSyndicatedDate_ConvertsDateToBlogTimezone()
        {
            // arrange
            DateTime utcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var post = new BlogMLPost { Approved = true, DateModified = utcNow };
            var blog = new Mock<Blog>();
            DateTime expected = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            blog.Setup(b => b.TimeZone.FromUtc(utcNow)).Returns(expected);
                var mapper = new BlogMLImportMapper();

            // act
            var entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), blog.Object);

            // assert
            Assert.AreEqual(expected, entry.DateSyndicated);
        }

        [Test]
        public void ConvertBlogPost_WithApprovedPost_SetsAppropriatePublishPropertiesOfEntry()
        {
            // arrange
            var post = new BlogMLPost {Approved = true};
            var mapper = new BlogMLImportMapper();

            // act
            var entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.IsTrue(entry.IsActive);
            Assert.IsTrue(entry.DisplayOnHomePage);
            Assert.IsTrue(entry.IncludeInMainSyndication);
            Assert.IsTrue(entry.IsAggregated);
        }

        [Test]
        public void ConvertBlogPost_WithAuthorMatchingBlogAuthor_SetsAuthorNameAndEmail()
        {
            // arrange
            var blog = new BlogMLBlog();
            blog.Authors.Add(new BlogMLAuthor { ID = "111", Title = "Not-Haacked", Email = "spam-me@example.com"});
            blog.Authors.Add(new BlogMLAuthor { ID = "222", Title = "Haacked", Email = "noneofyourbusiness@example.com"});
            var post = new BlogMLPost();
            post.Authors.Add("222");
            var mapper = new BlogMLImportMapper();
            
            // act
            var entry = mapper.ConvertBlogPost(post, blog, null);

            // assert
            Assert.AreEqual("Haacked", entry.Author);
            Assert.AreEqual("noneofyourbusiness@example.com", entry.Email);
        }

        // Subtext only supports one author per post.
        [Test]
        public void ConvertBlogPost_WithPostHavingTwoAuthors_SetsAuthorAndEmailToFirstAuthor()
        {
            // arrange
            var blog = new BlogMLBlog();
            blog.Authors.Add(new BlogMLAuthor { ID = "111", Title = "Not-Haacked", Email = "spam-me@example.com" });
            blog.Authors.Add(new BlogMLAuthor { ID = "222", Title = "Haacked", Email = "noneofyourbusiness@example.com" });
            var post = new BlogMLPost();
            post.Authors.Add("111");
            post.Authors.Add("222");
            var mapper = new BlogMLImportMapper();

            // act
            var entry = mapper.ConvertBlogPost(post, blog, null);

            // assert
            Assert.AreEqual("Not-Haacked", entry.Author);
            Assert.AreEqual("spam-me@example.com", entry.Email);
        }

        [Test]
        public void ConvertBlogPost_WithPostHavingTwoCategories_AddsBothCategoriesToEntry()
        {
            // arrange
            var blog = new BlogMLBlog();
            blog.Categories.Add(new BlogMLCategory{ID="abc", Title = "Category A"});
            blog.Categories.Add(new BlogMLCategory { ID = "def", Title = "Category B" });
            blog.Categories.Add(new BlogMLCategory { ID = "#@!", Title = "Category C" });
            var post = new BlogMLPost();
            post.Categories.Add("abc");
            post.Categories.Add("#@!");
            var mapper = new BlogMLImportMapper();

            // act
            var entry = mapper.ConvertBlogPost(post, blog, null);

            // assert
            Assert.AreEqual(2, entry.Categories.Count);
            Assert.AreEqual("Category A", entry.Categories.First());
            Assert.AreEqual("Category C", entry.Categories.Last());
        }

        [Test]
        public void ConvertBlogPost_WithTitleTooLong_TruncatesTitleToMaxLength()
        {
            // arrange
            var title = new string('a', 256);
            var post = new BlogMLPost { Title = title };
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual(255, entry.Title.Length);
        }

        [Test]
        public void ConvertBlogPost_WithAuthorTitleTooLong_TruncatesTitleToMaxLength()
        {
            // arrange
            var title = new string('a', 51);
            var blog = new BlogMLBlog();
            blog.Authors.Add(new BlogMLAuthor{ID = "123", Title = title});
            var post = new BlogMLPost();
            post.Authors.Add("123");
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, blog, null);

            // assert
            Assert.AreEqual(50, entry.Author.Length);
        }

        [Test]
        public void ConvertBlogPost_WithNullPostNameButWithPostUrlContainingBlogSpotDotCom_UsesLastSegmentAsEntryName()
        {
            // arrange
            var post = new BlogMLPost { PostUrl = "http://example.blogspot.com/2003/07/the-last-segment.html" };
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("the-last-segment", entry.EntryName);
        }

        [Test]
        public void ConvertBlogPost_WithNullTitleNameButWithPostUrlContainingBlogSpotDotCom_UsesLastSegmentAsTitle()
        {
            // arrange
            var post = new BlogMLPost {PostUrl = "http://example.blogspot.com/2003/07/the-last-segment.html"};
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("the last segment", entry.Title);
        }

        [Test]
        public void ConvertBlogPost_WithPostHavingBase64EncodedContent_DecodesContent()
        {
            // arrange
            var post = new BlogMLPost { Content = BlogMLContent.Create("This is a story about a 3 hour voyage", ContentTypes.Base64) };
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("This is a story about a 3 hour voyage", entry.Body);
        }

        [Test]
        public void ConvertBlogPost_WithPostHavingBase64EncodedExcerpt_DecodesContent()
        {
            // arrange
            var post = new BlogMLPost { HasExcerpt = true, Excerpt = BlogMLContent.Create("This is a story about a 3 hour voyage", ContentTypes.Base64) };
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("This is a story about a 3 hour voyage", entry.Description);
        }

        [Test]
        public void ConvertBlogPost_WithPostHavingExcerpt_SetsEntryDescription()
        {
            // arrange
            var post = new BlogMLPost{ HasExcerpt = true, Excerpt = new BlogMLContent {Text = "This is a story about a 3 hour voyage"}};
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("This is a story about a 3 hour voyage", entry.Description);
        }

        [Test]
        public void ConvertBlogPost_WithPostHavingNoTitleAndNoPostName_UsesPostId()
        {
            // arrange
            var post = new BlogMLPost { Title = null, PostName = null, ID = "87618298" };
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("Post #87618298", entry.Title);
        }

        [Test]
        public void GetTitleFromEntry_WithPostHavingNoTitle_CreatesUsesPostNameIfAvailable()
        {
            // arrange
            var post = new BlogMLPost { Title = null, PostName = "Hello World" };
            var mapper = new BlogMLImportMapper();

            // act
            Entry entry = mapper.ConvertBlogPost(post, new BlogMLBlog(), null);

            // assert
            Assert.AreEqual("Hello World", entry.Title);
        }

        [Test]
        public void ConvertCategory_WithTitleTooLong_TruncatesTitleToMaxLength()
        {
            // arrange
            var title = new string('a', 151);
            var category = new BlogMLCategory { Title = title };
            var mapper = new BlogMLImportMapper();

            // act
            LinkCategory linkCategory = mapper.ConvertCategory(category);

            // assert
            Assert.AreEqual(150, linkCategory.Title.Length);
        }

        [Test]
        public void ConvertComment_ReturnsFeedbackItemAsComment()
        {
            // arrange
            var comment = new BlogMLComment { UserUrl = "not-valid-url" };
            var mapper = new BlogMLImportMapper();

            // act
            var convertComment = mapper.ConvertComment(comment, "123");

            // assert
            Assert.AreEqual(FeedbackType.Comment, convertComment.FeedbackType);
        }

        [Test]
        public void ConvertComment_WithUnapprovedComment_SetsFeedbackToTrash()
        {
            // arrange
            var comment = new BlogMLComment { UserUrl = "not-valid-url", Approved = false};
            var mapper = new BlogMLImportMapper();

            // act
            var convertComment = mapper.ConvertComment(comment, "123");

            // assert
            Assert.IsFalse(convertComment.Approved);
            Assert.AreEqual(FeedbackStatusFlag.NeedsModeration, convertComment.Status);
        }

        [Test]
        public void ConvertComment_WithInvalidUserUrl_IgnoresUrl()
        {
            // arrange
            var comment = new BlogMLComment { UserUrl= "not-valid-url" };
            var mapper = new BlogMLImportMapper();

            // act
            var convertComment = mapper.ConvertComment(comment, "123");

            // assert
            Assert.AreEqual(null, convertComment.SourceUrl);
        }

        [Test]
        public void ConvertTrackback_ReturnsFeedbackItemAsPingTrack()
        {
            // arrange
            var trackback = new BlogMLTrackback();
            var mapper = new BlogMLImportMapper();

            // act
            var convertedTrackback = mapper.ConvertTrackback(trackback, "123");

            // assert
            Assert.AreEqual(FeedbackType.PingTrack, convertedTrackback.FeedbackType);
        }

        [Test]
        public void ConvertTrackback_WithInvalidSourceUrl_IgnoresUrl()
        {
            // arrange
            var trackback = new BlogMLTrackback {Url = "not-valid-url"};
            var mapper = new BlogMLImportMapper();

            // act
            var convertedTrackback = mapper.ConvertTrackback(trackback, "123");

            // assert
            Assert.AreEqual(null, convertedTrackback.SourceUrl);
        }

        [Test]
        public void ConvertTrackback_WithValidSourceUrl_SetsUrlAndAuthorUsingHostname()
        {
            // arrange
            var trackback = new BlogMLTrackback { Url = "http://example.com/valid-url" };
            var mapper = new BlogMLImportMapper();

            // act
            var convertedTrackback = mapper.ConvertTrackback(trackback, "123");

            // assert
            Assert.AreEqual(new Uri("http://example.com/valid-url"), convertedTrackback.SourceUrl);
            Assert.AreEqual("example.com", convertedTrackback.Author);
        }
    }
}
