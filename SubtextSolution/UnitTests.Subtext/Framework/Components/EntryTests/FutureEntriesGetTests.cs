#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Web.HttpModules;
using UnitTests.Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestFixture]
    public class FutureEntriesGetTests
    {
        [SetUp]
        public void Setup()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "", "");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);
            Config.CurrentBlog.TimeZoneId = TimeZonesTest.HawaiiTimeZoneId;
        }

        [Test]
        [RollBack2]
        public void GetRecentPostsDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null,
                                                                               NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null,
                                                                              NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null,
                                                                              NullValue.NullDateTime);

            //Persist entries.
            UnitTestHelper.Create(entryZero);
            UnitTestHelper.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);

            //Get Entries
            ICollection<Entry> entries = ObjectProvider.Instance().GetEntries(3, PostType.BlogPost, PostConfig.IsActive, true);

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = TimeZonesTest.PacificTimeZoneId;

            //Get Entries
            entries = ObjectProvider.Instance().GetEntries(3, PostType.BlogPost, PostConfig.IsActive, true);

            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetEntriesByTagDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null,
                                                                               NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null,
                                                                              NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null,
                                                                              NullValue.NullDateTime);

            //Persist entries.
            UnitTestHelper.Create(entryZero);
            UnitTestHelper.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);


            var tags = new List<string>(new[] {"Tag1", "Tag2"});
            new DatabaseObjectProvider().SetEntryTagList(entryZero.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryOne.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryTwo.Id, tags);


            ICollection<Entry> entries = ObjectProvider.Instance().GetEntriesByTag(3, "Tag1");

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = TimeZonesTest.PacificTimeZoneId;

            //Test outcome
            entries = ObjectProvider.Instance().GetEntriesByTag(3, "Tag1");

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetEntriesByCategoryDoesNotIncludeFuturePosts()
        {
            //Create Category
            int blogId = Config.CurrentBlog.Id;
            int categoryId = UnitTestHelper.CreateCategory(blogId, "Test Category");

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null,
                                                                               NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null,
                                                                              NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null,
                                                                              NullValue.NullDateTime);

            //Associate Category
            entryZero.Categories.Add("Test Category");
            entryOne.Categories.Add("Test Category");
            entryTwo.Categories.Add("Test Category");

            //Persist entries.
            UnitTestHelper.Create(entryZero);
            UnitTestHelper.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);


            //Get Entries
            ICollection<Entry> entries = ObjectProvider.Instance().GetEntriesByCategory(3, categoryId, true);


            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = TimeZonesTest.PacificTimeZoneId;

            //Test outcome
            entries = ObjectProvider.Instance().GetEntriesByCategory(3, categoryId, true);

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetPostsByDayRangeDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null,
                                                                               NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null,
                                                                              NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null,
                                                                              NullValue.NullDateTime);


            //Persist entries.
            UnitTestHelper.Create(entryZero);
            UnitTestHelper.Create(entryOne);
            entryTwo.DateSyndicated = Config.CurrentBlog.TimeZone.Now.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);

            //Get Entries
            DateTime now = Config.CurrentBlog.TimeZone.Now;
            var beginningOfMonth = new DateTime(now.Year, now.Month, 1);
            ICollection<Entry> entries = ObjectProvider.Instance().GetPostsByDayRange(beginningOfMonth, beginningOfMonth.AddMonths(1),
                                                                    PostType.BlogPost, true);

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = TimeZonesTest.PacificTimeZoneId;
            entries = ObjectProvider.Instance().GetPostsByDayRange(beginningOfMonth, beginningOfMonth.AddMonths(1), PostType.BlogPost,
                                                 true);

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
        }

        [Test]
        [RollBack2]
        public void GetPostCollectionByMonthDoesNotIncludeFuturePosts()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null,
                                                                               NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null,
                                                                              NullValue.NullDateTime);
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null,
                                                                              NullValue.NullDateTime);

            //Persist entries.
            UnitTestHelper.Create(entryZero);
            UnitTestHelper.Create(entryOne);
            DateTime now = Config.CurrentBlog.TimeZone.Now;
            entryTwo.DateSyndicated = now.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);

            //Get Entries
            ICollection<Entry> entries = ObjectProvider.Instance().GetPostsByMonth(now.Month, now.Year);

            //Test outcome
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Config.CurrentBlog.TimeZoneId = TimeZonesTest.PacificTimeZoneId;
            entries = ObjectProvider.Instance().GetPostsByMonth(now.Month, now.Year);

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
        }
    }
}