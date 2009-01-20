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
using System.Collections.ObjectModel;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestFixture]
    public class EntryDayGetTests
    {
        [SetUp]
        public void Setup()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(hostname, string.Empty);
        }

        [Test]
        [RollBack2]
        public void GetSingleDayReturnsDayWithEnclosure()
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

            //Get EntryDay
            EntryDay entries = Entries.GetSingleDay(DateTime.Now);

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
        public void GetBlogPostsReturnsAllPostsIfPostConfigNoneSpecified()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            entryZero.IsActive = true;
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            entryOne.IsActive = true;
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryTwo.IsActive = false;
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-three", "body-zero");
            entryThree.IsActive = true;
            entryThree.DateCreated = DateTime.Now.AddDays(10);
            entryThree.DateSyndicated = DateTime.Now.AddDays(10);

            //Persist entries.
            Entries.Create(entryZero);
            Thread.Sleep(500);
            Entries.Create(entryOne);
            Thread.Sleep(500);
            Entries.Create(entryTwo);
            Thread.Sleep(500);
            Entries.Create(entryThree);

            Assert.IsTrue(entryThree.DateSyndicated > DateTime.Now);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetBlogPosts(10, PostConfig.None);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two entry days.");
            Assert.AreEqual(1, entryList.First().Count, "Expected to find one entry in the first day.");
            Assert.AreEqual(3, entryList.ElementAt(1).Count, "Expected to find three entries in the second day.");
        }

        [Test]
        [RollBack2]
        public void GetBlogPostsReturnsActiveOnlyAndNoneInFuture()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            entryZero.IsActive = true;
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            entryOne.IsActive = true;
            entryOne.DateCreated = DateTime.Now.AddDays(-1);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryTwo.IsActive = false;
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-three", "body-zero");
            entryThree.IsActive = true;
            entryThree.DateCreated.AddDays(-2);
            entryThree.DateSyndicated = DateTime.Now.AddDays(10);

            //Persist entries.
            Entries.Create(entryZero);
            Thread.Sleep(500);
            Entries.Create(entryOne);
            Thread.Sleep(500);
            Entries.Create(entryTwo);
            Thread.Sleep(500);
            Entries.Create(entryThree);

            Assert.IsTrue(entryThree.DateSyndicated > DateTime.Now);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetBlogPosts(10, PostConfig.IsActive);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two entry days.");
            Assert.AreEqual(1, entryList.First().Count);
            Assert.AreEqual(1, entryList.ElementAt(1).Count);
        }

        [Test]
        [RollBack2]
        public void GetBlogPostsReturnsDaysWithEnclosure()
        {
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            Thread.Sleep(500);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one");
            Thread.Sleep(500);
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two");
            entryTwo.IsActive = false;
            Thread.Sleep(500);
            Entry entryThree = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-three", "body-three");

            //Persist entries.
            Entries.Create(entryZero);
            Entries.Create(entryOne);
            Entries.Create(entryTwo);
            Entries.Create(entryThree);

            Assert.IsTrue(entryZero.DateCreated < entryOne.DateCreated);
            Assert.IsTrue(entryOne.DateCreated < entryTwo.DateCreated);
            Assert.IsTrue(entryTwo.DateCreated < entryThree.DateCreated);

            //Add Enclosure
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetBlogPosts(10, PostConfig.IsActive);

            Collection<Entry> entries = entryList.First();
            //Test outcome
            Assert.AreEqual(1, entryList.Count, "Expected to find one entry day.");

            Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNull(entries.ElementAt(1).Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(2).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(2).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetHomePageEntriesReturnsDaysWithEnclosure()
        {
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
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetHomePageEntries(10);

            EntryDay[] days = new EntryDay[2];
            entryList.CopyTo(days, 0);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            EntryDay entries = days[1];
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(1).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(1).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetPostsByCategoryIDReturnsDaysWithEnclosure()
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
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get EntryDay
            ICollection<EntryDay> entryList = Entries.GetPostsByCategoryID(10, categoryId);

            EntryDay[] days = new EntryDay[2];
            entryList.CopyTo(days, 0);

            //Test outcome
            Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            EntryDay entries = days[1];
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");

            Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            Assert.IsNotNull(entries.ElementAt(1).Enclosure, "Entry should have enclosure.");
            UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(1).Enclosure);
        }

        [Test]
        [RollBack2]
        public void GetPostsByMonthReturnsDaysWithEnclosure()
        {
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
            Enclosure enc = UnitTestHelper.BuildEnclosure("Nothing to see here.", "httP://blablabla.com", "audio/mp3", entryZero.Id, 12345678, true, true);
            Enclosures.Create(enc);

            //Get EntryDay
            //ICollection<EntryDay> entryList = Entries.GetPostsByMonth(DateTime.Now.Month, DateTime.Now.Year);

            //EntryDay[] days = new EntryDay[2];
            //entryList.CopyTo(days, 0);

            ////Test outcome
            //Assert.AreEqual(2, entryList.Count, "Expected to find two days.");

            //EntryDay entries = days[1];
            //Assert.AreEqual(3, entries.Count, "Expected to find three entries.");

            //Assert.AreEqual(entries.First().Id, entryTwo.Id, "Ordering is off.");
            //Assert.AreEqual(entries.ElementAt(1).Id, entryOne.Id, "Ordering is off.");
            //Assert.AreEqual(entries.ElementAt(2).Id, entryZero.Id, "Ordering is off.");

            //Assert.IsNull(entries.First().Enclosure, "Entry should not have enclosure.");
            //Assert.IsNull(entries.ElementAt(1).Enclosure, "Entry should not have enclosure.");
            //Assert.IsNotNull(entries.ElementAt(2).Enclosure, "Entry should have enclosure.");
            //UnitTestHelper.AssertEnclosures(enc, entries.ElementAt(2).Enclosure);
        }
    }
}
