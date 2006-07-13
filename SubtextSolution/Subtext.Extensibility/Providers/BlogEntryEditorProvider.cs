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
	/// Provider for classes that implement the rich text editor 
	/// to edit text visually.
	/// </summary>
	public abstract class BlogEntryEditorProvider : ProviderBase
	{
		private static BlogEntryEditorProvider provider;
		private static GenericProviderCollection<BlogEntryEditorProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<BlogEntryEditorProvider>("BlogEntryEditor", out provider);

        public static BlogEntryEditorProvider Instance()
        {
            return provider;
        }

		/// <summary>
		/// Returns all the configured Email Providers.
		/// </summary>
		public static GenericProviderCollection<BlogEntryEditorProvider> Providers
		{
			get
			{
				return providers;
			}
		}
		
		/// <summary>
		/// Return the RichTextEditorControl to be displayed inside the page
		/// </summary>
		public abstract Control RichTextEditorControl{get;}
        /// <summary>
        /// Id of the control
        /// </summary>
		public abstract String ControlID{get;set;}
        /// <summary>
        /// The content of the area
        /// </summary>
		public abstract String Text{get;set;}
        /// <summary>
        /// The content of the area, but XHTML converted
        /// </summary>
		public abstract String Xhtml{get;}
        /// <summary>
        /// Width of the editor
        /// </summary>
		public abstract System.Web.UI.WebControls.Unit Width{get;set;}
        /// <summary>
        /// Height of the editor
        /// </summary>
		public abstract System.Web.UI.WebControls.Unit Height{get;set;}

        /// <summary>
        /// Initializes the Control to be displayed
        /// </summary>
		public abstract void InitializeControl();

	}
}
