using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestClass]
    public class EntryTagTests
    {
        [DatabaseIntegrationTestMethod]
        public void TagDoesNotRetrieveDraftEntry()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlogInternal("", "username", "password", hostname, string.Empty, 1);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            entry.IsActive = false;
            UnitTestHelper.Create(entry);
            var tags = new List<string>(new[] { "Tag1", "Tag2" });
            new DatabaseObjectProvider().SetEntryTagList(entry.Id, tags);
            ICollection<Entry> entries = repository.GetEntriesByTag(1, "Tag1");
            Assert.AreEqual(0, entries.Count, "Should not retrieve draft entry.");
        }

        [DatabaseIntegrationTestMethod]
        public void CanTagEntry()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlogInternal("", "username", "password", hostname, string.Empty, 1);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(hostname, string.Empty);

            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("me", "title-zero", "body-zero");
            UnitTestHelper.Create(entry);

            var tags = new List<string>(new[] { "Tag1", "Tag2" });
            new DatabaseObjectProvider().SetEntryTagList(entry.Id, tags);

            ICollection<Entry> entries = repository.GetEntriesByTag(1, "Tag1");
            Assert.AreEqual(1, entries.Count);
            Assert.AreEqual(entry.Id, entries.First().Id);
        }
    }
}