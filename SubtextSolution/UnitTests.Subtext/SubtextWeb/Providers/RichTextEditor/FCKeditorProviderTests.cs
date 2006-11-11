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
using System.Collections.Specialized;
using MbUnit.Framework;
using System.Web.UI.WebControls;
using Subtext.Providers.BlogEntryEditor.FCKeditor;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
	/// <summary>
	/// Summary description for FCKeditorProviderTests.
	/// </summary>
	[TestFixture]
	[Author("Simone Chiaretta", "simone@piyosailing.com", "http://blogs.ugidotnet.org/piyo/")]
	public class FCKeditorProviderTests
	{
		FckBlogEntryEditorProvider provider;

		[SetUp]
		public void SetUp()
		{
            this.provider = new FckBlogEntryEditorProvider();
			provider.Initialize("FKProvider", GetNameValueCollection());
		}

		private NameValueCollection GetNameValueCollection()
		{
			NameValueCollection configValues = new NameValueCollection();
			configValues.Add("WebFormFolder", "~/Providers/BlogEntryEditor/FCKeditor/");
			configValues.Add("ImageBrowserURL", "/imagebrowser/default.aspx");
			configValues.Add("LinkBrowserURL", "/LinkBrowser/default.aspx");
			configValues.Add("ImageConnectorURL", "/ImageConnector/default.aspx");
			configValues.Add("LinkConnectorURL", "/LinkConnector/default.aspx");
			configValues.Add("FileAllowedExtensions", ".htm, .txt");
			configValues.Add("ImageAllowedExtensions", ".gif, .jpg");
			configValues.Add("ToolbarSet", "SubText");
			configValues.Add("Skin", "office2003");
			configValues.Add("RemoveServerNamefromUrls", "false");
			return configValues;
		}

		[Test]
		public void SetControlID() 
		{
			string test="MyTestControlID";
			this.provider.ControlId=test;
			Assert.AreEqual(test,this.provider.ControlId);
		}

		[Test]
		[RollBack]
		public void SetText() 
		{
			UnitTestHelper.SetupBlog();
			
			string test="Lorem Ipsum";
			this.provider.InitializeControl();
			this.provider.Text=test;
			Assert.AreEqual(test,this.provider.Text);
			Assert.AreEqual(test,this.provider.Xhtml);
		}

		[Test]
		[RollBack]
		public void SetWidth() 
		{
			UnitTestHelper.SetupBlog();
			
			Unit test=200;
			this.provider.InitializeControl();
			this.provider.Width=test;
			Assert.AreEqual(test,this.provider.Width);
		}

		[Test]
		[RollBack]
		public void SetHeight() 
		{
			UnitTestHelper.SetupBlog();
			
			Unit test=100;
			this.provider.InitializeControl();
			this.provider.Height=test;
			Assert.AreEqual(test,this.provider.Height);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitializationWithNullName() 
		{
			this.provider.Initialize(null, new NameValueCollection());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitializationWithNullConfigValue() 
		{
			this.provider.Initialize("FCKProvider", null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInitializationWithEmptyWebFolder() 
		{
			this.provider.Initialize("FCKProvider", new NameValueCollection());
		}
	}

}
