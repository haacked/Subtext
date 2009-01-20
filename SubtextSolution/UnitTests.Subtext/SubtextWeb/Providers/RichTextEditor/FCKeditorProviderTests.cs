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
using System.Security.Principal;
using System.Threading;
using MbUnit.Framework;
using System.Web.UI.WebControls;
using Rhino.Mocks;
using Subtext.Framework.Configuration;
using Subtext.Providers.BlogEntryEditor.FCKeditor;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
	/// <summary>
	/// Summary description for FCKeditorProviderTests.
	/// </summary>
	[TestFixture]
	public class FCKeditorProviderTests
	{
        string _hostName;
		FckBlogEntryEditorProvider frtep;
		readonly MockRepository mocks = new MockRepository();

		[SetUp]
		public void SetUp()
		{
            _hostName = UnitTestHelper.GenerateUniqueHostname();

			IPrincipal principal;
			UnitTestHelper.SetCurrentPrincipalRoles(mocks, out principal, "Admins");
			mocks.ReplayAll();
			Thread.CurrentPrincipal = principal;
			frtep = new FckBlogEntryEditorProvider();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog", "Subtext.Web");
		}

		[TearDown]
		public void TearDown()
		{
			Thread.CurrentPrincipal = null;
			mocks.VerifyAll();
		}

		[Test]
		public void SetControlID() 
		{
			string test="MyTestControlID";
			frtep.ControlId=test;
			Assert.AreEqual(test,frtep.ControlId);
		}

		[Test]
		[RollBack]
		public void SetText() 
		{
			Config.CreateBlog("", "username", "password", _hostName, "MyBlog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "MyBlog");
			string test="Lorem Ipsum";
			frtep.InitializeControl();
			frtep.Text=test;
			Assert.AreEqual(test,frtep.Text);
			Assert.AreEqual(test,frtep.Xhtml);
		}

		[Test]
        [RollBack]
		public void SetWidth() 
		{
            Config.CreateBlog("", "username", "password", _hostName, "MyBlog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "MyBlog");

			Unit test=200;
			frtep.InitializeControl();
			frtep.Width=test;
			Assert.AreEqual(test,frtep.Width);
		}

		[Test]
        [RollBack]
		public void SetHeight() 
		{
            Config.CreateBlog("", "username", "password", _hostName, "MyBlog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "MyBlog");

			Unit test=100;
			frtep.InitializeControl();
			frtep.Height=test;
			Assert.AreEqual(test,frtep.Height);
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
			frtep.Initialize("FCKProvider", null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInitializationWithEmptyWebFolder() 
		{
			frtep.Initialize("FCKProvider", new System.Collections.Specialized.NameValueCollection());
		}
	}
}
