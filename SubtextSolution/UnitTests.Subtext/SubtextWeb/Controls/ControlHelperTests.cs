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
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.TestLibrary;
using Subtext.Web.Controls;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
	[TestFixture]
	public class ControlHelperTests
	{
		[Test]
		public void CanApplyRecursively()
		{
			Panel root = new Panel();
			root.ID = "root";

			Panel child1 = new Panel();
			child1.ID = "child1";

			Panel child2 = new Panel();
			child2.ID = "child2";

			TextBox textBox = new TextBox();
			textBox.ID = "txtBox";
			child2.Controls.Add(textBox);

			root.Controls.Add(child1);
			root.Controls.Add(child2);
			
			ControlHelper.ApplyRecursively(delegate(Control control) { 
			{
				TextBox txtBox = control as TextBox;
				if (txtBox != null)
					txtBox.Text = "Surprise!";
			}
			}, root);

			Assert.AreEqual("Surprise!", textBox.Text, "Expected the control action to be applied recursively.");
		}

		[RowTest]
		[Row("Subtext.Web", "~/", "/Subtext.Web/")]
		[Row("", "~/", "/")]
		[Row("Subtext.Web", "~/Something/", "/Subtext.Web/Something/")]
		[Row("", "/Something/", "/Something/")]
		[Row("Subtext.Web", "/Something/", "/Something/")]
		public void CanExpandTildePath(string applicationPath, string path, string expected)
		{
			using (new HttpSimulator(applicationPath).SimulateRequest())
			{
				string result = ControlHelper.ExpandTildePath(path);
				Assert.AreEqual(expected, result, "Did not expand tilde correctly.");
			}
		}
		
		[Test]
		public void CanDetermineIfAttributeIsDefined()
		{
			HtmlForm form = new HtmlForm();
			form.Attributes.Add("test", "it_is_here");
			Assert.IsTrue(ControlHelper.IsAttributeDefined(form, "test"));
			Assert.IsFalse(ControlHelper.IsAttributeDefined(form, "badtest"));

			Label label = new Label();
			label.Attributes.Add("test", "it_is_here");
			Assert.IsTrue(ControlHelper.IsAttributeDefined(label, "test"));
			Assert.IsFalse(ControlHelper.IsAttributeDefined(label, "badtest"));
		}
		
		[Test]
		public void CanFindControlWhenParentIsControl()
		{
			Label label = new Label();
			label.ID = "searchedForId";
			
			Assert.AreSame(label, ControlHelper.FindControlRecursively(label, "searchedForId"));
		}

		[Test]
		public void CanFindControlRecursively()
		{
			PlaceHolder parent = new PlaceHolder();
			parent.ID = "notMe";
			PlaceHolder child = new PlaceHolder();
			child.ID = "Not Me Too";
			parent.Controls.Add(child);
			
			Label label = new Label();
			label.ID = "searchedForId";
			child.Controls.Add(label);

			Assert.AreSame(label, ControlHelper.FindControlRecursively(parent, "searchedForId"));
			Assert.IsNull(ControlHelper.FindControlRecursively(parent, "couldNotFindThisId"));
		}
		
		[Test]
		public void GetPageFormClientIdCanFindFormWhenParentIsForm()
		{
			MockRepository mocks = new MockRepository();
			ISite site = mocks.DynamicMock<ISite>();
			SetupResult.For(site.DesignMode).Return(true);
			mocks.ReplayAll();

			Page page = new Page();
			page.ID = "thePage";
			page.Site = site;

			HtmlForm form = new HtmlForm();
			form.Page = page;
			form.ID = "aspnetForm";

			Assert.AreEqual("aspnetForm", ControlHelper.GetPageFormClientId(form));
		}	
		
		[Test]
		public void GetPageFormClientIdCanFindChildForm()
		{
			MockRepository mocks = new MockRepository();
			ISite site = mocks.DynamicMock<ISite>();
			SetupResult.For(site.DesignMode).Return(true);
			mocks.ReplayAll();
			
			Page page = new Page();
			page.ID = "thePage";
			page.Site = site;
			
			PlaceHolder parent = new PlaceHolder();
			parent.ID = "parentPlaceHolder";
			HtmlForm form = new HtmlForm();
			form.ID = "aspnetForm";

			parent.Controls.Add(form);

			parent.Page = page;
			form.Page = page;

			Assert.AreEqual("aspnetForm", ControlHelper.GetPageFormClientId(parent));
			Assert.IsNull(ControlHelper.GetPageFormClientId(new Panel()));
		}

		[Test]
		public void CanSetTitleIfNone()
		{
			Page page = new Page();
			LinkButton button = new LinkButton();
			button.Page = page;

			ControlHelper.SetTitleIfNone(button, "CoolTitle");
			Assert.AreEqual("CoolTitle", button.ToolTip);

			HyperLink link = new HyperLink();
			link.Page = page;
			ControlHelper.SetTitleIfNone(link, "AnotherCoolTitle");
			Assert.AreEqual("AnotherCoolTitle", link.ToolTip);
		}

		#region Argument Validation Tests
		[Test]
		[ExpectedArgumentNullException]
		public void GetPageFormClientIdThrowsArgumentNullException()
		{
			ControlHelper.GetPageFormClientId(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void IsAttributeDefinedThrowsArgumentNullExceptionForNullWebControl()
		{
			WebControl control = null;
			ControlHelper.IsAttributeDefined(control, "name");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void IsAttributeDefinedThrowsArgumentNullExceptionForNullHtmlControl()
		{
			HtmlControl control = null;
			ControlHelper.IsAttributeDefined(control, "name");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void IsAttributeDefinedThrowsArgumentNullExceptionForNullName()
		{
			WebControl control = new Label();
			ControlHelper.IsAttributeDefined(control, null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void IsAttributeDefinedThrowsArgumentNullExceptionForNullNameAndNonNullHtmlControl()
		{
			HtmlControl control = new HtmlForm();
			ControlHelper.IsAttributeDefined(control, null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ExportToExcelThrowsArgumentNullExceptionForNullFileName()
		{
			ControlHelper.ExportToExcel(new Label(), null);
		}

		[Test, Ignore("To Be Continued")]
		public void CanExportToExcel()
		{
			Label label = new Label();
			label.Page = new Page();
			label.Text = "Ha ha";

			using (new HttpSimulator("/").SimulateRequest())
			{
				ControlHelper.ExportToExcel(label, "MyFile.xls");
			}
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ExportToExcelThrowsArgumentNullExceptionForNullControl()
		{
			ControlHelper.ExportToExcel(null, "MyFile.xls");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ApplyRecursivelyThrowsNullArgumentExceptionForNullControlAction()
		{
			ControlHelper.ApplyRecursively(null, new Label());
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ApplyRecursivelyThrowsNullArgumentExceptionForNullControl()
		{
			ControlHelper.ApplyRecursively(TestControlAction, null);
		}
		
		static void TestControlAction(Control control)
		{
		}

		[Test]
		[ExpectedArgumentNullException]
		public void FindControlRecursivelyThrowsNullArgumentExceptionForNullControl()
		{
			ControlHelper.FindControlRecursively(null, "id");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void FindControlRecursivelyThrowsNullArgumentExceptionForNullId()
		{
			ControlHelper.FindControlRecursively(new Label(), null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ExpandTildePathThrowsArgumentNullException()
		{
			ControlHelper.ExpandTildePath(null);
		}
		#endregion
	}
}
