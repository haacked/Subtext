using MbUnit.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.Feedback;

namespace UnitTests.Subtext.SubtextWeb.Admin
{
    [TestFixture]
    public class FeedbackTests
    {
        [Test]
        public void WhenFeedbackApprovedUiShowsRelevantItems()
        {
            FeedbackState state = FeedbackState.GetUiState(FeedbackStatusFlag.Approved);
            Assert.AreEqual("Comments", state.HeaderText);
            Assert.IsFalse(state.Approvable);
            Assert.IsFalse(state.Destroyable);
            Assert.IsTrue(state.Deletable);
            Assert.AreEqual("", state.DeleteToolTip);
            Assert.IsTrue(state.Spammable);
            Assert.IsFalse(state.Emptyable);
            Assert.AreEqual("", state.EmptyToolTip);
            Assert.AreEqual("<em>There are no approved comments to display.</em>", state.NoCommentsHtml);
        }

        [Test]
        public void WhenFeedbackNeedsModerationUiShowsRelevantItems()
        {
            FeedbackState state = FeedbackState.GetUiState(FeedbackStatusFlag.NeedsModeration);
            Assert.AreEqual("Comments Pending Moderator Approval", state.HeaderText);
            Assert.IsTrue(state.Approvable);
            Assert.AreEqual("Approve", state.ApproveText);
            Assert.IsFalse(state.Destroyable);
            Assert.IsTrue(state.Deletable);
            Assert.AreEqual("", state.DeleteToolTip);
            Assert.IsTrue(state.Spammable);
            Assert.IsFalse(state.Emptyable);
            Assert.AreEqual("", state.EmptyToolTip);
            Assert.AreEqual("<em>No Entries Need Moderation.</em>", state.NoCommentsHtml);
        }

        [Test]
        public void WhenFeedbackFlaggedAsSpamUiShowsRelevantItems()
        {
            FeedbackState state = FeedbackState.GetUiState(FeedbackStatusFlag.FlaggedAsSpam);
            Assert.AreEqual("Comments Flagged as SPAM", state.HeaderText);
            Assert.IsTrue(state.Approvable);
            Assert.AreEqual("Approve", state.ApproveText);
            Assert.IsFalse(state.Destroyable);
            Assert.IsTrue(state.Deletable);
            Assert.AreEqual("Trashes checked spam", state.DeleteToolTip);
            Assert.IsFalse(state.Spammable);
            Assert.IsTrue(state.Emptyable);
            Assert.AreEqual("Destroy all spam, not just checked", state.EmptyToolTip);
            Assert.AreEqual("<em>No Entries Flagged as SPAM.</em>", state.NoCommentsHtml);
        }

        [Test]
        public void WhenFeedbackDeletedUiShowsRelevantItems()
        {
            FeedbackState state = FeedbackState.GetUiState(FeedbackStatusFlag.Deleted);
            Assert.AreEqual("Comments In The Trash Bin", state.HeaderText);
            Assert.IsTrue(state.Approvable);
            Assert.AreEqual("Undelete", state.ApproveText);
            Assert.IsTrue(state.Destroyable);
            Assert.IsFalse(state.Deletable);
            Assert.AreEqual("Trashes checked spam", state.DeleteToolTip);
            Assert.IsFalse(state.Spammable);
            Assert.IsTrue(state.Emptyable);
            Assert.AreEqual("Destroy all trash, not just checked", state.EmptyToolTip);
            Assert.AreEqual("<em>No Entries in the Trash.</em>", state.NoCommentsHtml);
        }
    }
}