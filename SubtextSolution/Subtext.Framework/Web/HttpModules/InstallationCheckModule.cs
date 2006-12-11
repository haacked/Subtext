using System;
using System.Configuration;
using System.Web;
using log4net;
using Subtext.Framework;
using Subtext.Framework.Logging;
using Subtext.Framework.Threading;
using Subtext.Framework.Web;
using Subtext.Installation;

namespace Subtext.Web.HttpModules
{
	/// <summary>
	/// Checks the current installation status for a Subtext installation. 
	/// If the installation needs an upgrade, then it will redirect to the 
	/// upgrade page.
	/// </summary>
	public class InstallationCheckModule : IHttpModule
	{
		private readonly static ILog Log = new Log();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="InstallationCheckModule"/> class.
		/// </summary>
		public InstallationCheckModule()
		{
		}

		/// <summary>
		/// Initializes a module and prepares it to handle
		/// requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, 
		/// and events common to all application objects within an ASP.NET application</param>
		public void Init(System.Web.HttpApplication context)
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
			
			// Want to redirect to install if installation is required, 
			// or if we're missing a HostInfo record.
			if((InstallationManager.IsInstallationActionRequired(VersionInfo.FrameworkVersion) || InstallationManager.HostInfoRecordNeeded))
			{
				InstallationState state = InstallationManager.GetCurrentInstallationState(VersionInfo.FrameworkVersion);
				if(state == InstallationState.NeedsInstallation && !InstallationManager.IsInHostAdminDirectory && !InstallationManager.IsInInstallDirectory)
				{
					HttpContext.Current.Response.Redirect("~/Install/", true);
					return;
				}

				if(state == InstallationState.NeedsUpgrade)
				{
					if(ConfigurationManager.AppSettings["StartUpgradeImmediately"] == "true" && HttpContext.Current.Application["UpgradeInitiated"] == null)
					{
						HttpContext.Current.Application.Lock();
						try
						{
							if (HttpContext.Current.Application["UpgradeInitiated"] == null)
							{
								HttpContext.Current.Application["UpgradeInitiated"] = DateTime.Now;
								StartUpgradeAsynch();
							}
						}
						finally
						{
							HttpContext.Current.Application.UnLock();
						}
					}
					
					if(!InstallationManager.IsOnLoginPage && !InstallationManager.IsInSystemMessageDirectory)
					{
						HttpContext.Current.Response.Redirect("~/SystemMessages/UpgradeInProgress.aspx", true);
						return;
					}
				}
			}
		}

		private static void StartUpgradeAsynch()
		{
			AsynchUpgrade asynchUpgrade = delegate(HttpApplicationState application)
			{
				try
				{
					Log.Warn("Upgrading Subtext Installation");
					Installer.Upgrade();
					Log.Warn("Upgrade Complete!");
										
					Log.Info("Attempting to update Application State to flag upgrade completion.");
					application.Lock();
					application["UpgradeInitiated"] = null;
					Log.Info("Updated Application State to flag upgrade completion!");
					application.UnLock();
				}
				catch(Exception exception)
				{
					Log.Error("Exception occurred during automatic upgrade", exception);
				}
			};

			AsyncHelper.FireAndForget(asynchUpgrade, HttpContext.Current.Application);
		}

		delegate void AsynchUpgrade(HttpApplicationState application);
		
	}
}
