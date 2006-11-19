using System;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	/// <summary>
	/// Some unit tests around updating entries.
	/// </summary>
	[TestFixture]
	public class EntryUpdateTests
	{
		[Test]
		[RollBack]
		public void CanDeleteEntry()
		{
			UnitTestHelper.SetupBlog();

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Haacked", "Title Test", "Body Rocking");
			Entries.Create(entry);

			Entry savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			Assert.IsNotNull(savedEntry);

			Entries.Delete(entry.Id);

			savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			Assert.IsNull(savedEntry, "Entry should now be null.");
		}
		
		/// <summary>
		/// Tests that setting the date syndicated to null removes the item from syndication.
		/// </summary>
		[Test]
		[RollBack]
		public void SettingDateSyndicatedToNullRemovesItemFromSyndication()
		{
			UnitTestHelper.SetupBlog();
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Haacked", "Title Test", "Body Rocking");
			Entries.Create(entry);

			Assert.IsTrue(entry.IncludeInMainSyndication, "Failed to setup this test properly.  This entry should be included in the main syndication.");
			Assert.IsFalse(NullValue.IsNull(entry.DateSyndicated), "Failed to setup this test properly. DateSyndicated should be not null.");
			
			entry.DateSyndicated = NullValue.NullDateTime;
			Assert.IsFalse(entry.IncludeInMainSyndication, "Setting the DateSyndicated to a null date should have reset 'IncludeInMainSyndication'.");
			
			//Ok, update and make sure these changes persist.
			Entries.Update(entry);

            Entry savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			Assert.IsFalse(savedEntry.IncludeInMainSyndication, "This item should still not be included in main syndication.");
			int allowableMarginOfError = 1; //ms
			Assert.IsTrue((savedEntry.DateSyndicated - entry.DateSyndicated).Milliseconds <= allowableMarginOfError, "The DateSyndicated was not stored in the db.");
			
			//Ok, make other changes.
			DateTime date = DateTime.Now;
			Thread.Sleep(1000);
			savedEntry.IncludeInMainSyndication = true;

			//Convert to UTC before comparing
			DateTime utcSyndicatedDate = Config.CurrentBlog.TimeZone.ToUniversalTime(savedEntry.DateSyndicated);
			Assert.IsTrue(utcSyndicatedDate >= date.ToUniversalTime(), "The DateSyndicated '{0}' should be updated to be later than '{1}.", savedEntry.DateSyndicated, date);
			Entries.Update(savedEntry);
            savedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);
			utcSyndicatedDate = Config.CurrentBlog.TimeZone.ToUniversalTime(savedEntry.DateSyndicated);
			Assert.IsTrue(utcSyndicatedDate >= date.ToUniversalTime(), "The DateSyndicated '{0}' should be updated to be later than '{1}.", savedEntry.DateSyndicated, date);
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
