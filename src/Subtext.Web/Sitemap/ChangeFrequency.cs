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

using System.Xml.Serialization;

namespace Subtext.Web.SiteMap
{
    [XmlType(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public enum ChangeFrequency
    {
        [XmlEnum(Name = "always")]
        Always,
        [XmlEnum(Name = "hourly")]
        Hourly,
        [XmlEnum(Name = "daily")]
        Daily,
        [XmlEnum(Name = "weekly")]
        Weekly,
        [XmlEnum(Name = "monthly")]
        Monthly,
        [XmlEnum(Name = "yearly")]
        Yearly,
        [XmlEnum(Name = "never")]
        Never
    }
}