using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.CommentTests
{
	[TestFixture]
	public class FeedbackTests
	{
		[RowTest]
		[Row(FeedbackStatusFlags.Approved, true, false, false, false)]
		[Row(FeedbackStatusFlags.ApprovedByModerator, true, false, false, false)]
		[Row(FeedbackStatusFlags.FalsePositive, true, false, false, true)]
		[Row(FeedbackStatusFlags.ConfirmedSpam, false, false, true, true)]
		[Row(FeedbackStatusFlags.FlaggedAsSpam, false, false, false, true)]
		[Row(FeedbackStatusFlags.NeedsModeration, false, true, false, false)]
		[Row(FeedbackStatusFlags.Deleted, false, false, true, false)]
		[RollBack2]
		public void CanCreateCommentWithStatus(FeedbackStatusFlags status, bool expectedApproved, bool expectedNeedsModeratorApproval, bool expectedDeleted, bool expectedFlaggedAsSpam)
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, status);
			
			Assert.IsTrue((comment.Status & status) == status, "Expected the " + status + "bit to be set.");
			Assert.AreEqual(expectedApproved, comment.Approved, "We expected 'Approved' to be " + expectedApproved);
			Assert.AreEqual(expectedNeedsModeratorApproval, comment.NeedsModeratorApproval, "Expected 'NeedsModeratorApproval' to be " + expectedNeedsModeratorApproval);
			Assert.AreEqual(expectedDeleted, comment.Deleted, "Expected 'Deleted' to be " + expectedDeleted);
			Assert.AreEqual(expectedFlaggedAsSpam, ((comment.Status & FeedbackStatusFlags.FlaggedAsSpam) == FeedbackStatusFlags.FlaggedAsSpam), "Expected that this item was ever flagged as spam to be " + expectedFlaggedAsSpam);
		}
		
		[Test]
		[RollBack2]
		public void ConfirmSpamRemovesApprovedBitAndSetsDeletedBit()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.ConfirmSpam(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsFalse(comment.Approved, "Should not be approved now.");
			Assert.IsTrue(comment.Deleted, "Should be moved to deleted folder now.");	
		}

		[Test]
		[RollBack2]
		public void DeleteCommentSetsDeletedBit()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.Delete(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsFalse(comment.Approved, "Should not be approved now.");
			Assert.IsTrue(comment.Deleted, "Should be moved to deleted folder now.");
		}

		[Test]
		[RollBack2]
		public void DestroyCommentByStatusDestroysOnlyThatStatus()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			CreateApprovedComments(3, entry);
			CreateFlaggedSpam(2, entry);
			CreateDeletedComments(3, entry);

			FeedbackItem newComment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			FeedbackItem.ConfirmSpam(newComment);
			newComment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.FlaggedAsSpam);
			Assert.IsFalse(newComment.Approved, "should not be approved");
			FeedbackItem.Delete(newComment); //Move it to trash.

			FeedbackCounts counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
			Assert.AreEqual(2, counts.FlaggedAsSpamCount, "Expected two items flagged as spam.");
			Assert.AreEqual(5, counts.DeletedCount, "Expected five in the trash");

		    FeedbackItem.Destroy(FeedbackStatusFlags.FlaggedAsSpam);
			counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
			Assert.AreEqual(0, counts.FlaggedAsSpamCount, "Expected the items flagged as spam to be gone.");
			Assert.AreEqual(5, counts.DeletedCount, "Destroying all flagged items should not touch the trash bin.");

		    CreateFlaggedSpam(3, entry);
			counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.FlaggedAsSpamCount, "Expected three items flagged as spam.");

			FeedbackItem.Destroy(FeedbackStatusFlags.Deleted);
			counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
			Assert.AreEqual(3, counts.FlaggedAsSpamCount, "Expected three approved still");
			Assert.AreEqual(0, counts.DeletedCount, "Destroying all deleted items should not touch the flagged items.");
		}

		private static void CreateComments(int count, Entry entry, FeedbackStatusFlags status)
		{
			for (int i = 0; i < count; i++)
			{
				CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, status);
			}
		}
		
		private static void CreateFlaggedSpam(int count, Entry entry)
		{
			CreateComments(count, entry, FeedbackStatusFlags.FlaggedAsSpam);
		}

		private static void CreateApprovedComments(int count, Entry entry)
		{
			CreateComments(count, entry, FeedbackStatusFlags.Approved);
		}
		
		private static void CreateDeletedComments(int count, Entry entry)
		{
			CreateComments(count, entry, FeedbackStatusFlags.Deleted);
		}

        [Test]
        [RollBack2]
        public void CreateFeedbackSetsBlogStatsCorrectly()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            BlogInfo info = Config.CurrentBlog;

            Assert.AreEqual(0, info.CommentCount);
            Assert.AreEqual(0, info.PingTrackCount);

            info = Config.GetBlogInfo(info.Host, info.Subfolder); // pull back the updated info from the datastore.
            Assert.AreEqual(0, info.CommentCount);
            Assert.AreEqual(0, info.PingTrackCount);

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlags.Approved);

            info = Config.GetBlogInfo(info.Host, info.Subfolder);
            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlags.Approved);

            info = Config.GetBlogInfo(info.Host, info.Subfolder);
            Assert.AreEqual(2, info.CommentCount, "Blog CommentCount should be 2");
            Assert.AreEqual(2, info.PingTrackCount, "Blog Ping/Trackback count should be 2");
        }

        [Test]
        [RollBack2]
        public void CreateEntryDoesNotResetBlogStats()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            BlogInfo info = Config.CurrentBlog;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlags.Approved);

            Entry entry2 = UnitTestHelper.CreateEntryInstanceForSyndication("johnny b goode", "foo-bar", "zaa zaa zoo.");
            Entries.Create(entry2);
            info = Config.GetBlogInfo(info.Host, info.Subfolder); // pull back the updated info from the datastore

            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");
        }

        [Test]
        [RollBack2]
        public void DeleteEntrySetsBlogStats()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            BlogInfo info = Config.CurrentBlog;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlags.Approved);

            info = Config.GetBlogInfo(info.Host, info.Subfolder);
            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");

            Entries.Delete(entry.Id);
            info = Config.GetBlogInfo(info.Host, info.Subfolder);

            Assert.AreEqual(0, info.CommentCount, "Blog CommentCount should be 0");
            Assert.AreEqual(0, info.PingTrackCount, "Blog Ping/Trackback count should be 0");
        }

	    [Test]
		[RollBack2]
		public void DestroyCommentReallyGetsRidOfIt()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");
			comment.Approved = false;
			FeedbackItem.Update(comment);

			FeedbackItem.Destroy(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsNull(comment);
		}

		[Test]
		[RollBack2]
		[ExpectedException(typeof(InvalidOperationException))]
		public void DestroyCommentCannotDestroyActiveComment()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.Destroy(comment);
		}

		[Test]
		[RollBack2]
		public void ApproveCommentRemovesDeletedAndConfirmedSpamBits()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.ConfirmedSpam | FeedbackStatusFlags.Deleted);
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
		[RollBack2]
		public void CanGetAllApprovedComments()
		{
			Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem commentOne = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			Thread.Sleep(10);
			FeedbackItem commentTwo = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.ApprovedByModerator);
			Thread.Sleep(10);
			FeedbackItem commentThree = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.ConfirmedSpam);
			Thread.Sleep(10);
			FeedbackItem.ConfirmSpam(commentThree);
			FeedbackItem commentFour = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.FalsePositive);
			
			//We expect three of the four.
			IPagedCollection<FeedbackItem> feedback = FeedbackItem.GetPagedFeedback(0, 10, FeedbackStatusFlags.Approved, FeedbackType.Comment);
			Assert.AreEqual(3, feedback.Count, "We expected three to match.");
			
			//Expect reverse order
			Assert.AreEqual(commentOne.Id, feedback[2].Id, "The first does not match");
			Assert.AreEqual(commentTwo.Id, feedback[1].Id, "The second does not match");
			Assert.AreEqual(commentFour.Id, feedback[0].Id, "The third does not match");
		}
		
		[Test]
		[RollBack2]
		public void OnlyApprovedItemsContributeToEntryFeedbackCount()
		{
			Entry entry = SetupBlogForCommentsAndCreateEntry();
			int entryId = entry.Id;

			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.AreEqual(1, entry.FeedBackCount, "Expected one approved feedback entry.");

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.FlaggedAsSpam);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.AreEqual(1, entry.FeedBackCount, "Expected one approved feedback entry.");

			comment.Approved = true;
			FeedbackItem.Update(comment);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.AreEqual(2, entry.FeedBackCount, "After approving the second comment, expected two approved feedback entry.");

			comment.Approved = false;
			FeedbackItem.Update(comment);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.AreEqual(1, entry.FeedBackCount, "After un-approving the second comment, expected one approved feedback entry.");
			
			FeedbackItem.Delete(comment);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.AreEqual(1, entry.FeedBackCount, "After un-approving the second comment, expected one approved feedback entry.");
		}


		/// <summary>
		/// Make sure that we can get all feedback that is flagged as 
		/// spam.  This should exclude items marked as deleted and 
		/// items that were flagged as spam, but subsequently approved.
		/// (FlaggedAsSpam | Approved).
		/// </summary>
		[Test]
		[RollBack2]
		public void CanGetItemsFlaggedAsSpam()
		{
		    Entry entry = SetupBlogForCommentsAndCreateEntry();

			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.FalsePositive);
			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.Approved);
			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.ConfirmedSpam);
			FeedbackItem included = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.FlaggedAsSpam);
			Thread.Sleep(10);
			FeedbackItem includedToo = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlags.FlaggedAsSpam | FeedbackStatusFlags.NeedsModeration);
			Assert.Greater(includedToo.DateCreated, included.DateCreated);

			//We expect 2 of the four.
			IPagedCollection<FeedbackItem> feedback = FeedbackItem.GetPagedFeedback(0, 10, FeedbackStatusFlags.FlaggedAsSpam, FeedbackStatusFlags.Approved | FeedbackStatusFlags.Deleted, FeedbackType.Comment);
			Assert.AreEqual(2, feedback.Count, "We expected two to match.");

			//Expect reverse order
			Assert.AreEqual(included.Id, feedback[1].Id, "The first does not match");
			Assert.AreEqual(includedToo.Id, feedback[0].Id, "The second does not match");
		}

		/// <summary>
		/// Makes sure that the content checksum hash is being created correctly.
		/// </summary>
		[Test]
		[RollBack2]
		public void CreateFeedbackHasContentHash()
		{
			UnitTestHelper.SetupBlog();

			FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
			trackback.DateCreated = DateTime.Now;
			trackback.SourceUrl = new Uri("http://" + UnitTestHelper.GenerateRandomString() + "/ThisUrl/");
			trackback.Title = "Some Title";
			trackback.Body = "Some Body";
			int id = FeedbackItem.Create(trackback, null);

			FeedbackItem savedEntry = FeedbackItem.Get(id);
			Assert.IsTrue(savedEntry.ChecksumHash.Length > 0, "The Content Checksum should be larger than 0.");
		}

		/// <summary>
		/// Makes sure that the content checksum hash is being created correctly.
		/// </summary>
		[RowTest]
		[Row("commenter@example.com", "http://haacked.com/", "", "/", "commenter@example.com", "http://haacked.com/")]
		[Row("", "http://haacked.com/", "", "/", "no email given", "http://haacked.com/")]
		[Row("commenter@example.com", "", "", "/", "commenter@example.com", "none given")]
		[Row("commenter@example.com", "", "/", "TEST", "commenter@example.com", "none given")]
		[Row("commenter@example.com", "", "/Subtext.Web", "TEST", "commenter@example.com", "none given")]
		[RollBack2]
		public void CreateFeedbackSendsCorrectEmail(string commenterEmail, string commenterUrl, string applicationPath, string subfolder, string expectedEmail, string expectedUrl)
		{
			UnitTestHelper.SetupBlog();
			HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("NotAnAdmin"), new string[] { "Anonymous" });
			Config.CurrentBlog.Owner.Email = "test@example.com";
			Membership.UpdateUser(Config.CurrentBlog.Owner);
			Config.CurrentBlog.Title = "You've been haacked";
            Config.CurrentBlog.CommentNoficationEnabled = true;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
			int entryId = Entries.Create(entry);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);

			FeedbackItem feedbackItem = new FeedbackItem(FeedbackType.Comment);
			feedbackItem.ParentEntryName = entry.EntryName;
			feedbackItem.Author = "Billy Bob";
			feedbackItem.Email = commenterEmail;
			feedbackItem.DateCreated = DateTime.Now;
			if (commenterUrl.Length > 0)
				feedbackItem.SourceUrl = new Uri(commenterUrl);
			feedbackItem.Title = "Some Title";
			feedbackItem.Body = "Some Body<br /> likes me.";
			feedbackItem.EntryId = entryId;
			int id = FeedbackItem.Create(feedbackItem, null);
			Thread.Sleep(100); //Sending email is asynch.

			string expectedMessageBody = "Comment from You've been haacked" + Environment.NewLine
			                             + "----------------------------------------------------" + Environment.NewLine
			                             + "From:\tBilly Bob <" + expectedEmail + ">" + Environment.NewLine
			                             + "Url:\t" + expectedUrl + Environment.NewLine +
			                             "IP:\t127.0.0.1" + Environment.NewLine
			                             + "====================================================" + Environment.NewLine + Environment.NewLine
			                             + "Some Body" + Environment.NewLine + " likes me." + Environment.NewLine + Environment.NewLine
			                             + "Source: " + entry.FullyQualifiedUrl + "#" + id;
			
			string expectedMessageBodyInCaseOfSpam = "Spam Flagged " + expectedMessageBody;

			UnitTestEmailProvider emailProvider = (UnitTestEmailProvider)EmailProvider.Instance();

            Assert.AreEqual(Config.CurrentBlog.Owner.Email, emailProvider.To, "Email should've been sent to the blog email addr.");
			if (String.IsNullOrEmpty(commenterEmail))
				expectedEmail = "admin@YOURBLOG.com";
			Assert.AreEqual("admin@YOURBLOG.com", emailProvider.From, "Email should have been sent from the value in App.config.");
            if (commenterEmail != "")
              Assert.AreEqual(expectedEmail, emailProvider.ReplyTo, "Email should have had Reply-To set to the comment from address");
			if (feedbackItem.FlaggedAsSpam)
			{
				Assert.AreEqual("[SPAM Flagged] Comment: Some Title (via You've been haacked)", emailProvider.Subject, "Comment subject line wrong.");
				Assert.AreEqual(expectedMessageBodyInCaseOfSpam, emailProvider.Message, "Did not receive the expected message.");
			}
			else
			{
				Assert.AreEqual("Comment: Some Title (via You've been haacked)", emailProvider.Subject, "Comment subject line wrong.");
				Assert.AreEqual(expectedMessageBody, emailProvider.Message, "Did not receive the expected message.");
			}
		}

		static FeedbackItem CreateAndUpdateFeedbackWithExactStatus(Entry entry, FeedbackType type, FeedbackStatusFlags status)
		{
			FeedbackItem feedback = new FeedbackItem(type);
			feedback.Title = UnitTestHelper.GenerateRandomString();
			feedback.Body = UnitTestHelper.GenerateRandomString();
			feedback.EntryId = entry.Id;
			int id = FeedbackItem.Create(feedback, null);

			feedback = FeedbackItem.Get(id);
			feedback.Status = status;
			FeedbackItem.Update(feedback);

			return FeedbackItem.Get(id);
		}

		static Entry SetupBlogForCommentsAndCreateEntry()
        {
            UnitTestHelper.SetupBlog();
            BlogInfo info = Config.CurrentBlog;
            info.Title = "You've been haacked";
            info.CommentsEnabled = true;
            info.ModerationEnabled = false;

            Config.UpdateConfigData(info);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
            Entries.Create(entry);
            return entry;
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

		[Test]
		[ExpectedArgumentNullException]
		public void DeleteNullCommentThrowsArgumentNull()
		{
			FeedbackItem.Delete(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void DestroyNullCommentThrowsArgumentNull()
		{
			FeedbackItem.Destroy(null);
		}
		#endregion

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
