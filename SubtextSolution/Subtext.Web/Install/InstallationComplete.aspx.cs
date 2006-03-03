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
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Summary description for InstallationComplete.
	/// </summary>
	public class InstallationComplete : InstallationBase
	{
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected System.Web.UI.HtmlControls.HtmlAnchor lnkHostAdmin;
		protected System.Web.UI.HtmlControls.HtmlAnchor lnkBlog;
		protected System.Web.UI.HtmlControls.HtmlAnchor importWizardAnchor;
		protected Subtext.Web.Controls.MasterPage MPContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			InstallationManager.ResetInstallationStatusCache();
			if(Config.CurrentBlog != null)
			{
				lnkBlog.HRef = Config.CurrentBlog.BlogHomeVirtualUrl;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
