using System;
using System.Web.UI;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework
{
	/// <summary>
	/// Summary description for ImportManager.
	/// </summary>
	public sealed class ImportManager
	{
		private ImportManager()
		{}
	
		/// <summary>
		/// Gets the import information control for the specified import provider.
		/// </summary>
		/// <param name="providerInfo">Provider info.</param>
		/// <returns></returns>
		public static Control GetImportInformationControl(ProviderInfo providerInfo)
		{
			return ImportProvider.Instance(providerInfo).GatherImportInformation();
		}

		/// <summary>
		/// Validates the import information provided by the user.  
		/// Returns a string with error information.  The string is 
		/// empty if there are no errors.
		/// </summary>
		/// <param name="populatedControl">Information.</param>
		/// <param name="providerInfo"></param>
		/// <returns></returns>
		public static string ValidateImportAnswers(Control populatedControl, ProviderInfo providerInfo)
		{
			return ImportProvider.Instance(providerInfo).ValidateImportInformation(populatedControl);
		}

		/// <summary>
		/// Begins the import using the information within the populated Control.
		/// </summary>
		/// <param name="populatedControl">Control containing the user's answers.</param>
		public static bool Import(Control populatedControl, ProviderInfo providerInfo)
		{
			return ImportProvider.Instance(providerInfo).Import(populatedControl);
		}
	}
}
