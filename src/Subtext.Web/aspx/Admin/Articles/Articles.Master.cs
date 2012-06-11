#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

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
            var newPostLink = new HyperLink { Text = Resources.Label_NewArticle, NavigateUrl = "Edit.aspx" };
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

        private void OnRebuildTagsClick(object sender, EventArgs e)
        {
            AdminMasterPage.Repository.RebuildAllTags();
        }
    }
}