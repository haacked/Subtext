using System;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Configuration section handler for the <see cref="InstallationProvider"/>.  
	/// Most of the implementation is in the base <see cref="ProviderSectionHandler"/> 
	/// class.  This merely follows the factory pattern in providing a method 
	/// to create a proper <see cref="ProviderConfiguration"/> instance.
	/// </summary>
	public class InstallationProviderSectionHandler : ProviderSectionHandler
	{
		/// <summary>
		/// Creates an <see cref="InstallationProviderConfiguration"/> instance which 
		/// is populated by this section handler.
		/// </summary>
		public override ProviderConfiguration CreateProviderConfigurationInstance()
		{
			return new InstallationProviderConfiguration();
		}
	}
}
