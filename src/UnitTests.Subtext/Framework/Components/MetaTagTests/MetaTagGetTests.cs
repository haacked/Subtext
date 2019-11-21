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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestClass]
    public class MetaTagGetTests
    {
        private Blog blog;

        [DatabaseIntegrationTestMethod]
        public void GetReturnsZeroWhenNoMetaTagsExistForBlog()
        {
            var repository = new DatabaseObjectProvider();
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            Assert.AreEqual(0, repository.GetMetaTagsForBlog(blog, 0, 100).Count,
                            "Shouldn't have found any MetaTags for this blog.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetReturnsZeroWhenNoMetaTagsExistForEntry()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();

            Entry e =
                UnitTestHelper.CreateEntryInstanceForSyndication("Steve Harman", "Loves Subtexting!", "Roses are red...");

            // Act
            UnitTestHelper.Create(e);

            // Assert
            Assert.AreEqual(0, repository.GetMetaTagsForEntry(e, 0, 100).Count,
                            "Shouldn't have found any MetaTags for this entry.");
        }

        [DatabaseIntegrationTestMethod]
        public void CanGetMetaTagsForBlog()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            InsertNewMetaTag("Adding description meta tag", "description", null, DateTime.UtcNow, blog.Id, null);
            InsertNewMetaTag("no-cache", null, "cache-control", DateTime.UtcNow, blog.Id, null);

            ICollection<MetaTag> tags = repository.GetMetaTagsForBlog(blog, 0, 100);

            Assert.AreEqual(2, tags.Count, "Should be two tags for this blog.");
        }

        [DatabaseIntegrationTestMethod]
        public void CanGetMetaTagsForEntry()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Steve-o", "Bar",
                                                                       "Steve is still rockin it... or is he?");
            UnitTestHelper.Create(e);

            InsertNewMetaTag("Adding description meta tag", "description", null, DateTime.UtcNow, blog.Id, null);
            InsertNewMetaTag("no-cache", null, "cache-control", DateTime.UtcNow, blog.Id, null);

            // insert a few entry specific tags
            InsertNewMetaTag("Yet Another MetaTag", "author", null, DateTime.UtcNow, blog.Id, e.Id);
            InsertNewMetaTag("One more for good measure", "description", null, DateTime.UtcNow, blog.Id, e.Id);
            InsertNewMetaTag("no-cache", null, "cache-control", DateTime.UtcNow, blog.Id, e.Id);
            InsertNewMetaTag("Mon, 22 Jul 2022 11:12:01 GMT", null, "expires", DateTime.UtcNow, blog.Id, e.Id);

            ICollection<MetaTag> tags = repository.GetMetaTagsForEntry(e, 0, 100);

            Assert.AreEqual(4, tags.Count, "Should have found 4 MetaTags for this entry.");
        }

        private static void InsertNewMetaTag(string content, string nameValue, string httpEquivValue, DateTime created,
                                             int blogId, int? entryId)
        {
            var repository = new DatabaseObjectProvider();
            var metaTag = new MetaTag();
            metaTag.Content = content;
            metaTag.Name = nameValue;
            metaTag.HttpEquiv = httpEquivValue;
            metaTag.DateCreatedUtc = created;
            metaTag.BlogId = blogId;
            metaTag.EntryId = entryId;
            repository.Create(metaTag);
        }
    }
}