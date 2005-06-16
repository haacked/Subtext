using System;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Summary description for InstallationBase.
	/// </summary>
	public class InstallationBase : System.Web.UI.Page
	{
		/// <summary>
		/// Ons the load.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnLoad(EventArgs e)
		{		
			InstallationState status = InstallationManager.GetInstallationState();

			switch(status)
			{
				case InstallationState.NeedsInstallation:
				case InstallationState.NeedsUpgrade:
				case InstallationState.NeedsRepair:
					EnsureInstallStep("Step01_GatherInstallData.aspx", "Step02_InstallData.aspx");
					break;
				
				default:
					HostInfo info = HostInfo.LoadHost(true);

					if(info == null)
						EnsureInstallStep("Step03_ConfigureHost.aspx");

					if(info != null && Config.BlogCount == 0)
						EnsureInstallStep("Step04_CreateBlog.aspx");
		
					if(info != null && Config.BlogCount > 0)
						EnsureInstallStep("InstallationComplete.aspx");
					break;
			}
			
			base.OnLoad(e);
		}

		//Make sure we're on this page.
		void EnsureInstallStep(string page)
		{
			EnsureInstallStep(page, "");
		}

		void EnsureInstallStep(params string[] pages)
		{
			if(pages.Length == 0)
				return;

			const bool caseSensitive = true;

			bool isInRightPlace = false;
			foreach(string page in pages)
			{
				if(page != null && page.Length > 0)
				{
					if(StringHelper.IndexOf(Request.Path, page, !caseSensitive) >= 0)
					{		
						isInRightPlace = true;
						return;

					}
				}
			}
			if(!isInRightPlace)
				Response.Redirect(pages[0], true);
		}
	}
}
