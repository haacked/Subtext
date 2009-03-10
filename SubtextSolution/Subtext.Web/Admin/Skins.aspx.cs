using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Web.Admin.Pages;
using Subtext.Framework.UI.Skinning;
using System.IO;
using Subtext.Web.Controls;
using Subtext.Framework.Web;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin
{
    public partial class Skins : AdminOptionsPage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack) {
                BindLocalUI();
            }
            base.OnLoad(e);
        }

        protected override void BindLocalUI()
        {
            skinRepeater.DataSource = SkinTemplates;
            mobileSkinRepeater.DataSource = MobileSkinTemplates;
            DataBind();
        }
        private SkinTemplateCollection skins;
        private SkinTemplateCollection mobileSkins;

        protected SkinTemplateCollection SkinTemplates
        {
            get
            {
                if (skins == null)
                {
                    skins = new SkinTemplateCollection();
                    foreach (SkinTemplate template in skins)
                    {
                        if (template.MobileSupport == MobileSupport.Supported)
                            template.Name += " (mobile ready)";
                    }
                }
                return skins;
            }
        }

        protected SkinTemplateCollection MobileSkinTemplates
        {
            get
            {
                if (mobileSkins == null)
                {
                    mobileSkins = new SkinTemplateCollection(true);
                    mobileSkins.Insert(0, SkinTemplate.Empty);
                }
                return mobileSkins;
            }
        }

        protected SkinTemplate EvalSkin(object o)
        {
            return o as SkinTemplate;
        }

        protected string GetSkinClientId(object o)
        {
            return (o as SkinTemplate).SkinKey.Replace(".", "_");
        }

        protected string EvalChecked(object o) {
            if (IsSelectedSkin(o))
            {
                return "checked=\"checked\"";
            }
            else {
                return "";
            }
        }

        protected string EvalSelected(object o)
        {
            if (IsSelectedSkin(o))
            {
                return " selected";
            }
            else
            {
                return "";
            }
        }

        private bool IsSelectedSkin(object o) { 
            string currentSkin = (o as SkinTemplate).SkinKey;
            string blogSkin = SubtextContext.Blog.Skin.SkinKey;
            return String.Equals(currentSkin, blogSkin, StringComparison.OrdinalIgnoreCase);
        }

        protected string GetSkinIconImage(object o) {
            var skin = o as SkinTemplate;

            string[] imageUrls = new[] { 
                string.Format("~/skins/{0}/SkinIcon.png", skin.TemplateFolder),
                string.Format("~/skins/{0}/{1}-SkinIcon.png", skin.TemplateFolder, skin.Name),
                "~/skins/_System/SkinIcon.png"
            };

            string imageUrl = imageUrls.First(path => File.Exists(Server.MapPath(path)));
            return HttpHelper.ExpandTildePath(imageUrl);
        }

        protected void OnSaveSkinClicked(object o, EventArgs args) {
            Blog blog = SubtextContext.Blog;
            SkinTemplate skinTemplate = new SkinTemplateCollection().GetTemplate(Request.Form["SkinKey"]);
            blog.Skin.TemplateFolder = skinTemplate.TemplateFolder;
            blog.Skin.SkinStyleSheet = skinTemplate.StyleSheet;
            Config.UpdateConfigData(blog);
            
            Messages.ShowMessage("Skin Saved!");
            BindLocalUI();
        }
    }
}
