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

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for Link.
    /// </summary>
    [Serializable]
    public class Link
    {
        public Link()
        {
            PostId = NullValue.NullInt32;
        }

        public int BlogId { get; set; }

        [XmlAttribute("LinkID")]
        public int Id { get; set; }

        public int PostId { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        public bool NewWindow { get; set; }

        public string Url { get; set; }

        public string Rss { get; set; }

        public string Title { get; set; }

        public string Relation { get; set; }

        public bool HasRss
        {
            get { return (Rss != null && Rss.Trim().Length > 0); }
        }
    }
}