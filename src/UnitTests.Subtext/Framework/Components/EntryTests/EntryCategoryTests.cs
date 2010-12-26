using System.Linq;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestFixture]
    public class EntryCategoryTests
    {
        [Test]
        [RollBack2]
        public void CanAddAndRemoveAllCategories()
        {
            string hostname = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("empty title", "username", "password", hostname, string.Empty);

            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, "/");
            BlogRequest.Current.Blog = Config.GetBlog(hostname, "");
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Unit Test Entry", "Body");
            int id = UnitTestHelper.Create(entry);

            int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "My Subtext UnitTest Category");

            ObjectProvider.Instance().SetEntryCategoryList(id, new[] { categoryId });

            Entry loaded = UnitTestHelper.GetEntry(id, PostConfig.None, true);
            Assert.AreEqual("My Subtext UnitTest Category", loaded.Categories.First(),
                            "Expected a category for this entry");

            ObjectProvider.Instance().SetEntryCategoryList(id, new int[] { });

            loaded = UnitTestHelper.GetEntry(id, PostConfig.None, true);
            Assert.AreEqual(0, loaded.Categories.Count, "Expected that our category would be removed.");
        }
    }
}