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
using System.Collections.Specialized;
using System.Web.UI;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement an import process 
	/// from another system.
	/// </summary>
    public abstract class ImportProvider : System.Configuration.Provider.ProviderBase
	{

        private static ImportProvider _provider = null;
        private static GenericProviderCollection<ImportProvider> _providers = null;
        private static object _lock = new object();

        public static GenericProviderCollection<ImportProvider> Providers
        {
            get
            {
                LoadProviders();
                return _providers;
            }
        }

        public static ImportProvider Instance()
        {
            LoadProviders();
            return _provider;
        }

        private static void LoadProviders()
        {
            // Avoid claiming lock if providers are already loaded
            if (_provider == null)
            {
                lock (_lock)
                {
                    // Do this again to make sure _provider is still null
                    if (_provider == null)
                    {
                        // Get a reference to the <Import> section
                        ImportProviderSectionHandler section = (ImportProviderSectionHandler)
                            WebConfigurationManager.GetSection
                            ("Import");

                        // Load registered providers and point _provider
                        // to the default provider
                        _providers = new GenericProviderCollection<ImportProvider>();
                        ProvidersHelper.InstantiateProviders
                            (section.Providers, _providers,
                            typeof(ImportProvider));
                        _provider = _providers[section.DefaultProvider];

                        if (_provider == null)
                            throw new ProviderException
                                ("Unable to load default ImportProvider");
                    }
                }
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
