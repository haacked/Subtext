using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Articles
{
    public partial class Articles : MasterPage
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
            var newPostLink = new HyperLink();
            newPostLink.Text = Resources.Label_NewArticle;
            newPostLink.NavigateUrl = "Edit.aspx";
            AdminMasterPage.AddToActions(newPostLink);

            HyperLink lnkEditCategories = Utilities.CreateHyperLink(Resources.Label_EditCategories,
                                                                    string.Format(CultureInfo.InvariantCulture
                                                                                  , "{0}?{1}={2}"
                                                                                  , "../" + Constants.URL_EDITCATEGORIES
                                                                                  , Keys.QRYSTR_CATEGORYTYPE
                                                                                  , categoryLinks.CategoryType));
            AdminMasterPage.AddToActions(lnkEditCategories);

            LinkButton lkbRebuildTags = Utilities.CreateLinkButton(Resources.Label_RebuildAllTags);
            lkbRebuildTags.CausesValidation = false;
            lkbRebuildTags.Click += OnRebuildTagsClick;
            AdminMasterPage.AddToActions(lkbRebuildTags);
        }

        private static void OnRebuildTagsClick(object sender, EventArgs e)
        {
            Entries.RebuildAllTags();
        }
    }
}