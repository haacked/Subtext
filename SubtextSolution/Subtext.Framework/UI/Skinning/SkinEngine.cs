using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.Serialization;
using Subtext.Framework.Util;

namespace Subtext.Framework.UI.Skinning
{
    public class SkinEngine
    {
        const string RootSkinsVirtualPath = "~/skins"; //Does this need to be configurable? Probably not for now.
        IDictionary<string, SkinTemplate> _mobileTemplates;
        IDictionary<string, SkinTemplate> _templates;

        public SkinEngine() : this(HostingEnvironment.VirtualPathProvider)
        {
        }

        public SkinEngine(VirtualPathProvider vpp)
        {
            VirtualPathProvider = vpp;
        }

        protected VirtualPathProvider VirtualPathProvider { get; private set; }

        public IDictionary<string, SkinTemplate> GetSkinTemplates(bool mobile)
        {
            IDictionary<string, SkinTemplate> allTemplates = mobile ? _mobileTemplates : _templates;
            if(allTemplates == null)
            {
                VirtualDirectory skinsDirectory = VirtualPathProvider.GetDirectory(RootSkinsVirtualPath);

                IEnumerable<SkinTemplate> allTemplateConfigs =
                    from dir in skinsDirectory.Directories.OfType<VirtualDirectory>()
                    where !dir.Name.StartsWith("_")
                    let templates = GetSkinTemplatesFromDir(dir)
                    from template in templates
                    select template;

                allTemplates = (from template in allTemplateConfigs
                                where ((template.MobileSupport > MobileSupport.None && mobile)
                                       || (template.MobileSupport < MobileSupport.MobileOnly && !mobile))
                                select template).ToDictionary(t => t.SkinKey, StringComparer.OrdinalIgnoreCase);

                if(!mobile)
                {
                    _templates = allTemplates;
                }
                else
                {
                    _mobileTemplates = allTemplates;
                }
            }
            return allTemplates;
        }

        private IEnumerable<SkinTemplate> GetSkinTemplatesFromDir(VirtualDirectory virtualDirectory)
        {
            string skinConfigPath = RootSkinsVirtualPath + "/" + virtualDirectory.Name + "/skin.config";

            if(VirtualPathProvider.FileExists(skinConfigPath))
            {
                IEnumerable<SkinTemplate> deserializedTemplates = GetSkinTemplates(VirtualPathProvider, skinConfigPath);
                deserializedTemplates.ForEach(t => t.TemplateFolder = virtualDirectory.Name);
                return deserializedTemplates;
            }
            else
            {
                return new[] {new SkinTemplate {Name = virtualDirectory.Name, TemplateFolder = virtualDirectory.Name}};
            }
        }

        private IEnumerable<SkinTemplate> GetSkinTemplates(VirtualPathProvider virtualPathProvider, string path)
        {
            VirtualFile virtualConfigFile = virtualPathProvider.GetFile(path);

            using(Stream configStream = virtualConfigFile.Open())
            {
                var templates = SerializationHelper.Load<SkinTemplates>(configStream);
                return templates.Templates;
            }
        }

        #region Nested type: SkinTemplates

        public class SkinTemplates
        {
            [XmlElement("SkinTemplate")]
            public SkinTemplate[] Templates { get; set; }
        }

        #endregion
    }
}