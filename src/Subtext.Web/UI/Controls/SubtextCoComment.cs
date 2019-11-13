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
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Implements CoComment for Subtext.
    /// </summary>
    public class SubtextCoComment : CoComment
    {
        public BlogUrlHelper Url
        {
            get
            {
                var page = Page as SubtextPage;
                return page.Url;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/>
        /// event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            BlogTitle = Config.CurrentBlog.Title;
            BlogTool = "Subtext";
            BlogUrl = Url.BlogUrl().ToFullyQualifiedUrl(Config.CurrentBlog).ToString();

            CommentAuthorFieldName = GetControlUniqueId("tbName");
            CommentButtonId = GetControlUniqueId("btnSubmit");
            if (string.IsNullOrEmpty(CommentButtonId))
            {
                CommentButtonId = GetControlUniqueId("btnCompliantSubmit");
            }
            CommentTextFieldName = GetControlUniqueId("tbComment");
            CommentFormId = ControlHelper.GetPageFormClientId(Page);
        }

        private string GetControlUniqueId(string controlId)
        {
            Control control = ControlHelper.FindControlRecursively(Page, controlId);
            if (control != null)
            {
                return control.UniqueID;
            }

            return string.Empty;
        }
    }
}