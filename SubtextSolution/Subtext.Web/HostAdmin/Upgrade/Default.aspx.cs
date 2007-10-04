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
using Subtext.Installation;
using Subtext.Scripting.Exceptions;

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
		protected void Page_Load(object sender, EventArgs e)
		{
			if(InstallationManager.CurrentInstallationState == InstallationState.Complete)
			{
				Response.Redirect("UpgradeComplete.aspx");
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
			this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);

		}
		#endregion

		private void btnUpgrade_Click(object sender, EventArgs e)
		{
			plcHolderUpgradeMessage.Visible = false;
			try
			{
				Installer.Upgrade();
				Response.Redirect("UpgradeComplete.aspx");
			}
			catch (SqlScriptExecutionException ex)
			{
				plcHolderUpgradeMessage.Visible = true;
				
				if (Installer.IsPermissionDeniedException(ex))
				{
					messageLiteral.Text = "<p class=\"error\">The database user specified in web.config does not have enough "
						+ "permission to perform the installation.  Please give the user database owner (dbo) rights and try again. "
						+ "You may remove them later.</p>";
					
					return;
				}

				messageLiteral.Text = "<p>Uh oh. Something went wrong with the installation.</p><p>" + ex.Message + "</p><p>" + ex.GetType().FullName + "</p>";
				messageLiteral.Text += "<p>" + ex.StackTrace + "</p>";
				
				return;
			}
			Response.Redirect("UpgradeComplete.aspx");
		}
	}
}
