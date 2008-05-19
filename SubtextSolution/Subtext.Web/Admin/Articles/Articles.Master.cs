using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Subtext.Framework;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Articles {
    public partial class Articles : System.Web.UI.MasterPage 
    {
        protected override void OnLoad(EventArgs e) 
        {
            BindLocalUI();
            base.OnLoad(e);
        }

        private void BindLocalUI() 
        {
            HyperLink newPostLink = new HyperLink();
            newPostLink.Text = "New Article";
            newPostLink.NavigateUrl = "Edit.aspx";
            AdminMasterPage.AddToActions(newPostLink);

            HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories",
                string.Format(System.Globalization.CultureInfo.InvariantCulture
                , "{0}?{1}={2}"
                , "../" + Constants.URL_EDITCATEGORIES
                , Keys.QRYSTR_CATEGORYTYPE
                , categoryLinks.CategoryType));
            AdminMasterPage.AddToActions(lnkEditCategories);

            LinkButton lkbRebuildTags = Utilities.CreateLinkButton("Rebuild All Tags");
            lkbRebuildTags.CausesValidation = false;
            lkbRebuildTags.Click += OnRebuildTagsClick;
            AdminMasterPage.AddToActions(lkbRebuildTags);
        }

        private static void OnRebuildTagsClick(object sender, EventArgs e) 
        {
            Entries.RebuildAllTags();
        }

        protected AdminPageTemplate AdminMasterPage 
        {
            get 
            {
                return this.Master as AdminPageTemplate;
            }
        }

    }
}
