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
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestFixture]
    public class MetaTagGetTests
    {
        private Blog blog;

        [Test]
        [RollBack2]
        public void GetReturnsZeroWhenNoMetaTagsExistForBlog()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            Assert.AreEqual(0, MetaTags.GetMetaTagsForBlog(blog, 0, 100).Count,
                            "Shouldn't have found any MetaTags for this blog.");
        }

        [Test]
        [RollBack2]
        public void GetReturnsZeroWhenNoMetaTagsExistForEntry()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();

            Entry e =
                UnitTestHelper.CreateEntryInstanceForSyndication("Steve Harman", "Loves Subtexting!", "Roses are red...");
            UnitTestHelper.Create(e);

            Assert.AreEqual(0, MetaTags.GetMetaTagsForEntry(e, 0, 100).Count,
                            "Shouldn't have found any MetaTags for this entry.");
        }

        [Test]
        [RollBack2]
        public void CanGetMetaTagsForBlog()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();

            InsertNewMetaTag("Adding description meta tag", "description", null, DateTime.Now, blog.Id, null);
            InsertNewMetaTag("no-cache", null, "cache-control", DateTime.Now, blog.Id, null);

            ICollection<MetaTag> tags = MetaTags.GetMetaTagsForBlog(blog, 0, 100);

            Assert.AreEqual(2, tags.Count, "Should be two tags for this blog.");
        }

        [Test]
        [RollBack2]
        public void CanGetMetaTagsForEntry()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();

            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Steve-o", "Bar",
                                                                       "Steve is still rockin it... or is he?");
            UnitTestHelper.Create(e);

            InsertNewMetaTag("Adding description meta tag", "description", null, DateTime.Now, blog.Id, null);
            InsertNewMetaTag("no-cache", null, "cache-control", DateTime.Now, blog.Id, null);

            // insert a few entry specific tags
            InsertNewMetaTag("Yet Another MetaTag", "author", null, DateTime.Now, blog.Id, e.Id);
            InsertNewMetaTag("One more for good measure", "description", null, DateTime.Now, blog.Id, e.Id);
            InsertNewMetaTag("no-cache", null, "cache-control", DateTime.Now, blog.Id, e.Id);
            InsertNewMetaTag("Mon, 22 Jul 2022 11:12:01 GMT", null, "expires", DateTime.Now, blog.Id, e.Id);

            ICollection<MetaTag> tags = MetaTags.GetMetaTagsForEntry(e, 0, 100);

            Assert.AreEqual(4, tags.Count, "Should have found 4 MetaTags for this entry.");
        }

        private static void InsertNewMetaTag(string content, string nameValue, string httpEquivValue, DateTime created,
                                             int blogId, int? entryId)
        {
            var metaTag = new MetaTag();
            metaTag.Content = content;
            metaTag.Name = nameValue;
            metaTag.HttpEquiv = httpEquivValue;
            metaTag.DateCreated = created;
            metaTag.BlogId = blogId;
            metaTag.EntryId = entryId;
            ObjectProvider.Instance().Create(metaTag);
        }
    }
}