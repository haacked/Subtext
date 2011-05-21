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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Subtext.Framework.Text;
using Subtext.Framework.Web;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Simple control used to render an anchor tag surrounded by a list item tag 
    /// for purposes of creating a menu.  It will not render an HREF when the HREF 
    /// points to the same page this page references.
    /// </summary>
    public class MenuItem : HtmlContainerControl
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value></value>
        public string Text
        {
            get { return InnerText; }
            set { InnerText = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value></value>
        public string Title
        {
            get
            {
                if (IsAttributeDefined("title"))
                {
                    return Attributes["title"];
                }
                return string.Empty;
            }
            set { Attributes["title"] = value; }
        }

        /// <summary>
        /// Gets or sets the CSS used to highlight this item.
        /// </summary>
        /// <value></value>
        public string HighlightCssClass
        {
            get
            {
                if (IsAttributeDefined("highlightclass"))
                {
                    return Attributes["highlightclass"];
                }
                return "current";
            }
            set { Attributes["highlightclass"] = value; }
        }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value></value>
        public string Href
        {
            get
            {
                if (IsAttributeDefined("href"))
                {
                    return ConvertToAppPath(Attributes["href"]);
                }
                return string.Empty;
            }
            set { Attributes["href"] = value; }
        }

        //Without the Default.aspx
        string CurrentRequestPath
        {
            get { return Context.Request.Path.LeftBefore("Default.aspx", StringComparison.OrdinalIgnoreCase); }
        }

        // This is the url that is the parent of this menu item.
        // For example, if this is a link in the about section 
        // /about/somefolder/mypage.aspx
        // Then the parent Path would be /about/
        public string ParentPath
        {
            get
            {
                if (IsAttributeDefined("parentpath"))
                {
                    return ConvertToAppPath(Attributes["parentpath"]);
                }
                return ConvertToAppPath("~/");
            }
            set { Attributes["parentpath"] = value; }
        }

        /// <summary>
        /// Returns true if the current request corresponds to this menu 
        /// item's page.  This tells us whether or not this control should 
        /// be rendered as a link.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [is on this menus page]; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnThisMenusPage
        {
            get
            {
                // If control points to /about/ and we're on /about/default.aspx 
                // then this is true.
                if (CurrentRequestPath == ParentPath && ParentPath == Href)
                {
                    return true;
                }

                // The reference must be longer than Parent Path This ensures that 
                // if we're pointing to /about/ and our parent is /about/, but are 
                // in the page /about/folder/default.aspx, we don't highlight this 
                // item.
                // We would highlight it if our parent was /
                bool referenceLongEnough = Href.Length > ParentPath.Length;

                // Make sure the reference is part of the request path now.
                bool referenceInRequestPath = CurrentRequestPath.IndexOf(Href, StringComparison.OrdinalIgnoreCase) > -1;

                return referenceLongEnough && referenceInRequestPath;
            }
        }

        /// <summary>
        /// Renders this menu item.
        /// </summary>
        /// <param name="writer">Writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            const string format = "<li{0}{1}><a{2} title=\"{3}\">{4}</a></li>";

            string cssClass = string.Empty;
            string hrefString = string.Empty;
            if (IsOnThisMenusPage && HighlightCssClass.Length > 0)
            {
                cssClass = string.Format(CultureInfo.InvariantCulture, " class=\"{0}\"", HighlightCssClass);
            }
            else
            {
                hrefString = string.Format(" href=\"{0}\"", Href);
            }

            string idText = string.Empty;
            if (IsAttributeDefined("id"))
            {
                idText = string.Format(" id=\"{0}\"", Attributes["id"]);
            }

            writer.Write(string.Format(CultureInfo.InvariantCulture, format, cssClass, idText, hrefString, Title, Text));
        }

        bool IsAttributeDefined(string name)
        {
            return ControlHelper.IsAttributeDefined(this, name);
        }

        static string ConvertToAppPath(string path)
        {
            return HttpHelper.ExpandTildePath(path);
        }
    }
}