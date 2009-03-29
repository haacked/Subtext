using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.IO;
using Subtext.Framework.Util;

namespace Subtext.Framework.UI.Skinning
{
    public class SkinEngine
    {
        const string RootSkinsVirtualPath = "~/skins"; //Does this need to be configurable? Probably not for now.

        public SkinEngine() : this(HostingEnvironment.VirtualPathProvider) { 
        
        }

        public SkinEngine(VirtualPathProvider vpp)
        {
            VirtualPathProvider = vpp;
        }

        protected VirtualPathProvider VirtualPathProvider
        { 
            get; 
            private set; 
        }

        public IDictionary<string, SkinTemplate> GetSkinTemplates(bool mobile) 
        {
            VirtualDirectory skinsDirectory = VirtualPathProvider.GetDirectory(RootSkinsVirtualPath);

            var templates = (from dir in skinsDirectory.Directories.OfType<VirtualDirectory>()
                             let template = GetSkinTemplateFromDir(dir)
                             where !dir.Name.StartsWith("_") && 
                             ((template.MobileSupport > MobileSupport.None && mobile)
                                || (template.MobileSupport < MobileSupport.MobileOnly && !mobile))
                            select template).ToDictionary(t => t.SkinKey);
            return templates;
        }

        private SkinTemplate GetSkinTemplateFromDir(VirtualDirectory virtualDirectory) {
            string skinConfigPath = RootSkinsVirtualPath + "/" + virtualDirectory.Name + "/skin.config";

            if (VirtualPathProvider.FileExists(skinConfigPath)) {
                SkinTemplate deserializedTemplate = GetSkinTemplate(VirtualPathProvider, skinConfigPath);
                deserializedTemplate.TemplateFolder = virtualDirectory.Name;
                return deserializedTemplate;
            }
            else {
                return new SkinTemplate { Name = virtualDirectory.Name, TemplateFolder = virtualDirectory.Name };
            }
        }

        private SkinTemplate GetSkinTemplate(VirtualPathProvider virtualPathProvider, string path)
        {
            VirtualFile virtualConfigFile = virtualPathProvider.GetFile(path);

            using (Stream configStream = virtualConfigFile.Open())
            {
                SkinTemplate template = SerializationHelper.Load<SkinTemplate>(configStream);
                return template;
            }
        }

    }
}
