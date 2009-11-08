using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SubtextUpgrader
{
    /// <summary>
    /// Represents the central Skins.config file where we used to store all skin
    /// configurations.
    /// </summary>
    public class LegacySkinsConfig
    {
        public LegacySkinsConfig(IFile file) : this(file.ToXml())
        {
        }

        public LegacySkinsConfig(XmlDocument xml)
        {
            Xml = xml;
        }

        public XmlDocument Xml 
        { 
            get; 
            private set;
        }

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

        public void UpgradeSkins(IDirectory skinsDirectory)
        {
            foreach(var skin in GetNewSkinConfigs())
            {
                IDirectory skinDirectory = skinsDirectory.Combine(skin.TemplateFolder);
                if(!skinDirectory.Exists)
                {
                    skinsDirectory.Create();
                }
                skinDirectory.CreateXmlFile("skin.config", skin.Xml);
            }
        }
    }
}
