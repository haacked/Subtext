#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
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
        protected override void OnLoad(EventArgs e)
        {
            if (Subtext.Extensibility.Providers.Installation.Provider.GetInstallationStatus(VersionInfo.FrameworkVersion) == InstallationState.Complete)
            {
                Response.Redirect("~/HostAdmin/Upgrade/UpgradeComplete.aspx");
            }

            btnUpgrade.Attributes["onclick"] = "this.disabled=true;"
                + ClientScript.GetPostBackEventReference(btnUpgrade, null).ToString();
            base.OnLoad(e);
        }

		protected void OnUpgradeClick(object sender, EventArgs e)
		{
			plcHolderUpgradeMessage.Visible = false;
			try
			{
				Extensibility.Providers.Installation.Provider.Upgrade();
                Response.Redirect("~/HostAdmin/Upgrade/UpgradeComplete.aspx");
			}
			catch (SqlScriptExecutionException ex)
			{
				plcHolderUpgradeMessage.Visible = true;
				
				if (Extensibility.Providers.Installation.Provider.IsPermissionDeniedException(ex))
				{
                    messageLiteral.Text = Resources.Upgrade_UserDoesNotHavePermission;
					return;
				}

				messageLiteral.Text = Resources.Upgrade_SomethingWentWrongWithInstall + "<p>" + ex.Message + "</p><p>" + ex.GetType().FullName + "</p>";
				messageLiteral.Text += "<p>" + ex.StackTrace + "</p>";
				
				return;
			}
		}
	}
}
