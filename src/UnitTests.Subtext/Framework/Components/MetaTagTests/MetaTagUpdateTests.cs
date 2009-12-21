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
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestFixture]
    public class MetatTagUpdateTests
    {
        [RowTest]
        [Row("Steven Harman", "author", null)]
        [Row("no-cache", null, "cache-control")]
        [RollBack2]
        public void CanUpdateMetaTag(string content, string name, string httpequiv)
        {
            var blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag(content, name, httpequiv, blog.Id, null, DateTime.Now);
            MetaTags.Create(tag);

            string randomStr = UnitTestHelper.GenerateUniqueString().Left(20);
            tag.Content = content + randomStr;

            if (!string.IsNullOrEmpty(name))
            {
                tag.Name = name + randomStr;
            }

            if (!string.IsNullOrEmpty(httpequiv))
            {
                tag.HttpEquiv = httpequiv + randomStr;
            }

            Assert.IsTrue(MetaTags.Update(tag));

            MetaTag updTag = MetaTags.GetMetaTagsForBlog(blog, 0, 100)[0];

            ValidateMetaTags(tag, updTag);
        }

        [Test]
        [RollBack2]
        public void CanRemoveNameAndAddHttpEquiv()
        {
            var blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag("Nothing to see here.", "description", null, blog.Id, null,
                                                      DateTime.Now);
            MetaTags.Create(tag);

            tag.HttpEquiv = "cache-control";
            tag.Name = null;
            tag.Content = "no-cache";

            MetaTags.Update(tag);

            ValidateMetaTags(tag, MetaTags.GetMetaTagsForBlog(blog, 0, 100)[0]);
        }

        [Test]
        [RollBack2]
        public void CanRemoveHttpEquivAndAddName()
        {
            var blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag("Still nothing to see here.", null, "expires", blog.Id, null,
                                                      DateTime.Now);
            MetaTags.Create(tag);

            tag.HttpEquiv = null;
            tag.Name = "author";
            tag.Content = "Steve-o-rino!";

            MetaTags.Update(tag);

            ValidateMetaTags(tag, MetaTags.GetMetaTagsForBlog(blog, 0, 100)[0]);
        }

        [Test]
        public void Update_WithInvalidMetaTag_ThrowsArgumentException()
        {
            // arrange
            var metaTag = new MetaTag(null);

            // act, assert
            Assert.IsFalse(metaTag.IsValid);
            UnitTestHelper.AssertThrows<ArgumentException>(() => MetaTags.Update(metaTag));
        }

        [Test]
        public void Update_WithNullMetaTag_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => MetaTags.Update(null));
        }

        private static void ValidateMetaTags(MetaTag expected, MetaTag result)
        {
            Assert.AreEqual(expected.Content, result.Content, "Content didn't get updated.");
            Assert.AreEqual(expected.Name, result.Name, "Name attribute didn't get updated.");
            Assert.AreEqual(expected.HttpEquiv, result.HttpEquiv, "Http-Equiv attribute didn't get updated");
        }
    }
}