
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NuGet;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services.NuGet;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.Web;
using Subtext.Web.aspx.Admin.ViewModels;
namespace Subtext.Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class SkinsController : Controller
    {
        public SkinsController(SubtextContext subtextContext)
        {
            SubtextContext = subtextContext;
        }

        protected SubtextContext SubtextContext
        {
            get;
            private set;
        }

        public ActionResult Online()
        {
            var projectManager = GetProjectManager();
            var packages = from p in (projectManager.SourceRepository.GetPackages().ToList())
                           select new PackageViewModel(p, null, null, false);
            return Json(packages.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Updates()
        {
            var projectManager = GetProjectManager();
            var updates = projectManager.SourceRepository.GetUpdates(projectManager.LocalRepository.GetPackages()).ToList();

            var results = from package in updates
                          select new { id = package.Id, version = package.Version.ToString() };

            if (Request.IsAjaxRequest())
            {
                return Json(results, JsonRequestBehavior.AllowGet);
            }

            return View(results);
        }

        [HttpPost]
        public ActionResult Install(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            var projectManager = GetProjectManager();
            var packages = projectManager.SourceRepository.FindPackagesById(id).OrderByDescending(p => p.Version);
            if (packages == null)
            {
                throw new InvalidOperationException("Package does not exist");
            }
            var package = packages.FirstOrDefault();
            if (package == null)
            {
                throw new InvalidOperationException("Package does not exist");
            }
            projectManager.InstallPackage(package);

            // Get the skin.
            var skinEngine = new SkinEngine();
            var skinsWithoutMobileOnly = skinEngine.GetSkinTemplatesGroupedByFolder(mobileOnly: false);

            var packageViewModel = new PackageViewModel(package, skinsWithoutMobileOnly, GetSkinIconImage, mobileOnly: false);

            if (Request.IsAjaxRequest())
            {
                return Json(packageViewModel, JsonRequestBehavior.AllowGet);
            }

            return View(package);
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

            string imageUrl = imageUrls.First(path => System.IO.File.Exists(Server.MapPath(path)));
            return HttpHelper.ExpandTildePath(imageUrl);
        }

        [HttpPost]
        public ActionResult Upgrade(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            var projectManager = GetProjectManager();
            var installed = GetInstalledPackage(projectManager, id);
            var update = projectManager.GetUpdate(installed);

            projectManager.UpdatePackage(update);

            if (Request.IsAjaxRequest())
            {
                return Json(new { version = update.Version.ToString() }, JsonRequestBehavior.AllowGet);
            }

            return View(update);
        }

        [HttpPost]
        public ActionResult Uninstall(string id, bool removeDependencies = false)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            var projectManager = GetProjectManager();
            var installed = GetInstalledPackage(projectManager, id);
            if (installed != null)
            {
                projectManager.UninstallPackage(installed, true);
            }

            installed = GetInstalledPackage(projectManager, id);
            bool success = true;
            string message = null;

            if (installed != null)
            {
                success = false;
                message = "The package was not fully installed and may need to be cleaned up manually.";
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success, message }, JsonRequestBehavior.AllowGet);
            }

            return View(installed);
        }

        [HttpPost]
        public ActionResult Save(string skinKey, bool mobile)
        {
            Blog blog = SubtextContext.Blog;
            var skinEngine = new SkinEngine();
            SkinTemplate skinTemplate =
                skinEngine.GetSkinTemplates(mobile).GetValueOrDefault(skinKey);
            if (!mobile)
            {
                blog.Skin.TemplateFolder = skinTemplate.TemplateFolder;
                blog.Skin.SkinStyleSheet = skinTemplate.StyleSheet;
            }
            else
            {
                blog.MobileSkin.TemplateFolder = skinTemplate.TemplateFolder;
                blog.MobileSkin.SkinStyleSheet = skinTemplate.StyleSheet;
            }
            SubtextContext.Repository.UpdateConfigData(blog);

            return Json(new { });
        }

        private WebProjectManager GetProjectManager()
        {
            string packageSource = "http://bit.ly/subtextnuget";
            string siteRoot = Request.MapPath("~/");

            return new WebProjectManager(packageSource, siteRoot);
        }

        private IPackage GetInstalledPackage(WebProjectManager projectManager, string packageId)
        {
            var installed = projectManager.GetInstalledPackages(packageId).Where(p => p.Id == packageId);

            var installedPackages = installed.ToList();
            return installedPackages.FirstOrDefault();
        }
    }
}
