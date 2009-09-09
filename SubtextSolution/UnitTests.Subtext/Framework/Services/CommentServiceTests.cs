using System;
using System.Collections;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        //TODO This test is RIDICULOUS! We need to refactor some code.
        [Test]
        [Ignore("Need to refactor")]
        public void CreateSetsDateCreatedToBlogTime()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.Now;
            blog.Object.Id = 1;
            blog.Setup(b => b.TimeZone.Now).Returns(dateCreated);
            var entry = new Entry(PostType.BlogPost, blog.Object) {Id = 123, BlogId = 1, CommentingClosed = false};
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);
            var context = new Mock<ISubtextContext>();
            context.SetupGet(c => c.Repository).Returns(repository.Object);
            context.SetupGet(c => c.Blog).Returns(blog.Object);
            context.SetupGet(c => c.HttpContext.Items).Returns(new Hashtable());
            context.SetupGet(c => c.Cache).Returns(new TestCache());

            var service = new CommentService(context.Object, null);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {EntryId = 123, BlogId = 1, Body = "test", Title = "title"};

            //act
            service.Create(comment);

            //assert
            Assert.AreEqual(dateCreated, comment.DateCreated);
        }

        [Test]
        public void CreateDoesNotChangeDateCreatedAndDateModifiedIfAlreadySpecified()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.Now;
            blog.Object.Id = 1;
            blog.Setup(b => b.TimeZone.Now).Returns(dateCreated);
            var entry = new Entry(PostType.BlogPost, blog.Object) {Id = 123, BlogId = 1, CommentingClosed = false};
            var repository = new Mock<ObjectProvider>();
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
                DateCreated = dateCreated.AddDays(-2),
                DateModified = dateCreated.AddDays(-1)
            };

            //act
            service.Create(comment);

            //assert
            Assert.AreEqual(dateCreated.AddDays(-2), comment.DateCreated);
            Assert.AreEqual(dateCreated.AddDays(-1), comment.DateModified);
        }

        [Test]
        public void CreateCallsIntoCommentService()
        {
            //arrange
            var blog = new Mock<Blog>();
            DateTime dateCreated = DateTime.Now;
            blog.Object.Id = 1;
            blog.Setup(b => b.TimeZone.Now).Returns(dateCreated);
            var entry = new Entry(PostType.BlogPost, blog.Object) {Id = 123, BlogId = 1, CommentingClosed = false};
            var repository = new Mock<ObjectProvider>();
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
                DateCreated = dateCreated.AddDays(-2),
                DateModified = dateCreated.AddDays(-1)
            };

            //act
            service.Create(comment);

            //assert
            Assert.IsTrue(wasBeforeCalled);
            Assert.IsTrue(wasAfterCalled);
        }
    }
}