using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for DTOPRoviderSectionHandler.
	/// </summary>
	public class DTOProviderSectionHandler : ProviderSectionHandler
	{
		/// <summary>
		/// Creates the provider configuration instance.
		/// </summary>
		/// <returns></returns>
		public override ProviderConfiguration CreateProviderConfigurationInstance()
		{
			return new DTOConfiguration();
		}
	}
}
