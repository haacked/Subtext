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

using System.Collections.Generic;
using System.Configuration;
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
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Document,
                Indent = true,
                IndentChars = ("\t")
            };

            using(XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                if(xmlWriter != null)
                {
                    xmlWriter.WriteStartDocument();

                    //OPML ROOT
                    xmlWriter.WriteStartElement("opml");
                    xmlWriter.WriteAttributeString("version", "1.0");
                    xmlWriter.WriteStartElement("head");
                    xmlWriter.WriteStartElement("title");
                    xmlWriter.WriteString(ConfigurationManager.AppSettings["AggregateTitle"]);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    //Body
                    xmlWriter.WriteStartElement("body");

                    xmlWriter.WriteStartElement("outline");
                    xmlWriter.WriteAttributeString("text", ConfigurationManager.AppSettings["AggregateTitle"] + " Feeds");
                    foreach(Blog blog in blogs)
                    {
                        xmlWriter.WriteStartElement("outline");

                        string title = blog.Title;
                        string xmlUrl = urlHelper.RssUrl(blog).ToString();

                        xmlWriter.WriteAttributeString("type", "rss");
                        xmlWriter.WriteAttributeString("text", title);
                        xmlWriter.WriteAttributeString("xmlUrl", xmlUrl);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement(); //outline
                    xmlWriter.WriteEndElement(); //body
                    xmlWriter.WriteEndElement(); //opml
                    xmlWriter.Flush();
                }
            }
        }
    }
}