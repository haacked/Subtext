using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Web.Controls;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class ImportManagerTests
	{
		[Test]
		public void CanGetImportInformationControl()
		{
			Control control = ImportManager.GetImportInformationControl(ImportProvider.Instance());
			Assert.IsNotNull(control);
			Panel panel = (Panel)control;
			ConnectionStringBuilder connBuilder = (ConnectionStringBuilder) panel.Controls[0];
			Assert.AreEqual("ctlConnectionStringBuilder", connBuilder.ID, "The id did not match our expectations.");
		}

		#region Exception Tests
		[Test]
		[ExpectedArgumentNullException]
		public void GetImportInformationControlThrowsArgumentNullException()
		{
			ImportManager.GetImportInformationControl(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ValidateImportAnswersThrowsArgumentNullExceptionForNullControl()
		{
			ImportManager.ValidateImportAnswers(null, ImportProvider.Instance());
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ValidateImportAnswersThrowsArgumentNullExceptionForNullProvider()
		{
			ImportManager.ValidateImportAnswers(new Label(), null);
		}
		#endregion
	}
}
