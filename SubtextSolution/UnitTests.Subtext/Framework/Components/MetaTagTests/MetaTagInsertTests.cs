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
using System.Linq;
using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;

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
        [Row(null, null, null, false, "MetaTag invalid - requires Contents and Name or Http-Equiv.", ExpectedException = typeof(ArgumentException))]
        [Row(null, "author", null, true, "MetaTag invalid - requires Contents.", ExpectedException = typeof(ArgumentException))]
        [Row(null, null, "expires", false, "MetaTag invalid - requires Contents.", ExpectedException = typeof(ArgumentException))]
        [Row("sending nulls", null, null, false, "MetaTag invalid - requires Name or Http-Equiv.", ExpectedException = typeof(ArgumentException))]
        [Row("set both attributes", "description", "expires", true, "MetaTag invalid - requires either Name or Http-Equiv.", ExpectedException = typeof(ArgumentException))]
        [RollBack2]
        public void CanInsertNewMetaTag(string content, string name, string httpEquiv, bool withEntry, string errMsg)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            int? entryId = null;
            if (withEntry)
            {
                Entry e = UnitTestHelper.CreateEntryInstanceForSyndication("Steven Harman", "My Post", "Foo Bar Zaa!");
                entryId = Entries.Create(e);
            }

            MetaTag mt = UnitTestHelper.BuildMetaTag(content, name, httpEquiv, blog.Id, entryId, DateTime.Now);

            // make sure there are no meta-tags for this blog in the data store
            ICollection<MetaTag> tags = MetaTags.GetMetaTagsForBlog(blog, 0, 100);
            Assert.AreEqual(0, tags.Count, "Should be zero MetaTags.");

            // add the meta-tag to the data store
            int tagId = MetaTags.Create(mt);

            tags = MetaTags.GetMetaTagsForBlog(blog, 0, 100);

            Assert.AreEqual(1, tags.Count, errMsg);

            MetaTag newTag = tags.First();

            // make sure all attributes of the meta-tag were written to the data store correctly.
            Assert.AreEqual(tagId, newTag.Id, "Wrong Id");
            Assert.AreEqual(mt.Content, newTag.Content, "Wrong content");
            Assert.AreEqual(mt.Name, newTag.Name, "wrong name attribute");
            Assert.AreEqual(mt.HttpEquiv, newTag.HttpEquiv, "Wrong http-equiv attriubte");
            Assert.AreEqual(mt.BlogId, newTag.BlogId, "Wrong blogId");
            Assert.AreEqual(mt.EntryId, newTag.EntryId, "Wrong entryId");
            Assert.AreEqual(mt.DateCreated.Date, newTag.DateCreated.Date, "Wrong created date");
        }

        [RowTest]
        [Row(null, null, null, "All attributs are null, should not be valid.", ExpectedException = typeof(ArgumentException))]
        [Row("This is content", null, null, "MetaTag requires either name or http-equiv.", ExpectedException = typeof(ArgumentException))]
        [Row(null, "description", "expires", "Can't have both name and http-equiv.", ExpectedException = typeof(ArgumentException))]
        [Row("Steven Harman's content", "description", "expires", "Can't have both name and http-equiv.", ExpectedException = typeof(ArgumentException))]
        [Row("", "", "", "All attributs are EmptyString, should not be valid.", ExpectedException = typeof(ArgumentException))]
        [Row("This is content", "", "", "MetaTag requires either name or http-equiv.", ExpectedException = typeof(ArgumentException))]
        [Row("", "description", "expires", "Can't have both name and http-equiv.", ExpectedException = typeof(ArgumentException))]
        [RollBack2]
        public void CanNotInsertInvalidMetaTag(string content, string name, string httpEquiv, string errMsg)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();
            MetaTag mt = UnitTestHelper.BuildMetaTag(content, name, httpEquiv, blog.Id, null, DateTime.Now);

            MetaTags.Create(mt);
        }

        [Test]
        [ExpectedArgumentNullException]
        public void CanNotInsertNullMetaTag()
        {
            MetaTags.Create(null);
        }
    }
}
