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
using System.ComponentModel;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework.Text;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Inherits from HyperLink. Allows one to set a CSS 
    /// class that applies when the current request matches 
    /// the NavigateUrl of this link.
    /// </summary>
    public class NavigationLink : HyperLink
    {
        /// <summary>
        /// Gets or sets the css for when the current page 
        /// matches the Navigate URL for this link.
        /// </summary>
        [DefaultValue("")]
        [Browsable(true)]
        [Category("Display")]
        public string ActiveCssClass
        {
            get { return ViewState["ActiveCssClass"] as string ?? string.Empty; }
            set { ViewState["ActiveCssClass"] = value; }
        }

        /// <summary>
        /// Attaches the ActievCss class if the current url matches 
        /// the navigate url for this link.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (ActiveCssClass.Length > 0 && IsRequestForSamePage())
            {
                if (!String.IsNullOrEmpty(CssClass))
                {
                    CssClass += " ";
                }
                CssClass += ActiveCssClass;
            }
            base.OnPreRender(e);
        }

        public bool IsRequestForSamePage(string navigatePath, string requestAbsolutePath)
        {
            if (IsDirectory(navigatePath) && !IsDirectory(requestAbsolutePath))
            {
                navigatePath += "Default.aspx";
            }

            if (IsDefaultPage(navigatePath) && IsDirectory(requestAbsolutePath))
            {
                navigatePath = navigatePath.LeftBefore("Default.aspx", StringComparison.OrdinalIgnoreCase);
            }

            return String.Equals(requestAbsolutePath, navigatePath, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsRequestForSamePage()
        {
            string navigatePath = VirtualPathUtility.ToAbsolute(NavigateUrl);
            return IsRequestForSamePage(navigatePath, Page.Request.Url.AbsolutePath);
        }

        private static bool IsDefaultPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            return url.EndsWith("Default.aspx", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsDirectory(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            return url.EndsWith("/", StringComparison.OrdinalIgnoreCase);
        }
    }
}