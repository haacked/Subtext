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
using System.Reflection;
using System.Resources;
using System.Web.UI;
using System.Web.UI.WebControls;
using FreeTextBoxControls;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Web;

namespace Subtext.Web.Providers.BlogEntryEditor.FTB
{
    /// <summary>
    /// Summary description for FtbRichTextEditor.
    /// </summary>
    public class FtbBlogEntryEditorProvider : BlogEntryEditorProvider
    {
        private static readonly ResourceManager Rm =
            new ResourceManager("Subtext.Web.aspx.Providers.BlogEntryEditor.FTB.resources.ErrorMessages",
                                Assembly.GetExecutingAssembly());

        bool _formatHtmlTagsToXhtml;
        FreeTextBox _ftbCtl; //There's a good reason to do this early.
        bool _removeServerNamefromUrls;
        string _toolbarlayout = string.Empty;
        string _webFormFolder = string.Empty;

        public override Control RichTextEditorControl
        {
            get { return _ftbCtl; }
        }

        public override String Text
        {
            get { return _ftbCtl.Text; }
            set { _ftbCtl.Text = value; }
        }

        public override String Xhtml
        {
            get { return _ftbCtl.Xhtml; }
        }

        public override void Initialize(string name, NameValueCollection configValue)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name", Rm.GetString("nameNeeded"));
            }

            if (configValue == null)
            {
                throw new ArgumentNullException("configValue", Rm.GetString("configNeeded"));
            }

            if (configValue["WebFormFolder"] != null)
            {
                _webFormFolder = configValue["WebFormFolder"];
            }
            else
            {
                throw new InvalidOperationException(Rm.GetString("WebFormFolderNeeded"));
            }

            if (configValue["toolbarlayout"] != null)
            {
                _toolbarlayout = configValue["toolbarlayout"];
            }
            if (configValue["FormatHtmlTagsToXhtml"] != null)
            {
                _formatHtmlTagsToXhtml = Boolean.Parse(configValue["FormatHtmlTagsToXhtml"]);
            }
            if (configValue["RemoveServerNamefromUrls"] != null)
            {
                _removeServerNamefromUrls = Boolean.Parse(configValue["RemoveServerNamefromUrls"]);
            }

            base.Initialize(name, configValue);
        }

        public override void InitializeControl(object context)
        {
            _ftbCtl = new FreeTextBox { ID = ControlId };

            if (Width != Unit.Empty)
            {
                _ftbCtl.Width = Width;
            }

            if (Height != Unit.Empty)
            {
                _ftbCtl.Height = Height;
            }

            if (_toolbarlayout != null && _toolbarlayout.Trim().Length != 0)
            {
                _ftbCtl.ToolbarLayout = _toolbarlayout;
            }
            _ftbCtl.FormatHtmlTagsToXhtml = _formatHtmlTagsToXhtml;
            _ftbCtl.RemoveServerNameFromUrls = _removeServerNamefromUrls;

            if (!string.IsNullOrEmpty(_webFormFolder))
            {
                _ftbCtl.ImageGalleryUrl =
                    HttpHelper.ExpandTildePath(_webFormFolder + "ftb.imagegallery.aspx?rif={0}&cif={0}");
            }
            var subtextContext = context as ISubtextContext;
            string blogImageRootPath = subtextContext.UrlHelper.ImageDirectoryUrl(subtextContext.Blog);
            _ftbCtl.ImageGalleryPath = blogImageRootPath;
        }
    }
}