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
using System.Reflection;
using System.Resources;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;

namespace Subtext.Providers.BlogEntryEditor.CKEditor
{
    public class CKEBlogEntryEditorProvider : BlogEntryEditorProvider
    {
        private static readonly ResourceManager rm =
    new ResourceManager("Subtext.Providers.BlogEntryEditor.CKEditor.resources.ErrorMessages",
                        Assembly.GetExecutingAssembly());

        TextBox _txtCtl;

        public override String Text
        {
            get { return _txtCtl.Text; }
            set { _txtCtl.Text = value; }
        }

        public override String Xhtml
        {
            get { return _txtCtl.Text; }
        }

        public override System.Web.UI.Control RichTextEditorControl
        {
            get { return _txtCtl; }
        }

        public override void InitializeControl(object subtextContext)
        {
            _txtCtl = new TextBox();
        }
    }
}
