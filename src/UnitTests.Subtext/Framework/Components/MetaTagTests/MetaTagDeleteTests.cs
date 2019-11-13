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
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestFixture]
    public class MetaTagDeleteTests
    {
        [Test]
        [RollBack2]
        public void CanDeleteBlogMetaTag()
        {
            var blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            MetaTag tag =
                UnitTestHelper.BuildMetaTag("Steve Harman likes to delete stuff!", "description", null, blog.Id, null,
                                            DateTime.UtcNow);
            repository.Create(tag);
            Assert.AreEqual(1, repository.GetMetaTagsForBlog(blog, 0, 100).Count,
                            "Should be one (1) MetaTag for this blog.");

            // Now let's remove it from the data store
            Assert.IsTrue(repository.DeleteMetaTag(tag.Id), "Deleting the MetaTag failed.");

            Assert.AreEqual(0, repository.GetMetaTagsForBlog(blog, 0, 100).Count,
                            "Should be zero (0) MetaTags for this blog.");
        }

        [Test]
        [RollBack2]
        public void CanDeleteEntryMetaTag()
        {
            var blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            Entry entry =
                UnitTestHelper.CreateEntryInstanceForSyndication("Steven Harman", "Sweet arse entry!",
                                                                 "Giddy, giddy, goo!");
            UnitTestHelper.Create(entry);

            MetaTag tag = UnitTestHelper.BuildMetaTag("Foo, bar, zaa?", "author", null, blog.Id, entry.Id, DateTime.UtcNow);
            repository.Create(tag);

            Assert.AreEqual(1, repository.GetMetaTagsForBlog(blog, 0, 100).Count,
                            "Should be one (1) MetaTag for this blog.");
            Assert.AreEqual(1, repository.GetMetaTagsForEntry(entry, 0, 100).Count,
                            "Should be one (1) MetaTag for this entry.");

            // Now let's remove it from the data store
            Assert.IsTrue(repository.DeleteMetaTag(tag.Id), "Deleting the MetaTag failed.");

            Assert.AreEqual(0, repository.GetMetaTagsForBlog(blog, 0, 100).Count,
                            "Should be zero (0) MetaTags for this blog.");
            Assert.AreEqual(0, repository.GetMetaTagsForEntry(entry, 0, 100).Count,
                            "Should be zero (0) MetaTag for this entry.");
        }
    }
}