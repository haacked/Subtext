using System;
using System.Web;
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

			bool isInRightPlace = false;
			foreach(string page in pages)
			{
				if(page != null && page.Length > 0)
				{
					if(IsOnPage(page))
					{		
						isInRightPlace = true;
						return;

					}
				}
			}
			if(!isInRightPlace)
				Response.Redirect(pages[0], true);
		}

		/// <summary>
		/// Gets the next step URL.
		/// </summary>
		/// <value></value>
		public static string NextStepUrl
		{
			get
			{
				for(int i = 0; i < _wizardPages.Length; i++)
				{
					if(IsOnPage(_wizardPages[i]) && i < _wizardPages.Length - 1)
						return _wizardPages[i+1];
				}
				return "InstallationComplete.aspx";
			}
		}

		static string[] _wizardPages =
			{
				"Step01_GatherInstallData.aspx"
				, "Step02_InstallData.aspx"
				, "Step03_ConfigureHost.aspx"
				, "Step04_CreateBlog.aspx"
			};

		static bool IsOnPage(string page)
		{
			bool caseSensitive = true;
			return StringHelper.IndexOf(HttpContext.Current.Request.Path, page, !caseSensitive) >= 0;
		}
	}
}
