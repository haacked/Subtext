using System;
using System.Web.UI.WebControls;
using Subtext.Web.Admin.Pages;
using Subtext.Framework;

namespace Subtext.Web.Admin.WebUI
{
    public partial class EditPosts : ConfirmationPage
    {
        private const string RES_SUCCESS = "Your tags were successfully updated.";
        private const string RES_FAILURE = "Tag update failed.";

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
            lkbNewPost.Click += new System.EventHandler(lkbNewPost_Click);
            AdminMasterPage.AddToActions(lkbNewPost);

            HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories",
                string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITCATEGORIES, Keys.QRYSTR_CATEGORYTYPE, categoryLinks.CategoryType));
            AdminMasterPage.AddToActions(lnkEditCategories);

            LinkButton lkbRebuildTags = Utilities.CreateLinkButton("Rebuild All Tags");
            lkbRebuildTags.CausesValidation = false;
            lkbRebuildTags.Click += new EventHandler(lkbRebuildTags_Click);
            AdminMasterPage.AddToActions(lkbRebuildTags);
        }

        private void lkbNewPost_Click(object sender, System.EventArgs e)
        {
            Editor.EditNewEntry();
        }

        private void lkbRebuildTags_Click(object sender, EventArgs e)
        {
            try
            {
                Entries.RebuildAllTags();
                this.Messages.ShowMessage(RES_SUCCESS);
            }
            catch (Exception ex)
            {
                this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
            }
        }
    }
}
