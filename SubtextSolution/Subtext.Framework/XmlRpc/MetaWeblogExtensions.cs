using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Framework.Components;

namespace Subtext.Framework.XmlRpc
{
    public static class MetaWeblogExtensions
    {
        public static void CopyValuesTo(this Enclosure source, Subtext.Framework.Components.Enclosure enclosure)
        {
            enclosure.Url = source.url;
            enclosure.MimeType = source.type;
            enclosure.Size = source.length;
        }

        public static void CopyValuesTo(this Post post, Entry entry)
        {
            entry.Body = post.description;
            entry.Title = post.title;
            if (post.categories != null)
            {
                entry.Categories.AddRange(post.categories);
            }
            if (!string.IsNullOrEmpty(post.wp_slug))
            {
                entry.EntryName = post.wp_slug;
            }
        }

        public static Subtext.Framework.Components.Enclosure CopyValuesToEnclosure(this Enclosure source)
        {
            Subtext.Framework.Components.Enclosure enclosure = new Subtext.Framework.Components.Enclosure();
            source.CopyValuesTo(enclosure);
            return enclosure;
        }
    }
}