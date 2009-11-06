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
using System.IO;
using System.Xml;

namespace SubtextUpgrader
{
    public class WebConfigUpgrader
    {
        public void UpgradeConfig(DirectoryInfo source, DirectoryInfo destination)
        {
            var newConfig = new FileInfo(Path.Combine(source.Name, @"Web.config"));
            var existingConfig = new FileInfo(Path.Combine(destination.Name, @"Web.config"));
            if(!existingConfig.Exists)
            {
                return;
            }
            UpgradeConfig(newConfig, existingConfig);
        }

        private static void UpgradeConfig(FileInfo newConfig, FileInfo existingConfig)
        {
            // backup
            newConfig.CopyTo(Path.Combine(newConfig.DirectoryName, "web.bak.config")); 
            existingConfig.CopyTo(Path.Combine(existingConfig.DirectoryName, "web.bak.config")); 
            
            var newXml = new XmlDocument();
            newXml.Load(newConfig.OpenRead());
            var existingXml = new XmlDocument();
            existingXml.Load(existingConfig.OpenRead());

            ApplyCustomizations(existingXml, newXml);
            
            newXml.Save(newConfig.OpenWrite());
            newConfig.CopyTo(existingConfig.FullName);
        }

        private static void ApplyCustomizations(XmlNode source, XmlNode destination)
        {
            OverwriteChildren(doc => doc.SelectConnectionStringsNode(), source, destination);
            OverwriteChildren(doc => doc.SelectEmailNode(), source, destination);
            OverwriteChildren(doc => doc.SelectEnclosureMimetypesNode(), source, destination);
            OverwriteChildren(doc => doc.SelectFriendlyUrlSettingsNode(), source, destination);
        }

        public static void OverwriteChildren(Func<XmlNode, XmlNode> nodeSelector, XmlNode sourceDocument, XmlNode destinationDocument)
        {
            var sourceNode = nodeSelector(sourceDocument);
            if(sourceNode == null || sourceNode.ChildNodes.Count == 0)
            {
                // we only want to copy nodes if there's something to copy.
                return;
            }
            var sourceChildNodes = sourceNode.ChildNodes;
            if(sourceChildNodes.Count == 0)
            {
                // we only want to copy nodes if there's something to copy.
                return;
            }

            var destinationNode = nodeSelector(destinationDocument);
            destinationNode.RemoveAll();
            
            foreach(XmlNode node in sourceChildNodes)
            {
                destinationNode.InnerXml = node.OuterXml;
            }
        }
    }
}
