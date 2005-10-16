using System;
using System.Web.UI;
using Subtext.Extensibility.Providers;
using Subtext.Framework;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Page used to gather information for the specified import provider.
	/// </summary>
	public class Step02_GatherInfo : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.PlaceHolder plcImportInformation;
		protected System.Web.UI.WebControls.Button btnNext;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.Literal ltlErrorMessage;
		protected System.Web.UI.WebControls.Button btnBeginImport;
		protected System.Web.UI.HtmlControls.HtmlGenericControl paraBeginImportText;
		ProviderInfo _providerInfo = null;
		protected Subtext.Web.Controls.ContentRegion MPSectionTitle;
		Control importInformationControl = null;

		private void Page_Load(object sender, System.EventArgs e)
		{
			paraBeginImportText.Visible = false;
			SetProviderFormQueryString();

			BindImportInformationControl();
		}

		private void SetProviderFormQueryString()
		{
			string providerName = Request.QueryString["Provider"];
			if(providerName == null || providerName.Length == 0)
				Response.Redirect("Step01_SelectImportProvider.aspx");
	
			_providerInfo = ImportProvider.Providers[providerName];
			if(_providerInfo == null)
				Response.Redirect("Step01_SelectImportProvider.aspx");
		}

		// Adds the control from the import provider used to 
		// display this control.
		void BindImportInformationControl()
		{
			importInformationControl = ImportManager.GetImportInformationControl(_providerInfo);
			if(importInformationControl != null)
			{
				if(importInformationControl.ID == null || importInformationControl.ID.Length == 0)
					importInformationControl.ID = "importInformationControl";

				this.plcImportInformation.Controls.Add(importInformationControl);
			}
			else
			{
				string noDataNeededMessage = "It looks like this import wizard doesn&#8217;t " 
					+ "need any more information.  You&#8217;re all set.";
				this.plcImportInformation.Controls.Add(new LiteralControl(noDataNeededMessage));
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			string errors = ImportManager.ValidateImportAnswers(importInformationControl, _providerInfo);
			if(errors == null || errors.Length == 0)
			{
				ImportManager.ValidateImportAnswers(importInformationControl, _providerInfo);
				paraBeginImportText.Visible = true;
				btnNext.Visible = false;
			}
			else
			{
				ltlErrorMessage.Text = errors;
			}
		}

		private void btnBeginImport_Click(object sender, EventArgs e)
		{
			string errors = ImportManager.ValidateImportAnswers(importInformationControl, _providerInfo);
			if(errors != null && errors.Length > 0)
			{
				//Ok, user must have changed the data despite disabling the control.
				paraBeginImportText.Visible = false;
				btnNext.Visible = true;
				ltlErrorMessage.Text = errors;
				return;
			}

			//Here we go.
			try
			{
				if(ImportManager.Import(this.importInformationControl, this._providerInfo))
				{
					Response.Redirect("ImportComplete.aspx");	
					return;
				}
				else
				{
					this.ltlErrorMessage.Text = "An unexpected unknown error occured.  I know, the worst kind.";
				}
			}
			catch(Exception exception)
			{
				this.ltlErrorMessage.Text = "Oooh. We had trouble with the import.  The error message follows : " + exception.Message;
			}
			paraBeginImportText.Visible = false;
			btnNext.Visible = true;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			this.btnBeginImport.Click += new System.EventHandler(this.btnBeginImport_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
