using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Installation
{
	/// <summary>
	/// Static class used to access the InstallerProvider.
	/// </summary>
	/// <remarks>
	///	This follows the pattern of the 'Membership' class in the MembershipProvider.
	/// </remarks>
	public static class Installer
	{
		private static InstallerProvider provider;
		private static GenericProviderCollection<InstallerProvider> providers = ProviderConfigurationHelper.LoadProviderCollection("Installation", out provider);

		/// <summary>
		/// Returns the currently configured InstallationProvider.
		/// </summary>
		/// <returns></returns>
		public static InstallerProvider Provider
		{
			get 
			{
				return provider;
			}
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



		/// <summary>
		/// Gets the current installation status.
		/// </summary>
		/// <returns></returns>
		public static InstallationState InstallationStatus
		{
			get { return Provider.InstallationStatus; }
		}

		/// <summary>
		/// Upgrades this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public static void Upgrade()
		{
			Provider.Upgrade();
		}

		/// <summary>
		/// Installs this instance.
		/// </summary>
		/// <param name="assemblyVersion">The current assembly version being installed.</param>
		public static void Install(Version assemblyVersion)
		{
			Provider.Install(assemblyVersion);
		}

		/// <summary>
		/// Determines whether the specified exception is due to 
		/// a problem with the installation.
		/// </summary>
		/// <param name="exception">exception.</param>
		/// <returns>
		/// 	<c>true</c> if this is an installation exception; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInstallationException(Exception exception)
		{
			return Provider.IsInstallationException(exception);
		}

		/// <summary>
		/// Determines whether the specified exception is due to a permission 
		/// denied error.
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		public static bool IsPermissionDeniedException(Exception exception)
		{
			return Provider.IsPermissionDeniedException(exception);
		}
	}
}
