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
using System.Web.UI.WebControls;
using System.Globalization;
using Subtext.Extensibility.Properties;
using System.Collections.Specialized;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provider for classes that implement the rich text editor 
	/// to edit text visually.
	/// </summary>
	public abstract class BlogEntryEditorProvider : ProviderBase
	{
		private static BlogEntryEditorProvider provider;
		private static readonly GenericProviderCollection<BlogEntryEditorProvider> providers = ProviderConfigurationHelper.LoadProviderCollection("BlogEntryEditor", out provider);
		
		/// <summary>
		/// Returns the default instance of this provider.
		/// </summary>
		/// <returns></returns>
        public static BlogEntryEditorProvider Instance()
        {
            return provider;
        }

		/// <summary>
		/// Returns all the configured Email Providers.
		/// </summary>
		public static GenericProviderCollection<BlogEntryEditorProvider> Providers
		{
			get { return providers; }
		}

		public override void  Initialize(string name, NameValueCollection config)
		{
			if (name == null)
				throw new ArgumentNullException("name", Resources.ArgumentNull_String);

			if (config == null)
				throw new ArgumentNullException("config", Resources.ArgumentNull_Collection);

			if (config["Width"] != null)
				this.Width = ParseUnit(config["Width"]);

			if (config["Height"] != null)
				this.Height = ParseUnit(config["Height"]);

			base.Initialize(name, config);
		}

		protected static Unit ParseUnit(string s)
		{
			try
			{
				return Unit.Parse(s, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
			}
			return Unit.Empty;
		}


		/// <summary>
		/// Id of the control
		/// </summary>
		public virtual string ControlId
		{
			get
			{
				return this.controlId;
			}
			set
			{
				this.controlId = value;
			}
		}

		string controlId;

		/// <summary>
		/// Width of the editor
		/// </summary>
		public Unit Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		Unit width = Unit.Empty;

		/// <summary>
		/// Height of the editor
		/// </summary>
		public Unit Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		Unit height = Unit.Empty;

		/// <summary>
		/// The content of the area
		/// </summary>
		public abstract String Text { get;set;}

		/// <summary>
		/// The content of the area, but XHTML converted
		/// </summary>
		public abstract String Xhtml { get;}

		/// <summary>
		/// Return the RichTextEditorControl to be displayed inside the page
		/// </summary>
		public abstract Control RichTextEditorControl { get;}

		/// <summary>
		/// Initializes the Control to be displayed
		/// </summary>
		public abstract void InitializeControl();

	}
}
