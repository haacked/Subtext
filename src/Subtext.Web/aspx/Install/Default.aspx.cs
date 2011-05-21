#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Infrastructure.Installation;

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
            if (InstallationManager.GetInstallationStatus(VersionInfo.CurrentAssemblyVersion) == InstallationState.Complete)
            {
                Response.Redirect("InstallationComplete.aspx");
            }

            btnInstallClick.Attributes["onclick"] = "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnInstallClick, null);


            litDatabaseName.Text = Config.ConnectionString.Database;
        }

        protected virtual void OnInstallClick(object sender, EventArgs e)
        {
            InstallationManager.Install(VersionInfo.CurrentAssemblyVersion);
            Response.Redirect(NextStepUrl);
        }
    }
}