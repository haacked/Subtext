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
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTestsi
{
    /// <summary>
    /// Tests the methods to obtain the previous and next entry to an entry.
    /// </summary>
    [TestFixture]
    public class PreviousNextTests
    {
        /// <summary>
        /// Test the case where we have a previous, but no next entry.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetPreviousAndNextEntriesReturnsPreviousWhenNoNextExists()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                   UnitTestHelper.GenerateUniqueString(),
                                                                                   DateTime.UtcNow.AddDays(-1));
            Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                  UnitTestHelper.GenerateUniqueString(),
                                                                                  DateTime.UtcNow);

            int previousId = UnitTestHelper.Create(previousEntry);
            int currentId = UnitTestHelper.Create(currentEntry);

            var entries = repository.GetPreviousAndNextEntries(currentId,
                                                                                             PostType.BlogPost);
            Assert.AreEqual(1, entries.Count, "Since there is no next entry, should return only 1");
            Assert.AreEqual(previousId, entries.First().Id, "The previous entry does not match expectations.");
        }

        /// <summary>
        /// Test the case where we have a next, but no previous entry.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetPreviousAndNextEntriesReturnsNextWhenNoPreviousExists()
        {
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                  UnitTestHelper.GenerateUniqueString(),
                                                                                  DateTime.UtcNow.AddDays(-1));
            Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                               UnitTestHelper.GenerateUniqueString(),
                                                                               DateTime.UtcNow);

            int currentId = UnitTestHelper.Create(currentEntry);
            int nextId = UnitTestHelper.Create(nextEntry);

            var entries = repository.GetPreviousAndNextEntries(currentId,
                                                                                             PostType.BlogPost);
            Assert.AreEqual(1, entries.Count, "Since there is no previous entry, should return only next");
            Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
        }

        /// <summary>
        /// Test the case where we have both a previous and next.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetPreviousAndNextEntriesReturnsBoth()
        {
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                   UnitTestHelper.GenerateUniqueString(),
                                                                                   DateTime.UtcNow.AddDays(-2));
            Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                  UnitTestHelper.GenerateUniqueString(),
                                                                                  DateTime.UtcNow.AddDays(-1));
            Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                               UnitTestHelper.GenerateUniqueString(),
                                                                               DateTime.UtcNow);

            int previousId = UnitTestHelper.Create(previousEntry);
            Thread.Sleep(100);
            int currentId = UnitTestHelper.Create(currentEntry);
            Thread.Sleep(100);
            int nextId = UnitTestHelper.Create(nextEntry);

            var entries = repository.GetPreviousAndNextEntries(currentId,
                                                                                             PostType.BlogPost);
            Assert.AreEqual(2, entries.Count, "Expected both previous and next.");

            //The more recent one is next because of desceding sort.
            Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
            Assert.AreEqual(previousId, entries.ElementAt(1).Id, "The previous entry does not match expectations.");
        }

        /// <summary>
        /// Test the case where we have more than three entries.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetPreviousAndNextEntriesReturnsCorrectEntries()
        {
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry firstEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                UnitTestHelper.GenerateUniqueString(),
                                                                                DateTime.UtcNow.AddDays(-3));
            Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                   UnitTestHelper.GenerateUniqueString(),
                                                                                   DateTime.UtcNow.AddDays(-2));
            Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                                  UnitTestHelper.GenerateUniqueString(),
                                                                                  DateTime.UtcNow.AddDays(-1));
            Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                               UnitTestHelper.GenerateUniqueString(),
                                                                               DateTime.UtcNow);
            Entry lastEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body",
                                                                               UnitTestHelper.GenerateUniqueString(),
                                                                               DateTime.UtcNow.AddDays(1));

            Thread.Sleep(100);
            int previousId = UnitTestHelper.Create(previousEntry);
            Thread.Sleep(100);
            int currentId = UnitTestHelper.Create(currentEntry);
            Thread.Sleep(100);
            int nextId = UnitTestHelper.Create(nextEntry);
            Thread.Sleep(100);

            var entries = repository.GetPreviousAndNextEntries(currentId, PostType.BlogPost);
            Assert.AreEqual(2, entries.Count, "Expected both previous and next.");

            //The more recent one is next because of desceding sort.
            Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
            Assert.AreEqual(previousId, entries.ElementAt(1).Id, "The previous entry does not match expectations.");
        }

        /// <summary>
        /// Make sure that previous and next are based on syndication date and not entry id.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetPreviousAndNextBasedOnSyndicationDateNotEntryId()
        {
            var repository = new DatabaseObjectProvider();
            string hostname = UnitTestHelper.GenerateUniqueString();
            repository.CreateBlog("", "username", "password", hostname, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry previousEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");
            Entry currentEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");
            Entry nextEntry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "test", "body");

            previousEntry.IsActive = false;
            currentEntry.IsActive = false;
            nextEntry.IsActive = false;

            //Create out of order.
            int currentId = UnitTestHelper.Create(currentEntry);
            int nextId = UnitTestHelper.Create(nextEntry);
            int previousId = UnitTestHelper.Create(previousEntry);

            //Now syndicate.
            previousEntry.IsActive = true;
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            subtextContext.Setup(c => c.Repository).Returns(repository);
            UnitTestHelper.Update(previousEntry, subtextContext.Object);
            Thread.Sleep(100);
            currentEntry.IsActive = true;
            UnitTestHelper.Update(currentEntry, subtextContext.Object);
            Thread.Sleep(100);
            nextEntry.IsActive = true;
            UnitTestHelper.Update(nextEntry, subtextContext.Object);

            Assert.IsTrue(previousId > currentId, "Ids are out of order.");

            var entries = repository.GetPreviousAndNextEntries(currentId, PostType.BlogPost);
            Assert.AreEqual(2, entries.Count, "Expected both previous and next.");
            //The first should be next because of descending sort.
            Assert.AreEqual(nextId, entries.First().Id, "The next entry does not match expectations.");
            Assert.AreEqual(previousId, entries.ElementAt(1).Id, "The previous entry does not match expectations.");
        }
    }
}