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
using System.Web.UI.WebControls;
using Subtext.Web.Providers.BlogEntryEditor.PlainText;

namespace UnitTests.Subtext.SubtextWeb.Providers.RichTextEditor
{
	/// <summary>
	/// Summary description for PlainTextProviderTests.
	/// </summary>
	[TestFixture]
	public class PlainTextProviderTests
	{
		PlainTextBlogEntryEditorProvider ptrtep;

		[SetUp]
		public void SetUp()
		{
            ptrtep = new PlainTextBlogEntryEditorProvider();
		}

		[Test]
		public void SetControlID() 
		{
			string test="MyTestControlID";
			ptrtep.ControlId=test;
			Assert.AreEqual(test,ptrtep.ControlId);
		}

		[Test]
		public void SetText() 
		{
			string test="Lorem Ipsum";
			ptrtep.InitializeControl();
			ptrtep.Text=test;
			Assert.AreEqual(test,ptrtep.Text);
			Assert.AreEqual(test,ptrtep.Xhtml);
		}

		[Test]
		public void SetWidth() 
		{
			Unit test=200;
			ptrtep.InitializeControl();
			ptrtep.Width=test;
			Assert.AreEqual(test,ptrtep.Width);
		}

		[Test]
		public void SetHeight() 
		{
			Unit test=100;
			ptrtep.InitializeControl();
			ptrtep.Height=test;
			Assert.AreEqual(test,ptrtep.Height);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitializationWithNullName() 
		{
			ptrtep.Initialize(null,null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitializationWithNullConfigValue() 
		{
			ptrtep.Initialize("PlainTextProvider",null);
		}


		[Test]
		public void TestInitialization() 
		{
			System.Collections.Specialized.NameValueCollection coll=GetNameValueCollection();
			ptrtep.Initialize("PlainTextProvider", coll);
			ptrtep.InitializeControl();
			Assert.IsTrue(ptrtep.RichTextEditorControl.GetType()==typeof(TextBox));
			TextBox txt = ptrtep.RichTextEditorControl as TextBox;
			Assert.AreEqual(ptrtep.Name,"PlainTextProvider");
			Assert.AreEqual(txt.Rows,10);
			Assert.AreEqual(txt.Columns,70);
			Assert.AreEqual(txt.CssClass,"myCssClass");
		}

		private System.Collections.Specialized.NameValueCollection GetNameValueCollection() 
		{
			System.Collections.Specialized.NameValueCollection ret=new System.Collections.Specialized.NameValueCollection(3);
			ret.Add("rows","10");
			ret.Add("cols","70");
			ret.Add("cssClass","myCssClass");
			return ret;
		}
	}
}
