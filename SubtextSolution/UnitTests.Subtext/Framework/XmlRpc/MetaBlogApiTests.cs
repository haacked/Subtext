using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.XmlRpc;

namespace UnitTests.Subtext.Framework.XmlRpc
{
    [TestFixture]
    public class MetaBlogApiTests
    {
        [Test]
        [RollBack]
        public void GetRecentPostsReturnsRecentPosts()
        {
            string hostname = UnitTestHelper.GenerateRandomHostname();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, ""));
            UnitTestHelper.SetHttpContextWithBlogRequest(hostname, "");
            Config.CurrentBlog.AllowServiceAccess = true;

            MetaWeblog api = new MetaWeblog();
            Post[] posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(0, posts.Length);

            LinkCategory category = new LinkCategory();
            category.BlogId = Config.CurrentBlog.Id;
            category.Title = "Test Category";
            int categoryId = Links.CreateLinkCategory(category);
            
            Entry entry = new Entry(PostType.BlogPost);
            entry.DateCreated = entry.DateSyndicated = entry.DateUpdated = DateTime.ParseExact("1/23/1975", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entries.Create(entry, categoryId);

            posts = api.getRecentPosts(Config.CurrentBlog.Id.ToString(), "username", "password", 10);
            Assert.AreEqual(1, posts.Length);
            Assert.AreEqual(1, posts[0].categories.Length, "Expected our categories to be there.");
            Assert.AreEqual("Test Category", posts[0].categories[0], "The category returned by the MetaBlogApi is wrong.");
        }
    }
}
