using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using NuGet;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services.NuGet;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.Web;
using Subtext.Web.Admin.Pages;
using Subtext.Web.aspx.Admin.ViewModels;

namespace Subtext.Web.Admin
{
    public partial class Skins : AdminOptionsPage
    {
        private ICollection<SkinTemplate> _mobileSkins;
        private Dictionary<string, PackageInstallationState> _installedSkinPackages = new Dictionary<string, PackageInstallationState>(StringComparer.OrdinalIgnoreCase);

        WebProjectManager _nuGetService = null;
        protected WebProjectManager NuGetService
        {
            get
            {
                if (_nuGetService == null)
                {
                    string packageSource = "http://bit.ly/subtextnuget";
                    string siteRoot = Request.MapPath("~/");

                    _nuGetService = new WebProjectManager(packageSource, siteRoot);
                }
                return _nuGetService;
            }
        }

        IEnumerable<IPackage> _availablePackages = null;
        protected IEnumerable<IPackage> AvailablePackages
        {
            get
            {
                if (_availablePackages == null)
                {
                    _availablePackages = (from p in NuGetService.SourceRepository.GetPackages()
                                          select p).ToList();
                }
                return _availablePackages;
            }
        }

        protected string Json(object value)
        {
            return new JavaScriptSerializer().Serialize(value);
        }

        IEnumerable<PackageViewModel> _installedPackages;
        protected IEnumerable<PackageViewModel> InstalledPackages
        {
            get
            {
                if (_installedPackages == null)
                {
                    _installedPackages = GetPackagesFromSkinTemplates(mobileOnly: false);
                }
                return _installedPackages;
            }
        }

        IEnumerable<PackageViewModel> _mobilePackages;
        protected IEnumerable<PackageViewModel> MobilePackages
        {
            get
            {
                if (_mobilePackages == null)
                {
                    _mobilePackages = GetPackagesFromSkinTemplates(mobileOnly: true);
                }
                return _mobilePackages;
            }
        }

        private IEnumerable<PackageViewModel> GetPackagesFromSkinTemplates(bool mobileOnly)
        {
            var packages = NuGetService.LocalRepository.GetPackages();

            var skinEngine = new SkinEngine();
            var skins = skinEngine.GetSkinTemplatesGroupedByFolder(mobileOnly: mobileOnly);


            return from p in packages
                   where skins.ContainsKey(p.Id)
                   select new PackageViewModel(p, skins, GetSkinIconImage, mobileOnly);
        }

        protected SkinViewModel SelectedSkin
        {
            get
            {
                return GetSkinFromSkinKey(Blog.Skin.SkinKey, mobileOnly: false);
            }
        }

        protected SkinViewModel SelectedMobileSkin
        {
            get
            {
                return GetSkinFromSkinKey(Blog.MobileSkin.SkinKey, mobileOnly: true);
            }
        }

        private SkinViewModel GetSkinFromSkinKey(string skinKey, bool mobileOnly)
        {
            var skinEngine = new SkinEngine();
            var skins = skinEngine.GetSkinTemplates(mobileOnly: mobileOnly);
            var skinTemplate = skins.GetValueOrDefault(skinKey ?? "Naked");
            if (skinTemplate == null)
            {
                skinTemplate = skins.First().Value;
            }
            return new SkinViewModel { name = skinTemplate.Name, icon = GetSkinIconImage(skinTemplate), skinKey = skinTemplate.SkinKey, mobile = mobileOnly };
        }

        private IPackage GetInstalledPackage(WebProjectManager projectManager, string packageId)
        {
            var installed = projectManager.GetInstalledPackages(packageId).Where(p => p.Id == packageId);

            var installedPackages = installed.ToList();
            var package = installedPackages.FirstOrDefault();
            return package;
        }

        protected ICollection<SkinTemplate> MobileSkinTemplates
        {
            get
            {
                if (_mobileSkins == null)
                {
                    var skinEngine = new SkinEngine();
                    var skins = new List<SkinTemplate>(skinEngine.GetSkinTemplates(true /* mobile */).Values);
                    skins.Insert(0, SkinTemplate.Empty);
                    _mobileSkins = skins;
                }
                return _mobileSkins;
            }
        }

        protected string GetSkinIconImage(SkinTemplate skin)
        {
            var imageUrls = new[]
            {
                string.Format(CultureInfo.InvariantCulture, "~/skins/{0}/SkinIcon.png", skin.TemplateFolder),
                string.Format(CultureInfo.InvariantCulture, "~/skins/{0}/{1}-SkinIcon.png", skin.TemplateFolder,
                              skin.Name),
                "~/skins/_System/SkinIcon.png"
            };

            string imageUrl = imageUrls.First(path => File.Exists(Server.MapPath(path)));
            return HttpHelper.ExpandTildePath(imageUrl);
        }

        protected void OnSaveSkinClicked()
        {
            Blog blog = SubtextContext.Blog;
            var skinEngine = new SkinEngine();
            SkinTemplate skinTemplate =
                skinEngine.GetSkinTemplates(false /* mobile */).GetValueOrDefault(Request.Form["SkinKey"]);
            blog.Skin.TemplateFolder = skinTemplate.TemplateFolder;
            blog.Skin.SkinStyleSheet = skinTemplate.StyleSheet;
            Repository.UpdateConfigData(blog);

            BindLocalUI();
        }
    }
}