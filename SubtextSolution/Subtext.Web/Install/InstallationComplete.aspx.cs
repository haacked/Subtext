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
	public partial class InstallationComplete : InstallationBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			InstallationManager.ResetInstallationStatusCache();
			if(paraBlogLink != null) paraBlogLink.Visible = false;
			if(paraBlogAdminLink != null) paraBlogAdminLink.Visible = false;
			if(paraBlogmlImport != null) paraBlogmlImport.Visible = false;
			
			if(Config.CurrentBlog != null)
			{
				if(lnkBlog != null && paraBlogLink != null)
				{
					paraBlogLink.Visible = true;
					lnkBlog.HRef = Config.CurrentBlog.HomeVirtualUrl;
				}
				
				if(lnkBlogAdmin != null && paraBlogAdminLink != null)
				{
					paraBlogAdminLink.Visible = true;
					lnkBlogAdmin.HRef = Config.CurrentBlog.AdminHomeVirtualUrl;
				}
				
				if(lnkBlogMl != null && paraBlogmlImport != null)
				{
					paraBlogmlImport.Visible = true;
					lnkBlogMl.HRef = Config.CurrentBlog.AdminDirectoryVirtualUrl + "ImportExport.aspx";
				}
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
