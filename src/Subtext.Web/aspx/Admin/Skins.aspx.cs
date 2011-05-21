using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.Web;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin
{
    public partial class Skins : AdminOptionsPage
    {
        private ICollection<SkinTemplate> _mobileSkins;
        private IEnumerable<SkinTemplate> _skins;

        protected override void OnLoad(EventArgs e)
        {
            if (String.IsNullOrEmpty(Request.Form["SkinKey"]))
            {
                BindLocalUI();
            }
            else
            {
                OnSaveSkinClicked();
            }
            base.OnLoad(e);
        }

        protected IEnumerable<SkinTemplate> SkinTemplates
        {
            get
            {
                if (_skins == null)
                {
                    var skinEngine = new SkinEngine();
                    var skins = from skin in skinEngine.GetSkinTemplates(false /* mobile */).Values
                                where skin.SkinKey != "AGGREGATE"
                                orderby skin.Name
                                select skin;
                    foreach (SkinTemplate template in skins)
                    {
                        if (template.MobileSupport == MobileSupport.Supported)
                        {
                            template.Name += Resources.Skins_MobileReady;
                        }
                    }
                    _skins = skins;
                }
                return _skins;
            }
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

        protected override void BindLocalUI()
        {
            skinRepeater.DataSource = SkinTemplates;
            mobileSkinRepeater.DataSource = MobileSkinTemplates;
            DataBind();
        }

        protected SkinTemplate EvalSkin(object o)
        {
            return o as SkinTemplate;
        }

        protected string GetSkinClientId(object o)
        {
            return (o as SkinTemplate).SkinKey.Replace(".", "_");
        }

        protected string EvalChecked(object o)
        {
            if (IsSelectedSkin(o))
            {
                return "checked=\"checked\"";
            }
            return string.Empty;
        }

        protected string EvalSelected(object o)
        {
            if (IsSelectedSkin(o))
            {
                return " selected";
            }
            return string.Empty;
        }

        private bool IsSelectedSkin(object o)
        {
            string currentSkin = (o as SkinTemplate).SkinKey;
            string blogSkin = SubtextContext.Blog.Skin.SkinKey;
            return String.Equals(currentSkin, blogSkin, StringComparison.OrdinalIgnoreCase);
        }

        protected string GetSkinIconImage(object o)
        {
            var skin = o as SkinTemplate;

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
                skinEngine.GetSkinTemplates(false /* mobile */).ItemOrNull(Request.Form["SkinKey"]);
            blog.Skin.TemplateFolder = skinTemplate.TemplateFolder;
            blog.Skin.SkinStyleSheet = skinTemplate.StyleSheet;
            Repository.UpdateConfigData(blog);

            BindLocalUI();
        }
    }
}