using System;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class Default : System.Web.UI.Page
	{
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected System.Web.UI.WebControls.HyperLink lnkNextStep;
		protected System.Web.UI.WebControls.Literal litDatabaseName;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.Button btnNext;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion) == InstallationState.Complete)
			{
				Response.Redirect("InstallationComplete.aspx");
			}
		
			litDatabaseName.Text = Config.Settings.ConnectionString.Database;
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
			this.Load += new System.EventHandler(this.Page_Load);
			btnNext.Click += new EventHandler(btnNext_Click);

		}
		#endregion

		private void btnNext_Click(object sender, EventArgs e)
		{
			Response.Redirect(InstallationBase.NextStepUrl);
		}
	}
}
