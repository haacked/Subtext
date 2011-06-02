using System;
using System.Collections.Generic;
using BlogML;
using BlogML.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Services.SearchEngine;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogImportRepositoryTests
    {
        [Test]
        public void CreateCategories_WithBlogHavingCategories_CreatesCategories()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 123 });
            bool categoryCreated = false;
            context.Setup(c => c.Repository.CreateLinkCategory(It.IsAny<LinkCategory>())).Callback(() => categoryCreated = true);
            var blog = new BlogMLBlog();
            blog.Categories.Add(new BlogMLCategory { Title = "Category Title", ID = "123" });
            var repository = new BlogImportRepository(context.Object, null, null, new BlogMLImportMapper());

            // act
            repository.CreateCategories(blog);

            // assert
            Assert.IsTrue(categoryCreated);
        }

        [Test]
        public void CreateCategories_WithBlogHavingNoCategories_DoesNotCreateCategories()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 123 });
            context.Setup(c => c.Repository.CreateLinkCategory(It.IsAny<LinkCategory>())).Throws(new InvalidOperationException());
            var blog = new BlogMLBlog();
            var repository = new BlogImportRepository(context.Object, null, null, new BlogMLImportMapper());

            // act, assert
            repository.CreateCategories(blog);
        }

        [Test]
        public void CreateBlogPost_WithEntryPublisher_PublishesBlogPostAndReturnsId()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog());
            var entryPublisher = new Mock<IEntryPublisher>();
            entryPublisher.Setup(p => p.Publish(It.IsAny<Entry>())).Returns(310);
            var blog = new BlogMLBlog();
            var post = new BlogMLPost();
            var repository = new BlogImportRepository(context.Object, null, entryPublisher.Object, new BlogMLImportMapper());

            // act
            var id = repository.CreateBlogPost(blog, post);

            // assert
            Assert.AreEqual("310", id);
        }

        [Test]
        public void CreateBlogPost_WithEntryPublisher_RemovesKeywordExpander()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog());
            context.Setup(c => c.Repository.Create(It.IsAny<Entry>(), It.IsAny<IEnumerable<int>>()));
            var transformation = new CompositeTextTransformation();
            var searchengine = new Mock<IIndexingService>();
            var entryPublisher = new EntryPublisher(context.Object, transformation, null, searchengine.Object);
            var keywordExpander = new KeywordExpander((IEnumerable<KeyWord>)null);
            transformation.Add(keywordExpander);
            var blog = new BlogMLBlog() { Title = "MyBlog" };
            var post = new BlogMLPost();
            var repository = new BlogImportRepository(context.Object, null, entryPublisher, new BlogMLImportMapper());

            // act
            repository.CreateBlogPost(blog, post);

            // assert
            Assert.IsFalse(transformation.Contains(keywordExpander));
        }

        [Test]
        public void CreateComment_WithComment_CreatesCommentUsingCommentService()
        {
            // arrange
            var commentService = new Mock<ICommentService>();
            bool commentCreated = false;
            commentService.Setup(s => s.Create(It.IsAny<FeedbackItem>(), false/*runFilters*/)).Callback(() => commentCreated = true);

            var repository = new BlogImportRepository(null, commentService.Object, null, new BlogMLImportMapper());

            // act
            repository.CreateComment(new BlogMLComment(), "123");

            // assert
            Assert.IsTrue(commentCreated);
        }

        [Test]
        public void GetAttachmentDirectoryPath_WithAttachment_CreatesTrackbackUsingTrackbackService()
        {
            // arrange
            var commentService = new Mock<ICommentService>();
            bool trackbackCreated = false;
            commentService.Setup(s => s.Create(It.IsAny<FeedbackItem>(), It.IsAny<bool>())).Callback(() => trackbackCreated = true);

            var repository = new BlogImportRepository(null, commentService.Object, null, new BlogMLImportMapper());

            // act
            repository.CreateTrackback(new BlogMLTrackback(), "123");

            // assert
            Assert.IsTrue(trackbackCreated);
        }

        [Test]
        public void CreateTrackback_WithTrackback_CreatesTrackbackUsingTrackbackService()
        {
            // arrange
            var commentService = new Mock<ICommentService>();
            bool trackbackCreated = false;
            commentService.Setup(s => s.Create(It.IsAny<FeedbackItem>(), It.IsAny<bool>())).Callback(() => trackbackCreated = true);
            var repository = new BlogImportRepository(null, commentService.Object, null, new BlogMLImportMapper());

            // act
            repository.CreateTrackback(new BlogMLTrackback(), "123");

            // assert
            Assert.IsTrue(trackbackCreated);
        }

        [Test]
        public void GetAttachmentDirectoryPath_DelegatesToUrlHelperForPath()
        {
            // arrange
            var blog = new Blog();
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog);
            context.Setup(c => c.UrlHelper.ImageDirectoryPath(blog)).Returns(@"c:\web\images");
            var repository = new BlogImportRepository(context.Object, null, null, null);

            // act
            var path = repository.GetAttachmentDirectoryPath();

            // assert
            Assert.AreEqual(@"c:\web\images", path);
        }

        [Test]
        public void GetAttachmentDirectoryUrl_DelegatesToUrlHelperForUrl()
        {
            // arrange
            var blog = new Blog();
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog);
            context.Setup(c => c.UrlHelper.ImageDirectoryUrl(blog)).Returns("/test");
            var repository = new BlogImportRepository(context.Object, null, null, null);

            // act
            var path = repository.GetAttachmentDirectoryUrl();

            // assert
            Assert.AreEqual(@"/test", path);
        }

        [Test]
        public void SetupBlogForImport_ReturnsBlogMLScope()
        {
            // arrange
            var blog = new Blog();
            var repository = new Mock<ObjectProvider>();
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            context.Setup(c => c.Blog).Returns(blog);
            var importRepository = new BlogImportRepository(context.Object, null, null, null);

            // act
            var scope = importRepository.SetupBlogForImport() as BlogImportSetup;

            // assert
            Assert.IsNotNull(scope);
            Assert.AreEqual(blog, scope.Blog);
            Assert.AreEqual(repository.Object, scope.Repository);
        }

        [Test]
        public void SetExtendedProperties_WithKeyForCommentModeration_EnablesModeration()
        {
            // arrange
            var extendedProperties = new BlogMLBlog.ExtendedPropertiesCollection { new Pair<string, string>(BlogMLBlogExtendedProperties.CommentModeration, "true") };
            var context = new Mock<ISubtextContext>();
            var blog = new Blog { ModerationEnabled = false };
            context.Setup(c => c.Blog).Returns(blog);
            bool blogUpdated = false;
            context.Setup(c => c.Repository.UpdateBlog(blog)).Callback(() => blogUpdated = true);
            var repository = new BlogImportRepository(context.Object, null, null, null);

            // act
            repository.SetExtendedProperties(extendedProperties);

            // assert
            Assert.IsTrue(blogUpdated);
            Assert.IsTrue(blog.ModerationEnabled);
        }

        [Test]
        public void SetExtendedProperties_WithKeyForTrackbacksEnabled_EnablesTrackbacks()
        {
            // arrange
            var extendedProperties = new BlogMLBlog.ExtendedPropertiesCollection { new Pair<string, string>(BlogMLBlogExtendedProperties.EnableSendingTrackbacks, "true") };
            var context = new Mock<ISubtextContext>();
            var blog = new Blog { TrackbacksEnabled = false };
            context.Setup(c => c.Blog).Returns(blog);
            bool blogUpdated = false;
            context.Setup(c => c.Repository.UpdateBlog(blog)).Callback(() => blogUpdated = true);
            var repository = new BlogImportRepository(context.Object, null, null, null);

            // act
            repository.SetExtendedProperties(extendedProperties);

            // assert
            Assert.IsTrue(blogUpdated);
            Assert.IsTrue(blog.TrackbacksEnabled);
        }
    }
}
