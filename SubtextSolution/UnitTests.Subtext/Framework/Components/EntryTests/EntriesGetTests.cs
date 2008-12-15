#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestFixture]
    public class EntriesGetTests
    {
        [SetUp]
        public void Setup()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "", "");
        }

        [Test]
        [RollBack2]
        public void GetRecentPostsIncludesEnclosure()
        {
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

            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get Entries
            ICollection<Entry> entries = Entries.GetRecentPosts(3, PostType.BlogPost, PostConfig.IsActive, true);

            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");

            Assert.AreEqual(1, entries.First().Categories.Count);
            Assert.AreEqual(5, entries.ElementAt(1).Categories.Count);
            Assert.AreEqual(5, entries.ElementAt(2).Categories.Count);

            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries.ElementAt(1).Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(2).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(2).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetEntriesByTagIncludesEnclosure()
        {
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");

            Entries.Create(entryZero);
            Entries.Create(entryOne);


            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            List<string> tags = new List<string>(new string[] { "Tag1", "Tag2" });
            new DatabaseObjectProvider().SetEntryTagList(entryZero.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryOne.Id, tags);


            ICollection<Entry> entries = Entries.GetEntriesByTag(3, "Tag1");

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Should have retrieved two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(1).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(1).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetEntriesByCategoryIncludesEnclosure()
        {
            //Create Category
            int blogId = Config.CurrentBlog.Id;
            int categoryId = UnitTestHelper.CreateCategory(blogId, "Test Category");

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");


            //Associate Category
            entryZero.Categories.Add("Test Category");
            entryOne.Categories.Add("Test Category");
            entryTwo.Categories.Add("Test Category");

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get Entries
            ICollection<Entry> entries = Entries.GetEntriesByCategory(3, categoryId, true);


            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");


            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries.ElementAt(1).Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(2).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(2).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetPostsByDayRangeIncludesEnclosure()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");


            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);


            //Get Entries
            DateTime beginningOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ICollection<Entry> entries = Entries.GetPostsByDayRange(beginningOfMonth, beginningOfMonth.AddMonths(1), PostType.BlogPost, true);


            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");


            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries.ElementAt(1).Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(2).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(2).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetPostCollectionByMonthIncludesEnclosure()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get Entries
            ICollection<Entry> entries = Entries.GetPostsByMonth(DateTime.Now.Month, DateTime.Now.Year);

            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");


            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries.ElementAt(1).Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(2).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(2).Enclosure);
        }
    }
}
