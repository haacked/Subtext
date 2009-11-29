using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BlogML;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogMLExportMapperTests
    {
        [Test]
        public void ConvertBlog_WithSubtextBlog_ReturnsCorrespondingBlogMLBlog()
        {
            // arrange
            var blog = new Blog {Title = "Test Blog Title", SubTitle = "Test Blog Subtitle", Author = "Charles Dickens", Host = "example.com", ModerationEnabled = true};
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var blogMLBlog = converter.ConvertBlog(blog);

            // assert
            Assert.AreEqual("Test Blog Title", blogMLBlog.Title);
            Assert.AreEqual("Test Blog Subtitle", blogMLBlog.SubTitle);
            Assert.AreEqual("http://example.com/", blogMLBlog.RootUrl);
            Assert.AreEqual("Charles Dickens", blogMLBlog.Authors[0].Title);
            Assert.AreEqual(BlogMLBlogExtendedProperties.CommentModeration, blogMLBlog.ExtendedProperties[0].Key);
            Assert.AreEqual("Enabled", blogMLBlog.ExtendedProperties[0].Value);
        }

        [Test]
        public void ConvertCategories_WithBlogCategories_ConvertsToBLogMLCategories()
        {
            // arrange
            var categories = new List<LinkCategory> {new LinkCategory(1, "Category Uno"), new LinkCategory(2, "Category Dos")};
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "example.com" });
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var blogMLCategories = converter.ConvertCategories(categories);

            // assert
            Assert.AreEqual(2, blogMLCategories.Count());
            Assert.AreEqual("Category Uno", blogMLCategories.First().Title);
            Assert.AreEqual("Category Dos", blogMLCategories.ElementAt(1).Title);
        }

        [Test]
        public void ConvertEntry_WithEntry_ConvertsToBLogMLPosts()
        {
            // arrange
            var entry = new EntryStatsView{ Title = "Test Entry"};
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "example.com" });
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual("Test Entry", post.Title);
        }

        [Test]
        public void ConvertEntry_WithEntry_ContvertsBodyToBase64Encoding()
        {
            // arrange
            var entry = new EntryStatsView { Body = "<style><![CDATA[Test]]></style>" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "example.com" });
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual("<style><![CDATA[Test]]></style>", post.Content.UncodedText);
            Assert.AreEqual(true, post.Content.Base64);
        }

        [Test]
        public void ConvertEntry_WithInActiveEntry_SetsDateModifiedToDateModified()
        {
            // arrange
            DateTime dateModified = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateModifiedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var entry = new EntryStatsView { Title = "Test Entry", DateModified = dateModified, IsActive = false };
            var subtextContext = new Mock<ISubtextContext>();
            var blog = new Mock<Blog>();
            blog.Object.Host = "example.com";
            blog.Setup(b => b.TimeZone.ToUtc(dateModified)).Returns(dateModifiedUtc);
            subtextContext.Setup(c => c.Blog).Returns(blog.Object);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual(dateModifiedUtc, post.DateModified);
        }

        [Test]
        public void ConvertEntry_WithActiveEntry_SetsDateModifiedToDateSyndicated()
        {
            // arrange
            DateTime dateSyndicated = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateSyndicatedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var entry = new EntryStatsView { Title = "Test Entry", DateSyndicated = dateSyndicated, IsActive = true};
            var subtextContext = new Mock<ISubtextContext>();
            var blog = new Mock<Blog>();
            blog.Object.Host = "example.com";
            blog.Setup(b => b.TimeZone.ToUtc(dateSyndicated)).Returns(dateSyndicatedUtc);
            subtextContext.Setup(c => c.Blog).Returns(blog.Object);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual(dateSyndicatedUtc, post.DateModified);
        }

        [Test]
        public void ConvertEntry_WithActiveEntry_SetsDateCreatedToLocalDateTime()
        {
            // arrange
            DateTime dateCreated = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateCreatedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var entry = new EntryStatsView { Title = "Test Entry", DateCreated = dateCreated, IsActive = true };
            var subtextContext = new Mock<ISubtextContext>();
            var blog = new Mock<Blog>();
            blog.Object.Host = "example.com";
            blog.Setup(b => b.TimeZone.ToUtc(dateCreated)).Returns(dateCreatedUtc);
            subtextContext.Setup(c => c.Blog).Returns(blog.Object);
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/irrelevant");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual(dateCreatedUtc, post.DateCreated);
        }

        [Test]
        public void ConvertEntry_WithEntryHavingEntryName_ConvertsToBLogMLPostWithPostName()
        {
            // arrange
            var entry = new EntryStatsView { EntryName = "My-Cool-Post" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "example.com" });
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/My-Cool-Post");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual("My-Cool-Post", post.PostName);
        }

        [Test]
        public void ConvertEntry_WithEntry_ConvertsToBLogMLPostWithPostUrl()
        {
            // arrange
            var entry = new EntryStatsView { EntryName = "my-cool-post" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "foo.example.com" });
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/my-cool-post.aspx");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual("http://foo.example.com/my-cool-post.aspx", post.PostUrl);
        }

        [Test]
        public void ConvertEntry_WithEntryHavingAuthor_ConvertsToBLogMLPostWithAuthorReference()
        {
            // arrange
            var entry = new EntryStatsView { EntryName = "my-cool-post" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id=321, Host = "foo.example.com" });
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/my-cool-post.aspx");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual("321", post.Authors[0].Ref);
        }

        [Test]
        public void ConvertEntry_WithEntryHavingAttachments_IncludesAttachmentsWithoutEmbedding()
        {
            // arrange
            var entry = new EntryStatsView { EntryName = "my-cool-post", Body = @"<div><img src=""/my-dogs.jpg"" />" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 321, Host = "foo.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/my-cool-post.aspx");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual(1, post.Attachments.Count);
            var attachment = post.Attachments[0];
            Assert.IsFalse(attachment.Embedded);
            Assert.AreEqual("/my-dogs.jpg", attachment.Url);
        }

        [Test]
        public void ConvertEntry_WithEntryHavingFullyQualifiedImage_IgnoresImage()
        {
            // arrange
            var entry = new EntryStatsView { EntryName = "my-cool-post", Body = @"<div><img src=""http://example.com/my-dogs.jpg"" />" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 321, Host = "foo.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/my-cool-post.aspx");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual(0, post.Attachments.Count);
        }

        [Test]
        public void ConvertEntry_WithEntryHavingAttachments_EmbedsAttachmentsWhenEmbedIsTrue()
        {
            // arrange
            var entry = new EntryStatsView { EntryName = "my-cool-post", Body = @"<div><img src=""/my-dogs.jpg"" />" };
            var subtextContext = new Mock<ISubtextContext>();
            string filePath = UnitTestHelper.UnpackEmbeddedBinaryResource("BlogMl.blank.gif", "blank.gif");
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 321, Host = "foo.example.com" });
            subtextContext.Setup(c => c.HttpContext.Server.MapPath("/my-dogs.jpg")).Returns(filePath);
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/my-cool-post.aspx");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, true /*embedAttachments*/);

            // assert
            Assert.AreEqual(1, post.Attachments.Count);
            var attachment = post.Attachments[0];
            Assert.IsTrue(attachment.Embedded);
            Assert.AreEqual("/my-dogs.jpg", attachment.Url);
            Assert.AreEqual("/my-dogs.jpg", attachment.Path);
            Assert.AreEqual("R0lGODlhAQABAID/AMDAwAAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==", Convert.ToBase64String(attachment.Data));
        }

        [Test]
        public void ConvertEntry_WithEntryHavingCommentsAndTrackbacks_IncludesCommentsAndTrackbacks()
        {
            // arrange
            var entry = new EntryStatsView();
            entry.Comments.AddRange(new[] { new FeedbackItem(FeedbackType.Comment), new FeedbackItem(FeedbackType.Comment), new FeedbackItem(FeedbackType.PingTrack) });
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 321, Host = "foo.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.BlogUrl()).Returns("/");
            subtextContext.Setup(c => c.UrlHelper.EntryUrl(It.IsAny<IEntryIdentity>())).Returns("/my-cool-post.aspx");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var post = converter.ConvertEntry(entry, false /*embedAttachments*/);

            // assert
            Assert.AreEqual(2, post.Comments.Count);
            Assert.AreEqual(1, post.Trackbacks.Count);
        }

        [Test]
        public void ConvertComment_WithFeedBackItem_ConvertsToBlogMlComment()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.Comment) {
                Id = 213,
                Title = "Comment Title", 
                Approved = true, 
                Author = "Anonymous Troll",
                Email = "test@example.com",
                SourceUrl = new Uri("http://subtextproject.com/"),
                Body = "<p>First!</p>",
            };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog());
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var comment = converter.ConvertComment(feedback);

            // assert
            Assert.AreEqual("213", comment.ID);
            Assert.AreEqual("Comment Title", comment.Title);
            Assert.IsTrue(comment.Approved);
            Assert.AreEqual("Anonymous Troll", comment.UserName);
            Assert.AreEqual("test@example.com", comment.UserEMail);
            Assert.AreEqual("http://subtextproject.com/", comment.UserUrl);
            Assert.AreEqual("<p>First!</p>", comment.Content.Text);
        }

        [Test]
        public void ConvertComment_WithDateCreated_ConvertsToUtc()
        {
            // arrange
            DateTime dateCreated = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateCreatedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var feedback = new FeedbackItem(FeedbackType.Comment)
            {
                DateCreated = dateCreated,
            };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog.TimeZone.ToUtc(dateCreated)).Returns(dateCreatedUtc);
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var comment = converter.ConvertComment(feedback);

            // assert
            Assert.AreEqual(dateCreatedUtc, comment.DateCreated);
        }

        [Test]
        public void ConvertComment_WithDateModified_ConvertsToUtc()
        {
            // arrange
            DateTime dateModified = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateModifiedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var feedback = new FeedbackItem(FeedbackType.Comment)
            {
                DateModified = dateModified,
            };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog.TimeZone.ToUtc(dateModified)).Returns(dateModifiedUtc);
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var comment = converter.ConvertComment(feedback);

            // assert
            Assert.AreEqual(dateModifiedUtc, comment.DateModified);
        }

        [Test]
        public void ConvertComment_WithNullFeedBackItem_ThrowsArgumentNullException()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => converter.ConvertComment(null));
        }

        [Test]
        public void ConvertComment_WithFeedBackItemHavingNonCommentFeedbackType_ThrowsArgumentException()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.PingTrack);
            var subtextContext = new Mock<ISubtextContext>();
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => converter.ConvertComment(feedback));
        }

        [Test]
        public void ConvertTrackback_WithDateCreated_ConvertsDateCreatedToUtc()
        {
            // arrange
            DateTime dateCreated = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateCreatedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            var feedback = new FeedbackItem(FeedbackType.PingTrack)
            {
                Id = 213,
                Title = "Comment Title",
                Approved = true,
                Author = "Anonymous Troll",
                Email = "test@example.com",
                SourceUrl = new Uri("http://subtextproject.com/"),
                Body = "<p>First!</p>",
                DateCreated = dateCreated,
                DateModified = dateCreated
            };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog.TimeZone.ToUtc(dateCreated)).Returns(dateCreatedUtc);
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var comment = converter.ConvertTrackback(feedback);

            // assert
            Assert.AreEqual(dateCreatedUtc, comment.DateCreated);
        }

        [Test]
        public void ConvertTrackback_WithDateModified_ConvertsDateModifiedToUtc()
        {
            // arrange
            DateTime dateModified = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime dateModifiedUtc = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            
            var dateCreated = DateTime.Now.AddDays(-1);
            var feedback = new FeedbackItem(FeedbackType.PingTrack)
            {
                Id = 213,
                Title = "Comment Title",
                Approved = true,
                Author = "Anonymous Troll",
                Email = "test@example.com",
                SourceUrl = new Uri("http://subtextproject.com/"),
                Body = "<p>First!</p>",
                DateCreated = dateCreated,
                DateModified = dateModified
            };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog.TimeZone.ToUtc(dateModified)).Returns(dateModifiedUtc);
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var comment = converter.ConvertTrackback(feedback);

            // assert
            Assert.AreEqual(dateModifiedUtc, comment.DateModified);
        }

        [Test]
        public void ConvertTrackback_WithFeedBackItem_ConvertsToBlogMlComment()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.PingTrack)
            {
                Id = 213,
                Title = "Comment Title",
                Approved = true,
                Author = "Anonymous Troll",
                Email = "test@example.com",
                SourceUrl = new Uri("http://subtextproject.com/"),
                Body = "<p>First!</p>",
            };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog());
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var comment = converter.ConvertTrackback(feedback);

            // assert
            Assert.AreEqual("213", comment.ID);
            Assert.AreEqual("Comment Title", comment.Title);
            Assert.IsTrue(comment.Approved);
            Assert.AreEqual("http://subtextproject.com/", comment.Url);
        }

        [Test]
        public void ConvertTrackback_WithNullFeedBackItem_ThrowsArgumentNullException()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => converter.ConvertTrackback(null));
        }

        [Test]
        public void ConvertTrackback_WithFeedBackItemHavingNonTrackbackFeedbackType_ThrowsArgumentException()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.Comment);
            var subtextContext = new Mock<ISubtextContext>();
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => converter.ConvertTrackback(feedback));
        }

        [Test]
        public void GetPostAttachments_WithFullyQualifiedImageSrcWithHostSameAsBlog_ReturnsAttachment()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Host = "test.example.com"});
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var attachments = converter.GetPostAttachments(@"<em>Test <img src=""http://test.example.com/images/foo.jpg"" /></em>", false);

            // assert
            Assert.AreEqual(1, attachments.Count());
            Assert.AreEqual("/images/foo.jpg", attachments.First().Path);
            Assert.AreEqual("http://test.example.com/images/foo.jpg", attachments.First().Url);
        }

        [Test]
        public void GetPostAttachments_WithBlogInVirtualAppFullyQualifiedImageSrcWithHostSameAsBlog_ReturnsAttachment()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "test.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/Subtext.Web/");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var attachments = converter.GetPostAttachments(@"<em>Test <img src=""http://test.example.com/subtext.web/images/foo.jpg"" /></em>", false);

            // assert
            Assert.AreEqual(1, attachments.Count());
            Assert.AreEqual("/subtext.web/images/foo.jpg", attachments.First().Path);
            Assert.AreEqual("http://test.example.com/subtext.web/images/foo.jpg", attachments.First().Url);
        }

        [Test]
        public void GetPostAttachments_WithImageSrcAsVirtualPath_ReturnsAttachment()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "test.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var attachments = converter.GetPostAttachments(@"<em>Test <img src=""/images/foo.jpg"" /></em>", false);

            // assert
            Assert.AreEqual(1, attachments.Count());
            Assert.AreEqual("/images/foo.jpg", attachments.First().Path);
            Assert.AreEqual("/images/foo.jpg", attachments.First().Url);
        }

        [Test]
        public void GetPostAttachments_WithBlogInVirtualApplicaitonImageSrcAsVirtualPath_ReturnsAttachment()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "test.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/Subtext.Web/");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var attachments = converter.GetPostAttachments(@"<em>Test <img src=""/Subtext.Web/images/foo.jpg"" /></em>", false);

            // assert
            Assert.AreEqual(1, attachments.Count());
            Assert.AreEqual("/subtext.web/images/foo.jpg", attachments.First().Path);
            Assert.AreEqual("/Subtext.Web/images/foo.jpg", attachments.First().Url);
        }

        [Test]
        public void GetPostAttachments_WithImageSrcAsRelativePath_ReturnsAttachment()
        {
            // arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Host = "test.example.com" });
            subtextContext.Setup(c => c.UrlHelper.AppRoot()).Returns("/");
            var converter = new BlogMLExportMapper(subtextContext.Object);

            // act
            var attachments = converter.GetPostAttachments(@"<em>Test <img src=""foo.jpg"" /></em>", false);

            // assert
            Assert.AreEqual(1, attachments.Count());
            Assert.AreEqual("foo.jpg", attachments.First().Path);
            Assert.AreEqual("foo.jpg", attachments.First().Url);
        }
    }
}