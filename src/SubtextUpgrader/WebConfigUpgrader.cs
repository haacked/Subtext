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
using System.Xml;

namespace SubtextUpgrader
{
    public class WebConfigUpgrader
    {
        public WebConfigUpgrader(IDirectory sourceWebRoot)
        {
            SourceWebRootDirectory = sourceWebRoot;
        }

        protected IDirectory SourceWebRootDirectory
        {
            get; 
            private set;
        }

        public void UpgradeConfig(IDirectory destination)
        {
            var newConfig = SourceWebRootDirectory.CombineFile("Web.config");
            var existingConfig = destination.CombineFile("Web.config");
            if(!existingConfig.Exists)
            {
                return;
            }
            UpgradeConfig(newConfig, existingConfig);
        }

        private static void UpgradeConfig(IFile newConfig, IFile existingConfig)
        {
            var newXml = newConfig.ToXml();
            var existingXml = existingConfig.ToXml();

            ApplyCustomizations(existingXml, newXml);

            using(var stream = newConfig.OpenWrite())
            {
                newXml.Save(stream);
            }
            newConfig.Overwrite(existingConfig);
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
