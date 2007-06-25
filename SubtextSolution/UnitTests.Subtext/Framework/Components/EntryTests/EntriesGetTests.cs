using System;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	[TestFixture]
	public class EntriesGetTests
	{
		[Test]
		[RollBack2]
		public void CanGetPostCollectionByMonth()
		{
			UnitTestHelper.SetupBlog();
			IList<Entry> entries = Entries.GetPostCollectionByMonth(DateTime.Now.Month, DateTime.Now.Year);
			Assert.AreEqual(0, entries.Count);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			entry.DateSyndicated = DateTime.Now;
			Entries.Create(entry);
			
			//Create one a couple months ago.
			entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			entry.DateSyndicated = DateTime.Now.AddMonths(-2);
			Entries.Create(entry);

			entries = Entries.GetPostCollectionByMonth(DateTime.Now.Month, DateTime.Now.Year);
			Assert.AreEqual(1, entries.Count);
		}

		[Test]
		[RollBack2]
		public void CanGetPostByCategoryId()
		{
			UnitTestHelper.SetupBlog();
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "Test");
			ICollection<EntryDay> entries = Entries.GetPostsByCategoryID(10, categoryId);
			Assert.AreEqual(0, entries.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			int entryId = Entries.Create(entry);
			Entries.SetEntryCategoryList(entryId, new int[] {categoryId});

			//Create one without a category
			entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			Entries.Create(entry);

			entries = Entries.GetPostsByCategoryID(10, categoryId);
			Assert.AreEqual(1, entries.Count);
		}

		[Test]
		[RollBack2]
		public void CanGetPostByDayRange()
		{
			UnitTestHelper.SetupBlog();
			bool activeOnly = true;
			IList<Entry> entries = Entries.GetPostsByDayRange(DateTime.Now.Date, DateTime.Now.Date.AddDays(1), PostType.BlogPost, activeOnly);
			Assert.AreEqual(0, entries.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			entry.IsActive = true;
			entry.DateCreated = DateTime.Now;
			Entries.Create(entry);

			//Create an inactive one for today.
			entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			entry.IsActive = false;
			entry.DateCreated = DateTime.Now;
			Entries.Create(entry);

			//Create an active one, but for yesterday.
			entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test2", "Body Rockin2");
			entry.IsActive = true;
			entry.DateSyndicated = DateTime.Now.AddDays(-1);
			Entries.Create(entry);

			entries = Entries.GetPostsByDayRange(DateTime.Now.Date, DateTime.Now.Date.AddDays(1), PostType.BlogPost, activeOnly);
			Assert.AreEqual(1, entries.Count);
		}

		[Test]
		[RollBack2]
		public void CanGetSingleDay()
		{
			UnitTestHelper.SetupBlog();
            EntryDay entries = Entries.GetSingleDay(DateTime.Today);
			Assert.AreEqual(0, entries.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
            entry.DateSyndicated = DateTime.Today;
			Entries.Create(entry);

			//Create one for yesterday.
			entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test 2", "Body Rockin Twice in another day!");
            entry.DateSyndicated = DateTime.Today.AddDays(-1);
			Entries.Create(entry);

            entries = Entries.GetSingleDay(DateTime.Today);
			Assert.AreEqual(1, entries.Count);

            entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test 2", "Body Rockin Twice in another day!");
            entry.DateSyndicated = DateTime.Now.AddMinutes(5);
            Entries.Create(entry);
            entries = Entries.GetSingleDay(DateTime.Today);
            Assert.AreEqual(1, entries.Count, "Future entries should be ignored.");
		}

		[Test]
		[RollBack2]
		public void CanGetHomePageEntries()
		{
			UnitTestHelper.SetupBlog();
			ICollection<EntryDay> entries = Entries.GetHomePageEntries(10);
			Assert.AreEqual(0, entries.Count);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			Entries.Create(entry);

			entries = Entries.GetHomePageEntries(10);
			Assert.AreEqual(1, entries.Count);

            // Make sure that future entries aren't included.
            entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test 2", "Body Rockin part 2");
            entry.DateSyndicated = DateTime.Now.AddMinutes(5);
            Entries.Create(entry);
            entries = Entries.GetHomePageEntries(10);
            Assert.AreEqual(1, entries.Count, "Future entries shouldn't be included on the home page");            
		}

		[Test]
		[RollBack2]
		public void CanGetEntriesByCategory()
		{
			UnitTestHelper.SetupBlog();
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "Category42");

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			int entryId = Entries.Create(entry);
			ICollection<Entry> entries = Entries.GetEntriesByCategory(int.MaxValue, categoryId, false);
			Assert.AreEqual(0, entries.Count);
			
			Entries.SetEntryCategoryList(entryId, new int[] {categoryId});

			entries = Entries.GetEntriesByCategory(10, categoryId, false);
			Assert.AreEqual(1, entries.Count);

            // Make sure future entries aren't included.
            entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test 2", "Body Rockin part 2");
            entry.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(5);
            entryId = Entries.Create(entry);
            Entries.SetEntryCategoryList(entryId, new int[] { categoryId });

            entries = Entries.GetEntriesByCategory(10, categoryId, false);
            Assert.AreEqual(1, entries.Count, "Future entries shouldn't be included in the category list");
		}

		[Test]
		[RollBack2]
		public void CanGetRecentPosts()
		{
			UnitTestHelper.SetupBlog();

			int blogId = Config.CurrentBlog.Id;
			for (int i = 0; i < 10; i++)
				UnitTestHelper.CreateCategory(blogId, "cat" + i);

			//Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
			Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
			
			//Associate categories.
			for (int i = 0; i < 5; i++)
			{
				entryZero.Categories.Add("cat" + (i + 1));
				entryOne.Categories.Add("cat" + i);
			}
			entryTwo.Categories.Add("cat8");
			
			//Persist entries.
			Entries.Create(entryZero);
			Entries.Create(entryOne);
			Entries.Create(entryTwo);

			IList<Entry> entries = Entries.GetRecentPosts(3, PostType.BlogPost, PostConfig.IsActive, true);
			Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
			
			Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
			Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
			Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");

			Assert.AreEqual(1, entries[0].Categories.Count);
			Assert.AreEqual(5, entries[1].Categories.Count);
			Assert.AreEqual(5, entries[2].Categories.Count);

            //Future entries shouldn't be included
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(5);
            Entries.Update(entryTwo);
            entries = Entries.GetRecentPosts(3, PostType.BlogPost, PostConfig.IsActive, true);
            Assert.AreEqual(2, entries.Count, "Most recent entry has a future date and should not be included.");
		}
	}
}
