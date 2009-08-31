#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Exceptions;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to help make determine whether an installation is required or not.
	/// </summary>
	public class InstallationManager : IInstallationManager
	{
        public InstallationManager(Installation installationProvider) {
            InstallationProvider = installationProvider;
        }

        protected Installation InstallationProvider {
            get;
            private set;
        }

		/// <summary>
		/// Gets a value indicating whether this instance is installation action required.
		/// </summary>
		/// <param name="assemblyVersion">The version of the currently installed assembly.</param>
		/// <value>
		/// 	<c>true</c> if this instance is installation action required; otherwise, <c>false</c>.
		/// </value>
		public virtual bool InstallationActionRequired(Version assemblyVersion)
		{
			InstallationState currentState = InstallationProvider.GetInstallationStatus(assemblyVersion);
            return InstallationActionRequired(currentState);
		}

        public bool InstallationActionRequired(InstallationState currentState)
        {
            bool needsUpgrade = (currentState == InstallationState.NeedsInstallation
                         || currentState == InstallationState.NeedsUpgrade);

            return needsUpgrade;
        }

		public void ResetInstallationStatusCache()
		{
			if(HttpContext.Current != null && HttpContext.Current.Application["NeedsInstallation"] != null)
				HttpContext.Current.Application["NeedsInstallation"] = null;
		}

		/// <summary>
		/// Determines whether an installation action is required by 
		/// examining the specified unhandled Exception.
		/// </summary>
		/// <param name="unhandledException">Unhandled exception.</param>
		/// <param name="assemblyVersion">The version of the currently installed assembly.</param>
		/// <returns>
		/// 	<c>true</c> if an installation action is required; otherwise, <c>false</c>.
		/// </returns>
        public bool InstallationActionRequired(Version assemblyVersion, Exception unhandledException)
		{
            if (unhandledException is HostDataDoesNotExistException) {
                return true;
            }

			if(Installation.Provider.IsInstallationException(unhandledException))
				return true;

			InstallationState status = Installation.Provider.GetInstallationStatus(assemblyVersion);
			switch(status)
			{
				case InstallationState.NeedsInstallation:
				case InstallationState.NeedsUpgrade:
				{
					return true;		
				}
			}

			return false;
		}

        public virtual InstallationState GetInstallationStatus(Version currentAssemblyVersion)
        {
            return InstallationProvider.GetInstallationStatus(currentAssemblyVersion);
        }
    }
}