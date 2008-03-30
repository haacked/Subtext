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
using Subtext.Framework.Exceptions;

namespace Subtext.Web
{
	/// <summary>
	/// Displays the blog not active message.
	/// </summary>
	public partial class UpgradeInProgress : System.Web.UI.Page
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				InstallationState state = InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion);
				if(state == InstallationState.NeedsUpgrade || state == InstallationState.NeedsRepair)
				{
					plcUpgradeInProgressMessage.Visible = true;
					plcNothingToSeeHere.Visible = false;
				}
				else
				{
					plcUpgradeInProgressMessage.Visible = true;
					plcNothingToSeeHere.Visible = false;
					lnkBlog.HRef = Config.CurrentBlog.HomeVirtualUrl;
				}
			}
			catch(BlogDoesNotExistException)
			{
				plcUpgradeInProgressMessage.Visible = true;
				plcNothingToSeeHere.Visible = false;
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
