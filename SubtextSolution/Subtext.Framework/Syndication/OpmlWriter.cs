using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Syndication
{
    public class OpmlWriter
    {
        public virtual void Write(IEnumerable<Blog> blogs, TextWriter writer, UrlHelper urlHelper)
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Indent = true;
            settings.IndentChars = ("\t");

            using(XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                xmlWriter.WriteStartDocument();

                //OPML ROOT
                xmlWriter.WriteStartElement("opml");
                xmlWriter.WriteAttributeString("version", "1.0");

                //Body
                xmlWriter.WriteStartElement("body");

                foreach(Blog blog in blogs)
                {
                    xmlWriter.WriteStartElement("outline");

                    string title = blog.Title;
                    VirtualPath htmlPath = urlHelper.BlogUrl(blog);
                    string htmlUrl = htmlPath.ToFullyQualifiedUrl(blog).ToString();
                    string xmlUrl = urlHelper.RssUrl(blog).ToString();

                    xmlWriter.WriteAttributeString("title", title);
                    xmlWriter.WriteAttributeString("htmlUrl", htmlUrl);
                    xmlWriter.WriteAttributeString("xmlUrl", xmlUrl);

                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }
    }
}