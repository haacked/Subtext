using System;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services;
using Subtext.Web.Controllers;

namespace UnitTests.Subtext.SubtextWeb.Controllers
{
    [TestFixture]
    public class CommentApiControllerTests
    {
        [Test]
        public void CtorSetsCommentService()
        {
            // arrange
            ICommentService service = new Mock<ICommentService>().Object;
            ISubtextContext subtextContext = new Mock<ISubtextContext>().Object;

            // act
            var controller = new CommentApiController(subtextContext, service);

            // assert
            Assert.AreSame(service, controller.CommentService);
        }

        [Test]
        public void CreateWithNullXmlThrowsInvalidOperationException()
        {
            // arrange
            ICommentService service = new Mock<ICommentService>().Object;
            ISubtextContext subtextContext = new Mock<ISubtextContext>().Object;
            var controller = new CommentApiController(subtextContext, service);

            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => controller.Create(1, null));
        }

        [Test]
        public void CreatePassesFeedbackItemToService()
        {
            // arrange
            var service = new Mock<ICommentService>();
            var subtextContext = new Mock<ISubtextContext>();

            FeedbackItem comment = null;
            service.Setup(s => s.Create(It.IsAny<FeedbackItem>(), It.IsAny<bool>())).Callback<FeedbackItem, bool>((f, b) => comment = f);
            var controller = new CommentApiController(subtextContext.Object, service.Object);
            string xmlText =
                @"<?xml version=""1.0""?>
                            <item>
                                <title>Haack's Wild Ride</title>
                                <description>This tests the CommentAPI</description>
                                <author>Me</author>
                                <link>http://subtextproject.com/</link>
                            </item>";
            var doc = new XmlDocument();
            doc.LoadXml(xmlText);

            // act
            controller.Create(123, doc);

            // assert
            Assert.IsNotNull(comment);
            Assert.AreEqual("Haack's Wild Ride", comment.Title);
            Assert.AreEqual("This tests the CommentAPI", comment.Body);
            Assert.AreEqual("Me", comment.Author);
            Assert.AreEqual("http://subtextproject.com/", comment.SourceUrl.ToString());
        }

        [Test]
        public void CreateMissingAuthorDoesNotThrowException()
        {
            // arrange
            var service = new Mock<ICommentService>();
            var subtextContext = new Mock<ISubtextContext>();

            FeedbackItem comment = null;
            service.Setup(s => s.Create(It.IsAny<FeedbackItem>(), It.IsAny<bool>())).Callback<FeedbackItem, bool>((f, b) => comment = f);
            var controller = new CommentApiController(subtextContext.Object, service.Object);
            const string xmlText = @"<?xml version=""1.0""?>
                            <item>
                                <title>Haack's Wild Ride</title>
                                <description>This tests the CommentAPI</description>
                                <link>http://subtextproject.com/</link>
                            </item>";
            var doc = new XmlDocument();
            doc.LoadXml(xmlText);

            // act
            controller.Create(123, doc);

            // assert
            Assert.IsNotNull(comment);
            Assert.AreEqual("Haack's Wild Ride", comment.Title);
            Assert.AreEqual("This tests the CommentAPI", comment.Body);
            Assert.AreEqual(string.Empty, comment.Author);
            Assert.AreEqual("http://subtextproject.com/", comment.SourceUrl.ToString());
        }
    }
}