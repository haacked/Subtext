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
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Components.MetaTagTests
{
    [TestFixture]
    public class MetatTagUpdateTests
    {
        private BlogInfo blog;

        [RowTest]
        [Row("Steven Harman", "author", null)]
        [Row("no-cache", null, "cache-control")]
        [RollBack2]
        public void CanUpdateMetaTag(string content, string name, string httpequiv)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag(content, name, httpequiv, blog.Id, null, DateTime.Now);
            MetaTags.Create(tag);

            string randomStr = UnitTestHelper.GenerateUniqueString().Left(20);
            tag.Content = content + randomStr;

            if (!string.IsNullOrEmpty(name))
                tag.Name = name + randomStr;
            
            if (!string.IsNullOrEmpty(httpequiv))
                tag.HttpEquiv = httpequiv + randomStr;

            Assert.IsTrue(MetaTags.Update(tag));

            MetaTag updTag = MetaTags.GetMetaTagsForBlog(blog, 0, 100)[0];

            ValidateMetaTags(tag, updTag);
        }

        [Test]
        [RollBack2]
        public void CanRemoveNameAndAddHttpEquiv()
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag("Nothing to see here.", "description", null, blog.Id, null, DateTime.Now);
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
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag("Still nothing to see here.", null, "expires", blog.Id, null, DateTime.Now);
            MetaTags.Create(tag);

            tag.HttpEquiv = null;
            tag.Name = "author";
            tag.Content = "Steve-o-rino!";

            MetaTags.Update(tag);

            ValidateMetaTags(tag, MetaTags.GetMetaTagsForBlog(blog, 0, 100)[0]);
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
        public void CantUpdateWithInvalidMetaTags(string updContent, string updName, string updHttpEquiv, string errMsg)
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag("Nothing to see here.", "description", null, blog.Id, null, DateTime.Now);
            MetaTags.Create(tag);

            tag.Content = updContent;
            tag.Name = updName;
            tag.HttpEquiv = updHttpEquiv;

            MetaTags.Update(tag);
        }

        [Test]
        [ExpectedArgumentNullException]
        [RollBack2]
        public void CantUpateWithNullMetaTag()
        {
            this.blog = UnitTestHelper.CreateBlogAndSetupContext();

            MetaTag tag = UnitTestHelper.BuildMetaTag("Yet again...", null, "description", blog.Id, null, DateTime.Now);
            MetaTags.Create(tag);

            MetaTags.Update(null);
        }

        private static void ValidateMetaTags(MetaTag expected, MetaTag result)
        {
            Assert.AreEqual(expected.Content, result.Content, "Content didn't get updated.");
            Assert.AreEqual(expected.Name, result.Name, "Name attribute didn't get updated.");
            Assert.AreEqual(expected.HttpEquiv, result.HttpEquiv, "Http-Equiv attribute didn't get updated");
        }
    }
}
