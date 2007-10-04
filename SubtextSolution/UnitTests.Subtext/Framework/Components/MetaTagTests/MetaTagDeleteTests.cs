#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestFixture]
    class MetaTagDeleteTests
    {
        private BlogInfo blog;

        [Test]
        [RollBack2]
        public void CanDeleteBlogMetaTag()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag =
                UnitTestHelper.BuildMetaTag("Steve Harman likes to delete stuff!", "description", null, blog.Id, null,
                                            DateTime.Now);
            MetaTags.Create(tag);
            Assert.AreEqual(1, MetaTags.GetMetaTagsForBlog(blog).Count, "Should be one (1) MetaTag for this blog.");

            // Now let's remove it from the data store
            Assert.IsTrue(MetaTags.Delete(tag.Id), "Deleting the MetaTag failed.");

            Assert.AreEqual(0, MetaTags.GetMetaTagsForBlog(blog).Count, "Should be zero (0) MetaTags for this blog.");
        }

        [Test]
        [RollBack2]
        public void CanDeleteEntryMetaTag()
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            Entry entry =
                UnitTestHelper.CreateEntryInstanceForSyndication("Steven Harman", "Sweet arse entry!",
                                                                 "Giddy, giddy, goo!");
            Entries.Create(entry);

            MetaTag tag = UnitTestHelper.BuildMetaTag("Foo, bar, zaa?", "author", null, blog.Id, entry.Id, DateTime.Now);
            MetaTags.Create(tag);

            Assert.AreEqual(1, MetaTags.GetMetaTagsForBlog(blog).Count, "Should be one (1) MetaTag for this blog.");
            Assert.AreEqual(1, MetaTags.GetMetaTagsForEntry(entry).Count, "Should be one (1) MetaTag for this entry.");

            // Now let's remove it from the data store
            Assert.IsTrue(MetaTags.Delete(tag.Id), "Deleting the MetaTag failed.");

            Assert.AreEqual(0, MetaTags.GetMetaTagsForBlog(blog).Count, "Should be zero (0) MetaTags for this blog.");
            Assert.AreEqual(0, MetaTags.GetMetaTagsForEntry(entry).Count, "Should be zero (0) MetaTag for this entry.");
        }
    }
}
