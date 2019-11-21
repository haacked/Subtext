using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestClass]
    public class EntryCategoryTests
    {
        [DatabaseIntegrationTestMethod]
        public void CanAddAndRemoveAllCategories()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("empty title", "username", "password", hostname, string.Empty);

            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, "/");
            BlogRequest.Current.Blog = new DatabaseObjectProvider().GetBlog(hostname, "");
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Unit Test Entry", "Body");
            int id = UnitTestHelper.Create(entry);

            int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "My Subtext UnitTest Category");

            repository.SetEntryCategoryList(id, new[] { categoryId });

            Entry loaded = UnitTestHelper.GetEntry(id, PostConfig.None, true);
            Assert.AreEqual("My Subtext UnitTest Category", loaded.Categories.First(),
                            "Expected a category for this entry");

            repository.SetEntryCategoryList(id, new int[] { });

            loaded = UnitTestHelper.GetEntry(id, PostConfig.None, true);
            Assert.AreEqual(0, loaded.Categories.Count, "Expected that our category would be removed.");
        }
    }
}