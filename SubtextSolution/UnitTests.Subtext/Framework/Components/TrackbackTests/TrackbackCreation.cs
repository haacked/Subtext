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
		/// <summary>
		/// We had a problem in which creating a trackback in the database did 
		/// not set the PostConfig bitmask column correctly.  Thus we could not 
		/// select out the trackbacks.
		/// </summary>
		[Test]
		[RollBack]
		public void CreateTrackbackSetsPostConfigCorrectly()
		{
			string hostname = UnitTestHelper.GenerateRandomHostname();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			
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
	}
}
