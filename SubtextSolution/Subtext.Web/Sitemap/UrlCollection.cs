using System.Collections.Generic;
using System.Xml.Serialization;

namespace Subtext.Web.SiteMap
{
    [XmlType(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", TypeName = "urlset")]
    [XmlRootAttribute(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", ElementName = "urlset", IsNullable = false)]
    public class UrlCollection : List<Url> { }
}
