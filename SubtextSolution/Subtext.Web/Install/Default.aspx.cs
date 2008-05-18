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
	public partial class Default : InstallationBase
	{		
		protected override void OnLoad(EventArgs e)
		{
			if(InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion) == InstallationState.Complete)
			{
				Response.Redirect("InstallationComplete.aspx");
			}

            this.btnInstallClick.Attributes["onclick"] = "this.disabled=true;" 
                + ClientScript.GetPostBackEventReference(this.btnInstallClick, null).ToString();

		
			litDatabaseName.Text = Config.Settings.ConnectionString.Database;
		}

		protected virtual void OnInstallClick(object sender, EventArgs e)
		{
			Extensibility.Providers.Installation.Provider.Install(VersionInfo.FrameworkVersion);
			Response.Redirect(NextStepUrl);
		}
    }
}
