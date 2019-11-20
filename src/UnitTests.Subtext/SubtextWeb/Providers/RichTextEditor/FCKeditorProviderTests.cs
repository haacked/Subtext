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
using System.Security.Principal;
using System.Threading;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework;
using Subtext.Providers.BlogEntryEditor.FCKeditor;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
    /// <summary>
    /// Summary description for FCKeditorProviderTests.
    /// </summary>
    [TestClass]
    public class FCKeditorProviderTests
    {
        string _hostName;
        FckBlogEntryEditorProvider frtep;

        [TestInitialize]
        public void TestInitialize()
        {
            _hostName = UnitTestHelper.GenerateUniqueHostname();

            IPrincipal principal = UnitTestHelper.MockPrincipalWithRoles("Admins");
            Thread.CurrentPrincipal = principal;
            frtep = new FckBlogEntryEditorProvider();
            UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog", "Subtext.Web");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Thread.CurrentPrincipal = null;
        }

        [TestMethod]
        public void SetControlID()
        {
            string test = "MyTestControlID";
            frtep.ControlId = test;
            Assert.AreEqual(test, frtep.ControlId);
        }

        [TestMethod]
        public void SetText()
        {
            var blog = new Blog {Host = "localhost", Subfolder = "subfolder"};
            string test = "Lorem Ipsum";
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            frtep.InitializeControl(subtextContext.Object);
            frtep.Text = test;
            Assert.AreEqual(test, frtep.Text);
            Assert.AreEqual(test, frtep.Xhtml);
        }

        [TestMethod]
        public void SetWidth()
        {
            Unit test = 200;
            var blog = new Blog {Host = "localhost", Subfolder = "subfolder"};
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            frtep.InitializeControl(subtextContext.Object);

            frtep.Width = test;
            Assert.AreEqual(test, frtep.Width);
        }

        [TestMethod]
        public void SetHeight()
        {
            Unit test = 100;
            var blog = new Blog {Host = "localhost", Subfolder = "subfolder"};
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(blog);
            frtep.InitializeControl(subtextContext.Object);
            frtep.Height = test;
            Assert.AreEqual(test, frtep.Height);
        }

        [TestMethod]
        public void TestInitializationWithNullName()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => frtep.Initialize(null, new NameValueCollection()));
        }

        [TestMethod]
        public void TestInitializationWithNullConfigValue()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => frtep.Initialize("FCKProvider", null));
        }

        [TestMethod]
        public void TestInitializationWithEmptyWebFolder()
        {
            UnitTestHelper.AssertThrows<InvalidOperationException>(() => frtep.Initialize("FCKProvider", new NameValueCollection()));
        }
    }
}