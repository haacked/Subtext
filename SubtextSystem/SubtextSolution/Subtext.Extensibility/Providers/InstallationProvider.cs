using System;
using System.Collections.Specialized;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement installation procedures. 
	/// This allows new data providers to implement their own installation 
	/// code.
	/// </summary>
	public abstract class InstallationProvider : ProviderBase
	{
		/// <summary>
		/// Returns the configured concrete instance of a <see cref="InstallationProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static InstallationProvider Instance()
		{
			return (InstallationProvider)ProviderBase.Instance("Installation");
		}

		/// <summary>
		/// <p>
		/// This method is called by the installation engine in order to ask the 
		/// provider what pieces of information it needs from the user in order 
		/// to proceed with the installation.
		/// </p>
		/// <p>
		/// The <see cref="NameValueCollection"/> returned by this method should 
		/// contain entries with the Name being the piece of information being 
		/// requested and the Value being a description of the requested information.
		/// </p>
		/// <p>
		/// Upon gathering this information, the <see cref="ProvideInstallationInformation"/> method 
		/// will be called passing in a <see cref="NameValueCollection"/> with the 
		/// values containing the user's input for each name.
		/// </p>
		/// </summary>
		/// <returns></returns>
		public abstract NameValueCollection QueryInstallationInformation();

		/// <summary>
		/// Provides the installation information to this installation provider. 
		/// See <see cref="QueryInstallationInformation"/> for more information.
		/// </summary>
		/// <param name="information">Information.</param>
		public abstract void ProvideInstallationInformation(NameValueCollection information);

		/// <summary>
		/// Gets the installation status.
		/// </summary>
		/// <returns></returns>
		public abstract InstallationState GetInstallationStatus();

		/// <summary>
		/// Upgrades this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public abstract bool Upgrade();

		/// <summary>
		/// Installs this instance.  Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public abstract bool Install();

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
