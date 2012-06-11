
using System;
using Subtext.Framework.UI.Skinning;
namespace Subtext.Web.aspx.Admin.ViewModels
{
    public class SkinViewModel
    {
        public SkinViewModel() { }

        public SkinViewModel(SkinTemplate skinTemplate, Func<SkinTemplate, string> getIconUrl, bool mobileOnly)
        {
            this.name = System.Web.HttpUtility.HtmlEncode(skinTemplate.Name);
            this.icon = System.Web.HttpUtility.HtmlEncode(getIconUrl(skinTemplate));
            this.skinKey = System.Web.HttpUtility.HtmlEncode(skinTemplate.SkinKey);
            this.mobile = mobileOnly;
        }
        public string name { get; set; }
        public string icon { get; set; }
        public string skinKey { get; set; }
        public bool mobile { get; set; }
    }
}