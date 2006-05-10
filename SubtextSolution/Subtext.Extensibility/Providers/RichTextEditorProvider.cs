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

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement the rich text editor 
	/// to edit text visually.
	/// </summary>
	public abstract class RichTextEditorProvider : ProviderBase
	{
		const string SECTION_NAME = "RichTextEditor";
		string _name;

		/// <summary>
		/// Instantiates and returns the configured concrete 
		/// default instance of a <see cref="RichTextEditorProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static RichTextEditorProvider Instance()
		{
			return (RichTextEditorProvider)ProviderBase.Instance(SECTION_NAME);
		}


		/// <summary>
		/// Instantiates and returns the <see cref="ImportProvider"/> specified 
		/// by the <see cref="RichTextEditorProvider"/> instance.
		/// </summary>
		/// <param name="providerInfo">Name of the provider.</param>
		/// <returns></returns>
		public static RichTextEditorProvider Instance(ProviderInfo providerInfo)
		{
			return (RichTextEditorProvider)ProviderBase.Instance(SECTION_NAME, providerInfo);
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
		/// instances for each <see cref="RichTextEditorProvider"/>.  Note that these are not the 
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
		/// Return the RichTextEditorControl to be displayed inside the page
		/// </summary>
		public abstract Control RichTextEditorControl{get;}
		public abstract String ControlID{get;set;}
		public abstract String Text{get;set;}
		public abstract String Xhtml{get;}
		public abstract System.Web.UI.WebControls.Unit Width{get;set;}
		public abstract System.Web.UI.WebControls.Unit Height{get;set;}

		public abstract void InitializeControl();

	}
}
