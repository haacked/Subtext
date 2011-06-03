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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin.Commands;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Pages
{
    // TODO: import - reconcile duplicates
    // TODO: CheckAll client-side, confirm bulk delete (add cmd)

    public partial class EditLinks : AdminPage
    {
        private const string VSKEY_LINKID = "LinkID";

        private bool _isListHidden = false;

        protected CheckBoxList cklCategories;
        private int _resultsPageNumber = 0;

        public EditLinks()
        {
            TabSectionId = "Links";
        }

        private int? filterCategoryID
        {
            get
            {
                if (ViewState["filterCategoryID"] == null)
                {
                    return null;
                }
                else
                {
                    return (int)ViewState["filterCategoryID"];
                }
            }
            set { ViewState["filterCategoryID"] = value; }
        }

        public int LinkID
        {
            get
            {
                if (ViewState[VSKEY_LINKID] != null)
                {
                    return (int)ViewState[VSKEY_LINKID];
                }
                else
                {
                    return NullValue.NullInt32;
                }
            }
            set { ViewState[VSKEY_LINKID] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rprSelectionList.Visible = true;
            headerLiteral.Visible = true;
            BindLocalUI();

            if (!IsPostBack)
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                {
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                }

                if (Request.QueryString[Keys.QRYSTR_CATEGORYID] != null)
                {
                    filterCategoryID = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);
                }

                resultsPager.PageSize = Preferences.ListingItemCount;
                resultsPager.PageIndex = _resultsPageNumber;

                if (filterCategoryID != null)
                {
                    resultsPager.UrlFormat += string.Format(CultureInfo.InvariantCulture, "&{0}={1}",
                                                            Keys.QRYSTR_CATEGORYID, filterCategoryID);
                }

                BindList();
            }
        }

        private void BindLocalUI()
        {
            LinkButton lkbNewLink = Utilities.CreateLinkButton("New Link");
            lkbNewLink.Click += lkbNewLink_Click;
            lkbNewLink.CausesValidation = false;
            AdminMasterPage.AddToActions(lkbNewLink);
            HyperLink lnkEditCategories = Utilities.CreateHyperLink(Resources.Label_EditCategories,
                                                                    string.Format(CultureInfo.InvariantCulture,
                                                                                  "{0}?{1}={2}",
                                                                                  Constants.URL_EDITCATEGORIES,
                                                                                  Keys.QRYSTR_CATEGORYTYPE,
                                                                                  CategoryType.LinkCollection));
            AdminMasterPage.AddToActions(lnkEditCategories);
        }

        private void BindList()
        {
            Edit.Visible = false;

            IPagedCollection<Link> selectionList = Repository.GetPagedLinks(filterCategoryID, _resultsPageNumber,
                                                                            resultsPager.PageSize, true);

            if (selectionList.Count > 0)
            {
                resultsPager.ItemCount = selectionList.MaxItems;
                rprSelectionList.DataSource = selectionList;
                rprSelectionList.DataBind();
            }
            else
            {
                // TODO: no existing items handling. add label and indicate no existing items. pop open edit.
            }
        }

        private void BindLinkEdit()
        {
            Link currentLink = Repository.GetLink(LinkID);

            rprSelectionList.Visible = false;
            headerLiteral.Visible = false;
            //			ImportExport.Visible = false;
            Edit.Visible = true;

            lblEntryID.Text = currentLink.Id.ToString(CultureInfo.InvariantCulture);
            txbTitle.Text = currentLink.Title;
            txbUrl.Text = currentLink.Url;
            txbRss.Text = currentLink.Rss;
            txtXfn.Text = currentLink.Relation;

            ckbIsActive.Checked = currentLink.IsActive;

            BindLinkCategories();
            ddlCategories.Items.FindByValue(currentLink.CategoryId.ToString(CultureInfo.InvariantCulture)).Selected =
                true;

            if (AdminMasterPage != null)
            {
                string title = string.Format(CultureInfo.InvariantCulture, "Editing Link \"{0}\"", currentLink.Title);
                AdminMasterPage.Title = title;
            }
        }

        public void BindLinkCategories()
        {
            ICollection<LinkCategory> selectionList = Repository.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
            if (selectionList != null && selectionList.Count != 0)
            {
                ddlCategories.DataSource = selectionList;
                ddlCategories.DataValueField = "Id";
                ddlCategories.DataTextField = "Title";
                ddlCategories.DataBind();
            }
            else
            {
                Messages.ShowError(Resources.EditLinks_NeedToAddCategoryFirst);
                Edit.Visible = false;
            }
        }

        private void UpdateLink()
        {
            string successMessage = Constants.RES_SUCCESSNEW;

            try
            {
                var link = new Link
                {
                    Title = txbTitle.Text,
                    Url = txbUrl.Text,
                    Rss = txbRss.Text,
                    IsActive = ckbIsActive.Checked,
                    CategoryId = Convert.ToInt32(ddlCategories.SelectedItem.Value),
                    Id = Config.CurrentBlog.Id,
                    Relation = txtXfn.Text
                };

                if (LinkID > 0)
                {
                    successMessage = Constants.RES_SUCCESSEDIT;
                    link.Id = LinkID;
                    Repository.UpdateLink(link);
                }
                else
                {
                    LinkID = Repository.CreateLink(link);
                }

                if (LinkID > 0)
                {
                    BindList();
                    Messages.ShowMessage(successMessage);
                }
                else
                {
                    Messages.ShowError(Constants.RES_FAILUREEDIT
                                       + " There was a baseline problem posting your link.");
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
                                                 Constants.RES_FAILUREEDIT, ex.Message));
            }
            finally
            {
                rprSelectionList.Visible = true;
                headerLiteral.Visible = true;
            }
        }

        private void ResetPostEdit(bool showEdit)
        {
            LinkID = NullValue.NullInt32;

            rprSelectionList.Visible = !showEdit;
            headerLiteral.Visible = !showEdit;
            Edit.Visible = showEdit;

            lblEntryID.Text = String.Empty;
            txbTitle.Text = String.Empty;
            txbUrl.Text = String.Empty;
            txbRss.Text = String.Empty;

            ckbIsActive.Checked = Preferences.AlwaysCreateIsActive;

            if (showEdit)
            {
                BindLinkCategories();
            }

            ddlCategories.SelectedIndex = -1;
        }

        private void ConfirmDelete(int linkID, string linkTitle)
        {
            var command = new DeleteLinkCommand(Repository, linkID, linkTitle)
            {
                ExecuteSuccessMessage = String.Format(CultureInfo.CurrentCulture, "Link '{0}' deleted", linkTitle)
            };
            Messages.ShowMessage(command.Execute());
            BindList();
        }

        private void ImportOpml()
        {
            if (OpmlImportFile.PostedFile.FileName.Trim().Length > 0)
            {
                OpmlItemCollection importedLinks = OpmlProvider.Import(OpmlImportFile.PostedFile.InputStream);

                if (importedLinks.Count > 0)
                {
                    var command = new ImportLinksCommand(Repository, importedLinks,
                                                         Int32.Parse(ddlImportExportCategories.SelectedItem.Value));
                    Messages.ShowMessage(command.Execute());
                }

                BindList();
            }
        }

        // REFACTOR
        public string CheckHiddenStyle()
        {
            if (_isListHidden)
            {
                return Constants.CSSSTYLE_HIDDEN;
            }
            else
            {
                return String.Empty;
            }
        }

        override protected void OnInit(EventArgs e)
        {
            rprSelectionList.ItemCommand += new RepeaterCommandEventHandler(rprSelectionList_ItemCommand);
            base.OnInit(e);
        }

        protected void lkbImportOpml_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ImportOpml();
            }
        }

        private void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "edit":
                    LinkID = Convert.ToInt32(e.CommandArgument);
                    BindLinkEdit();
                    break;

                case "delete":
                    int id = Convert.ToInt32(e.CommandArgument);
                    Link link = Repository.GetLink(id);
                    ConfirmDelete(id, link.Title);
                    break;

                default:
                    break;
            }
        }

        protected void lkbCancel_Click(object sender, EventArgs e)
        {
            ResetPostEdit(false);
        }

        protected void lkbPost_Click(object sender, EventArgs e)
        {
            UpdateLink();
        }

        private void lkbNewLink_Click(object sender, EventArgs e)
        {
            ResetPostEdit(true);
        }
    }
}