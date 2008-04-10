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
    public class EntryDayGetTests
    {
        [Test]
        [RollBack2]
        public void GetSingleDayReturnsDayWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

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
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678);
            Enclosures.Create(enc);

            //Get EntryDay
            EntryDay entries = Entries.GetSingleDay(DateTime.Now);

            //Test outcome
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");


            Assert.IsNull(entries[0].Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries[1].Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries[2].Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries[2].Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetBlogPostsReturnsDaysWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(500);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(500);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryTwo.IsActive = false;
            Thread.Sleep(500);
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            entryThree.DateCreated = DateTime.Now.AddDays(1);

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);
            Entries.Create(entryThree);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetBlogPosts(10, PostConfig.None);

            EntryDay[] days = new EntryDay[2];
            entryList.CopyTo(days,0);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            EntryDay entries = days[1];
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");


            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryTwo.Id, "Ordering is off.");

            Assert.IsNull(entries[0].Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries[2].Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries[1].Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries[1].Enclosure);
        }


        [Test]
        [RollBack2]
        public void GetHomePageEntriesReturnsDaysWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryTwo.DisplayOnHomePage = false;
            Thread.Sleep(100);
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryThree.DateCreated = DateTime.Now.AddDays(1);

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);
            Entries.Create(entryThree);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetHomePageEntries(10);

            EntryDay[] days = new EntryDay[2];
            entryList.CopyTo(days, 0);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            EntryDay entries = days[1];
            Assert.AreEqual(2, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Assert.IsNull(entries[0].Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries[1].Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries[1].Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetPostsByCategoryIDReturnsDaysWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

            //Create Category
            int blogId = Config.CurrentBlog.Id;
            int categoryId = UnitTestHelper.CreateCategory(blogId, "Test Category");

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            Thread.Sleep(100);
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryThree.DateCreated = DateTime.Now.AddDays(1);


            //Associate Category
            entryZero.Categories.Add("Test Category");
            entryOne.Categories.Add("Test Category");
            entryThree.Categories.Add("Test Category");


            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);
            Entries.Create(entryThree);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetPostsByCategoryID(10, categoryId);

            EntryDay[] days = new EntryDay[2];
            entryList.CopyTo(days, 0);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            EntryDay entries = days[1];
            Assert.AreEqual(2, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries[0].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryZero.Id, "Ordering is off.");

            Assert.IsNull(entries[0].Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries[1].Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries[1].Enclosure);
        }


        [Test]
        [RollBack2]
        public void GetPostsByMonthReturnsDaysWithEnclosure()
        {
            string hostname = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);

            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(100);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(100);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            Thread.Sleep(100);
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryThree.DateCreated = DateTime.Now.AddDays(1);


            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);
            Entries.Create(entryThree);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetPostsByMonth(DateTime.Now.Month, DateTime.Now.Year);

            EntryDay[] days = new EntryDay[2];
            entryList.CopyTo(days, 0);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            EntryDay entries = days[1];
            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.AreEqual(entries[0].Id, entryTwo.Id, "Ordering is off.");
            Assert.AreEqual(entries[1].Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries[2].Id, entryZero.Id, "Ordering is off.");

            Assert.IsNull(entries[0].Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries[1].Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries[2].Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries[2].Enclosure);
        }

    }
}
