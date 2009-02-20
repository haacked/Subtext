#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Sgml;

namespace Subtext.Framework.Services
{
    public class XhtmlConverter : ITextTransformation
    {
        private Converter<string, string> _innerTextConverter = null;
        private SgmlReader _reader;

        public XhtmlConverter() : this(null, new SgmlReader()) { 
        }

        public XhtmlConverter(Converter<string, string> innerTextConverter) : this(innerTextConverter, new SgmlReader()) {
        }

        public XhtmlConverter(Converter<string, string> innerTextConverter, SgmlReader sgmlReader)
        {
            _innerTextConverter = innerTextConverter;
            _reader = sgmlReader;
        }

        public string Transform(string original)
        {
            return ConvertHtmlToXHtml(original, _innerTextConverter);
        }

        /// <summary>
        /// Converts the specified html into XHTML compliant text.
        /// </summary>
        /// <param name="html">html to convert.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        private string ConvertHtmlToXHtml(string html, Converter<string, string> converter)
        {
            _reader.DocType = "html";
            _reader.WhitespaceHandling = WhitespaceHandling.All;
            // Hack to fix SF bug #1678030
            html = RemoveNewLineBeforeCDATA(html);
            _reader.InputStream = new StringReader("<html>" + html + "</html>");
            _reader.CaseFolding = CaseFolding.ToLower;
            StringWriter writer = new StringWriter();
            XmlWriter xmlWriter = null;
            try
            {
                xmlWriter = new XmlTextWriter(writer);

                bool insideAnchor = false;
                bool skipRead = false;
                while ((skipRead || _reader.Read()) && !_reader.EOF)
                {
                    skipRead = false;
                    switch (_reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            //Special case for anchor tags for the time being. 
                            //We need some way to communicate which elements the current node is nested within 
                            if (_reader.IsEmptyElement)
                            {
                                xmlWriter.WriteStartElement(_reader.LocalName);
                                xmlWriter.WriteAttributes(_reader, true);
                                if (_reader.LocalName == "a" || _reader.LocalName == "script" || _reader.LocalName == "iframe" || _reader.LocalName == "object")
                                    xmlWriter.WriteFullEndElement();
                                else
                                    xmlWriter.WriteEndElement();
                            }
                            else
                            {
                                if (_reader.LocalName == "a")
                                    insideAnchor = true;
                                xmlWriter.WriteStartElement(_reader.LocalName);
                                xmlWriter.WriteAttributes(_reader, true);
                            }
                            break;

                        case XmlNodeType.Text:
                            string text = _reader.Value;

                            if (converter != null && !insideAnchor)
                                xmlWriter.WriteRaw(converter(HttpUtility.HtmlEncode(text)));
                            else
                                xmlWriter.WriteString(text);
                            break;

                        case XmlNodeType.EndElement:
                            if (_reader.LocalName == "a")
                                insideAnchor = false;

                            if (_reader.LocalName == "a" || _reader.LocalName == "script" || _reader.LocalName == "iframe" || _reader.LocalName == "object")
                                xmlWriter.WriteFullEndElement();
                            else
                                xmlWriter.WriteEndElement();
                            break;

                        default:
                            xmlWriter.WriteNode(_reader, true);
                            skipRead = true;
                            break;
                    }
                }
            }
            finally
            {
                if (xmlWriter != null)
                {
                    xmlWriter.Close();
                }
            }

            string xml = writer.ToString();
            return xml.Substring("<html>".Length, xml.Length - "<html></html>".Length);
        }

        // Ugly hack to remove any new line that sits between a tag end
        // and the beginning of a CDATA section.
        // This to make sure the Xhtml is well formatted before processing it
        private static string RemoveNewLineBeforeCDATA(string text)
        {
            if (!String.IsNullOrEmpty(text)) {
                return _newLineStripperRegex.Replace(text, "><![CDATA[");
            }
            return text;
        }

        static Regex _newLineStripperRegex = new Regex(@">(\r\n)+<!\[CDATA\[", RegexOptions.Compiled);
    }
}
