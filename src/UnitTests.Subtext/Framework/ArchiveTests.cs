using System;
using System.Collections.Generic;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework
{
    [TestFixture]
    public class ArchiveTests
    {
        [Test]
        [RollBack2]
        public void CanGetPostsByYearArchive()
        {
            DateTime utcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToUniversalTime();

            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            ICollection<ArchiveCount> counts = repository.GetPostCountsByYear();
            Assert.AreEqual(0, counts.Count);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title", "body");
            entry.DatePublishedUtc = utcNow;
            UnitTestHelper.Create(entry);
            counts = repository.GetPostCountsByYear();
            Assert.AreEqual(1, counts.Count);

            foreach (ArchiveCount archiveCount in counts)
            {
                Console.WriteLine("archiveCount.Date: " + archiveCount.Date);

                Assert.AreEqual(1, archiveCount.Count, "Expected one post in the archive.");
                Assert.AreEqual(utcNow.Year, archiveCount.Date.Year);
                return;
            }
        }

        [Test]
        [RollBack2]
        public void CanGetPostsByCategoryArchive()
        {
            UnitTestHelper.SetupBlog();
            var repository = ObjectProvider.Instance();
            ICollection<ArchiveCount> counts = repository.GetPostCountsByCategory();
            Assert.AreEqual(0, counts.Count);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title", "body");
            int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "Test");
            int entryId = UnitTestHelper.Create(entry);
            repository.SetEntryCategoryList(entryId, new[] { categoryId });
            counts = repository.GetPostCountsByCategory();
            Assert.AreEqual(1, counts.Count);

            foreach (ArchiveCount archiveCount in counts)
            {
                Assert.AreEqual(1, archiveCount.Count, "Expected one post in the archive.");
                return;
            }
        }
    }
}