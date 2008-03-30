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
using System.Data.SqlClient;
using System.Web.UI;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement installation procedures. 
	/// This allows new data providers to implement their own installation 
	/// code.
	/// </summary>
    public abstract class Installation : System.Configuration.Provider.ProviderBase
	{
		private static Installation provider;
		private static GenericProviderCollection<Installation> providers = ProviderConfigurationHelper.LoadProviderCollection("Installation", out provider);

		/// <summary>
		/// Returns the currently configured InstallationProvider.
		/// </summary>
		/// <returns></returns>
        public static Installation Provider
        {
			get { return provider; }
        }

		/// <summary>
		/// Returns all the configured InstallationProvider.
		/// </summary>
		public static GenericProviderCollection<Installation> Providers
		{
			get
			{
				return providers;
			}
		}

        #region InstallationProvider methods
        /// <summary>
        /// <p>
        /// This method is called by the installation engine in order to ask the 
        /// provider what pieces of information it needs from the user in order 
        /// to proceed with the installation.
        /// </p>
        /// <p>
        /// This method returns a <see cref="Control"/> used to gather 
        /// the required installation information.  This will be returned 
        /// back to the provider after the user provides the information.
        /// </p>
        /// </summary>
        /// <returns></returns>
        public abstract Control GatherInstallationInformation();

        /// <summary>
        /// Provides the installation information as provided by the user 
        /// back into the provider. 
        /// The control passed in should be the same as that provided in 
        /// <see cref="GatherInstallationInformation"/>, but with user values 
        /// supplied within it.
        /// </summary>
        /// <param name="populatedControl">Populated control.</param>
        public abstract void ProvideInstallationInformation(Control populatedControl);

        /// <summary>
        /// Validates the installation information provided by the user.  
        /// Returns a string with an explanation of why it is incorrect.
        /// </summary>
        /// <param name="control">control used to provide information.</param>
        /// <returns></returns>
        public abstract string ValidateInstallationInformation(Control control);

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
        /// Attempts to repair this instance. Returns true if it was successful.
        /// </summary>
        /// <returns></returns>
        public abstract bool Repair();

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
		/// Determines whether the specified exception is due to a permission 
		/// denied error.
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		public abstract bool IsPermissionDeniedException(Exception exception);

        /// <summary>
        /// Gets the <see cref="Version"/> of the current Subtext installation.
        /// </summary>
        /// <returns></returns>
        public abstract Version GetCurrentInstallationVersion();

        /// <summary>
        /// Updates the current installed version.
        /// </summary>
        /// <param name="newVersion">The new version that is now current.</param>
        /// <param name="transaction">The transaction to perform this upgrade within.</param>
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
