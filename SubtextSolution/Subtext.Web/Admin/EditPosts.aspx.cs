using System;
using System.Web.UI.WebControls;
using Subtext.Web.Admin.Pages;
using Subtext.Framework;

namespace Subtext.Web.Admin.WebUI
{
    public partial class EditPosts : ConfirmationPage
    {
        public EditPosts()
        {
            TabSectionId = "Posts";
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            BindLocalUI();
        }

        private void BindLocalUI()
        {
            LinkButton lkbNewPost = Utilities.CreateLinkButton("New Post");
            lkbNewPost.CausesValidation = false;
            lkbNewPost.Click += OnNewPostClick;
            AdminMasterPage.AddToActions(lkbNewPost);

            HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories",
                string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITCATEGORIES, Keys.QRYSTR_CATEGORYTYPE, categoryLinks.CategoryType));
            AdminMasterPage.AddToActions(lnkEditCategories);

            LinkButton lkbRebuildTags = Utilities.CreateLinkButton("Rebuild All Tags");
            lkbRebuildTags.CausesValidation = false;
            lkbRebuildTags.Click += OnRebuildTagsClick;
            AdminMasterPage.AddToActions(lkbRebuildTags);
        }

        private void OnNewPostClick(object sender, EventArgs e)
        {
            Editor.EditNewEntry();
        }

        private static void OnRebuildTagsClick(object sender, EventArgs e)
        {
            Entries.RebuildAllTags();
        }
    }
}
