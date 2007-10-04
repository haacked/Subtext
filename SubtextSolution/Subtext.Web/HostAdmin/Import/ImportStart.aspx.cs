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

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// First page in the .TEXT Import wizard.
	/// </summary>
	public partial class ImportStart : HostAdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
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

		}
		#endregion

		protected void btnNext_Click(object sender, EventArgs e)
		{
			Response.Redirect("Step02_GatherInfo.aspx?Provider=DotText095ImportProvider");
		}

		private void btnRestartWizard_Click(object sender, EventArgs e)
		{
			Response.Redirect("ImportStart.aspx");
		}
	}
}
