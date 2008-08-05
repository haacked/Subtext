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
    public class FutureEntriesGetTests
    {
        const int PacificTimeZoneId = -2037797565;
        const int HawaiiTimeZoneId = 1106595067;

        [SetUp]
        public void Setup()
        {
            string hostname = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, ""));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "", "");
            Config.CurrentBlog.TimeZoneId = HawaiiTimeZoneId;
        }

        [Test]
        [RollBack2]
        public void GetRecentPostsDoesNotIncludeFuturePosts()
        {

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, NullValue.NullDateTime);

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            Entries.Create(entryTwo);

            //Get Entries
            IList<Entry> entries = Entries.GetRecentPosts(3, PostType.BlogPost, PostConfig.IsActive, true);

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = PacificTimeZoneId;

            //Get Entries
            entries = Entries.GetRecentPosts(3, PostType.BlogPost, PostConfig.IsActive, true);

            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetEntriesByTagDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, NullValue.NullDateTime);

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            Entries.Create(entryTwo);


            List<string> tags = new List<string>(new string[] { "Tag1", "Tag2" });
            new DatabaseObjectProvider().SetEntryTagList(entryZero.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryOne.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryTwo.Id, tags);


            IList<Entry> entries = Entries.GetEntriesByTag(3, "Tag1");

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = PacificTimeZoneId;

            //Test outcome
            entries = Entries.GetEntriesByTag(3, "Tag1");

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetEntriesByCategoryDoesNotIncludeFuturePosts()
        {
            //Create Category
            int blogId = Config.CurrentBlog.Id;
            int categoryId = UnitTestHelper.CreateCategory(blogId, "Test Category");

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, NullValue.NullDateTime);

            //Associate Category
            entryZero.Categories.Add("Test Category");
            entryOne.Categories.Add("Test Category");
            entryTwo.Categories.Add("Test Category");

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            Entries.Create(entryTwo);


            //Get Entries
            IList<Entry> entries = Entries.GetEntriesByCategory(3, categoryId, true);


            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = PacificTimeZoneId;

            //Test outcome
            entries = Entries.GetEntriesByCategory(3, categoryId, true);

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetPostsByDayRangeDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, NullValue.NullDateTime);


            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            Entries.Create(entryTwo);



            //Get Entries
            DateTime beginningOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            IList<Entry> entries = Entries.GetPostsByDayRange(beginningOfMonth, beginningOfMonth.AddMonths(1), PostType.BlogPost, true);

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = PacificTimeZoneId;
            entries = Entries.GetPostsByDayRange(beginningOfMonth, beginningOfMonth.AddMonths(1), PostType.BlogPost, true);

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");

        }

        [Test]
        [RollBack2]
        public void GetPostCollectionByMonthDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, NullValue.NullDateTime);


            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            Entries.Create(entryTwo);


            //Get Entries
            IList<Entry> entries = Entries.GetPostCollectionByMonth(DateTime.Now.Month, DateTime.Now.Year);


            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = PacificTimeZoneId;
            entries = Entries.GetPostCollectionByMonth(DateTime.Now.Month, DateTime.Now.Year);

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");


        }
    }
}
