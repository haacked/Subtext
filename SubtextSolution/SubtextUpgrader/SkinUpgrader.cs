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

using System.IO;
using System.Xml;

namespace SubtextUpgrader
{
    public class SkinUpgrader
    {
        public void UpgradeCustomSkins(DirectoryInfo destination)
        {
            var skinConfig = new FileInfo(Path.Combine(destination.Name, @"Admin\Skins.user.config"));
            if(!skinConfig.Exists)
            {
                return;
            }
            UpgradeConfig(skinConfig);
        }

        private static void UpgradeConfig(FileInfo skinConfig)
        {
            var xml = new XmlDocument();
            xml.Load(skinConfig.OpenRead());
            var templates = xml.SelectNodes("/SkinTemplates/Skins/SkinTemplate");
            foreach(XmlNode templateNode in templates)
            {
                TransformTemplateToSkinConfig(templateNode);
            }
            // Delete Skins.user.config
        }

        public static XmlNode TransformTemplateToSkinConfig(XmlNode templateNode)
        {
            var skinNode = templateNode.SelectSingleNode("/SkinTemplates/Skins");
            templateNode.InnerXml = skinNode.InnerXml;
            var transformed = new XmlDocument();
            templateNode.FirstChild.Attributes.RemoveNamedItem("TemplateFolder");
            transformed.LoadXml(@"<?xml version=""1.0""?><SkinTemplates>" + templateNode.OuterXml + "</SkinTemplates>");
            return transformed;
        }
    }
}
