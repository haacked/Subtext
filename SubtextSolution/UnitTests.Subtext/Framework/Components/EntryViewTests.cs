using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class EntryViewTests
    {
        [Test]
        public void CtorInitializesIdsToNullValue()
        {
            var view = new EntryView();
            Assert.AreEqual(NullValue.NullInt32, view.EntryId);
            Assert.AreEqual(NullValue.NullInt32, view.BlogId);
        }

        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var view = new EntryView();
            UnitTestHelper.AssertSimpleProperties(view);
        }

        [Test]
        [RollBack2]
        public void CanSetAndGetSimpleEntryStatsViewProperties()
        {
            string host = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("title", "blah", "blah", host, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(host, string.Empty);
            BlogRequest.Current.Blog = Config.GetBlog(host, string.Empty);
            var view = new EntryStatsView();
            UnitTestHelper.AssertSimpleProperties(view);
        }
    }
}