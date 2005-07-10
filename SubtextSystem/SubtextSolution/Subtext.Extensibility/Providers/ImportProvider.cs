using System;
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

		/// <summary>
		/// Returns the configured concrete instance of a <see cref="InstallationProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static InstallationProvider Instance()
		{
			return (InstallationProvider)ProviderBase.Instance(SECTION_NAME);
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
