using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
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
			//TODO: Handle controller logic here.
			
			HostInfo info = null;
			if(!HostInfo.HostInfoTableExists)
				EnsureInstallStep("Step01_InstallData.aspx");
			else	
				info = HostInfo.LoadHost(true);

			if(info == null)
				EnsureInstallStep("Step02_ConfigureHost.aspx");

			if(info != null && Config.BlogCount == 0)
				EnsureInstallStep("Step03_CreateBlog.aspx");

			if(Config.InstallationComplete)
				EnsureInstallStep("InstallationComplete.aspx");
			
			base.OnLoad (e);
		}

		//Make sure we're on this page.
		void EnsureInstallStep(string page)
		{
			const bool caseSensitive = true;
			if(StringHelper.IndexOf(Request.Path, page, !caseSensitive) < 0)
			{
				Response.Redirect(page);
			}
		}
	}
}
