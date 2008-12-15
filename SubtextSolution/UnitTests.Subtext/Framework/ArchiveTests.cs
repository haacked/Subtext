using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class ArchiveTests
	{
		[Test]
		[RollBack2]
		public void CanGetPostsByMonthArchive()
		{
			UnitTestHelper.SetupBlog();
			ICollection<ArchiveCount> counts = Archives.GetPostCountByMonth();
			Assert.AreEqual(0, counts.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title", "body");
			Entries.Create(entry);
			counts = Archives.GetPostCountByMonth();
			Assert.AreEqual(1, counts.Count);
			
			foreach(ArchiveCount archiveCount in counts)
			{
				Assert.AreEqual(1, archiveCount.Count, "Expected one post in the archive.");
				Assert.AreEqual(DateTime.Now.Date.AddDays(-1 * DateTime.Now.Date.Day + 1), archiveCount.Date, "Expected date to be this month.");
				Assert.AreEqual(null, archiveCount.Title);
				Assert.AreEqual(0, archiveCount.Id);
				return;
			}
		}

		[Test]
		[RollBack2]
		public void CanGetPostsByYearArchive()
		{
			UnitTestHelper.SetupBlog();
			ICollection<ArchiveCount> counts = Archives.GetPostCountByYear();
			Assert.AreEqual(0, counts.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title", "body");
            entry.DateSyndicated = DateTime.Now.AddDays(-1);
            Entries.Create(entry);
			counts = Archives.GetPostCountByYear();
			Assert.AreEqual(1, counts.Count);

			foreach (ArchiveCount archiveCount in counts)
			{
				Assert.AreEqual(1, archiveCount.Count, "Expected one post in the archive.");
				Assert.AreEqual(DateTime.Now.Date.AddDays(-1 * DateTime.Now.DayOfYear + 1), archiveCount.Date, "Expected date to be this month.");
				Assert.AreEqual(null, archiveCount.Title);
				Assert.AreEqual(0, archiveCount.Id);
				return;
			}
		}

		[Test]
		[RollBack2]
		public void CanGetPostsByCategoryArchive()
		{
			UnitTestHelper.SetupBlog();
			ICollection<ArchiveCount> counts = Archives.GetPostCountByCategory();
			Assert.AreEqual(0, counts.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title", "body");
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "Test");
			int entryId = Entries.Create(entry);
			Entries.SetEntryCategoryList(entryId, new int[] { categoryId });
			counts = Archives.GetPostCountByCategory();
			Assert.AreEqual(1, counts.Count);

			foreach (ArchiveCount archiveCount in counts)
			{
				Assert.AreEqual(1, archiveCount.Count, "Expected one post in the archive.");
				archiveCount.Title = "Test";
				Assert.AreEqual("Test", archiveCount.Title);
				archiveCount.Id = 10;
				Assert.AreEqual(10, archiveCount.Id);
				return;
			}
		}
	}
}
