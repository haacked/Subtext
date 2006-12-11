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
using System.Configuration.Provider;
using System.Data.SqlClient;
using Subtext.Extensibility.Providers;

namespace Subtext.Installation
{
	/// <summary>
	/// Provider for classes that implement installation procedures. 
	/// This allows new data providers to implement their own installation 
	/// code.
	/// </summary>
    public abstract class InstallerProvider : ProviderBase
	{
		private static InstallerProvider provider;
		private static GenericProviderCollection<InstallerProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<InstallerProvider>("Installation", out provider);

		/// <summary>
		/// Returns the currently configured InstallationProvider.
		/// </summary>
		/// <returns></returns>
        public static InstallerProvider Instance()
        {
            return provider;
        }

		/// <summary>
		/// Returns all the configured InstallationProvider.
		/// </summary>
		public static GenericProviderCollection<InstallerProvider> Providers
		{
			get
			{
				return providers;
			}
		}

        #region InstallationProvider methods
        /// <summary>
        /// Gets the installation status.
        /// </summary>
        /// <param name="currentAssemblyVersion">The version of the assembly that represents this installation.</param>
        /// <returns></returns>
        public abstract InstallationState GetInstallationStatus(Version currentAssemblyVersion);

        /// <summary>
        /// Upgrades this instance. Returns true if it was successful.
        /// </summary>
        /// <returns></returns>
        public abstract void Upgrade();

        /// <summary>
        /// Installs this instance.
        /// </summary>
        /// <param name="assemblyVersion">The current assembly version being installed.</param>
        public abstract void Install(Version assemblyVersion);

        /// <summary>
        /// Determines whether the specified exception is due to 
        /// a problem with the installation.
        /// </summary>
        /// <param name="exception">exception.</param>
        /// <returns>
        /// 	<c>true</c> if this is an installation exception; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsInstallationException(Exception exception);

        /// <summary>
        /// Gets the <see cref="Version"/> of the current Subtext installation.
        /// </summary>
        /// <returns></returns>
        public abstract Version GetCurrentInstallationVersion();

        /// <summary>
        /// Updates the current installed version.
        /// </summary>
        /// <param name="newVersion">The new version that is now current.</param>
        /// <returns></returns>
        public abstract void UpdateInstallationVersionNumber(Version newVersion, SqlTransaction transaction); 
        #endregion
	}

	/// <summary>
	/// Returns the current state of the installation.
	/// </summary>
	public enum InstallationState
	{
		/// <summary>No information available</summary>
		None = 0,
		/// <summary>Subtext is installed, but needs to be upgraded.</summary>
		NeedsUpgrade = 1,
		/// <summary>Subtext is installed, but needs to be repaired.</summary>
		NeedsRepair = 2,
		/// <summary>Subtext needs to be installed.</summary>
		NeedsInstallation = 3,
		/// <summary>Subtext is installed and seems to be working properly.</summary>
		Complete = 4,
	}
}
