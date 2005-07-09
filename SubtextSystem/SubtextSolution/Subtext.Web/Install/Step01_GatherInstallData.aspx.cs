using System;
using System.Web.UI;
using Subtext.Framework;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class Step01_GatherInstallData : InstallationBase
	{
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.ContentRegion Content;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Literal ltlErrorMessage;
		protected System.Web.UI.WebControls.CheckBox chkFullInstallation;
		protected System.Web.UI.WebControls.Panel pnlInstallationInformation;
		Control installationInformationControl = null;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			EnableViewState = false;
			DataBind();
		}

		/// <summary>
		/// Binds the data to the form controls.
		/// </summary>
		public override void DataBind()
		{
			installationInformationControl = InstallationManager.GetInstallationInformationControl();
			pnlInstallationInformation.Controls.Add(installationInformationControl);
			base.DataBind ();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			string errors = InstallationManager.ValidateInstallationAnswers(installationInformationControl);
			if(errors == null || errors.Length == 0)
			{			
				InstallationManager.SetInstallationQuestionAnswers(installationInformationControl);
				Response.Redirect(NextStepUrl);
			}
			else
			{
				ltlErrorMessage.Text = errors;
			}
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
