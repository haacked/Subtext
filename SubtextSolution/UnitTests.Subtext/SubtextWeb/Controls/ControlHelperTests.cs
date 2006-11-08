using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Subtext.Web.Controls;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
	[TestFixture]
	public class ControlHelperTests
	{
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
		}
		
		[Test]
		public void GetPageFormClientIdCanFindFormWhenParentIsForm()
		{
			HtmlForm form = new HtmlForm();
			form.ID = "myForm";

			Assert.AreEqual("myForm", ControlHelper.GetPageFormClientId(form));
		}
		
		[Test]
		public void GetPageFormClientIdCanFindChildForm()
		{
			PlaceHolder parent = new PlaceHolder();
			HtmlForm form = new HtmlForm();
			form.ID = "myForm";
			
			parent.Controls.Add(form);
			
			Assert.AreEqual("myForm", ControlHelper.GetPageFormClientId(parent));
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
			ControlHelper.ApplyRecursively(new ControlAction(TestControlAction), null);
		}
		
		void TestControlAction(Control control)
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
		#endregion
	}
}
