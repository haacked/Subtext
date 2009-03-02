using System;
using System.Linq;
using System.Collections.Generic;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
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
		[RollBack]
		public void CreateTrackbackSetsFeedbackTypeCorrectly()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
            Blog blog = Config.GetBlog(hostname, string.Empty);
            BlogRequest.Current.Blog = blog;

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			int parentId = UnitTestHelper.Create(entry);

            Trackback trackback = new Trackback(parentId, "title", new Uri("http://url"), "phil", "body", blog.TimeZone.Now);
			int id = FeedbackItem.Create(trackback, null, blog);

			FeedbackItem loadedTrackback = FeedbackItem.Get(id);
			Assert.IsNotNull(loadedTrackback, "Was not able to load trackback from storage.");
			Assert.AreEqual(FeedbackType.PingTrack, loadedTrackback.FeedbackType, "Feedback should be a PingTrack");
		}
		
		/// <summary>
		/// Make sure that trackbacks show up when displaying feedback for an entry.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackbackShowsUpInFeedbackList()
		{
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("", "username", "password", hostname, "blog");
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "blog", string.Empty);
            Blog blog = Config.GetBlog(hostname, "blog");
            BlogRequest.Current.Blog = blog;
			
			Entry parentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("philsath aeuoa asoeuhtoensth", "sntoehu title aoeuao eu", "snaot hu aensaoehtu body");
			int parentId = UnitTestHelper.Create(parentEntry);

            ICollection<FeedbackItem> entries = Entries.GetFeedBack(parentEntry);
			Assert.AreEqual(0, entries.Count, "Did not expect any feedback yet.");
			
			Trackback trackback = new Trackback(parentId, "title", new Uri("http://url"), "phil", "body", blog.TimeZone.Now);
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			int trackbackId = FeedbackItem.Create(trackback, null, blog);
			FeedbackItem.Approve(trackback, null);
			
			entries = Entries.GetFeedBack(parentEntry);
			Assert.AreEqual(1, entries.Count, "Expected a trackback.");
			Assert.AreEqual(trackbackId, entries.First().Id, "The feedback was not the same one we expected. The IDs do not match.");
		}
	}
}
