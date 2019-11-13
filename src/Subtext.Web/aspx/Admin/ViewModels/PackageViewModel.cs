using System;
using System.Collections.Generic;
using System.Linq;
using NuGet;
using Subtext.Framework.UI.Skinning;

namespace Subtext.Web.aspx.Admin.ViewModels
{
    public class PackageViewModel
    {
        public PackageViewModel(IPackage package, IDictionary<string, IEnumerable<SkinTemplate>> skinTemplates, Func<SkinTemplate, string> getIconUrl, bool mobileOnly)
        {
            id = System.Web.HttpUtility.HtmlEncode(package.Id);
            hasUpdate = false;
            version = package.Version.ToString();
            if (skinTemplates != null)
            {
                skins = from skin in skinTemplates[id]
                        select new SkinViewModel(skin, getIconUrl, mobileOnly);
            }
        }

        public string id { get; set; }
        public string version { get; set; }
        public bool hasUpdate { get; set; }
        public IEnumerable<SkinViewModel> skins { get; set; }
    }
}