using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Summary description for Step01_SelectImportProvider.
	/// </summary>
	public class Step01_SelectImportProvider : System.Web.UI.Page
	{
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected System.Web.UI.WebControls.Button btnNext;
		protected System.Web.UI.WebControls.RadioButtonList rdlImportProviders;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldImportProviders;
		protected Subtext.Web.Controls.ContentRegion MPSectionTitle;
		protected Subtext.Web.Controls.MasterPage MPContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				BindData();
			}
		}

		void BindData()
		{
			this.rdlImportProviders.DataSource = ImportProvider.Providers.Values;
			this.rdlImportProviders.DataTextField = "Description";
			this.rdlImportProviders.DataValueField = "Name";
			this.rdlImportProviders.DataBind();
		}
		
		private void btnNext_Click(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				Response.Redirect("Step02_GatherInfo.aspx?Provider=" + this.rdlImportProviders.SelectedValue);
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
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


	}
}
