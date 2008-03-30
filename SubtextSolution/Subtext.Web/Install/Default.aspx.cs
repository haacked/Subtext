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
	public partial class Default : System.Web.UI.Page
	{		
		protected void Page_Load(object sender, EventArgs e)
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
			btnNext.Click += new EventHandler(btnNext_Click);

		}
		#endregion

		private void btnNext_Click(object sender, EventArgs e)
		{
			Response.Redirect(InstallationBase.NextStepUrl);
		}
	}
}
