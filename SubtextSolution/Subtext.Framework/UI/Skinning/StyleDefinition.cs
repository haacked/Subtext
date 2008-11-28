using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.UI.Skinning
{
    public class StyleDefinition : IEquatable<StyleDefinition>
    {
        public StyleDefinition() {
            Media = string.Empty;
        }

        public string Href
        {
            get;
            set;
        }

        public string Media
        {
            get;
            set;
        }

        public StyleDefinition(string href)
        {
            Href = href;
        }

        public StyleDefinition(string href, string media) : this(href)
        {
            Media = media;
        }

        public override string ToString()
        {
            return "Href: " + Href + ", Media: " + Media;
        }

        public bool Equals(StyleDefinition styleDefinition)
        {
            if (styleDefinition == null) return false;
            return Equals(Href, styleDefinition.Href) && Equals(Media, styleDefinition.Media);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StyleDefinition);
        }

        public override int GetHashCode()
        {
            return Href.GetHashCode() + 29 * Media.GetHashCode();
        }
    }
}
