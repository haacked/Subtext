using MbUnit.Framework;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
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
                new DatabaseObjectProvider().Approve(null, service));
        }
    }
}