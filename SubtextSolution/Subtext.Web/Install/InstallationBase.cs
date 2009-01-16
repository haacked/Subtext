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
using System.Web;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Summary description for InstallationBase.
	/// </summary>
	public class InstallationBase : RoutablePage
	{
		/// <summary>
		/// Ons the load.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnLoad(EventArgs e)
		{		
			InstallationState status = InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion);

			switch(status)
			{
				case InstallationState.NeedsInstallation:
				case InstallationState.NeedsUpgrade:
				case InstallationState.NeedsRepair:
					EnsureInstallStep("Default.aspx", "Step02_ConfigureHost.aspx");
					break;
				
				default:
					HostInfo info = HostInfo.LoadHost(true);

					if(info == null)
						EnsureInstallStep("Step02_ConfigureHost.aspx");

					if(info != null && Config.BlogCount == 0)
						EnsureInstallStep("Step03_CreateBlog.aspx");
		
					if(info != null && Config.BlogCount > 0)
						EnsureInstallStep("InstallationComplete.aspx");
					break;
			}
			
			base.OnLoad(e);
		}

		//Make sure we're on this page.
		void EnsureInstallStep(string page) {
			EnsureInstallStep(page, "");
		}

		void EnsureInstallStep(params string[] pages)
		{
			if(pages.Length == 0)
				return;

			foreach(string page in pages)
			{
				if(page != null && page.Length > 0)
				{
					if(IsOnPage(page))
					{		
						return;
					}
				}
			}
			
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
				"Default.aspx"
				, "Step02_ConfigureHost.aspx"
				, "Step03_CreateBlog.aspx"
			};

		static bool IsOnPage(string page)
		{
			return HttpContext.Current.Request.Path.IndexOf(page, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}
	}
}
