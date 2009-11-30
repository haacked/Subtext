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
using System.Web.UI.WebControls;

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
            if(ActiveCssClass.Length > 0 && IsRequestForSamePage(NavigateUrl))
            {
                if(!String.IsNullOrEmpty(CssClass))
                {
                    CssClass += " ";
                }
                CssClass += ActiveCssClass;
            }
            base.OnPreRender(e);
        }

        private bool IsRequestForSamePage(string navigateUrl)
        {
            if(navigateUrl == "/")
            {
                navigateUrl += "Default.aspx";
            }

            return String.Equals(Page.Request.Url.AbsolutePath, navigateUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}