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
using Subtext.Extensibility.Providers;

namespace Subtext.Providers.BlogEntryEditor.CKEditor
{
    public class CKEBlogEntryEditorProvider : BlogEntryEditorProvider
    {
        public override string Text
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string Xhtml
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.UI.Control RichTextEditorControl
        {
            get { throw new NotImplementedException(); }
        }

        public override void InitializeControl(object subtextContext)
        {
            throw new NotImplementedException();
        }
    }
}
