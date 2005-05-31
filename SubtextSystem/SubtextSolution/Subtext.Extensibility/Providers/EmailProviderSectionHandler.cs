using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Configuration section handler for the <see cref="EmailProvider"/>.  Most of 
	/// the implementation is in the base <see cref="ProviderSectionHandler"/> 
	/// class.  This merely follows the factory pattern in providing a method 
	/// to create a proper <see cref="ProviderConfiguration"/> instance.
	/// </summary>
	public class EmailProviderSectionHandler : ProviderSectionHandler
	{
		/// <summary>
		/// Creates the provider configuration instance.
		/// </summary>
		/// <returns></returns>
		public override ProviderConfiguration CreateProviderConfigurationInstance()
		{
			return new EmailProviderConfiguration();
		}
	}
}
