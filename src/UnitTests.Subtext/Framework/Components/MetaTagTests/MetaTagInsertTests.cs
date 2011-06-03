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
using System.Linq;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestFixture]
    public class MetaTagInsertTests
    {
        private Blog blog;

        [RowTest]
        [Row("Steve loves Testing.", "description", null, false, "Did not create blog specific MetaTag.")]
        [Row("Still testing.", "description", null, true, "Did not create Entry specific MetaTag.")]
        [Row("no-cache", null, "cache-control", false, "Did not create blog specific MetaTag.")]
        [Row("Mon, 11 Jul 2020 11:12:01 GMT", null, "expires", true, "Did not create blog specific MetaTag.")]
        [RollBack2]
        public void CanInsertNewMetaTag(string content, string name, string httpEquiv, bool withEntry, string errMsg)
        {
            blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            int? entryId = null;
            if (withEntry)
            {
                Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Steven Harman", "My Post", "Foo Bar Zaa!");
                entryId = UnitTestHelper.Create(e);
            }

            MetaTag mt = UnitTestHelper.BuildMetaTag(content, name, httpEquiv, blog.Id, entryId, DateTime.UtcNow);

            // make sure there are no meta-tags for this blog in the data store
            ICollection<MetaTag> tags = repository.GetMetaTagsForBlog(blog, 0, 100);
            Assert.AreEqual(0, tags.Count, "Should be zero MetaTags.");

            // add the meta-tag to the data store
            int tagId = repository.Create(mt);

            tags = repository.GetMetaTagsForBlog(blog, 0, 100);

            Assert.AreEqual(1, tags.Count, errMsg);

            MetaTag newTag = tags.First();

            // make sure all attributes of the meta-tag were written to the data store correctly.
            Assert.AreEqual(tagId, newTag.Id, "Wrong Id");
            Assert.AreEqual(mt.Content, newTag.Content, "Wrong content");
            Assert.AreEqual(mt.Name, newTag.Name, "wrong name attribute");
            Assert.AreEqual(mt.HttpEquiv, newTag.HttpEquiv, "Wrong http-equiv attriubte");
            Assert.AreEqual(mt.BlogId, newTag.BlogId, "Wrong blogId");
            Assert.AreEqual(mt.EntryId, newTag.EntryId, "Wrong entryId");
            Assert.AreEqual(mt.DateCreatedUtc.Date, newTag.DateCreatedUtc.Date, "Wrong created date");
        }

        [Test]
        public void Create_WithNullMetaTag_ThrowsArgumentNullException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Create((MetaTag)null));
        }

        [Test]
        public void Create_WithInvalidMetaTag_ThrowsArgumentException()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrows<ArgumentException>(() => repository.Create(new MetaTag { Content = null }));
        }

        [Test]
        public void CanNotInsertNullMetaTag()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() => repository.Create((MetaTag)null));
        }
    }
}