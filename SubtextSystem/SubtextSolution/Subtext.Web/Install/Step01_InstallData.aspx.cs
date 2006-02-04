using System;
using System.Data.SqlClient;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Data;
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
	public class Step01_InstallData : InstallationBase
	{
		InstallationState _state = InstallationState.None;
		protected System.Web.UI.WebControls.Literal installationStateMessage;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.ContentRegion Content;
		protected System.Web.UI.WebControls.CheckBox chkStoredProcs;
		protected System.Web.UI.WebControls.RadioButton radUpgrade;
		protected System.Web.UI.WebControls.RadioButton radInstallFresh;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.WebControls.CheckBox chkFullInstallation;
		protected System.Web.UI.WebControls.Button btnInstall;
	
		private void Page_Load(object sender, System.EventArgs e)
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnInstall_Click(object sender, EventArgs e)
		{
			if(chkFullInstallation.Checked)
			{
				InstallationProvider.Instance().Install(VersionInfo.FrameworkVersion);
				Response.Redirect(NextStepUrl);
				return;
			}

			switch(_state)
			{
				case InstallationState.NeedsInstallation:
					try
					{
						InstallationProvider.Instance().Install(VersionInfo.FrameworkVersion);
					}
					catch(SqlScriptExecutionException ex)
					{
						if(IsPermissionDeniedException(ex))
						{
							installationStateMessage.Text = "<p class=\"error\">The database user specified in web.config does not have enough "
								+ "permission to perform the installation.  Please give the user database owner (dbo) rights and try again. " 
								+ "You may remove them later.</p>";
							return;
						}
						
						installationStateMessage.Text = "<p>Uh oh. Something went wrong with the installation.</p><p>" + ex.Message + "</p><p>" + ex.GetType().FullName + "</p>";
#if DEBUG
						installationStateMessage.Text += "<p>" + ex.StackTrace + "</p>";
#endif
						return;
					}
					break;
				case InstallationState.NeedsUpgrade:
					if(!InstallationProvider.Instance().Upgrade(VersionInfo.FrameworkVersion))
					{
						installationStateMessage.Text = "Uh oh. Something went wrong with the upgrade.";
						return;
					}
					break;
				default:
					installationStateMessage.Text = "Hmmm, your installation is in an unknown state. ";
					break;
			}

			Response.Redirect(NextStepUrl);
		}

		private bool IsPermissionDeniedException(SqlScriptExecutionException exception)
		{
			SqlException sqlexc = exception.InnerException as SqlException;
			return sqlexc != null 
				&& 
				(
				sqlexc.Number == (int)SqlErrorMessages.PermissionDeniedInDatabase
				||	sqlexc.Number == (int)SqlErrorMessages.PermissionDeniedInOnColumn
				||	sqlexc.Number == (int)SqlErrorMessages.PermissionDeniedInOnObject
				);
		}
	}
}
