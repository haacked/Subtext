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
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Web.Providers.BlogEntryEditor.PlainText;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
    /// <summary>
    /// Summary description for PlainTextProviderTests.
    /// </summary>
    [TestFixture]
    public class PlainTextProviderTests
    {
        [Test]
        public void SetControlID()
        {
            string test = "MyTestControlID";
            var provider = new PlainTextBlogEntryEditorProvider();
            provider.ControlId = test;
            Assert.AreEqual(test, provider.ControlId);
        }

        [Test]
        public void SetText()
        {
            string test = "Lorem Ipsum";
            var provider = new PlainTextBlogEntryEditorProvider();
            provider.InitializeControl(new Mock<ISubtextContext>().Object);
            provider.Text = test;
            Assert.AreEqual(test, provider.Text);
            Assert.AreEqual(test, provider.Xhtml);
        }

        [Test]
        public void SetWidth()
        {
            Unit test = 200;
            var provider = new PlainTextBlogEntryEditorProvider();
            provider.InitializeControl(new Mock<ISubtextContext>().Object);
            provider.Width = test;
            Assert.AreEqual(test, provider.Width);
        }

        [Test]
        public void SetHeight()
        {
            Unit test = 100;
            var provider = new PlainTextBlogEntryEditorProvider();
            provider.InitializeControl(new Mock<ISubtextContext>().Object);
            provider.Height = test;
            Assert.AreEqual(test, provider.Height);
        }

        [Test]
        public void TestInitializationWithNullName()
        {
            var provider = new PlainTextBlogEntryEditorProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                                                               provider.Initialize(null, null)
                );
        }

        [Test]
        public void TestInitializationWithNullConfigValue()
        {
            var provider = new PlainTextBlogEntryEditorProvider();
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                                                               provider.Initialize("PlainTextProvider", null)
                );
        }

        [Test]
        public void TestInitialization()
        {
            var provider = new PlainTextBlogEntryEditorProvider();
            NameValueCollection coll = GetNameValueCollection();
            provider.Initialize("PlainTextProvider", coll);
            provider.InitializeControl(new Mock<ISubtextContext>().Object);
            Assert.IsTrue(provider.RichTextEditorControl.GetType() == typeof(TextBox));
            var txt = provider.RichTextEditorControl as TextBox;
            Assert.AreEqual(provider.Name, "PlainTextProvider");
            Assert.AreEqual(txt.Rows, 10);
            Assert.AreEqual(txt.Columns, 70);
            Assert.AreEqual(txt.CssClass, "myCssClass");
        }

        private NameValueCollection GetNameValueCollection()
        {
            var ret = new NameValueCollection(3);
            ret.Add("rows", "10");
            ret.Add("cols", "70");
            ret.Add("cssClass", "myCssClass");
            return ret;
        }
    }
}