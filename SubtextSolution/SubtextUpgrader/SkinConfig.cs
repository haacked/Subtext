using System.Xml;

namespace SubtextUpgrader
{
    public class SkinConfig
    {
        public SkinConfig(XmlDocument configXml, string templateFolder)
        {
            Xml = configXml;
            TemplateFolder = templateFolder;
        }

        public XmlDocument Xml { get; private set; }
        public string TemplateFolder { get; private set; }
    }
}
