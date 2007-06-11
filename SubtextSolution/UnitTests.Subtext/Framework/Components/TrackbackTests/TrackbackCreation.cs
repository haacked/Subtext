using System;
using System.Collections.Generic;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using MbUnit.Framework;
using Subtext.Framework.Configuration;

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
			UnitTestHelper.SetupBlog();
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			int parentId = Entries.Create(entry);
			
			Trackback trackback = new Trackback(parentId, "title", new Uri("http://url"), "phil", "body");
			int id = FeedbackItem.Create(trackback, null);

			FeedbackItem loadedTrackback = FeedbackItem.Get(id);
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
			UnitTestHelper.SetupBlog("blog");
			
			Entry parentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("philsath aeuoa asoeuhtoensth", "sntoehu title aoeuao eu", "snaot hu aensaoehtu body");
			int parentId = Entries.Create(parentEntry);

            IList<FeedbackItem> entries = Entries.GetFeedBack(parentEntry);
			Assert.AreEqual(0, entries.Count, "Did not expect any feedback yet.");
			
			Trackback trackback = new Trackback(parentId, "title", new Uri("http://url"), "phil", "body");
			Config.CurrentBlog.DuplicateCommentsEnabled = true;
			int trackbackId = FeedbackItem.Create(trackback, null);
			FeedbackItem.Approve(trackback);
			
			entries = Entries.GetFeedBack(parentEntry);
			Assert.AreEqual(1, entries.Count, "Expected a trackback.");
			Assert.AreEqual(trackbackId, entries[0].Id, "The feedback was not the same one we expected. The IDs do not match.");
		}
	}
}
