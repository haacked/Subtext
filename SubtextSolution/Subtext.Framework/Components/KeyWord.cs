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
using System.Xml.Serialization;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for KeyWord.
    /// </summary>
    [Serializable]
    public class KeyWord
    {
        private string _format;

        /// <summary>
        /// Gets or sets the word or words to look for. Could also be short hand. ie, ScottW would end up <a href = "http://scottwater.com/blog">Scott Watermasysk</a>
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// The text value of an anchor tag: <a href="#">TEXT</a>
        /// </summary>
        public string Text { get; set; }

        public bool ReplaceFirstTimeOnly { get; set; }

        public bool CaseSensitive { get; set; }

        public bool OpenInNewWindow { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// Rel entries for a keyword. For instance, you could add "Friend" to a keyword, and XFN would pick it up as a Friend link
        /// </summary>
        public string Rel { get; set; }

        public int BlogId { get; set; }

        [XmlAttribute]
        public int Id { get; set; }

        public string GetFormat
        {
            get
            {
                if(_format == null)
                {
                    ProcessFormat();
                }

                return _format;
            }
        }

        private void ProcessFormat()
        {
            _format = string.Format(CultureInfo.InvariantCulture, "<a href=\"{1}\"{0}{2}{3}>{4}</a>",
                                    Title != null ? string.Format(" title=\"{0}\"", Title) : String.Empty, Url,
                                    Rel != null ? string.Format(" rel=\"{0}\"", Rel) : string.Empty,
                                    OpenInNewWindow ? " target=\"_blank\"" : string.Empty, Text);
        }
    }
}