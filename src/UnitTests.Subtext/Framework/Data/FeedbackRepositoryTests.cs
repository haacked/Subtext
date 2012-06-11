using System;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Data
{
    [TestFixture]
    public class FeedbackRepositoryTests
    {
        [Test]
        public void Create_WithFeedbackItem_SetsDateCreatedAndModifiedToUtcNow()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var sps = new Mock<StoredProcedures>("test");
            sps.Setup(s => s.InsertFeedback(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime>()));
            var repository = new DatabaseObjectProvider(blogId: 1, procedures: sps.Object);
            var feedback = new FeedbackItem(FeedbackType.Comment) { Body = "blah" };

            // Act
            repository.Create(feedback);

            // Assert
            Assert.GreaterEqualThan(DateTime.UtcNow, feedback.DateCreatedUtc);
            Assert.GreaterEqualThan(DateTime.UtcNow, feedback.DateModifiedUtc);
            Assert.GreaterEqualThan(feedback.DateCreatedUtc, now);
            Assert.GreaterEqualThan(feedback.DateModifiedUtc, now);
        }
    }
}
