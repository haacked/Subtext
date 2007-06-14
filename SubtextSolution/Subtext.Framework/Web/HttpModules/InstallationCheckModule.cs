using System;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Web;
using Subtext.Installation;

namespace Subtext.Web.HttpModules
{
	/// <summary>
	/// Maps an incoming URL to a blog.
	/// </summary>
	public class InstallationCheckModule : IHttpModule
	{
		/// <summary>
		/// Initializes a module and prepares it to handle
		/// requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, 
		/// and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			context.BeginRequest += CheckInstallationStatus;
		}

		/// <summary>
		/// Disposes of the resources (other than memory) used by the
		/// module that implements <see langword="IHttpModule."/>
		/// </summary>
		public void Dispose()
		{
			//Do nothing.
		}

		/// <summary>
		/// Checks the installation status and redirects if necessary.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		public void CheckInstallationStatus(object sender, EventArgs e)
		{
			//Bypass for static files.
			if (HttpHelper.IsStaticFileRequest())
				return;
         if (HttpHelper.IsWebResource())
            return;

			// Want to redirect to install if installation is required, 
			// or if we're missing a HostInfo record.
			if ((InstallationManager.IsInstallationActionRequired() || InstallationManager.HostInfoRecordNeeded))
			{
				InstallationState state = InstallationManager.CurrentInstallationState;
				if (state == InstallationState.NeedsInstallation && !(InstallationManager.IsInHostAdminDirectory ||InstallationManager.IsInInstallDirectory))
				{
					HttpContext.Current.Response.Redirect("~/Install/", true);
					return;
				}

				if (state == InstallationState.NeedsUpgrade || state == InstallationState.NeedsRepair)
				{
					if (!(InstallationManager.IsInUpgradeDirectory || InstallationManager.IsOnLoginPage || InstallationManager.IsInSystemMessageDirectory))
					{
						HttpContext.Current.Response.Redirect("~/SystemMessages/UpgradeInProgress.aspx", true);
						return;
					}
				}
			}
		}
	}
}
