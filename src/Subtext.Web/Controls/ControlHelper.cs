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
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Static class containing helper methods for various controls 
    /// that can't be placed within the control hierarchy.
    /// </summary>
    public static class ControlHelper
    {
        public static void AddCssClass(HtmlControl control, string cssClass)
        {
            string classes = control.Attributes["class"] ?? string.Empty;

            var regex = new Regex(@"(^|\s)" + Regex.Escape(cssClass) + @"($|\s)", RegexOptions.IgnoreCase);
            if (regex.IsMatch(classes))
            {
                return;
            }

            if (classes.Length > 0)
            {
                control.Attributes["class"] += " " + cssClass;
            }
            else
            {
                control.Attributes["class"] = cssClass;
            }
        }

        public static void AddCssClass(WebControl control, string cssClass)
        {
            string classes = control.CssClass ?? string.Empty;

            var regex = new Regex(@"(^|\s)" + Regex.Escape(cssClass) + @"($|\s)", RegexOptions.IgnoreCase);
            if (regex.IsMatch(classes))
            {
                return;
            }

            if (control.CssClass != null)
            {
                if (control.CssClass.Length > 0)
                {
                    control.CssClass += " " + cssClass;
                }
                else
                {
                    control.CssClass = cssClass;
                }
            }
        }

        /// <summary>
        /// Returns true if the specified attribute is defined 
        /// on the control.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <param name="name">Name.</param>
        /// <returns></returns>
        public static bool IsAttributeDefined(HtmlControl control, string name)
        {
            return !string.IsNullOrEmpty(control.Attributes[name]);
        }

        /// <summary>
        /// Returns true if the specified attribute is defined 
        /// on the control.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <param name="name">Name.</param>
        /// <returns></returns>
        public static bool IsAttributeDefined(WebControl control, string name)
        {
            return !string.IsNullOrEmpty(control.Attributes[name]);
        }

        /// <summary>
        /// Applies the specified control action recursively.
        /// </summary>
        /// <param name="controlAction">The control action.</param>
        /// <param name="root">The root control.</param>
        public static void ApplyRecursively(Action<Control> controlAction, Control root)
        {
            foreach (Control control in root.Controls)
            {
                controlAction(control);
                ApplyRecursively(controlAction, control);
            }
        }

        /// <summary>
        /// Recursively searches for the server form.
        /// </summary>
        /// <param name="parent">The parent to start the recursive search from.</param>
        /// <param name="id">Id of the control to find.</param>
        /// <returns></returns>
        public static Control FindControlRecursively(Control parent, string id)
        {
            foreach (Control child in parent.Controls)
            {
                if (child.ID == id)
                {
                    return child;
                }
                Control foundControl = FindControlRecursively(child, id);
                if (foundControl != null)
                {
                    return foundControl;
                }
            }

            return null;
        }

        /// <summary>
        /// Recursively searches for the server form's client id.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static string GetPageFormClientId(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is HtmlForm)
                {
                    return child.ClientID;
                }
                string id = GetPageFormClientId(child);
                if (id != null)
                {
                    return id;
                }
            }

            return null;
        }

        /// <summary>
        /// Exports the specified control to excel.
        /// </summary>
        /// <remarks>
        /// Calling this function will prompt the user with a dialog 
        /// to save the trade blotter grid as an Excel file named 
        /// TradeBlotter.xls.
        /// </remarks>
        public static void ExportToExcel(Control control, string fileName)
        {
            // Set the content type to Excel
            HttpContext.Current.Response.AddHeader("Content-Disposition", "filename=" + fileName);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            control.Page.EnableViewState = false;

            //Remove the charset from the Content-Type header
            HttpContext.Current.Response.Charset = String.Empty;

            var writer = new StringWriter();
            var htmlWriter = new HtmlTextWriter(writer);

            //Get the HTML for the control
            control.RenderControl(htmlWriter);

            HttpContext.Current.Response.Write(writer.ToString());
            HttpContext.Current.Response.End();
        }

        public static void SetTitleIfNone(LinkButton link, string title)
        {
            SetTitleIfNoneInternal(link, title);
        }

        public static void SetTitleIfNone(HyperLink link, string title)
        {
            SetTitleIfNoneInternal(link, title);
        }

        /// <summary>
        /// Sets the title attribute for the hyperlink if none exists.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="title">The title.</param>
        private static void SetTitleIfNoneInternal(WebControl link, string title)
        {
            if (link == null)
            {
                throw new ArgumentNullException("link");
            }

            // Use the ToolTip propety since that is rendered as the title attribute.
            if (link.ToolTip.Length == 0)
            {
                link.ToolTip = title;
            }
        }
    }
}