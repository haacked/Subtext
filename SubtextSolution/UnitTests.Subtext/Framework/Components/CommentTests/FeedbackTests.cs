using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
	[TestFixture]
	public class FeedbackTests
	{
		string _hostName = string.Empty;

		[RowTest]
		[Row(FeedbackStatusFlag.Approved, true, false, false, false)]
		[Row(FeedbackStatusFlag.ApprovedByModerator, true, false, false, false)]
		[Row(FeedbackStatusFlag.FalsePositive, true, false, false, true)]
		[Row(FeedbackStatusFlag.ConfirmedSpam, false, false, true, true)]
		[Row(FeedbackStatusFlag.FlaggedAsSpam, false, false, false, true)]
		[Row(FeedbackStatusFlag.NeedsModeration, false, true, false, false)]
		[Row(FeedbackStatusFlag.Deleted, false, false, true, false)]
		[RollBack]
		public void CanCreateCommentWithStatus(FeedbackStatusFlag status, bool expectedApproved, bool expectedNeedsModeratorApproval, bool expectedDeleted, bool expectedFlaggedAsSpam)
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = false;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
			Entries.Create(entry);

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, status);
			
			Assert.IsTrue((comment.Status & status) == status, "Expected the " + status + "bit to be set.");
			Assert.AreEqual(expectedApproved, comment.Approved, "We expected 'Approved' to be " + expectedApproved);
			Assert.AreEqual(expectedNeedsModeratorApproval, comment.NeedsModeratorApproval, "Expected 'NeedsModeratorApproval' to be " + expectedNeedsModeratorApproval);
			Assert.AreEqual(expectedDeleted, comment.Deleted, "Expected 'Deleted' to be " + expectedDeleted);
			Assert.AreEqual(expectedFlaggedAsSpam, ((comment.Status & FeedbackStatusFlag.FlaggedAsSpam) == FeedbackStatusFlag.FlaggedAsSpam), "Expected that this item was ever flagged as spam to be " + expectedFlaggedAsSpam);
		}
		
		[Test]
		[RollBack]
		public void ConfirmSpamRemovesApprovedBitAndSetsDeletedBit()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = false;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
			Entries.Create(entry);

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.ConfirmSpam(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsFalse(comment.Approved, "Should not be approved now.");
			Assert.IsTrue(comment.Deleted, "Should not be moved to deleted folder now.");	
		}

		[Test]
		[RollBack]
		public void ApproveCommentRemovesDeletedAndConfirmedSpamBits()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = false;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
			Entries.Create(entry);

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.ConfirmedSpam | FeedbackStatusFlag.Deleted);
			Assert.IsFalse(comment.Approved, "should not be approved");
			Assert.IsTrue(comment.Deleted, "should be deleted");
			Assert.IsTrue(comment.ConfirmedSpam, "should be confirmed spam");

			FeedbackItem.Approve(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsTrue(comment.Approved, "Should be approved now.");
			Assert.IsFalse(comment.Deleted, "Should not be deleted.");
			Assert.IsFalse(comment.ConfirmedSpam, "Should not be confirmed spam.");
		}
		
		/// <summary>
		/// Create some comments that are approved, approved with moderation, 
		/// approved as not spam.  Make sure we get all of them when we get comments.
		/// </summary>
		[Test]
		[RollBack]
		public void CanGetAllApprovedComments()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = false;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
			Entries.Create(entry);

			FeedbackItem commentOne = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			FeedbackItem commentTwo = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.ApprovedByModerator);
			FeedbackItem commentThree = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.ConfirmedSpam);
			FeedbackItem.ConfirmSpam(commentThree);
			FeedbackItem commentFour = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FalsePositive);
			
			//We expect three of the four.
			IPagedCollection<FeedbackItem> feedback = FeedbackItem.GetPagedFeedback(0, 10, FeedbackStatusFlag.Approved, FeedbackType.Comment);
			Assert.AreEqual(3, feedback.Count, "We expected three to match.");
			
			//Expect reverse order
			Assert.AreEqual(commentOne.Id, feedback[2].Id, "The first does not match");
			Assert.AreEqual(commentTwo.Id, feedback[1].Id, "The first does not match");
			Assert.AreEqual(commentFour.Id, feedback[0].Id, "The first does not match");
		}

		/// <summary>
		/// Make sure that we can get all feedback that is flagged as 
		/// spam.  This should exclude items marked as deleted and 
		/// items that were flagged as spam, but subsequently approved.
		/// (FlaggedAsSpam | Approved).
		/// </summary>
		[Test]
		[RollBack]
		public void CanGetItemsFlaggedAsSpam()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			Config.CurrentBlog.CommentsEnabled = true;
			Config.CurrentBlog.ModerationEnabled = false;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
			Entries.Create(entry);

			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FalsePositive);
			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.ConfirmedSpam);
			FeedbackItem included = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FlaggedAsSpam);
			FeedbackItem includedToo = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FlaggedAsSpam | FeedbackStatusFlag.NeedsModeration);

			//We expect 2 of the four.
			IPagedCollection<FeedbackItem> feedback = FeedbackItem.GetPagedFeedback(0, 10, FeedbackStatusFlag.FlaggedAsSpam, FeedbackStatusFlag.Approved | FeedbackStatusFlag.Deleted, FeedbackType.Comment);
			Assert.AreEqual(2, feedback.Count, "We expected two to match.");

			//Expect reverse order
			Assert.AreEqual(included.Id, feedback[1].Id, "The first does not match");
			Assert.AreEqual(includedToo.Id, feedback[0].Id, "The second does not match");
		}

		/// <summary>
		/// Makes sure that the content checksum hash is being created correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void EntryCreateHasContentHash()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
			trackback.DateCreated = DateTime.Now;
			trackback.SourceUrl = new Uri("http://" + UnitTestHelper.GenerateRandomString() + "/ThisUrl/");
			trackback.Title = "Some Title";
			trackback.Body = "Some Body";
			int id = FeedbackItem.Create(trackback);

			FeedbackItem savedEntry = FeedbackItem.Get(id);
			Assert.IsTrue(savedEntry.ChecksumHash.Length > 0, "The Content Checksum should be larger than 0.");
		}

		static FeedbackItem CreateAndUpdateFeedbackWithExactStatus(Entry entry, FeedbackType type, FeedbackStatusFlag status)
		{
			FeedbackItem feedback = new FeedbackItem(type);
			feedback.Title = UnitTestHelper.GenerateRandomString();
			feedback.Body = UnitTestHelper.GenerateRandomString();
			feedback.EntryId = entry.Id;
			int id = FeedbackItem.Create(feedback);

			feedback = FeedbackItem.Get(id);
			feedback.Status = status;
			FeedbackItem.Update(feedback);

			return FeedbackItem.Get(id);
		}

		#region ArgumentNullChecks
		[Test]
		[ExpectedArgumentNullException]
		public void UpdateThrowsArgumentNull()
		{
			FeedbackItem.Update(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ApproveThrowsArgumentNull()
		{
			FeedbackItem.Approve(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ConfirmSpamThrowsArgumentNull()
		{
			FeedbackItem.ConfirmSpam(null);
		}
		#endregion

		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
			CommentFilter.ClearCommentCache();
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
