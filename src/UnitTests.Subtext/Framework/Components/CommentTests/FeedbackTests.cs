using System;
using System.Collections;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;

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
        [RollBack2]
        public void CanCreateCommentWithStatus(FeedbackStatusFlag status, bool expectedApproved,
                                               bool expectedNeedsModeratorApproval, bool expectedDeleted,
                                               bool expectedFlaggedAsSpam)
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();

            FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, status);

            Assert.IsTrue((comment.Status & status) == status, "Expected the " + status + "bit to be set.");
            Assert.AreEqual(expectedApproved, comment.Approved, "We expected 'Approved' to be " + expectedApproved);
            Assert.AreEqual(expectedNeedsModeratorApproval, comment.NeedsModeratorApproval,
                            "Expected 'NeedsModeratorApproval' to be " + expectedNeedsModeratorApproval);
            Assert.AreEqual(expectedDeleted, comment.Deleted, "Expected 'Deleted' to be " + expectedDeleted);
            Assert.AreEqual(expectedFlaggedAsSpam,
                            ((comment.Status & FeedbackStatusFlag.FlaggedAsSpam) == FeedbackStatusFlag.FlaggedAsSpam),
                            "Expected that this item was ever flagged as spam to be " + expectedFlaggedAsSpam);
        }

        [Test]
        [RollBack2]
        public void ConfirmSpamRemovesApprovedBitAndSetsDeletedBit()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            var repository = new DatabaseObjectProvider();
            FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                          FeedbackStatusFlag.Approved);
            Assert.IsTrue(comment.Approved, "should be approved");

            repository.ConfirmSpam(comment, null);
            comment = repository.Get(comment.Id);
            Assert.IsFalse(comment.Approved, "Should not be approved now.");
            Assert.IsTrue(comment.Deleted, "Should be moved to deleted folder now.");
        }

        [Test]
        [RollBack2]
        public void DeleteCommentSetsDeletedBit()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            var repository = new DatabaseObjectProvider();
            FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            Assert.IsTrue(comment.Approved, "should be approved");

            repository.Delete(comment);
            comment = repository.Get(comment.Id);
            Assert.IsFalse(comment.Approved, "Should not be approved now.");
            Assert.IsTrue(comment.Deleted, "Should be moved to deleted folder now.");
        }

        [Test]
        [RollBack2]
        public void DestroyCommentByStatusDestroysOnlyThatStatus()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            var repository = new DatabaseObjectProvider();
            CreateApprovedComments(3, entry);
            CreateFlaggedSpam(2, entry);
            CreateDeletedComments(3, entry);

            FeedbackItem newComment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                             FeedbackStatusFlag.Approved);
            repository.ConfirmSpam(newComment, null);
            newComment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                FeedbackStatusFlag.FlaggedAsSpam);
            Assert.IsFalse(newComment.Approved, "should not be approved");
            repository.Delete(newComment); //Move it to trash.

            FeedbackCounts counts = repository.GetFeedbackCounts();
            Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
            Assert.AreEqual(2, counts.FlaggedAsSpamCount, "Expected two items flagged as spam.");
            Assert.AreEqual(5, counts.DeletedCount, "Expected five in the trash");

            repository.Destroy(FeedbackStatusFlag.FlaggedAsSpam);
            counts = repository.GetFeedbackCounts();
            Assert.AreEqual(3, counts.ApprovedCount, "Expected three approved still");
            Assert.AreEqual(0, counts.FlaggedAsSpamCount, "Expected the items flagged as spam to be gone.");
            Assert.AreEqual(5, counts.DeletedCount, "Destroying all flagged items should not touch the trash bin.");

            CreateFlaggedSpam(3, entry);
            counts = repository.GetFeedbackCounts();
            Assert.AreEqual(3, counts.FlaggedAsSpamCount, "Expected three items flagged as spam.");

            repository.Destroy(FeedbackStatusFlag.Deleted);
            counts = repository.GetFeedbackCounts();
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
        [RollBack2]
        public void CreateFeedbackSetsBlogStatsCorrectly()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            Blog info = Config.CurrentBlog;

            Assert.AreEqual(0, info.CommentCount);
            Assert.AreEqual(0, info.PingTrackCount);

            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(info.Host, info.Subfolder); // pull back the updated info from the datastore.
            Assert.AreEqual(0, info.CommentCount);
            Assert.AreEqual(0, info.PingTrackCount);

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(info.Host, info.Subfolder);
            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(info.Host, info.Subfolder);
            Assert.AreEqual(2, info.CommentCount, "Blog CommentCount should be 2");
            Assert.AreEqual(2, info.PingTrackCount, "Blog Ping/Trackback count should be 2");
        }

        [Test]
        [RollBack2]
        public void CreateEntryDoesNotResetBlogStats()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            Blog info = Config.CurrentBlog;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            Entry entry2 = UnitTestHelper.CreateEntryInstanceForSyndication("johnny b goode", "foo-bar", "zaa zaa zoo.");
            UnitTestHelper.Create(entry2);
            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(info.Host, info.Subfolder); // pull back the updated info from the datastore

            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");
        }

        [Test]
        [RollBack2]
        public void DeleteEntrySetsBlogStats()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            Blog info = Config.CurrentBlog;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.PingTrack, FeedbackStatusFlag.Approved);

            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(info.Host, info.Subfolder);
            Assert.AreEqual(1, info.CommentCount, "Blog CommentCount should be 1");
            Assert.AreEqual(1, info.PingTrackCount, "Blog Ping/Trackback count should be 1");

            ObjectProvider.Instance().DeleteEntry(entry.Id);
            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(info.Host, info.Subfolder);

            Assert.AreEqual(0, info.CommentCount, "Blog CommentCount should be 0");
            Assert.AreEqual(0, info.PingTrackCount, "Blog Ping/Trackback count should be 0");
        }

        [Test]
        public void DestroyCommentCannotDestroyActiveComment()
        {
            // arrange
            var comment = new FeedbackItem(FeedbackType.Comment) { Approved = true };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository.GetFeedback(123)).Returns(comment);
            var service = new CommentService(context.Object, null);

            // act, assert
            UnitTestHelper.AssertThrows<InvalidOperationException>(() => service.Destroy(123));
        }

        [Test]
        [RollBack2]
        public void ApproveCommentRemovesDeletedAndConfirmedSpamBits()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            var repository = new DatabaseObjectProvider();
            FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                          FeedbackStatusFlag.ConfirmedSpam |
                                                                          FeedbackStatusFlag.Deleted);
            Assert.IsFalse(comment.Approved, "should not be approved");
            Assert.IsTrue(comment.Deleted, "should be deleted");
            Assert.IsTrue(comment.ConfirmedSpam, "should be confirmed spam");

            repository.Approve(comment, null);
            comment = repository.Get(comment.Id);
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
            var repository = new DatabaseObjectProvider();
            FeedbackItem commentOne = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                             FeedbackStatusFlag.Approved);
            FeedbackItem commentTwo = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                             FeedbackStatusFlag.ApprovedByModerator);
            FeedbackItem commentThree = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                               FeedbackStatusFlag.ConfirmedSpam);
            repository.ConfirmSpam(commentThree, null);
            FeedbackItem commentFour = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                              FeedbackStatusFlag.FalsePositive);

            //We expect three of the four.
            IPagedCollection<FeedbackItem> feedback = repository.GetPagedFeedback(0, 10,
                                                                                FeedbackStatusFlag.
                                                                                    Approved,
                                                                                FeedbackStatusFlag.None,
                                                                                FeedbackType.Comment);
            Assert.AreEqual(3, feedback.Count, "We expected three to match.");

            //Expect reverse order
            Assert.AreEqual(commentOne.Id, feedback[2].Id, "The first does not match");
            Assert.AreEqual(commentTwo.Id, feedback[1].Id, "The first does not match");
            Assert.AreEqual(commentFour.Id, feedback[0].Id, "The first does not match");
        }

        [Test]
        [RollBack2]
        public void OnlyApprovedItemsContributeToEntryFeedbackCount()
        {
            Entry entry = SetupBlogForCommentsAndCreateEntry();
            var repository = new DatabaseObjectProvider();
            int entryId = entry.Id;

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            entry = UnitTestHelper.GetEntry(entryId, PostConfig.None, false);
            Assert.AreEqual(1, entry.FeedBackCount, "Expected one approved feedback entry.");

            FeedbackItem comment = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                          FeedbackStatusFlag.FlaggedAsSpam);
            entry = UnitTestHelper.GetEntry(entryId, PostConfig.None, false);
            Assert.AreEqual(1, entry.FeedBackCount, "Expected one approved feedback entry.");

            comment.Approved = true;
            repository.Update(comment);
            entry = UnitTestHelper.GetEntry(entryId, PostConfig.None, false);
            Assert.AreEqual(2, entry.FeedBackCount,
                            "After approving the second comment, expected two approved feedback entry.");

            comment.Approved = false;
            repository.Update(comment);
            entry = UnitTestHelper.GetEntry(entryId, PostConfig.None, false);
            Assert.AreEqual(1, entry.FeedBackCount,
                            "After un-approving the second comment, expected one approved feedback entry.");

            repository.Delete(comment);
            entry = UnitTestHelper.GetEntry(entryId, PostConfig.None, false);
            Assert.AreEqual(1, entry.FeedBackCount,
                            "After un-approving the second comment, expected one approved feedback entry.");
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

            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.FalsePositive);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.Approved);
            CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment, FeedbackStatusFlag.ConfirmedSpam);
            FeedbackItem included = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                           FeedbackStatusFlag.FlaggedAsSpam);
            FeedbackItem includedToo = CreateAndUpdateFeedbackWithExactStatus(entry, FeedbackType.Comment,
                                                                              FeedbackStatusFlag.FlaggedAsSpam |
                                                                              FeedbackStatusFlag.NeedsModeration);

            //We expect 2 of the four.
            IPagedCollection<FeedbackItem> feedback = ObjectProvider.Instance().GetPagedFeedback(0, 10,
                                                                                                 FeedbackStatusFlag.
                                                                                                     FlaggedAsSpam,
                                                                                                 FeedbackStatusFlag.
                                                                                                     Approved |
                                                                                                 FeedbackStatusFlag.
                                                                                                     Deleted,
                                                                                                 FeedbackType.Comment);
            Assert.AreEqual(2, feedback.Count, "We expected two to match.");

            //Expect reverse order
            Assert.AreEqual(included.Id, feedback[1].Id, "The first does not match");
            Assert.AreEqual(includedToo.Id, feedback[0].Id, "The second does not match");
        }

        /// <summary>
        /// Makes sure that the content checksum hash is being created correctly.
        /// </summary>
        [Test]
        public void ChecksumHashReturnsChecksumOfCommentBody()
        {
            var comment = new FeedbackItem(FeedbackType.Comment) { Body = "Some Body" };
            Console.WriteLine(comment.ChecksumHash);
            Assert.AreEqual("834.5baPHSvKBNtABZePE+OpeQ==", comment.ChecksumHash);
        }

        static FeedbackItem CreateAndUpdateFeedbackWithExactStatus(Entry entry, FeedbackType type,
                                                                   FeedbackStatusFlag status)
        {
            var repository = new DatabaseObjectProvider();
            var feedback = new FeedbackItem(type);
            feedback.Title = UnitTestHelper.GenerateUniqueString();
            feedback.Body = UnitTestHelper.GenerateUniqueString();
            feedback.EntryId = entry.Id;
            feedback.Author = "TestAuthor";

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Cache).Returns(new TestCache());
            subtextContext.SetupBlog(Config.CurrentBlog);
            subtextContext.SetupRepository(ObjectProvider.Instance());
            subtextContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            subtextContext.Setup(c => c.HttpContext).Returns(new HttpContextWrapper(HttpContext.Current));

            var service = new CommentService(subtextContext.Object, null);
            int id = service.Create(feedback, true/*runFilters*/);

            feedback = repository.Get(id);
            feedback.Status = status;
            repository.Update(feedback);

            return repository.Get(id);
        }

        Entry SetupBlogForCommentsAndCreateEntry()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog(string.Empty, "username", "password", _hostName, string.Empty);
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName, string.Empty);
            BlogRequest.Current.Blog = info;
            info.Email = "test@example.com";
            info.Title = "You've been haacked";
            info.CommentsEnabled = true;
            info.ModerationEnabled = false;

            ObjectProvider.Instance().UpdateConfigData(info);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("blah", "blah", "blah");
            UnitTestHelper.Create(entry);
            return entry;
        }

        [SetUp]
        public void SetUp()
        {
            _hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void UpdateThrowsArgumentNull()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => new DatabaseObjectProvider().Update((FeedbackItem)null));
        }

        [Test]
        public void ApproveThrowsArgumentNull()
        {
            // arrange
            var service = new Mock<ICommentSpamService>().Object;

            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new DatabaseObjectProvider().Approve(null, service));
        }

        [Test]
        public void ConfirmSpamThrowsArgumentNull()
        {
            // arrange
            var service = new Mock<ICommentSpamService>().Object;

            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new DatabaseObjectProvider().ConfirmSpam(null, service));
        }

        [Test]
        public void DeleteNullCommentThrowsArgumentNull()
        {
            // arrange
            var service = new Mock<ICommentSpamService>().Object;

            // act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => new DatabaseObjectProvider().Delete((FeedbackItem)null));
        }
    }
}