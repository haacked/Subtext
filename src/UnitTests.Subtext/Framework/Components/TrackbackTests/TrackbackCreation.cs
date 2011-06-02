using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.TrackbackTests
{
    /// <summary>
    /// Summary description for TrackbackCreation.
    /// </summary>
    [TestFixture]
    public class TrackbackCreation
    {
        /// <summary>
        /// We had a problem in which creating a trackback in the database did 
        /// not set the PostConfig bitmask column correctly.  Thus we could not 
        /// select out the trackbacks.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreateTrackbackSetsFeedbackTypeCorrectly()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
            Blog blog = repository.GetBlog(hostname, string.Empty);
            BlogRequest.Current.Blog = blog;

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
            int parentId = UnitTestHelper.Create(entry);

            var trackback = new Trackback(parentId, "title", new Uri("http://url"), "phil", "body");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(repository);
            subtextContext.Setup(c => c.Cache).Returns(new TestCache());
            subtextContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            var commentService = new CommentService(subtextContext.Object, null);
            int id = commentService.Create(trackback, true/*runFilters*/);

            FeedbackItem loadedTrackback = repository.Get(id);
            Assert.IsNotNull(loadedTrackback, "Was not able to load trackback from storage.");
            Assert.AreEqual(FeedbackType.PingTrack, loadedTrackback.FeedbackType, "Feedback should be a PingTrack");
        }

        /// <summary>
        /// Make sure that trackbacks show up when displaying feedback for an entry.
        /// </summary>
        [Test]
        [RollBack2]
        public void TrackbackShowsUpInFeedbackList()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("", "username", "password", hostname, "blog");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "blog", string.Empty);
            Blog blog = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(hostname, "blog");
            BlogRequest.Current.Blog = blog;

            Entry parentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("philsath aeuoa asoeuhtoensth",
                                                                                 "sntoehu title aoeuao eu",
                                                                                 "snaot hu aensaoehtu body");
            int parentId = UnitTestHelper.Create(parentEntry);

            ICollection<FeedbackItem> entries = ObjectProvider.Instance().GetFeedbackForEntry(parentEntry);
            Assert.AreEqual(0, entries.Count, "Did not expect any feedback yet.");

            var trackback = new Trackback(parentId, "title", new Uri("http://url"), "phil", "body");
            Config.CurrentBlog.DuplicateCommentsEnabled = true;
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Cache).Returns(new TestCache());
            subtextContext.SetupBlog(Config.CurrentBlog);
            subtextContext.SetupRepository(ObjectProvider.Instance());
            subtextContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            var commentService = new CommentService(subtextContext.Object, null);
            int trackbackId = commentService.Create(trackback, true/*runFilters*/);

            new DatabaseObjectProvider().Approve(trackback, null);

            entries = ObjectProvider.Instance().GetFeedbackForEntry(parentEntry);
            Assert.AreEqual(1, entries.Count, "Expected a trackback.");
            Assert.AreEqual(trackbackId, entries.First().Id,
                            "The feedback was not the same one we expected. The IDs do not match.");
        }
    }
}