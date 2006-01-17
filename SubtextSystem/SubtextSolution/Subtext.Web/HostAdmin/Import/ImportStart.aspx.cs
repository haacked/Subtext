using System;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// First page in the .TEXT Import wizard.
	/// </summary>
	public class ImportStart : System.Web.UI.Page
	{
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.Button btnRestartWizard;
		protected Subtext.Web.Controls.ContentRegion MPSideBar;
		protected System.Web.UI.WebControls.Button btnNext;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
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
			this.btnRestartWizard.Click += new System.EventHandler(this.btnRestartWizard_Click);
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnNext_Click(object sender, EventArgs e)
		{
			Response.Redirect("Step02_GatherInfo.aspx?Provider=DotText095ImportProvider");
		}

		private void btnRestartWizard_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("ImportStart.aspx");
		}
	}
}
