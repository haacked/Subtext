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
using FreeTextBoxControls;
using Subtext.Framework.Configuration;
using Subtext.Web.Providers.BlogEntryEditor.FTB;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
	/// <summary>
	/// Summary description for FtbProviderTests.
	/// </summary>
	[TestFixture]
	public class FtbProviderTests
	{
		readonly string _hostName = Guid.NewGuid().ToString().Replace("-", string.Empty) + ".com";
		readonly string _testToolbarLayout = "Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|FontFacesMenu,FontSizesMenu,FontForeColorsMenu|InsertTable|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,Insert,InsertRule|Cut,Copy,Paste;Undo,Redo|ieSpellCheck,WordClean|InsertImage,InsertImageFromGallery";
		FtbBlogEntryEditorProvider provider;

		[SetUp]
		public void SetUp()
		{
            this.provider = new FtbBlogEntryEditorProvider();
			this.provider.Initialize("FtbProvider", GetNameValueCollection());
		}

		private System.Collections.Specialized.NameValueCollection GetNameValueCollection()
		{
			NameValueCollection configValues = new NameValueCollection(3);
			configValues.Add("WebFormFolder", "~/Providers/RichTextEditor/FTB/");
			configValues.Add("toolbarlayout", _testToolbarLayout);
			configValues.Add("FormatHtmlTagsToXhtml", "true");
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
		[RollBack2]
		public void SetText() 
		{
			UnitTestHelper.SetupBlog();

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
			this.provider.Initialize("FTBProvider", null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInitializationWithEmptyWebFolder() 
		{
			this.provider.Initialize("FTBProvider", new NameValueCollection());
		}

		[Test]
		[RollBack]
		public void TestInitialization() 
		{
			UnitTestHelper.SetupBlog();
			
			NameValueCollection coll = GetNameValueCollection();
			this.provider = new FtbBlogEntryEditorProvider();
			this.provider.Initialize("FTBProvider", coll);
			this.provider.InitializeControl();
			Assert.IsTrue(this.provider.RichTextEditorControl.GetType()==typeof(FreeTextBox));
			FreeTextBox txt = this.provider.RichTextEditorControl as FreeTextBox;
			Assert.AreEqual(this.provider.Name,"FTBProvider");
			Assert.AreEqual(txt.ToolbarLayout,_testToolbarLayout);
			Assert.AreEqual(txt.FormatHtmlTagsToXhtml,true);
			Assert.AreEqual(txt.RemoveServerNameFromUrls,false);
		}
	}
}
