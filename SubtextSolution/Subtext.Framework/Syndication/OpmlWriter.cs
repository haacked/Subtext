#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

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
}