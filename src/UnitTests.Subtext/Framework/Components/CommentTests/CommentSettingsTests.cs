using MbUnit.Framework;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
    [TestFixture]
    public class CommentSettingsTests
    {
        [Test]
        public void ApproveThrowsArgumentNullException()
        {
            // arrange
            var service = new Mock<ICommentSpamService>().Object;
            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                FeedbackItem.Approve(null, service));
        }
    }
}