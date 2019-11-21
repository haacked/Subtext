using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class EntryViewTests
    {
        [TestMethod]
        public void CtorInitializesIdsToNullValue()
        {
            var view = new EntryView();
            Assert.AreEqual(NullValue.NullInt32, view.EntryId);
            Assert.AreEqual(NullValue.NullInt32, view.BlogId);
        }

        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var view = new EntryView();
            UnitTestHelper.AssertSimpleProperties(view);
        }

        [DatabaseIntegrationTestMethod]
        public void CanSetAndGetSimpleEntryStatsViewProperties()
        {
            string host = UnitTestHelper.GenerateUniqueString();
            var repository = new DatabaseObjectProvider();
            repository.CreateBlogInternal("title", "blah", "blah", host, string.Empty, 1);
            UnitTestHelper.SetHttpContextWithBlogRequest(host, string.Empty);
            BlogRequest.Current.Blog = repository.GetBlog(host, string.Empty);
            var view = new EntryStatsView();
            UnitTestHelper.AssertSimpleProperties(view);
        }
    }
}