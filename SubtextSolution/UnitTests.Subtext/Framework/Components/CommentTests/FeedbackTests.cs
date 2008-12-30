using System;
using System.Globalization;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;

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
            Entry entry = SetupBlogForCommentsAndCreateEntry();

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
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.ConfirmSpam(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsFalse(comment.Approved, "Should not be approved now.");
			Assert.IsTrue(comment.Deleted, "Should be moved to deleted folder now.");	
		}

		[Test]
		[RollBack]
		public void DeleteCommentSetsDeletedBit()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.Delete(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsFalse(comment.Approved, "Should not be approved now.");
			Assert.IsTrue(comment.Deleted, "Should be moved to deleted folder now.");
		}

		[Test]
		[RollBack]
		public void DestroyCommentByStatusDestroysOnlyThatStatus()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			CreateApprovedComments(3, entry);
			CreateFlaggedSpam(2, entry);
			CreateDeletedComments(3, entry);

			FeedbackItem newComment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			FeedbackItem.ConfirmSpam(newComment);
			newComment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FlaggedAsSpam);
			Assert.IsFalse(newComment.Approved, "should not be approved");
			FeedbackItem.Delete(newComment); //Move it to trash.

			FeedbackCounts counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
			Assert.AreEqual(2, counts.FlaggedAsSpamCount, "Expected two items flagged as spam.");
			Assert.AreEqual(5, counts.DeletedCount, "Expected five in the trash");

		    FeedbackItem.Destroy(FeedbackStatusFlag.FlaggedAsSpam);
			counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
			Assert.AreEqual(0, counts.FlaggedAsSpamCount, "Expected the items flagged as spam to be gone.");
			Assert.AreEqual(5, counts.DeletedCount, "Destroying all flagged items should not touch the trash bin.");

		    CreateFlaggedSpam(3, entry);
			counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.FlaggedAsSpamCount, "Expected three items flagged as spam.");

			FeedbackItem.Destroy(FeedbackStatusFlag.Deleted);
			counts = FeedbackItem.GetFeedbackCounts();
			Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
			Assert.AreEqual(3, counts.FlaggedAsSpamCount, "Expected three approved still");
			Assert.AreEqual(0, counts.DeletedCount, "Destroying all deleted items should not touch the flagged items.");
		}

		private static void CreateComments(int count, Entry entry, FeedbackStatusFlag status)
		{
			for (int i = 0; i < count; i++)
			{
				CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, status);
			}
		}
		
		private static void CreateFlaggedSpam(int count, Entry entry)
		{
			CreateComments(count, entry, FeedbackStatusFlag.FlaggedAsSpam);
		}

		private static void CreateApprovedComments(int count, Entry entry)
		{
			CreateComments(count, entry, FeedbackStatusFlag.Approved);
		}
		
		private static void CreateDeletedComments(int count, Entry entry)
		{
			CreateComments(count, entry, FeedbackStatusFlag.Deleted);
		}

        [Test]
        [RollBack]
        public void CreateFeedbackSetsBlogStatsCorrectly()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            Blog info = Config.CurrentBlog;

            Assert.AreEqual(0, info.CommentCount);
            Assert.AreEqual(0, info.PingTrackCount);

            info = Config.GetBlog(info.Host, info.Subfolder); // pull back the updated info from the datastore.
            Assert.AreEqual(0, info.CommentCount);
            Assert.AreEqual(0, info.PingTrackCount);

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            info = Config.GetBlog(info.Host, info.Subfolder);
            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            info = Config.GetBlog(info.Host, info.Subfolder);
            Assert.AreEqual(2, info.CommentCount, "Blog CommentCount should be 2");
            Assert.AreEqual(2, info.PingTrackCount, "Blog Ping/Trackback count should be 2");
        }

        [Test]
        [RollBack]
        public void CreateEntryDoesNotResetBlogStats()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            Blog info = Config.CurrentBlog;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            Entry entry2 = UnitTestHelper.CreateEntryInstanceForSyndication("johnny b goode", "foo-bar", "zaa zaa zoo.");
            Entries.Create(entry2);
            info = Config.GetBlog(info.Host, info.Subfolder); // pull back the updated info from the datastore

            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");
        }

        [Test]
        [RollBack]
        public void DeleteEntrySetsBlogStats()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            Blog info = Config.CurrentBlog;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            info = Config.GetBlog(info.Host, info.Subfolder);
            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");

            Entries.Delete(entry.Id);
            info = Config.GetBlog(info.Host, info.Subfolder);

            Assert.AreEqual(0, info.CommentCount, "Blog CommentCount should be 0");
            Assert.AreEqual(0, info.PingTrackCount, "Blog Ping/Trackback count should be 0");
        }

	    [Test]
		[RollBack]
		public void DestroyCommentReallyGetsRidOfIt()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");
			comment.Approved = false;
			FeedbackItem.Update(comment);

			FeedbackItem.Destroy(comment);
			comment = FeedbackItem.Get(comment.Id);
			Assert.IsNull(comment);
		}

		[Test]
		[RollBack]
		[ExpectedException(typeof(InvalidOperationException))]
		public void DestroyCommentCannotDestroyActiveComment()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			Assert.IsTrue(comment.Approved, "should be approved");

			FeedbackItem.Destroy(comment);
		}

		[Test]
		[RollBack]
		public void ApproveCommentRemovesDeletedAndConfirmedSpamBits()
		{
            Entry entry = SetupBlogForCommentsAndCreateEntry();

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
			Entry entry = SetupBlogForCommentsAndCreateEntry();

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
		
		[Test]
		[RollBack]
		public void OnlyApprovedItemsContributeToEntryFeedbackCount()
		{
			Entry entry = SetupBlogForCommentsAndCreateEntry();
			int entryId = entry.Id;

			CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.AreEqual(1, entry.FeedBackCount, "Expected one approved feedback entry.");

			FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FlaggedAsSpam);
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
		[RollBack]
		public void CanGetItemsFlaggedAsSpam()
		{
		    Entry entry = SetupBlogForCommentsAndCreateEntry();

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
		public void CreateFeedbackHasContentHash()
		{
			Config.CreateBlog(string.Empty, "username", "password", _hostName, string.Empty);

			FeedbackItem trackback = new FeedbackItem(FeedbackType.PingTrack);
			trackback.DateCreated = DateTime.Now;
			trackback.SourceUrl = new Uri("http://" + UnitTestHelper.GenerateUniqueString() + "/ThisUrl/");
			trackback.Title = "Some Title";
			trackback.Body = "Some Body";
			int id = FeedbackItem.Create(trackback, null);

			FeedbackItem savedEntry = FeedbackItem.Get(id);
			Assert.IsTrue(savedEntry.ChecksumHash.Length > 0, "The Content Checksum should be larger than 0.");
		}

	    /// <summary>
	    /// Make sure that we can create Feedback items with specific dates, needed for Import functionality.
	    /// </summary>
	    [Test]
	    [RollBack]
	    public void CreateFeedbackWithSpecifiedDateCreated()
	    {
	        Config.CreateBlog(string.Empty, "username", "password", _hostName, string.Empty);
            DateTime dateCreated = DateTime.ParseExact("2005/01/23 05:05:05", "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
	        
            FeedbackItem savedComment = CreateFeedbackWithSpecifiedDates(dateCreated, NullValue.NullDateTime);
            Assert.IsTrue(dateCreated.CompareTo(savedComment.DateCreated) == 0, "The Comment's Date Created was not saved correctly.");
            Assert.IsTrue(dateCreated.CompareTo(savedComment.DateModified) == 0, "The Comment's Date Modified was not saved correctly.");
	    }

        /// <summary>
        /// Make sure that we can create Feedback items with specific dates, needed for Import functionality.
        /// </summary>
        [Test]
        [RollBack]
        public void CreateFeedbackWithSpecifiedDateModified()
        {
            Config.CreateBlog(string.Empty, "username", "password", _hostName, string.Empty);
            DateTime dateCreated = DateTime.ParseExact("2005/01/23 05:05:05", "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime dateModified = dateCreated.AddDays(5);

            FeedbackItem savedComment = CreateFeedbackWithSpecifiedDates(dateCreated, dateModified);
            Assert.IsTrue(dateCreated.CompareTo(savedComment.DateCreated) == 0, "The Comment's Date Created was not saved correctly.");
            Assert.IsTrue(dateModified.CompareTo(savedComment.DateModified) == 0, "The Comment's Date Modified was not saved correctly.");
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
		[RollBack]
		public void CreateFeedbackSendsCorrectEmail(string commenterEmail, string commenterUrl, string applicationPath, string subfolder, string expectedEmail, string expectedUrl)
		{
			Config.CreateBlog(string.Empty, "username", "password", _hostName, subfolder);
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, subfolder, applicationPath);
			Config.CurrentBlog.Email = "test@example.com";
			Config.CurrentBlog.Title = "You've been haacked";

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
            entry.DateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int entryId = Entries.Create(entry);
			entry = Entries.GetEntry(entryId, PostConfig.None, false);

			FeedbackItem feedbackItem = new FeedbackItem(FeedbackType.Comment);
			feedbackItem.ParentEntryName = entry.EntryName;
			feedbackItem.Author = "Billy Bob";
			feedbackItem.Email = commenterEmail;
			feedbackItem.DateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            feedbackItem.ParentDateCreated = entry.DateCreated;
			if (commenterUrl.Length > 0)
				feedbackItem.SourceUrl = new Uri(commenterUrl);
			feedbackItem.Title = "Some Title";
			feedbackItem.Body = "Some Body<br /> likes me.";
			feedbackItem.EntryId = entryId;
			int id = FeedbackItem.Create(feedbackItem, null);
			Thread.Sleep(100); //Sending email is asynch.

            string virtualPath = applicationPath ?? "/";
            if (!virtualPath.EndsWith("/")) {
                virtualPath += "/";
            }
            if (!String.IsNullOrEmpty(subfolder) && !subfolder.EndsWith("/")) {
                virtualPath += subfolder + "/";
            }

            string feedbackUrl = "http://" + _hostName + virtualPath + "archive/2008/01/23/blah.aspx#" + id;

			string expectedMessageBody = "Comment from You've been haacked" + Environment.NewLine
			                             + "----------------------------------------------------" + Environment.NewLine
			                             + "From:\tBilly Bob <" + expectedEmail + ">" + Environment.NewLine
			                             + "Url:\t" + expectedUrl + Environment.NewLine +
			                             "IP:\t127.0.0.1" + Environment.NewLine
			                             + "====================================================" + Environment.NewLine + Environment.NewLine
			                             + "Some Body" + Environment.NewLine + " likes me." + Environment.NewLine + Environment.NewLine
			                             + "Source: " + feedbackUrl;
			
			string expectedMessageBodyInCaseOfSpam = "Spam Flagged " + expectedMessageBody;

			UnitTestEmailProvider emailProvider = (UnitTestEmailProvider)EmailProvider.Instance();

			Assert.AreEqual("test@example.com", emailProvider.To, "Email should've been sent to the blog email addr.");
			if (String.IsNullOrEmpty(commenterEmail))
				expectedEmail = "admin@YOURBLOG.com";
			Assert.AreEqual(expectedEmail, emailProvider.From, "Email should have been sent from the value in App.config.");
			if (feedbackItem.FlaggedAsSpam)
			{
				Assert.AreEqual("[SPAM Flagged] Comment: Some Title (via You've been haacked)", emailProvider.Subject, "Comment subject line wrong.");
				Assert.AreEqual(expectedMessageBodyInCaseOfSpam, emailProvider.Message);
			}
			else
			{
				Assert.AreEqual("Comment: Some Title (via You've been haacked)", emailProvider.Subject, "Comment subject line wrong.");
				Assert.AreEqual(expectedMessageBody, emailProvider.Message, "Did not receive the expected message.");
			}
		}

        static FeedbackItem CreateFeedbackWithSpecifiedDates(DateTime created, DateTime modified)
        {
            FeedbackItem comment = new FeedbackItem(FeedbackType.Comment);
            comment.SourceUrl = new Uri("http://" + UnitTestHelper.GenerateUniqueString() + "/ThisUrl/");
            comment.Title = UnitTestHelper.GenerateUniqueString();
            comment.Body = UnitTestHelper.GenerateUniqueString();
            comment.DateCreated = created;
            comment.DateModified = modified;

            int feedbackId = FeedbackItem.Create(comment, null);
            return FeedbackItem.Get(feedbackId);
        }
	    
		static FeedbackItem CreateAndUpdateFeedbackWithExactStatus(Entry entry, FeedbackType type, FeedbackStatusFlag status)
		{
			FeedbackItem feedback = new FeedbackItem(type);
			feedback.Title = UnitTestHelper.GenerateUniqueString();
			feedback.Body = UnitTestHelper.GenerateUniqueString();
			feedback.EntryId = entry.Id;
            feedback.Author = "TestAuthor";
			int id = FeedbackItem.Create(feedback, null);

			feedback = FeedbackItem.Get(id);
			feedback.Status = status;
			FeedbackItem.Update(feedback);

			return FeedbackItem.Get(id);
		}

        Entry SetupBlogForCommentsAndCreateEntry()
        {
            Config.CreateBlog(string.Empty, "username", "password", _hostName, string.Empty);
            Blog info = Config.CurrentBlog;
            info.Email = "test@example.com";
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

		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
