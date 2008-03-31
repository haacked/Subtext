using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Hosting;
using System.Web;
using System.IO;
using Subtext.Framework.Util;
using System.Xml.Serialization;

namespace Subtext.Framework.UI.Skinning
{
    public class SkinTemplateCollection : List<SkinTemplate>
    {
        public SkinTemplateCollection() : this(HostingEnvironment.VirtualPathProvider)
        { }

        public SkinTemplateCollection(bool mobile) : this(HostingEnvironment.VirtualPathProvider, mobile)
        { }


        public SkinTemplateCollection(VirtualPathProvider vpp) : this(vpp, false)
        { }

        //for now it's either mobile, or not mobile.
        public SkinTemplateCollection(VirtualPathProvider vpp, bool mobile) : base()
        {
            SkinTemplates skinTemplates = null;
            if (HttpContext.Current != null && HttpContext.Current.Cache != null)
                skinTemplates = (SkinTemplates)HttpContext.Current.Cache["SkinTemplates"];
            
            if (skinTemplates == null)
            {
                skinTemplates = Load(vpp);

                if (skinTemplates != null)
                {
                    if (HttpContext.Current != null && HttpContext.Current.Cache != null)
                        HttpContext.Current.Cache.Insert("SkinTemplates", skinTemplates, vpp.GetCacheDependency("~/Admin/Skins.config", null, DateTime.Now.ToUniversalTime()));
                }
            }
            foreach (SkinTemplate template in skinTemplates.Templates)
            {
                if (template.MobileSupport > MobileSupport.None && mobile || template.MobileSupport == MobileSupport.None && !mobile)
                {
                    this.Add(template);
                }
            }
        }

        /// <summary>
        /// Instantiates an instance of <see cref="SkinTemplates" /> from the Admin/Skins.config file 
        /// as well as the Admin/Skins.User.config file using the specified <see cref="VirtualPathProvider" />
        /// </summary>
        /// <param name="vpathProvider"></param>
        /// <returns></returns>
        static SkinTemplates Load(VirtualPathProvider vpathProvider)
        {
            SkinTemplates skinTemplates = GetSkinTemplates(vpathProvider, "~/Admin/Skins.config");

            if (vpathProvider.FileExists("~/Admin/Skins.User.config"))
            {
                SkinTemplates userSpecificTemplates = GetSkinTemplates(vpathProvider, "~/Admin/Skins.User.config");
                if (userSpecificTemplates != null)
                {
                    foreach (SkinTemplate template in userSpecificTemplates.Templates)
                    {
                        skinTemplates.Templates.Add(template);
                    }
                }
            }
            return skinTemplates;
        }

        private static SkinTemplates GetSkinTemplates(VirtualPathProvider virtualPathProvider, string path)
        {
            VirtualFile virtualConfigFile = virtualPathProvider.GetFile(path);

            using (Stream configStream = virtualConfigFile.Open())
            {
                SkinTemplates templates = SerializationHelper.Load<SkinTemplates>(configStream);
                return templates;
            }
        }

        private Dictionary<string, SkinTemplate> _ht;

        /// <summary>
        /// Gets the template based on the skin id.
        /// </summary>
        /// <param name="skinKey">The id.</param>
        /// <returns></returns>
        public SkinTemplate GetTemplate(string skinKey)
        {
            if (_ht == null)
            {
                _ht = new Dictionary<string, SkinTemplate>();
                for (int i = 0; i < this.Count; i++)
                {
                    _ht.Add(this[i].SkinKey, this[i]);
                }
            }

            if (_ht.ContainsKey(skinKey.ToUpper(System.Globalization.CultureInfo.InvariantCulture)))
            {
                return _ht[skinKey.ToUpper(System.Globalization.CultureInfo.InvariantCulture)];
            }
            return null;
        }

        /// <summary>
        /// Represents skin templates configured within the Skins.config file.
        /// </summary>
        [Serializable]
        public class SkinTemplates
        {
            public SkinTemplates()
            {
            }


            [XmlArray("Skins")]
            public List<SkinTemplate> Templates
            {
                get { return this._skinTemplates; }
                set { this._skinTemplates = value; }
            }

            private List<SkinTemplate> _skinTemplates;
        }

    }
}
