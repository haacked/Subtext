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
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Scripting.Exceptions;
using Subtext.Web.Properties;

namespace Subtext.Web.HostAdmin.Upgrade
{
    /// <summary>
    /// Page used to create an initial configuration for the blog.
    /// </summary>
    /// <remarks>
    /// This page will ONLY be displayed if there are no 
    /// blog configurations within the database.
    /// </remarks>
    public partial class Default : HostAdminPage
    {
        [Inject]
        public IInstallationManager InstallationManager
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (InstallationManager.GetInstallationStatus(VersionInfo.CurrentAssemblyVersion) == InstallationState.Complete)
            {
                Response.Redirect("~/HostAdmin/Upgrade/UpgradeComplete.aspx");
            }

            btnUpgrade.Attributes["onclick"] = "this.disabled=true;"
                                               + ClientScript.GetPostBackEventReference(btnUpgrade, null);
            base.OnLoad(e);
        }

        protected void OnUpgradeClick(object sender, EventArgs e)
        {
            plcHolderUpgradeMessage.Visible = false;
            try
            {
                InstallationManager.Upgrade(VersionInfo.CurrentAssemblyVersion);
                Response.Redirect("~/HostAdmin/Upgrade/UpgradeComplete.aspx");
            }
            catch (SqlScriptExecutionException exception)
            {
                plcHolderUpgradeMessage.Visible = true;

                if (InstallationManager.IsPermissionDeniedException(exception))
                {
                    messageLiteral.Text = Resources.Upgrade_UserDoesNotHavePermission;
                    return;
                }

                messageLiteral.Text = Resources.Upgrade_SomethingWentWrongWithInstall + "<p>" + exception.Message + "</p><p>" +
                                      exception.GetType().FullName + "</p>";
                messageLiteral.Text += "<p>" + exception.StackTrace + "</p>";

                return;
            }
        }
    }
}