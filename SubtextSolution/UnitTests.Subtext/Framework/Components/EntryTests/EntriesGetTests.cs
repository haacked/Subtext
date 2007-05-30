using System;
using System.Collections.Generic;
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
		[RollBack]
		public void CanGetHomePageEntries()
		{
			UnitTestHelper.SetupBlog();
			ICollection<EntryDay> entries = Entries.GetHomePageEntries(10);
			Assert.AreEqual(0, entries.Count);
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Test", "Body Rockin");
			Entries.Create(entry);

			entries = Entries.GetHomePageEntries(10);
			Assert.AreEqual(1, entries.Count);
		}

		[Test]
		[RollBack]
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
		}

		[Test]
		[RollBack]
		public void CanGetRecentPosts()
		{
			UnitTestHelper.SetupBlog();

			int blogId = Config.CurrentBlog.Id;
			for (int i = 0; i < 10; i++)
				UnitTestHelper.CreateCategory(blogId, "cat" + i);

			//Create some entries.
			Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
			Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
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
			Assert.AreEqual(3, entries.Count, "Expected to find two entries.");
			
			Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
			Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
			Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");

			Assert.AreEqual(1, entries[0].Categories.Count);
			Assert.AreEqual(5, entries[1].Categories.Count);
			Assert.AreEqual(5, entries[2].Categories.Count);
		}
	}
}
