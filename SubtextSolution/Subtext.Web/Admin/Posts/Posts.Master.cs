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

namespace Subtext.Web.Admin.Posts {
    public partial class Posts : AdminMasterPage 
    {
        protected override void OnLoad(EventArgs e) 
        {
            BindLocalUI();
            base.OnLoad(e);
        }

        private void BindLocalUI() 
        {
            HyperLink newPostLink = new HyperLink();
            newPostLink.Text = "New Post";
            newPostLink.NavigateUrl = AdminUrl.PostsEdit();
            AdminMasterPage.AddToActions(newPostLink);

            HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories",
                AdminUrl.EditCategories(categoryLinks.CategoryType));
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
