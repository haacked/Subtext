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

namespace Subtext.Framework.UI.Skinning
{
    public class StyleDefinition : IEquatable<StyleDefinition>
    {
        public StyleDefinition()
        {
            Media = string.Empty;
        }

        public StyleDefinition(string href)
        {
            Href = href;
        }

        public StyleDefinition(string href, string media)
            : this(href)
        {
            Media = media;
        }

        public string Href { get; set; }

        public string Media { get; set; }

        public bool Equals(StyleDefinition styleDefinition)
        {
            if (styleDefinition == null)
            {
                return false;
            }
            return Equals(Href, styleDefinition.Href) && Equals(Media, styleDefinition.Media);
        }

        public override string ToString()
        {
            return string.Format("Href: {0}, Media: {1}", Href, Media);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return Equals(obj as StyleDefinition);
        }

        public override int GetHashCode()
        {
            return Href.GetHashCode() + 29 * Media.GetHashCode();
        }
    }
}