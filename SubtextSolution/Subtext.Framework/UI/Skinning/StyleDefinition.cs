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

        public StyleDefinition(string href, string media) : this(href)
        {
            Media = media;
        }

        public string Href { get; set; }

        public string Media { get; set; }

        #region IEquatable<StyleDefinition> Members

        public bool Equals(StyleDefinition styleDefinition)
        {
            if(styleDefinition == null)
            {
                return false;
            }
            return Equals(Href, styleDefinition.Href) && Equals(Media, styleDefinition.Media);
        }

        #endregion

        public override string ToString()
        {
            return "Href: " + Href + ", Media: " + Media;
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(this, obj))
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