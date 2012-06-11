using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Services;
using Subtext.Web.Controllers;

namespace UnitTests.Subtext.SubtextWeb.Controllers.Admin
{
    [TestFixture]
    public class EntryControllerTests
    {
        [Test]
        public void Delete_WithEntryId_CallsDeleteEntryOnRepository()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.Comment) { Id = 123, Author = "Bugs" };
            var service = new Mock<ICommentService>();
            service.Setup(s => s.Get(123)).Returns(feedback);
            service.Setup(s => s.UpdateStatus(feedback, FeedbackStatusFlag.Deleted));
            var controller = new CommentController(service.Object);

            // act
            var result = controller.UpdateStatus(123, FeedbackStatusFlag.Deleted) as JsonResult;

            // assert
            service.Verify(c => c.UpdateStatus(feedback, FeedbackStatusFlag.Deleted));
            var data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("Comment by Bugs", data["subject"]);
            Assert.AreEqual("has been removed", data["predicate"]);
        }

        [Test]
        public void Destroy_WithEntryId_CallsDestroyFeedbackOnRepository()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.Comment) { Id = 123, Author = "Calvin"};
            var service = new Mock<ICommentService>();
            service.Setup(s => s.Get(123)).Returns(feedback);
            service.Setup(s => s.Destroy(123));
            var controller = new CommentController(service.Object);

            // act
            var result = controller.Destroy(123) as JsonResult;

            // assert
            service.Verify(c => c.Destroy(123));
            var data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("Comment by Calvin", data["subject"]);
            Assert.AreEqual("was destroyed (there is no undo)", data["predicate"]);
        }
    }
}
