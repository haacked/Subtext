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
		[RollBack2]
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
		[RollBack2]
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

        [Test]
        [RollBack2]
        public void UpdateEntryCorrectsNumericEntryName()
        {
        	UnitTestHelper.SetupBlog();
            BlogInfo info = Config.CurrentBlog;
            Config.UpdateConfigData(info);

            Entry entry = new Entry(PostType.BlogPost);
        	entry.DateCreated = DateTime.Now;
            entry.Title = "My Title";
            entry.Body = "My Post Body";

            Entries.Create(entry);
            entry = Entries.GetEntry(entry.Id, PostConfig.None, false);

			entry.DateSyndicated = NullValue.NullDateTime;
			entry.IsActive = true;
			entry.IncludeInMainSyndication = true;
            entry.EntryName = "4321";
            Entries.Update(entry);
            Entry updatedEntry = Entries.GetEntry(entry.Id, PostConfig.None, false);

            Assert.AreEqual("n_4321", updatedEntry.EntryName, "Expected entryName = 'n_4321'");
        }

		[Test]
        [RollBack2]
        public void UpdateEntryWithNullDateSetsDate()
		{
			UnitTestHelper.SetupBlog();
			DateTime now = Config.CurrentBlog.TimeZone.Now;
			TestEntry entry = new TestEntry();
			entry.Title = "My Title";
			entry.Body = "My Post Body";
			Entries.Create(entry);
			
			entry.IsActive = true;
			entry.DateSyndicated = NullValue.NullDateTime;
			entry.SetIncludeInMainSyndicationWithoutChangingDateSyndicated();

			Assert.LowerThan(entry.DateSyndicated, now);
			Entries.Update(entry);
			Assert.GreaterEqualThan(entry.DateSyndicated, now);
		}

		internal class TestEntry : Entry
		{
			public TestEntry() : base(PostType.BlogPost)
			{
			}

			public void SetIncludeInMainSyndicationWithoutChangingDateSyndicated()
			{
				PostConfigSetter(PostConfig.IncludeInMainSyndication, true);
			}
		}

		[Test]
		[ExpectedArgumentNullException]
		public void UpdateThrowsArgumentNullException()
		{
			Entries.Update(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void UpdateWithCategoriesThrowsArgumentNullException()
		{
			Entries.Update(null, 1, 2);
		}

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
