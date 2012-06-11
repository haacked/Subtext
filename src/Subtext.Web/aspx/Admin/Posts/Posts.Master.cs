using System;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Posts
{
    public partial class Posts : AdminMasterPage
    {
        protected AdminPageTemplate AdminMasterPage
        {
            get { return Master as AdminPageTemplate; }
        }

        protected override void OnLoad(EventArgs e)
        {
            BindLocalUI();
            base.OnLoad(e);
        }

        private void BindLocalUI()
        {
            var newPostLink = new HyperLink { Text = Resources.Label_NewPost, NavigateUrl = AdminUrl.PostsEdit() };
            AdminMasterPage.AddToActions(newPostLink);

            HyperLink lnkEditCategories = Utilities.CreateHyperLink(Resources.Label_EditCategories,
                                                                    AdminUrl.EditCategories(categoryLinks.CategoryType));
            AdminMasterPage.AddToActions(lnkEditCategories);

            LinkButton lkbRebuildTags = Utilities.CreateLinkButton(Resources.Label_RebuildAllTags);
            lkbRebuildTags.CausesValidation = false;
            lkbRebuildTags.Click += OnRebuildTagsClick;
            AdminMasterPage.AddToActions(lkbRebuildTags);
        }

        private void OnRebuildTagsClick(object sender, EventArgs e)
        {
            Repository.RebuildAllTags();
        }
    }
}