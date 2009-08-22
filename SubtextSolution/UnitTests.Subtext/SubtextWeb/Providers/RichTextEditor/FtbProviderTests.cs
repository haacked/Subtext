#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web.UI.WebControls;
using FreeTextBoxControls;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Web.Providers.BlogEntryEditor.FTB;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
    /// <summary>
    /// Summary description for FtbProviderTests.
    /// </summary>
    [TestFixture]
    public class FtbProviderTests
    {
        string _hostName;
        readonly string _testToolbarLayout = "Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|InsertTable|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,Insert,InsertRule|Cut,Copy,Paste;Undo,Redo|ieSpellCheck,WordClean|InsertImage,InsertImageFromGallery";
        FtbBlogEntryEditorProvider frtep;

        [SetUp]
        public void SetUp()
        {
            _hostName = UnitTestHelper.GenerateUniqueHostname();
            frtep = new FtbBlogEntryEditorProvider();
            UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog", "Subtext.Web");
        }

        [Test]
        public void SetControlID()
        {
            string test = "MyTestControlID";
            frtep.ControlId = test;
            Assert.AreEqual(test, frtep.ControlId);
        }

        [Test]
        public void SetText()
        {
            Blog blog = new Blog { Host = "localhost", Subfolder = "subfolder" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.ImageDirectoryUrl(blog)).Returns("/images");

            string test = "Lorem Ipsum";
            frtep.InitializeControl(subtextContext.Object);
            frtep.Text = test;
            Assert.AreEqual(test, frtep.Text);
            Assert.AreEqual(test, frtep.Xhtml);
        }

        [Test]
        public void SetWidth()
        {
            Blog blog = new Blog { Host = "localhost", Subfolder = "subfolder" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.ImageDirectoryUrl(blog)).Returns("/images");

            Unit test = 200;
            frtep.InitializeControl(subtextContext.Object);
            frtep.Width = test;
            Assert.AreEqual(test, frtep.Width);
        }

        [Test]
        public void SetHeight()
        {
            Blog blog = new Blog { Host = "localhost", Subfolder = "subfolder" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.ImageDirectoryUrl(blog)).Returns("/images");

            Unit test = 100;
            frtep.InitializeControl(subtextContext.Object);
            frtep.Height = test;
            Assert.AreEqual(test, frtep.Height);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitializationWithNullName()
        {
            frtep.Initialize(null, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitializationWithNullConfigValue()
        {
            frtep.Initialize("FTBProvider", null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInitializationWithEmptyWebFolder()
        {
            frtep.Initialize("FTBProvider", new System.Collections.Specialized.NameValueCollection());
        }

        [Test]
        public void TestInitialization()
        {
            Blog blog = new Blog { Host = "localhost", Subfolder = "subfolder" };
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            subtextContext.Setup(c => c.UrlHelper.ImageDirectoryUrl(blog)).Returns("/images");

            System.Collections.Specialized.NameValueCollection coll = GetNameValueCollection();
            frtep.Initialize("FTBProvider", coll);
            frtep.InitializeControl(subtextContext.Object);
            Assert.IsTrue(frtep.RichTextEditorControl.GetType() == typeof(FreeTextBox));
            FreeTextBox txt = frtep.RichTextEditorControl as FreeTextBox;
            Assert.AreEqual(frtep.Name, "FTBProvider");
            Assert.AreEqual(txt.ToolbarLayout, _testToolbarLayout);
            Assert.AreEqual(txt.FormatHtmlTagsToXhtml, true);
            Assert.AreEqual(txt.RemoveServerNameFromUrls, false);
        }

        private System.Collections.Specialized.NameValueCollection GetNameValueCollection()
        {
            System.Collections.Specialized.NameValueCollection ret = new System.Collections.Specialized.NameValueCollection(3);
            ret.Add("WebFormFolder", "~/Providers/RichTextEditor/FTB/");
            ret.Add("toolbarlayout", _testToolbarLayout);
            ret.Add("FormatHtmlTagsToXhtml", "true");
            ret.Add("RemoveServerNamefromUrls", "false");
            return ret;
        }
    }
}
