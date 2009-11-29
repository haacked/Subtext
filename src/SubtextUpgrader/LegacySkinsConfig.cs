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
using System.Xml;

namespace SubtextUpgrader
{
    /// <summary>
    /// Represents the central Skins.config file where we used to store all skin
    /// configurations.
    /// </summary>
    public class LegacySkinsConfig
    {
        public LegacySkinsConfig(IFile file)
        {
            SkinsConfigFile = file;
        }

        protected IFile SkinsConfigFile
        {
            get; 
            private set;
        }

        public XmlDocument Xml 
        { 
            get
            {
                if(_xml == null)
                {
                    _xml = SkinsConfigFile.ToXml();
                }
                return _xml;
            }
        }

        XmlDocument _xml;

        /// <summary>
        /// Extracts the new skin config files from the old one.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SkinConfig> GetNewSkinConfigs()
        {
            var skinConfigTemplate = "<SkinTemplates />".ToXml();
            var xmlDocs = Xml.ExtractDocuments("/SkinTemplates/Skins/SkinTemplate", skinConfigTemplate);
            foreach(XmlDocument configXml in xmlDocs)
            {
                var templateNode = configXml.SelectSingleNode("/SkinTemplates/SkinTemplate");
                XmlAttribute templateFolderAttribute = templateNode.Attributes["TemplateFolder"];
                string templateFolder = templateFolderAttribute.Value;
                templateNode.Attributes.Remove(templateFolderAttribute);
                yield return new SkinConfig(configXml, templateFolder);
            }
        }

        public void UpgradeSkins(IDirectory newSkinsDirectory)
        {
            var oldSkinsDirectory = SkinsConfigFile.Directory.Parent.Combine("Skins");

            foreach(var skin in GetNewSkinConfigs())
            {
                IDirectory skinDirectory = newSkinsDirectory.Combine(skin.TemplateFolder).Ensure();
                oldSkinsDirectory.Combine(skin.TemplateFolder).CopyTo(skinDirectory);
                skinDirectory.CreateXmlFile("skin.config", skin.Xml);
            }
        }
    }
}
