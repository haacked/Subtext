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
using Subtext.Extensibility.Providers;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Summary description for Step01_SelectImportProvider.
	/// </summary>
	public partial class Step01_SelectImportProvider : HostAdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if(!IsPostBack)
			{
				BindData();
			}
		}

		void BindData()
		{
			this.rdlImportProviders.DataSource = ImportProvider.Providers;
			this.rdlImportProviders.DataTextField = "Description";
			this.rdlImportProviders.DataValueField = "Name";
			this.rdlImportProviders.DataBind();
		}
		
		protected void btnNext_Click(object sender, EventArgs e)
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

		}
		#endregion


	}
}
