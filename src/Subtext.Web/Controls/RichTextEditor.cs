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
using Subtext.Web.Admin;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Summary description for RichTextEditorCtl.
    /// </summary>
    [ValidationProperty("Text")]
    public class RichTextEditor : WebControl, INamingContainer
    {
        private Control editor;
        private BlogEntryEditorProvider _provider;

        public string Text
        {
            get { return _provider.Text; }
            set { _provider.Text = value; }
        }

        public override Unit Height { get; set; }

        public override Unit Width { get; set; }


        public string Xhtml
        {
            get { return _provider.Xhtml; }
        }

        public event EventHandler<RichTextEditorErrorEventArgs> Error;

        public void OnError(Exception ex)
        {
            EventHandler<RichTextEditorErrorEventArgs> error = Error;
            if (error != null)
            {
                error(this, new RichTextEditorErrorEventArgs(ex));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitControls(e);
        }

        /// <summary>
        /// Initialize text editor provider and controls
        /// 
        /// Note: made public to be accessible by unit tests
        /// </summary>
        /// <param name="e">EventArgs</param>
        public void InitControls(EventArgs e)
        {
            try
            {
                _provider = CreateProvider();
                _provider.ControlId = ID;
                _provider.InitializeControl((Page as SubtextPage).SubtextContext);

                if (Height != Unit.Empty)
                {
                    _provider.Height = Height;
                }
                if (Width != Unit.Empty)
                {
                    _provider.Width = Width;
                }

                editor = _provider.RichTextEditorControl;
                Controls.Add(editor);
                base.OnInit(e);
            }
            catch (ArgumentNullException ex)
            {
                OnError(ex);
            }
            catch (InvalidOperationException ex)
            {
                OnError(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// Creates a provider instance based on User preferences
        /// 
        /// If UsePlainHtmlEditor preference is set, PlainTextBlogEntryEditorProvider is created, 
        /// otherwise default provider is created
        /// </summary>
        /// <returns></returns>
        private BlogEntryEditorProvider CreateProvider()
        {
            BlogEntryEditorProvider provider;
            if (Preferences.UsePlainHtmlEditor)
            {
                provider = BlogEntryEditorProvider.Providers["PlainTextBlogEntryEditorProvider"];
            }
            else
            {
                //TODO: might be changed from default to exact provider here..
                provider = BlogEntryEditorProvider.Instance();
            }

            return provider;
        }

        /// <summary>
        /// Get editor provider
        /// 
        /// Note: made public to be accessible by unit tests
        /// </summary>
        public BlogEntryEditorProvider Provider
        {
            get
            {
                return _provider;
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