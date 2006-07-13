#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web.UI;
using System.Configuration.Provider;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement an import process 
	/// from another system.
	/// </summary>
    public abstract class ImportProvider : ProviderBase
	{
		private static ImportProvider provider;
		private static GenericProviderCollection<ImportProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<ImportProvider>("Import", out provider);

		/// <summary>
		/// Returns the currently configured ImportProvider.
		/// </summary>
		/// <returns></returns>
		public static ImportProvider Instance()
		{
			return provider;
		}

		/// <summary>
		/// Returns all the configured ImportProviders.
		/// </summary>
		public static GenericProviderCollection<ImportProvider> Providers
		{
			get
			{
				return providers;
			}
		}

        #region ImportProvider Methods

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
        /// into the import provider and begins the import.
        /// The control passed in should be the same as that provided in 
        /// <see cref="GatherImportInformation"/>, but with user values 
        /// supplied within it.
        /// </summary>
        /// <param name="populatedControl">Populated control.</param>
        public abstract void Import(Control populatedControl);

        /// <summary>
        /// Validates the installation information provided by the user.  
        /// Returns a string with an explanation of why it is incorrect.
        /// </summary>
        /// <param name="control">control used to provide information.</param>
        /// <returns></returns>
        public abstract string ValidateImportInformation(Control control); 
        #endregion

    }

}
