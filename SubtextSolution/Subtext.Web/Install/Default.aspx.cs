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
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using Subkismet.Captcha;
using Subtext.Data;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Installation;
using Subtext.Scripting.Exceptions;
using Subtext.Web.Controls;

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
		///<summary>
		///Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
		///</summary>
		///<param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
		protected override void OnLoad(EventArgs e)
		{
			if (this.txtHostAdminEmail == null)
				this.txtHostAdminEmail = ControlHelper.FindControlRecursively(this, "txtHostAdminEmail") as TextBox;

			if (this.txtHostAdminUserName == null)
				this.txtHostAdminUserName = ControlHelper.FindControlRecursively(this, "txtHostAdminUserName") as TextBox;

			if (this.txtHostAdminPassword == null)
				this.txtHostAdminPassword = ControlHelper.FindControlRecursively(this, "txtHostAdminPassword") as TextBox;

			if (this.txtAdminEmail == null)
				this.txtAdminEmail = ControlHelper.FindControlRecursively(this, "txtAdminEmail") as TextBox;

			if (this.txtAdminUserName == null)
				this.txtAdminUserName = ControlHelper.FindControlRecursively(this, "txtAdminUserName") as TextBox;

			if (this.txtAdminPassword == null)
				this.txtAdminPassword = ControlHelper.FindControlRecursively(this, "txtAdminPassword") as TextBox;

			switch (InstallationManager.CurrentInstallationState)
			{
				case InstallationState.NeedsInstallation:
					break;

				case InstallationState.NeedsUpgrade:
					Response.Redirect("~/HostAdmin/Upgrade/", true);
					return;
					
				case InstallationState.Complete:
					installationWizard.ActiveStepIndex = installationWizard.WizardSteps.Count - 1;
					break;
			}
			
			this.DataBind();
			base.OnLoad(e);
		}

		/// <summary>
		/// Gets a value indicating whether this is a single blog setup or not.
		/// </summary>
		/// <value><c>true</c> if [single blog setup]; otherwise, <c>false</c>.</value>
		public bool SingleBlogSetup
		{
			get
			{
				return this.chkSingle.Checked;
			}
		}
		
		/// <summary>
		/// Gets the name of the admin user.
		/// </summary>
		/// <value>The name of the admin user.</value>
		protected string AdminUserName
		{
			get
			{
				return this.txtHostAdminUserName.Text.Length == 0 ? this.txtAdminUserName.Text : this.txtHostAdminUserName.Text;
			}
		}

		/// <summary>
		/// Gets the name of the admin user.
		/// </summary>
		/// <value>The name of the admin user.</value>
		protected string AdminEmail
		{
			get
			{
				return this.txtHostAdminEmail.Text.Length == 0 ? this.txtAdminEmail.Text : this.txtHostAdminEmail.Text;
			}
		}

		/// <summary>
		/// Gets the name of the admin user.
		/// </summary>
		/// <value>The name of the admin user.</value>
		protected string AdminPassword
		{
			get
			{
				if (String.IsNullOrEmpty(ViewState["AdminPassword"] as string))
					return string.Empty;

				return CaptchaBase.DecryptString((string)ViewState["AdminPassword"]);
			}
		}

		protected void OnFinishButtonClick(object sender, WizardNavigationEventArgs args)
		{
			StartInstall(args);
		}

		/// <summary>
		/// Called when [next button click].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.Web.UI.WebControls.WizardNavigationEventArgs"/> instance containing the event data.</param>
		protected void OnNextButtonClick(object sender, WizardNavigationEventArgs args)
		{
			switch (args.CurrentStepIndex)
			{
				case 0:
					HandleInstallationTypeStep();
					break;

				case 1:
					HandleAdministrationAccountStep(args, "HostAdministration");
					break;

				case 2:
					HandleAdministrationAccountStep(args, "BlogAdministration");
					break;
			}
		}

		void HandleInstallationTypeStep()
		{
			if (this.chkSingle.Checked)
				this.installationWizard.ActiveStepIndex = 2;
		}

		void HandleAdministrationAccountStep(WizardNavigationEventArgs args, string validationGroup)
		{
			Page.Validate(validationGroup);
			if (Page.IsValid)
			{
				StorePasswordInViewState();
				this.installationWizard.ActiveStepIndex = 3;
			}
			else
				args.Cancel = true;
		}
		
		void StorePasswordInViewState()
		{
			string adminPassword = String.IsNullOrEmpty(this.txtHostAdminPassword.Text) ? this.txtAdminPassword.Text : this.txtHostAdminPassword.Text;
			ViewState["AdminPassword"] = CaptchaBase.EncryptString(adminPassword);
		}
		
		void StartInstall(WizardNavigationEventArgs args)
		{
			try
			{
				Installer.Install(VersionInfo.FrameworkVersion);
				MembershipUser user = Membership.CreateUser(AdminUserName, AdminPassword, AdminEmail);
				HostInfo.CreateHost(user);
				
				if(SingleBlogSetup && Config.CreateBlog("TEMPORARY BLOG NAME", Request.Url.Host, string.Empty, HostInfo.Instance.Owner) == null)
				{
					installationStateMessage.Text = "I'm sorry, but we had a problem creating your blog. "
					+ "Please <a href=\"http://sourceforge.net/tracker/?group_id=137896&atid=739979\" title=\"Subtext Bug Report\">report "
					+ "this issue</a> to the Subtext team.";
					return;
				}

				InstallationManager.ResetInstallationStatusCache();
			}
			catch(SqlScriptExecutionException ex)
			{
				args.Cancel = true;
				
				this.installationStateMessage.Visible = true;

				if (IsPermissionDeniedException(ex))
				{
					installationStateMessage.Text = "<p class=\"error\">The database user specified in web.config does not have enough "
						+ "permission to perform the installation.  Please give the user database owner (dbo) rights and try again. "
						+ "You may remove them later.</p>";
					return;
				}

				installationStateMessage.Text = "<p>I&#8217;m sorry, but we had a problem creating your blog. "
					+ "Please <a href=\"http://sourceforge.net/tracker/?group_id=137896&atid=739979\" title=\"Subtext Bug Report\">report "
					+ "this issue</a> to the Subtext team.</p>" 
					+ "<hr /><p>Message: " + ex.Message + "</p>" 
					+ "<p>" + ex.GetType().FullName + "</p>" 
					+ "<p>" + ex.StackTrace + "</p>";
				return;
			}
		}

		private static bool IsPermissionDeniedException(SqlScriptExecutionException exception)
		{
			SqlException sqlexc = exception.InnerException as SqlException;
			return sqlexc != null
				&&
				(
				sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedInDatabase
				|| sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedInOnColumn
				|| sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedInOnObject
				);
		}
	}
}
