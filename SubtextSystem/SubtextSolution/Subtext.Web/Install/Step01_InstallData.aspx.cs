using System;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class Step01_InstallData : InstallationBase
	{
		protected System.Web.UI.WebControls.Literal ltlMessage;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.ContentRegion Content;
		protected System.Web.UI.WebControls.CheckBox chkStoredProcs;
		protected System.Web.UI.WebControls.RadioButton radUpgrade;
		protected System.Web.UI.WebControls.RadioButton radInstallFresh;
		protected Subtext.Web.Controls.MasterPage MPContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			ltlMessage.Text = string.Empty;
			
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

		}
		#endregion
	}
}
