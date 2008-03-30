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
using Subtext.Scripting.Exceptions;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public partial class Step01_InstallData : InstallationBase
	{
		InstallationState _state = InstallationState.None;
	
		protected void Page_Load(object sender, EventArgs e)
		{
			_state = InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion);
			switch(_state)
			{
				case InstallationState.NeedsInstallation:
					installationStateMessage.Text = "It appears that you are in need of a full " 
						+ "installation of the Subtext Database and Stored Procedures.";
					chkFullInstallation.Visible = false;
					break;
				
				case InstallationState.NeedsUpgrade:
					installationStateMessage.Text = "As far as I can tell, it appears that you are performing an " 
						+ "upgrade from .TEXT 0.95 to Subtext 1.0.  If you wish to perform a clean install, "
						+ "check the &#8220;Full Install&#8221; checkbox.";
					break;
				
				case InstallationState.Complete:
					installationStateMessage.Text = "Hmmm, It seems to me that your installation "
						+ "is complete. If you&#8217;d like to overwrite the current installation "
						+ "check the &#8220;Full Install&#8221; checkbox.";
					break;

				default:
					installationStateMessage.Text = "Hmmm, your installation is in an unknown state. "
						+ "If you&#8217;d like to overwrite the current installation "
						+ "check the &#8220;Full Install&#8221; checkbox.";
					break;
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
			this.btnInstall.Click += new EventHandler(btnInstall_Click);

		}
		#endregion

		private void btnInstall_Click(object sender, EventArgs e)
		{
			if(chkFullInstallation.Checked)
			{
				Extensibility.Providers.Installation.Provider.Install(VersionInfo.FrameworkVersion);
				Response.Redirect(NextStepUrl);
				return;
			}

			try
				{
			switch(_state)
			{
				
					case InstallationState.NeedsInstallation:
						Extensibility.Providers.Installation.Provider.Install(VersionInfo.FrameworkVersion);
						break;

					case InstallationState.NeedsUpgrade:
						Extensibility.Providers.Installation.Provider.Upgrade();
						break;

					default:
						installationStateMessage.Text = "Hmmm, your installation is in an unknown state. ";
						break;
				}
			}
			catch (SqlScriptExecutionException ex)
			{
				if (Extensibility.Providers.Installation.Provider.IsPermissionDeniedException(ex))
				{
					installationStateMessage.Text = "<p class=\"error\">The database user specified in web.config does not have enough "
						+ "permission to perform the installation.  Please give the user database owner (dbo) rights and try again. "
						+ "You may remove them later.</p>";
					return;
				}

				installationStateMessage.Text = "<p>Uh oh. Something went wrong with the installation.</p><p>" + ex.Message + "</p><p>" + ex.GetType().FullName + "</p>";
				installationStateMessage.Text += "<p>" + ex.StackTrace + "</p>";
				return;
			}

			Response.Redirect(NextStepUrl);
		}
	}
}
