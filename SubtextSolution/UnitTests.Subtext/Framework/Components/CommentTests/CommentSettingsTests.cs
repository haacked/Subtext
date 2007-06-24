using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
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
		[Test]
		[RollBack2]
		public void CommentModerationDisabledCausesNewCommentsToBeActive()
		{
			UnitTestHelper.SetupBlog("MyBlog1");

			//Need to set our user to a non-admin
			HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("NotAnAdmin"), new string[] {"Anonymous"});
			
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
		[RollBack2]
		public void CommentModerationEnabledCausesNewCommentsToBeInactive()
		{
			UnitTestHelper.SetupBlog("MyBlog1");
			//Need to set our user to a non-admin
			HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("NotAnAdmin"), new string[] { "Anonymous" });
			
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
		[RollBack2]
		[ExpectedArgumentNullException]
		public void ApproveThrowsArgumentNullException()
		{
			UnitTestHelper.SetupBlog("MyBlog1");
			
			FeedbackItem.Approve(null);
		}
	}
}
