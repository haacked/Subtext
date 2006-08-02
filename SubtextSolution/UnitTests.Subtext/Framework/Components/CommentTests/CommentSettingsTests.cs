using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
	[TestFixture]
	public class CommentSettingsTests
	{
		string hostName;

		[RowTest]
		[Row("", "Subtext.Web", "http://{0}/Subtext.Web/archive/2006/01/23/Test.aspx#1021")]
		[Row("", "", "http://{0}/archive/2006/01/23/Test.aspx#1021")]
		[Row("blog", "Subtext.Web", "http://{0}/Subtext.Web/blog/archive/2006/01/23/Test.aspx#1021")]
		[RollBack]
		public void CommentFullyQualifiedUrlSetProperly(string subfolder, string application, string expectedFormat)
		{
			this.hostName = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(this.hostName, subfolder, application);
			Config.CreateBlog("", "username", "thePassword", this.hostName, subfolder);
			
			Entry comment = new Entry(PostType.Comment);
			comment.Id = 1021;
			comment.Url = "/archive/2006/01/23/Test.aspx#1021";
			Assert.AreEqual(comment.FullyQualifiedUrl, string.Format(expectedFormat, this.hostName));
		}
		
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
			Entry comment = UnitTestHelper.CreateCommentInstance(entryId, "joe schmoe", "blah blah.", "I have nothing to say.", DateTime.Now);
			Assert.IsFalse(Security.IsAdmin, "Comment moderation would not affect admins");
			int commentId = Entries.CreateComment(comment);
			Entry commentFromDb = Entries.GetEntry(commentId, PostConfig.None, false);
			Assert.IsTrue(commentFromDb.IsActive, "Because comment moderation is turned off, we expect that a new comment should be active.");
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
			Entry comment = UnitTestHelper.CreateCommentInstance(entryId, "joe schmoe", "blah xsatho setnuh blah.", "I have nothing interesting to say at all.", DateTime.Now);
			int commentId = Entries.CreateComment(comment);
			Entry commentFromDb = Entries.GetEntry(commentId, PostConfig.None, false);
			Assert.IsFalse(commentFromDb.IsActive, "Because comment moderation is turned on, we expect that a new comment should be inactive.");
			Assert.IsTrue(commentFromDb.NeedsModeratorApproval, "Because comment moderation is turned off, we expect that a new comment should need moderator approval.");
			
			//Let's approve it.
			Entries.Approve(commentFromDb);
			commentFromDb = Entries.GetEntry(commentId, PostConfig.None, false);
			Assert.IsTrue(commentFromDb.IsActive, "Because the comment has been approved, it should be active.");
			Assert.IsFalse(commentFromDb.NeedsModeratorApproval, "Because the comment is approved, it should not need moderator approval.");
		}
		
		[Test]
		[RollBack]
		[ExpectedArgumentNullException]
		public void ApproveThrowsArgumentNullException()
		{
			Config.CreateBlog("", "username", "thePassword", this.hostName, "MyBlog1");
			Entries.Approve(null);
		}

		[Test]
		[RollBack]
		[ExpectedArgumentException]
		public void ApproveThrowsArgumentExceptionForNonComment()
		{
			Config.CreateBlog("", "username", "thePassword", this.hostName, "MyBlog1");
			Entries.Approve(UnitTestHelper.CreateEntryInstanceForSyndication("test", "title", "body"));
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
