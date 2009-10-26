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
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Summary description for RichTextEditorCtl.
    /// </summary>
    [ValidationProperty("Text")]
    public class RichTextEditor : WebControl, INamingContainer
    {
        private Control editor;
        private BlogEntryEditorProvider provider;

        public string Text
        {
            get { return provider.Text; }
            set { provider.Text = value; }
        }

        public override Unit Height { get; set; }

        public override Unit Width { get; set; }


        public string Xhtml
        {
            get { return provider.Xhtml; }
        }

        public event EventHandler<RichTextEditorErrorEventArgs> Error;

        public void OnError(Exception ex)
        {
            EventHandler<RichTextEditorErrorEventArgs> error = Error;
            if(error != null)
            {
                error(this, new RichTextEditorErrorEventArgs(ex));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                provider = BlogEntryEditorProvider.Instance();
                provider.ControlId = ID;
                provider.InitializeControl((Page as SubtextPage).SubtextContext);

                if(Height != Unit.Empty)
                {
                    provider.Height = Height;
                }
                if(Width != Unit.Empty)
                {
                    provider.Width = Width;
                }

                editor = provider.RichTextEditorControl;
                Controls.Add(editor);
                base.OnInit(e);
            }
            catch(ArgumentNullException ex)
            {
                OnError(ex);
            }
            catch(InvalidOperationException ex)
            {
                OnError(ex);
            }
            catch(UnauthorizedAccessException ex)
            {
                OnError(ex);
            }
        }
    }

    public class RichTextEditorErrorEventArgs : EventArgs
    {
        public RichTextEditorErrorEventArgs(Exception ex)
        {
            Exception = ex;
        }

        public Exception Exception { get; private set; }
    }
}