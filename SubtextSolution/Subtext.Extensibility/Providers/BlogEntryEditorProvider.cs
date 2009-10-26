#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Extensibility.Providers
{
    /// <summary>
    /// Provider for classes that implement the rich text editor 
    /// to edit text visually.
    /// </summary>
    public abstract class BlogEntryEditorProvider : ProviderBase
    {
        private static readonly GenericProviderCollection<BlogEntryEditorProvider> providers =
            ProviderConfigurationHelper.LoadProviderCollection("BlogEntryEditor", out _provider);

        private static BlogEntryEditorProvider _provider;

        protected BlogEntryEditorProvider()
        {
            Height = Unit.Empty;
            Width = Unit.Empty;
        }

        /// <summary>
        /// Returns all the configured Email Providers.
        /// </summary>
        public static GenericProviderCollection<BlogEntryEditorProvider> Providers
        {
            get { return providers; }
        }


        /// <summary>
        /// Id of the control
        /// </summary>
        public virtual string ControlId { get; set; }

        /// <summary>
        /// Width of the editor
        /// </summary>
        public Unit Width { get; set; }

        /// <summary>
        /// Height of the editor
        /// </summary>
        public Unit Height { get; set; }

        /// <summary>
        /// The content of the area
        /// </summary>
        public abstract String Text { get; set; }

        /// <summary>
        /// The content of the area, but XHTML converted
        /// </summary>
        public abstract String Xhtml { get; }

        /// <summary>
        /// Return the RichTextEditorControl to be displayed inside the page
        /// </summary>
        public abstract Control RichTextEditorControl { get; }

        /// <summary>
        /// Returns the default instance of this provider.
        /// </summary>
        /// <returns></returns>
        public static BlogEntryEditorProvider Instance()
        {
            return _provider;
        }

        public override void Initialize(string name, NameValueCollection configValue)
        {
            if(configValue["Width"] != null)
            {
                Width = ParseUnit(configValue["Width"]);
            }

            if(configValue["Height"] != null)
            {
                Height = ParseUnit(configValue["Height"]);
            }

            base.Initialize(name, configValue);
        }

        protected Unit ParseUnit(string s)
        {
            try
            {
                return Unit.Parse(s);
            }
            catch(Exception)
            {
            }
            return Unit.Empty;
        }

        /// <summary>
        /// Initializes the Control to be displayed
        /// </summary>
        public abstract void InitializeControl(object subtextContext);
    }
}