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
using System.IO;
using System.Linq;
using System.Security;
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

        public SkinEngine()
            : this(HostingEnvironment.VirtualPathProvider)
        {
        }

        public SkinEngine(VirtualPathProvider vpp)
        {
            VirtualPathProvider = vpp;
        }

        protected VirtualPathProvider VirtualPathProvider { get; private set; }

        public IDictionary<string, SkinTemplate> GetSkinTemplates(bool mobileOnly)
        {
            IDictionary<string, SkinTemplate> allTemplates = mobileOnly ? _mobileTemplates : _templates;
            if (allTemplates == null)
            {
                IEnumerable<SkinTemplate> allTemplateConfigs;
                try
                {
                    // Full trust can use VirtualPathProvider
                    VirtualDirectory skinsDirectory = VirtualPathProvider.GetDirectory(RootSkinsVirtualPath);

                    allTemplateConfigs =
                        from dir in skinsDirectory.Directories.OfType<VirtualDirectory>()
                        where !dir.Name.StartsWith("_") && !dir.Name.StartsWith(".")
                        let templates = GetSkinTemplatesFromDir(dir)
                        from template in templates
                        select template;

                    allTemplates = GetSkinTemplateFromConfigs(allTemplateConfigs, mobileOnly);

                }
                catch (SecurityException)
                {
                    // Partial trust has to use the filesystem directly
                    var skinsDir = new DirectoryInfo
                        (AppDomain.CurrentDomain.BaseDirectory
                        + Path.DirectorySeparatorChar
                        + "skins"
                        );

                    allTemplateConfigs =
                        from dir in skinsDir.GetDirectories()
                        where !dir.Name.StartsWith("_") && !dir.Name.StartsWith(".")
                        let templates = GetSkinTemplatesFromDir(dir)
                        from template in templates
                        select template;

                    allTemplates = GetSkinTemplateFromConfigs(allTemplateConfigs, mobileOnly);
                }

                if (!mobileOnly)
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

        public IDictionary<string, IEnumerable<SkinTemplate>> GetSkinTemplatesGroupedByFolder(bool mobileOnly)
        {
            var skins = GetSkinTemplates(mobileOnly).Values;
            return skins.GroupBy(s => s.TemplateFolder).ToDictionary(s => s.First().TemplateFolder, v => v.AsEnumerable(), StringComparer.OrdinalIgnoreCase);
        }

        private static IDictionary<string, SkinTemplate> GetSkinTemplateFromConfigs(IEnumerable<SkinTemplate> skinTemplates, bool mobileOnly)
        {
            return (from template in skinTemplates
                    where ((template.MobileSupport > MobileSupport.None && mobileOnly)
                           || (template.MobileSupport < MobileSupport.MobileOnly && !mobileOnly))
                    select template).ToDictionary(t => t.SkinKey, StringComparer.OrdinalIgnoreCase);
        }

        private IEnumerable<SkinTemplate> GetSkinTemplatesFromDir(VirtualFileBase virtualDirectory)
        {
            string skinConfigPath = string.Format("{0}/{1}/skin.config", RootSkinsVirtualPath, virtualDirectory.Name);

            if (VirtualPathProvider.FileExists(skinConfigPath))
            {
                IEnumerable<SkinTemplate> deserializedTemplates = GetSkinTemplates(VirtualPathProvider, skinConfigPath);
                deserializedTemplates.ForEach(t => t.TemplateFolder = virtualDirectory.Name);
                return deserializedTemplates;
            }
            return new[] { new SkinTemplate { Name = virtualDirectory.Name, TemplateFolder = virtualDirectory.Name } };
        }

        private IEnumerable<SkinTemplate> GetSkinTemplatesFromDir(DirectoryInfo directory)
        {
            string skinConfigPath = directory.FullName + Path.DirectorySeparatorChar + "skin.config";

            if (File.Exists(skinConfigPath))
            {
                IEnumerable<SkinTemplate> deserializedTemplates = GetSkinTemplates(skinConfigPath);
                deserializedTemplates.ForEach(t => t.TemplateFolder = directory.Name);
                return deserializedTemplates;
            }
            return new[] { new SkinTemplate { Name = directory.Name, TemplateFolder = directory.Name } };
        }

        private static IEnumerable<SkinTemplate> GetSkinTemplates(VirtualPathProvider virtualPathProvider, string path)
        {
            VirtualFile virtualConfigFile = virtualPathProvider.GetFile(path);

            using (Stream configStream = virtualConfigFile.Open())
            {
                var templates = SerializationHelper.Load<SkinTemplates>(configStream);
                return templates.Templates;
            }
        }

        private static IEnumerable<SkinTemplate> GetSkinTemplates(string path)
        {
            var configFile = new FileInfo(path);

            using (Stream configStream = configFile.OpenRead())
            {
                var templates = SerializationHelper.Load<SkinTemplates>(configStream);
                return templates.Templates;
            }
        }

        public class SkinTemplates
        {
            [XmlElement("SkinTemplate")]
            public SkinTemplate[] Templates { get; set; }
        }
    }
}