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
using Subtext.Framework.Web;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// Renders a clickable help tooltip.  This class does not embed the 
    /// necessary scripts and CSS (against the general practice). Instead, 
    /// it relies on the user having declared helptooltip.js and helptooltip.css.
    /// </summary>
    public class HelpToolTip : HtmlContainerControl
    {
        /// <summary>
        /// Gets or sets the Help Text.  This is the 
        /// text displayed when clicking on the tooltip.
        /// </summary>
        /// <value></value>
        public string HelpText
        {
            get { return Attributes["helptext"] ?? string.Empty; }
            set { Attributes["helptext"] = value; }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl
        {
            get { return Attributes["ImageUrl"] ?? string.Empty; }
            set { Attributes["ImageUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public int ImageHeight
        {
            get
            {
                int result;
                if (!int.TryParse(Attributes["ImageHeight"], out result))
                {
                    return int.MinValue;
                }
                return result;
            }
            set { Attributes["ImageHeight"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public int ImageWidth
        {
            get
            {
                int result;
                if (!int.TryParse(Attributes["ImageWidth"], out result))
                {
                    return int.MinValue;
                }
                return result;
            }
            set { Attributes["ImageWidth"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Renders this tool tip.  The format looks like: 
        /// &lt;a class="helplink" onclick="showHelpTip(event, 'help text'); 
        /// return false;" href="?"&gt;Label Text&lt;a&gt;
        /// 
        /// //TODO: Look into embedding helplink.js and helplink.css
        /// </summary>
        /// <param name="writer">Writer.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            const string format = @"<a class=""helpLink"" onclick=""showHelpTip(event, '{0}'); return false;"" href=""?"">";
            string helpText = HelpText.Replace("'", "\\'").Replace("\r\n", "<br />").Replace("\n", "<br />");
            writer.Write(string.Format(CultureInfo.InvariantCulture, format, helpText));
            RenderChildren(writer);
            if (ImageUrl.Length > 0)
            {
                string imageUrl = HttpHelper.ExpandTildePath(ImageUrl);
                writer.Write(String.Format(CultureInfo.InvariantCulture, "<img src=\"{0}\" ", imageUrl));
                if (ImageWidth > 0)
                {
                    writer.Write(string.Format(CultureInfo.InvariantCulture, "width=\"{0}\" ", ImageWidth));
                }
                if (ImageHeight > 0)
                {
                    writer.Write(string.Format(CultureInfo.InvariantCulture, "height=\"{0}\" ", ImageHeight));
                }
                writer.Write("/>");
            }
            writer.Write("</a>");
        }
    }
}