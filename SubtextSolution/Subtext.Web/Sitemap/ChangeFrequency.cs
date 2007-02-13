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
