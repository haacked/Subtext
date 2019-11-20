using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestClass]
    public class CommentServiceTests
    {
        [TestMethod]
        public void CreateSetsDateCreated()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreatedUtc = DateTime.UtcNow;
            blog.Object.Id = 1;
            var entry = new Entry(PostType.BlogPost, blog.Object) { Id = 123, BlogId = 1, CommentingClosed = false };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);
            var context = new Mock<ISubtextContext>();
            context.SetupGet(c => c.Repository).Returns(repository.Object);
            context.SetupGet(c => c.Blog).Returns(blog.Object);
            context.SetupGet(c => c.HttpContext.Items).Returns(new Hashtable());
            context.SetupGet(c => c.Cache).Returns(new TestCache());

            var service = new CommentService(context.Object, null);
            var comment = new FeedbackItem(FeedbackType.Comment) { EntryId = 123, BlogId = 1, Body = "test", Title = "title" };

            //act
            service.Create(comment, true/*runFilters*/);

            //assert
            Assert.IsTrue(comment.DateCreatedUtc >= dateCreatedUtc);
            Assert.IsTrue(DateTime.UtcNow >= comment.DateCreatedUtc);
        }

        [TestMethod]
        public void CreateDoesNotChangeDateCreatedAndDateModifiedIfAlreadySpecified()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.UtcNow;
            blog.Object.Id = 1;
            var entry = new Entry(PostType.BlogPost, blog.Object) { Id = 123, BlogId = 1, CommentingClosed = false };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);
            var context = new Mock<ISubtextContext>();
            context.SetupGet(c => c.Repository).Returns(repository.Object);
            context.SetupGet(c => c.Blog).Returns(blog.Object);
            context.SetupGet(c => c.HttpContext.Items).Returns(new Hashtable());
            context.SetupGet(c => c.Cache).Returns(new TestCache());

            var service = new CommentService(context.Object, null);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {
                EntryId = 123,
                BlogId = 1,
                Body = "test",
                Title = "title",
                DateCreatedUtc = dateCreated.AddDays(-2),
                DateModifiedUtc = dateCreated.AddDays(-1)
            };

            //act
            service.Create(comment, true/*runFilters*/);

            //assert
            Assert.AreEqual(dateCreated.AddDays(-2), comment.DateCreatedUtc);
            Assert.AreEqual(dateCreated.AddDays(-1), comment.DateModifiedUtc);
        }

        [TestMethod]
        public void Create_WithFilters_CallsIntoCommentFilters()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.UtcNow;
            blog.Object.Id = 1;
            var entry = new Entry(PostType.BlogPost, blog.Object) { Id = 123, BlogId = 1, CommentingClosed = false };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);
            var context = new Mock<ISubtextContext>();
            context.SetupGet(c => c.Repository).Returns(repository.Object);
            context.SetupGet(c => c.Blog).Returns(blog.Object);
            context.SetupGet(c => c.HttpContext.Items).Returns(new Hashtable());
            context.SetupGet(c => c.Cache).Returns(new TestCache());

            var commentFilter = new Mock<ICommentFilter>();
            bool wasBeforeCalled = false;
            bool wasAfterCalled = false;
            commentFilter.Setup(f => f.FilterBeforePersist(It.IsAny<FeedbackItem>())).Callback(
                () => wasBeforeCalled = true);
            commentFilter.Setup(f => f.FilterAfterPersist(It.IsAny<FeedbackItem>())).Callback(
                () => wasAfterCalled = true);
            var service = new CommentService(context.Object, commentFilter.Object);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {
                EntryId = 123,
                BlogId = 1,
                Body = "test",
                Title = "title",
                DateCreatedUtc = dateCreated.AddDays(-2),
                DateModifiedUtc = dateCreated.AddDays(-1)
            };

            //act
            service.Create(comment, true /*runFilters*/);

            //assert
            Assert.IsTrue(wasBeforeCalled);
            Assert.IsTrue(wasAfterCalled);
            Assert.IsTrue(comment.FlaggedAsSpam);
        }

        [TestMethod]
        public void Create_ForEntry_SetsEntryPropertyBeforeCallingFilters()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.UtcNow;
            blog.Object.Id = 1;
            var entry = new Entry(PostType.BlogPost, blog.Object) { Id = 123, BlogId = 1, CommentingClosed = false };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);
            var context = new Mock<ISubtextContext>();
            context.SetupGet(c => c.Repository).Returns(repository.Object);
            context.SetupGet(c => c.Blog).Returns(blog.Object);
            context.SetupGet(c => c.HttpContext.Items).Returns(new Hashtable());
            context.SetupGet(c => c.Cache).Returns(new TestCache());

            var commentFilter = new Mock<ICommentFilter>();
            FeedbackItem feedback = null;
            commentFilter.Setup(f => f.FilterBeforePersist(It.IsAny<FeedbackItem>())).Callback<FeedbackItem>(fb => feedback = fb);
            var service = new CommentService(context.Object, commentFilter.Object);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {
                EntryId = 123,
                BlogId = 1,
                Body = "test",
                Title = "title",
                DateCreatedUtc = dateCreated.AddDays(-2),
                DateModifiedUtc = dateCreated.AddDays(-1)
            };

            //act
            service.Create(comment, true /*runFilters*/);

            //assert
            Assert.AreEqual(entry, feedback.Entry);
        }


        [TestMethod]
        public void Create_WithRunFiltersFalse_DoesNotSetFlaggedSpamToTrue()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.UtcNow;
            blog.Object.Id = 1;
            var entry = new Entry(PostType.BlogPost, blog.Object) { Id = 123, BlogId = 1, CommentingClosed = false };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);
            var context = new Mock<ISubtextContext>();
            context.SetupGet(c => c.Repository).Returns(repository.Object);
            context.SetupGet(c => c.Blog).Returns(blog.Object);
            context.SetupGet(c => c.HttpContext.Items).Returns(new Hashtable());
            context.SetupGet(c => c.Cache).Returns(new TestCache());

            var service = new CommentService(context.Object, null);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {
                EntryId = 123,
                BlogId = 1,
                Body = "test",
                Title = "title",
                DateCreatedUtc = dateCreated.AddDays(-2),
                DateModifiedUtc = dateCreated.AddDays(-1)
            };

            //act
            service.Create(comment, false /*runFilters*/);

            //assert
            Assert.IsFalse(comment.FlaggedAsSpam);
        }

        [TestMethod]
        public void UpdateStatus_WithDeletedFlag_SetsDeleted()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            var feedback = new FeedbackItem(FeedbackType.Comment) { Approved = true, Deleted = false };
            context.Setup(c => c.Repository.GetFeedback(112)).Returns(feedback);
            var service = new CommentService(context.Object, null);

            // act
            service.UpdateStatus(feedback, FeedbackStatusFlag.Deleted);

            // assert
            Assert.IsTrue(feedback.Deleted);
        }

        [TestMethod]
        public void Destroy_DestroysTheFeedback()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            var feedback = new FeedbackItem(FeedbackType.Comment) { Approved = true, Deleted = false };
            context.Setup(c => c.Repository.GetFeedback(112)).Returns(feedback);
            context.Setup(c => c.Repository.DestroyFeedback(123));
            var service = new CommentService(context.Object, null);

            // act
            service.Destroy(123);

            // assert
            context.Verify(c => c.Repository.DestroyFeedback(123));
        }
    }
}