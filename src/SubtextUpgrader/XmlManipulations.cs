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
using System.Collections.Generic;
using System.Xml;

namespace SubtextUpgrader
{
    public static class XmlManipulations
    {
        /// <summary>
        /// Moves the node up the hierarchy as a sibling to its parent.
        /// </summary>
        public static XmlNode MoveUp(this XmlNode node)
        {
            var parent = node.ParentNode;
            if(parent == node.OwnerDocument.DocumentElement)
            {
                return node;
            }
            var nodeToMove = node.CloneNode(true);
            node.ParentNode.ParentNode.AppendChild(nodeToMove);
            node.ParentNode.RemoveChild(node);
            return node;
        }

        public static XmlDocument ToXml(this string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        public static XmlDocument ExtractNodeAsDocument(this XmlDocument doc, string xpath)
        {
            var node = doc.SelectSingleNode(xpath);
            return node.ToXmlDocument();
        }

        public static IEnumerable<XmlDocument> ExtractDocuments(this XmlDocument doc, string xpath)
        {
            return doc.ExtractDocuments(xpath, null);
        }

        public static IEnumerable<XmlDocument> ExtractDocuments(this XmlDocument doc, string xpath, XmlDocument templateXml)
        {
            return doc.ExtractDocuments(xpath, templateXml, null);
        }

        public static IEnumerable<XmlDocument> ExtractDocuments(this XmlDocument doc, string xpath, XmlDocument templateXml, string insertXPath)
        {
            XmlNodeList nodes = doc.SelectNodes(xpath);
            foreach(XmlNode node in nodes)
            {
                yield return node.ToXmlDocument(templateXml, insertXPath);
            }
        }

        public static XmlDocument ToXmlDocument(this XmlNode node)
        {
            return node.ToXmlDocument(null, null);
        }

        public static XmlDocument ToXmlDocument(this XmlNode node, XmlDocument templateXml, string insertXPath)
        {
            var xml = (templateXml ?? new XmlDocument()).CloneNode(true) as XmlDocument;
            var importedNode = xml.ImportNode(node, true);

            
            var insertionNode = xml.SelectSingleNode(insertXPath?? "/.");
            if(insertionNode.FirstChild == null || insertionNode.FirstChild != xml.DocumentElement)
            {
                insertionNode.AppendChild(importedNode);
            }
            else
            {
                insertionNode.FirstChild.AppendChild(importedNode);
            }
            return xml;
        }

        public static XmlDocument ToXml(this IFile file)
        {
            return file.Contents.ToXml();
        }
    }
}
