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
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.Commands;

namespace Subtext.Web.Admin.Pages
{
    // TODO: import - reconcile duplicates
    // TODO: CheckAll client-side, confirm bulk delete (add cmd)

    public partial class EditKeyWords : AdminOptionsPage
    {
        private const string VSKEY_KEYWORDID = "LinkID";

        private bool _isListHidden = false;
        private int _resultsPageNumber;

        #region Accessors

        public int KeyWordID
        {
            get
            {
                if (ViewState[VSKEY_KEYWORDID] != null)
                {
                    return (int)ViewState[VSKEY_KEYWORDID];
                }
                else
                {
                    return NullValue.NullInt32;
                }
            }
            set { ViewState[VSKEY_KEYWORDID] = value; }
        }

        #endregion

        private new void Page_Load(object sender, EventArgs e)
        {
            //BindLocalUI(); //no need to call

            if (!IsPostBack)
            {
                if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
                {
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                }

                resultsPager.PageSize = Preferences.ListingItemCount;
                resultsPager.PageIndex = _resultsPageNumber;

                BindList();
                //BindImportExportCategories();
            }
        }

        /*
                private void BindLocalUI()
                {
                    //wasn't working. I have added a button to GUI for this. - GY
                    LinkButton lkbNewLink = Utilities.CreateLinkButton("New KeyWord");
                    lkbNewLink.Click += new System.EventHandler(lkbNewKeyWord_Click);
                    lkbNewLink.CausesValidation =false;
                    PageContainer.AddToActions(lkbNewLink);
                }
        */

        private void BindList()
        {
            Edit.Visible = false;

            IPagedCollection<KeyWord> selectionList = Repository.GetPagedKeyWords(_resultsPageNumber,
                                                                                resultsPager.PageSize);

            if (selectionList.Count > 0)
            {
                resultsPager.ItemCount = selectionList.MaxItems;
                rprSelectionList.DataSource = selectionList;
                rprSelectionList.DataBind();
            }
        }

        private void BindLinkEdit()
        {
            KeyWord kw = Repository.GetKeyWord(KeyWordID);

            Results.Visible = false;
            Edit.Visible = true;

            txbTitle.Text = kw.Title;
            txbUrl.Text = kw.Url;
            txbWord.Text = kw.Word;
            txbRel.Text = kw.Rel;
            txbText.Text = kw.Text;


            chkFirstOnly.Checked = kw.ReplaceFirstTimeOnly;
            chkCaseSensitive.Checked = kw.CaseSensitive;

            if (AdminMasterPage != null)
            {
                string title = string.Format(CultureInfo.InvariantCulture, "Editing KeyWord \"{0}\"", kw.Title);
                AdminMasterPage.Title = title;
            }
        }

        private void UpdateLink()
        {
            string successMessage = Constants.RES_SUCCESSNEW;

            try
            {
                var keyword = new KeyWord
                {
                    Title = txbTitle.Text,
                    Url = txbUrl.Text,
                    Text = txbText.Text,
                    ReplaceFirstTimeOnly = chkFirstOnly.Checked,
                    CaseSensitive = chkCaseSensitive.Checked,
                    Rel = txbRel.Text,
                    Word = txbWord.Text
                };

                if (KeyWordID > 0)
                {
                    successMessage = Constants.RES_SUCCESSEDIT;
                    keyword.Id = KeyWordID;
                    Repository.UpdateKeyWord(keyword);
                }
                else
                {
                    KeyWordID = Repository.InsertKeyWord(keyword);
                }

                if (KeyWordID > 0)
                {
                    BindList();
                    Messages.ShowMessage(successMessage);
                }
                else
                {
                    Messages.ShowError(Constants.RES_FAILUREEDIT
                                       + " There was a baseline problem posting your KeyWord.");
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
                                                 Constants.RES_FAILUREEDIT, ex.Message));
            }
            finally
            {
                Results.Visible = true;
            }
        }

        private void ResetPostEdit(bool showEdit)
        {
            KeyWordID = NullValue.NullInt32;

            Results.Visible = !showEdit;
            Edit.Visible = showEdit;

            txbTitle.Text = string.Empty;
            txbText.Text = string.Empty;
            txbUrl.Text = string.Empty;
            txbRel.Text = string.Empty;
            txbWord.Text = string.Empty;
            chkFirstOnly.Checked = false;
            chkCaseSensitive.Checked = false;
        }

        private void ConfirmDelete(int keywordId, string keyword)
        {
            var command = new DeleteKeyWordCommand(Repository, keywordId, keyword)
            {
                ExecuteSuccessMessage = String.Format(CultureInfo.CurrentCulture, "Keyword '{0}' deleted", keyword)
            };
            Messages.ShowMessage(command.Execute());
            BindList();
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

        protected void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "edit":
                    KeyWordID = Convert.ToInt32(e.CommandArgument);
                    BindLinkEdit();
                    break;
                case "delete":
                    int id = Convert.ToInt32(e.CommandArgument);
                    KeyWord kw = Repository.GetKeyWord(id);
                    ConfirmDelete(id, kw.Word);
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ResetPostEdit(true);
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion
    }
}