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
using System.Xml.Serialization;

namespace Subtext.Framework.UI.Skinning
{
    /// <summary>
    /// Declaration of a <code>style</code> element in the skin. Supports the capability 
    /// to specify css stylesheet includes in the <code>head</code> element of the page.
    /// </summary>
    [Serializable]
    public class Style
    {
        /// <summary>
        /// Location of the script. Specified as relative to the skin directory
        /// </summary>
        [XmlAttribute("href")]
        public string Href { get; set; }

        /// <summary>
        /// Title of the styesheet.
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Media for the stylesheet.  Can be a comma delimited list.
        /// </summary>
        /// <remarks>
        /// Allowed media... aural, braille, emboss, handheld, print, projection 
        /// screen, tty, tv
        /// </remarks>
        [XmlAttribute("media")]
        public string Media { get; set; }

        /// <summary>
        /// Adds a conditional comment around this stylesheet declaration. 
        /// Note that conditional comments only work in IE on Windows.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property should only set the conditional statement. For example, 
        /// a proper value would be "if IE" and not "[if IE]".
        /// </para>
        /// <para>
        /// For more information, check out http://www.quirksmode.org/css/condcom.html
        /// </para>
        /// </remarks>
        [XmlAttribute("conditional")]
        public string Conditional { get; set; }
    }
}