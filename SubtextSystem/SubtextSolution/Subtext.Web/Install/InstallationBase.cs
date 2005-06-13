using System;
using Subtext.Framework;
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
			if(HostInfo.Instance == null)
				EnsureInstallStep("Step02_ConfigureHost.aspx");
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
