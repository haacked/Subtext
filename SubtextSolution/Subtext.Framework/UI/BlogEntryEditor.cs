using System;
using System.Web.UI;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework.UI
{
	public static class BlogEntryEditor
	{
		private static BlogEntryEditorProvider provider;
		private static GenericProviderCollection<BlogEntryEditorProvider> providers = ProviderConfigurationHelper.LoadProviderCollection("BlogEntryEditor", out provider);

		/// <summary>
		/// Returns the default instance of this provider.
		/// </summary>
		/// <returns></returns>
		public static BlogEntryEditorProvider Provider
		{
			get { return provider; }
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
		/// Gets the rich text editor control.
		/// </summary>
		/// <value>The rich text editor control.</value>
		public static Control RichTextEditorControl
		{
			get
			{
				return Provider.RichTextEditorControl;
			}
		}

		/// <summary>
		/// The content of the area
		/// </summary>
		public static String Text
		{
			get
			{
				return Provider.Text;
			}
			set
			{
				Provider.Text = value;
			}
		}

		/// <summary>
		/// The content of the area, but XHTML converted
		/// </summary>
		public static String Xhtml
		{
			get
			{
				return Provider.Xhtml;
			}
		}
	}
}
