using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
    [TestClass]
    public class CommentSettingsTests
    {
        [TestMethod]
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