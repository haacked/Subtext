using System;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement an import process from another system.
	/// </summary>
	public abstract class ImportProvider : ProviderBase
	{
		/// <summary>
		/// Returns the configured concrete instance of a <see cref="InstallationProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static InstallationProvider Instance()
		{
			return (InstallationProvider)ProviderBase.Instance("Import");
		}

		/// <summary>
		/// Imports data into this instance.
		/// </summary>
		/// <returns></returns>
		public abstract bool Import();
	}

}
