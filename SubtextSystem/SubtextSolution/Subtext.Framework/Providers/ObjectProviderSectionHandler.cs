using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Configuration section handler for the <see cref="ObjectProvider"/>.  Most of 
	/// the implementation is in the base <see cref="ProviderSectionHandler"/> 
	/// class.  This merely follows the factory pattern in providing a method 
	/// to create a proper <see cref="ProviderConfiguration"/> instance.
	/// </summary>
	public class ObjectProviderSectionHandler : ProviderSectionHandler
	{
		/// <summary>
		/// Creates an <see cref="ObjectProviderConfiguration"/> instance which 
		/// is populated by this section handler.
		/// </summary>
		/// <returns></returns>
		public override ProviderConfiguration CreateProviderConfigurationInstance()
		{
			return new ObjectProviderConfiguration();
		}
	}
}
