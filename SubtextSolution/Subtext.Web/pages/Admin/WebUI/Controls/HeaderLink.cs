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

using System.Globalization;
using System.Web.UI;

namespace Subtext.Web.Admin.WebUI
{
    /// <summary>
    /// Renders a link tag for CSS.
    /// </summary>
    public class HeaderLink : Control
    {
        private string _href;
        private string _linkType;
        private string _media = "screen";
        private string _rel;
        private string _title;

        public string Rel
        {
            get { return _rel; }
            set { _rel = value; }
        }

        public string Href
        {
            get { return Utilities.ResourcePath + _href; }
            set { _href = value; }
        }

        public string LinkType
        {
            get { return _linkType; }
            set { _linkType = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Media
        {
            get { return _media; }
            set { _media = value; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(string.Format(CultureInfo.InvariantCulture,
                                       "<link href=\"{1}\" rel=\"{0}\" type=\"{2}\" title=\"{3}\" media=\"{4}\" />",
                                       _rel, Href, _linkType, _title, _media));
        }
    }
}