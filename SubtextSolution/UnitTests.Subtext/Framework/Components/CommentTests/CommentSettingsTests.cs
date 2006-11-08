using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
	[TestFixture]
	public class CommentSettingsTests
	{
		string hostName;

		[Test]
		[RollBack]
		public void CommentModerationDisabledCausesNewCommentsToBeActive()
		{
			Config.CreateBlog("", "username", "thePassword", this.hostName, "MyBlog1");
			
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = false;
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "test entry", "the body of the entry");
			int entryId = Entries.Create(entry);
			//create comment.
			FeedbackItem comment = UnitTestHelper.CreateCommentInstance(entryId, "joe schmoe", "blah blah.", "I have nothing to say.", DateTime.Now);
			Assert.IsFalse(SecurityHelper.IsAdmin, "Comment moderation would not affect admins");
			int commentId = FeedbackItem.Create(comment, new CommentFilter(null));
			FeedbackItem commentFromDb = FeedbackItem.Get(commentId);
			Assert.IsTrue(commentFromDb.Approved, "Because comment moderation is turned off, we expect that a new comment should be active.");
			Assert.IsFalse(commentFromDb.NeedsModeratorApproval, "Because comment moderation is turned off, we expect that a new comment should not need moderator approval.");
		}
		
		[Test]
		[RollBack]
		public void CommentModerationEnabledCausesNewCommentsToBeInactive()
		{
			Config.CreateBlog("", "username", "thePassword", this.hostName, "MyBlog1");
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = true;
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "test entry", "the body of the entry");
			int entryId = Entries.Create(entry);
			
			//create comment.
			FeedbackItem comment = UnitTestHelper.CreateCommentInstance(entryId, "joe schmoe", "blah xsatho setnuh blah.", "I have nothing interesting to say at all.", DateTime.Now);
			int commentId = FeedbackItem.Create(comment, new CommentFilter(null));
			FeedbackItem commentFromDb = FeedbackItem.Get(commentId);
			Assert.IsFalse(commentFromDb.Approved, "Because comment moderation is turned on, we expect that a new comment should note be approved.");
			Assert.IsTrue(commentFromDb.NeedsModeratorApproval, "Because comment moderation is turned on, we expect that a new comment should need moderator approval.");
			
			//Let's approve it.
			FeedbackItem.Approve(commentFromDb);
			commentFromDb = FeedbackItem.Get(commentId);
			Assert.IsTrue(commentFromDb.Approved, "The comment should have the status of approved.");
			Assert.IsTrue(commentFromDb.ApprovedByModerator, "Since this was approved by a moderator, that extra bit of info should be present.");
			Assert.IsFalse(commentFromDb.NeedsModeratorApproval, "Because the comment is approved, it should not need moderator approval.");
		}
		
		[Test]
		[RollBack]
		[ExpectedArgumentNullException]
		public void ApproveThrowsArgumentNullException()
		{
			Config.CreateBlog("", "username", "thePassword", this.hostName, "MyBlog1");
			FeedbackItem.Approve(null);
		}

		[SetUp]
		public void SetUp()
		{
			this.hostName = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(this.hostName, "MyBlog1");
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
