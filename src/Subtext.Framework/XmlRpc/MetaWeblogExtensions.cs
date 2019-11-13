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

using Subtext.Framework.Components;

namespace Subtext.Framework.XmlRpc
{
    public static class MetaWeblogExtensions
    {
        public static void CopyValuesTo(this Enclosure source, Components.Enclosure enclosure)
        {
            enclosure.Url = source.url;
            enclosure.MimeType = source.type;
            enclosure.Size = source.length;
        }

        public static void CopyValuesTo(this Post post, Entry entry)
        {
            entry.Body = post.description;
            entry.Title = post.title;

            if (post.excerpt != null)
            {
                entry.Description = post.excerpt;
            }

            if (post.categories != null)
            {
                entry.Categories.AddRange(post.categories);
            }
            if (!string.IsNullOrEmpty(post.wp_slug))
            {
                entry.EntryName = post.wp_slug;
            }
        }

        public static Components.Enclosure CopyValuesToEnclosure(this Enclosure source)
        {
            var enclosure = new Components.Enclosure();
            source.CopyValuesTo(enclosure);
            return enclosure;
        }
    }
}