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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Web.HttpModules;
using UnitTests.Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestClass]
    public class FutureEntriesGetTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("", "username", "password", hostname, "");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "", "");
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);
            Config.CurrentBlog.TimeZoneId = TimeZonesTest.HawaiiTimeZoneId;
        }

        [DatabaseIntegrationTestMethod]
        public void GetRecentEntries_WithFuturePosts_OnlyReturnsPastPosts()
        {
            // Arrange
            var repository = new DatabaseObjectProvider();
            DateTime utcNow = DateTime.UtcNow.AddDays(-2);
            var blog1 = UnitTestHelper.CreateBlogAndSetupContext(null, "subfolder1");
            var entryOne = UnitTestHelper.CreateAndSaveEntryForSyndication("me", "blog1-entry-one", "body-zero", null, utcNow, utcNow);
            var entryTwo = UnitTestHelper.CreateAndSaveEntryForSyndication("me", "blog1-entry-two", "body-zero", null, utcNow, utcNow.AddDays(10));
            var entryThree = UnitTestHelper.CreateAndSaveEntryForSyndication("me", "blog1-entry-three", "body-zero", null, utcNow, utcNow);
            var blog2 = UnitTestHelper.CreateBlogAndSetupContext(blog1.Host, "subfolder2");
            var entryFour = UnitTestHelper.CreateAndSaveEntryForSyndication("me", "blog2-entry-four", "body-zero", null, utcNow, utcNow.AddDays(10));
            var entryFive = UnitTestHelper.CreateAndSaveEntryForSyndication("me", "blog2-entry-five", "body-zero", null, utcNow, utcNow);

            UnitTestHelper.WriteTableToOutput("subtext_Config");
            UnitTestHelper.WriteTableToOutput("subtext_Content");
            // Act
            ICollection<Entry> entries = repository.GetRecentEntries(null, null, 10);

            // Assert (reverse ordering)
            Assert.AreEqual(3, entries.Count);
            Assert.AreEqual(entries.First().Id, entryFive.Id);
            Assert.AreEqual(entries.ElementAt(1).Id, entryThree.Id);
            Assert.AreEqual(entries.ElementAt(2).Id, entryOne.Id);
        }

        [DatabaseIntegrationTestMethod]
        public void GetRecentPostsDoesNotIncludeFuturePosts()
        {
            // Arrange
            var repository = new DatabaseObjectProvider();
            DateTime utcNow = DateTime.UtcNow.AddMinutes(-5);
            //Create some entries.
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, utcNow);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, utcNow.AddMinutes(1));
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, utcNow.AddMinutes(2));

            entryZero.DatePublishedUtc = entryZero.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryZero);
            entryOne.DatePublishedUtc = entryOne.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryOne);
            entryTwo.DatePublishedUtc = utcNow.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);

            // Act
            ICollection<Entry> entries = repository.GetEntries(3, PostType.BlogPost, PostConfig.IsActive, true);

            // Assert
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetEntriesByTagDoesNotIncludeFuturePosts()
        {
            // Arrange
            var repository = new DatabaseObjectProvider();
            DateTime now = DateTime.UtcNow.AddMinutes(-5);
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, now);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, now.AddMinutes(1));
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, now.AddMinutes(2));

            entryZero.DatePublishedUtc = entryZero.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryZero);
            entryOne.DatePublishedUtc = entryOne.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryOne);
            entryTwo.DatePublishedUtc = DateTime.UtcNow.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);


            var tags = new List<string>(new[] { "Tag1", "Tag2" });
            new DatabaseObjectProvider().SetEntryTagList(entryZero.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryOne.Id, tags);
            new DatabaseObjectProvider().SetEntryTagList(entryTwo.Id, tags);

            // Act
            ICollection<Entry> entries = repository.GetEntriesByTag(3, "Tag1");

            // Assert
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetEntriesByCategoryDoesNotIncludeFuturePosts()
        {
            // Arrange
            var repository = new DatabaseObjectProvider();
            DateTime now = DateTime.UtcNow.AddMinutes(-5);
            int blogId = Config.CurrentBlog.Id;
            int categoryId = UnitTestHelper.CreateCategory(blogId, "Test Category");

            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, now);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, now.AddMinutes(1));
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, now.AddMinutes(2));

            entryZero.Categories.Add("Test Category");
            entryOne.Categories.Add("Test Category");
            entryTwo.Categories.Add("Test Category");

            entryZero.DatePublishedUtc = entryZero.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryZero);
            entryOne.DatePublishedUtc = entryOne.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryOne);
            entryTwo.DatePublishedUtc = DateTime.UtcNow.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);


            // Act
            ICollection<Entry> entries = repository.GetEntriesByCategory(3, categoryId, true);

            // Assert
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetPostsByDayRangeDoesNotIncludeFuturePosts()
        {
            // Arrange
            var repository = new DatabaseObjectProvider();
            DateTime now = DateTime.UtcNow.AddMinutes(-5);
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, now);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, now.AddMinutes(1));
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, now.AddMinutes(2));

            entryZero.DatePublishedUtc = entryZero.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryZero);
            entryOne.DatePublishedUtc = entryOne.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryOne);
            entryTwo.DatePublishedUtc = DateTime.UtcNow.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);

            // Act
            var beginningOfMonth = new DateTime(now.Year, now.Month, 1);
            ICollection<Entry> entries = repository.GetPostsByDayRange(beginningOfMonth, beginningOfMonth.AddMonths(1),
                                                                    PostType.BlogPost, true);

            // Assert
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetPostCollectionByMonthDoesNotIncludeFuturePosts()
        {
            // Arrange
            var repository = new DatabaseObjectProvider();
            DateTime now = DateTime.UtcNow.AddMinutes(-5);
            Entry entryZero = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero", null, now);
            Entry entryOne = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-one", "body-one", null, now.AddMinutes(1));
            Entry entryTwo = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-two", "body-two", null, now.AddMinutes(2));

            entryZero.DatePublishedUtc = entryZero.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryZero);
            entryOne.DatePublishedUtc = entryOne.DateCreatedUtc.AddSeconds(1);
            UnitTestHelper.Create(entryOne);
            entryTwo.DatePublishedUtc = DateTime.UtcNow.AddMinutes(20);
            UnitTestHelper.Create(entryTwo);

            // Act
            ICollection<Entry> entries = repository.GetPostsByMonth(now.Month, now.Year);

            // Assert
            Assert.AreEqual(2, entries.Count, "Expected to find two entries.");

            Assert.AreEqual(entries.First().Id, entryOne.Id, "Ordering is off.");
            Assert.AreEqual(entries.ElementAt(1).Id, entryZero.Id, "Ordering is off.");
        }
    }
}