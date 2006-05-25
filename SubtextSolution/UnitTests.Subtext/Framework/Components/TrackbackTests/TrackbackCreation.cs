using System;
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
		string _hostName = string.Empty;
		
		/// <summary>
		/// We had a problem in which creating a trackback in the database did 
		/// not set the PostConfig bitmask column correctly.  Thus we could not 
		/// select out the trackbacks.
		/// </summary>
		[Test]
		[RollBack]
		public void CreateTrackbackSetsPostConfigCorrectly()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			int parentId = Entries.Create(entry);
			
			Trackback trackback = new Trackback(parentId, "title", "titleUrl", "phil", "body");
			int id = Entries.Create(trackback);
			
			Entry loadedTrackback = Entries.GetEntry(id, EntryGetOption.All);
			Assert.IsNotNull(loadedTrackback, "Was not able to load trackback from storage.");
			Assert.IsTrue(loadedTrackback.IsActive, "This item is active");
			Assert.IsTrue(loadedTrackback.PostConfig > 0, "PostConfig was 0");
			
			Entry activeTrackback = Entries.GetEntry(id, EntryGetOption.ActiveOnly);
			Assert.IsNotNull(activeTrackback, "The trackback was not active.");
		}
		
		/// <summary>
		/// Make sure that trackbacks show up when displaying feedback for an entry.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackbackShowsUpInFeedbackList()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));
			
			Entry parentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			int parentId = Entries.Create(parentEntry);
			
			EntryCollection entries = Entries.GetFeedBack(parentEntry);
			Assert.AreEqual(0, entries.Count, "Did not expect any feedback yet.");
			
			Trackback trackback = new Trackback(parentId, "title", "titleUrl", "phil", "body");
			int trackbackId = Entries.Create(trackback);
			
			entries = Entries.GetFeedBack(parentEntry);
			Assert.AreEqual(1, entries.Count, "Expected a trackback.");
			Assert.AreEqual(trackbackId, entries[0].EntryID, "The feedback was not the same one we expected. The IDs do not match.");
		}
		
		[SetUp]
		public void SetUp()
		{
			_hostName = System.Guid.NewGuid().ToString().Replace("-", "") + ".com";
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
