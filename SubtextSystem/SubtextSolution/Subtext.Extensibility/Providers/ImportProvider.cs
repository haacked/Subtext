using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement an import process 
	/// from another system.
	/// </summary>
	public abstract class ImportProvider : ProviderBase
	{
		const string SECTION_NAME = "Import";
		string _name;

		/// <summary>
		/// Instantiates and returns the configured concrete 
		/// default instance of a <see cref="ImportProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static ImportProvider Instance()
		{
			return (ImportProvider)ProviderBase.Instance(SECTION_NAME);
		}

		/// <summary>
		/// Instantiates and returns the <see cref="ImportProvider"/> specified 
		/// by the <see cref="ProviderInfo"/> instance.
		/// </summary>
		/// <param name="providerInfo">Name of the provider.</param>
		/// <returns></returns>
		public static ImportProvider Instance(ProviderInfo providerInfo)
		{
			return (ImportProvider)ProviderBase.Instance(SECTION_NAME, providerInfo);
		}

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_name = name;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Returns a <see cref="ProviderCollection"/> containing <see cref="ProviderInfo"/> 
		/// instances for each <see cref="ImportProvider"/>.  Note that these are not the 
		/// actual providers, simply information about the installed providers.
		/// </summary>
		/// <value></value>
		public static ProviderCollection Providers
		{
			get
			{
				return ProviderBase.GetProviders(SECTION_NAME);
			}
		}

		/// <summary>
		/// Imports data into this instance.
		/// </summary>
		/// <returns></returns>
		public abstract bool Import();

		/// <summary>
		/// <p>
		/// This method is called by the import engine in order to ask the 
		/// provider what pieces of information it needs from the user in order 
		/// to proceed with the import.
		/// </p>
		/// <p>
		/// This method returns a <see cref="Control"/> used to gather 
		/// the required installation information.  This control will be returned 
		/// back to the provider after the user provides the information.
		/// </p>
		/// </summary>
		/// <returns></returns>
		public abstract Control GatherImportInformation();

		/// <summary>
		/// Provides the import information as provided by the user back 
		/// into the import provider. 
		/// The control passed in should be the same as that provided in 
		/// <see cref="GatherImportInformation"/>, but with user values 
		/// supplied within it.
		/// </summary>
		/// <param name="populatedControl">Populated control.</param>
		public abstract void ProvideImportInformation(Control populatedControl);

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a string with an explanation of why it is incorrect.
		/// </summary>
		/// <param name="control">control used to provide information.</param>
		/// <returns></returns>
		public abstract string ValidateImportInformation(Control control);
	}

}
