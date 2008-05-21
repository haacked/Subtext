#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Controls;
using System.Web;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Displays comments posted to the blog and allows the 
    /// admin to delete comments.
    /// </summary>
    public partial class Feedback : ConfirmationPage
    {
        private const string VSKEY_FEEDBACKID = "FeedbackID";
        private int pageIndex = 0;
        private bool _isListHidden = false;
        LinkButton btnViewApprovedComments;
        LinkButton btnViewModerateComments;
        LinkButton btnViewSpam;
        LinkButton btnViewTrash;
        private bool _hasViewChanged = false;

        /// <summary>
        /// Constructs an image of this page. Sets the tab section to "Feedback".
        /// </summary>
        public Feedback()
        {
            this.TabSectionId = "Feedback";
        }

        /// <summary>
        /// Whether or not to moderate comments.
        /// </summary>
        protected FeedbackStatusFlag FeedbackStatusFilter
        {
            get
            {
                return (FeedbackStatusFlag)(ViewState["FeedbackStatusFilter"] ?? FeedbackStatusFlag.Approved);
            }
            set
            {
                ViewState["FeedbackStatusFilter"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnViewApprovedComments = AddFolderLink("Approved", "btnViewActiveComments", "Approved Comments", OnViewApprovedCommentsClick);
            this.btnViewModerateComments = AddFolderLink("Moderate", "btnModerateComments", "Comments in need of moderation", OnViewCommentsForModerationClick
                , CreateAdminRssUrl("ModeratedCommentRss.axd"));
            this.btnViewModerateComments.Enabled = Config.CurrentBlog.ModerationEnabled;
            this.btnViewSpam = AddFolderLink("Flagged Spam", "btnViewSpam", "Comments Flagged As Spam By Filters", OnViewSpamClick);
            this.btnViewTrash = AddFolderLink("Trash", "btnViewTrash", "Comments In The Trash Bin (Confirmed Spam or Deleted Items)", OnViewTrashClick);

            if (!IsPostBack)
            {
                this.FeedbackStatusFilter = GetStatusFromQueryString();
                //this.cbShowOnlyComments.Checked = Preferences.FeedbackShowOnlyComments;
                this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
                BindList();

                string feedbackIDText = Request.QueryString["FeedbackID"];
                int feedbackID = NullValue.NullInt32;
                if (feedbackIDText != null && feedbackIDText.Length > 0)
                {
                    //Ok, we came from outside the admin tool.
                    ReturnToOriginalPost = int.TryParse(feedbackIDText, out feedbackID);
                }
                if (feedbackID > NullValue.NullInt32)
                {
                    this.FeedbackID = feedbackID;
                    BindFeedbackEdit();
                }
            }
        }

        FeedbackStatusFlag GetStatusFromQueryString()
        {
            string filter = Request.QueryString["status"] ?? "1";
            int filterId;
            if (!int.TryParse(filter, out filterId))
            {
                return FeedbackStatusFlag.Approved;
            }
            return (FeedbackStatusFlag)filterId;
        }
        private LinkButton AddFolderLink(string label, string id, string title, EventHandler handler)
        {
            return AddFolderLink(label, id, title, handler, "");
        }
        
        private LinkButton AddFolderLink(string label, string id, string title, EventHandler handler, string RssUrl)
        {
            LinkButton button = Utilities.CreateLinkButton(label);
            button.ID = id;
            button.CausesValidation = false;
            button.Click += handler;
            button.Attributes["title"] = title;
            AdminMasterPage.AddToActions(button, RssUrl);
            return button;
        }

        void OnViewApprovedCommentsClick(object sender, EventArgs e)
        {
            if (this.FeedbackStatusFilter != FeedbackStatusFlag.Approved)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlag.Approved;
                this._hasViewChanged = true;
            }

            this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
            this.headerLiteral.InnerText = "Comments";
            HtmlHelper.AppendCssClass(this.btnViewApprovedComments, "active");
            this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&status=" + (int)FeedbackStatusFilter;
            this.btnApprove.Visible = false;
            this.btnDestroy.Visible = false;
            this.btnDelete.Visible = true;
            this.btnConfirmSpam.Visible = true;
            this.btnEmpty.Visible = false;

            BindList();
        }

        void OnViewCommentsForModerationClick(object sender, EventArgs e)
        {
            if (this.FeedbackStatusFilter != FeedbackStatusFlag.NeedsModeration)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlag.NeedsModeration;
                this._hasViewChanged = true;
            }

            this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
            this.headerLiteral.InnerText = "Comments Pending Moderator Approval";
            HtmlHelper.AppendCssClass(this.btnViewModerateComments, "active");
            this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&status=" + (int)FeedbackStatusFilter;
            this.btnApprove.Visible = true;
            this.btnApprove.Text = "Approve";
            this.btnConfirmSpam.Visible = true;
            this.btnDestroy.Visible = false;
            this.btnDelete.Visible = true;
            this.btnEmpty.Visible = false;
            BindList();
        }

        void OnViewSpamClick(object sender, EventArgs e)
        {
            if (this.FeedbackStatusFilter != FeedbackStatusFlag.FlaggedAsSpam)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlag.FlaggedAsSpam;
                this._hasViewChanged = true;
            }

            this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
            HtmlHelper.AppendCssClass(this.btnViewSpam, "active");
            this.headerLiteral.InnerText = "Comments Flagged As SPAM";
            this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&status=" + (int)FeedbackStatusFilter;
            this.btnApprove.Visible = true;
            this.btnApprove.Text = "Approve";
            this.btnDelete.Visible = true;
            this.btnDelete.ToolTip = "Trashes checked spam";
            this.btnDestroy.Visible = false;
            this.btnConfirmSpam.Visible = false;
            this.btnEmpty.Visible = true;
            this.btnEmpty.ToolTip = "Destroy all spam, not just checked";
            BindList();
        }

        void OnViewTrashClick(object sender, EventArgs e)
        {
            if (this.FeedbackStatusFilter != FeedbackStatusFlag.Deleted)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlag.Deleted;
                this._hasViewChanged = true;
            }
            this.FeedbackStatusFilter = FeedbackStatusFlag.Deleted;
            this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
            HtmlHelper.AppendCssClass(this.btnViewTrash, "active");
            this.headerLiteral.InnerText = "Comments In The Trash Bin";
            this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&status=" + (int)FeedbackStatusFilter;
            this.btnApprove.Visible = true;
            this.btnApprove.Text = "Undelete";
            this.btnConfirmSpam.Visible = false;
            this.btnDelete.Visible = false;
            this.btnDestroy.Visible = true;
            this.btnEmpty.Visible = true;
            this.btnEmpty.ToolTip = "Destroy all trash, not just checked";
            BindList();
        }

        //I implemented version 2 before I let you guys see version 1.
        //Therefore, I kept the version 1 code in case you wished to use it instead.
        //Version 1 was "show only comments". Version 2 is a filter that can show all, show only comments, or show only pingtracks.
        [Obsolete("Use rbFeedbackFilter", false)]
        protected void cbShowOnlyComments_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.FeedbackShowOnlyComments = this.cbShowOnlyComments.Checked;
            BindList();
        }

        protected void rbFeedbackFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Preferences.SetFeedbackItemFilter(this.rbFeedbackFilter.SelectedValue, FeedbackStatusFilter);
            BindList();
        }

        /// <summary>
        /// Gets the body of the feedback represented by the dataItem.
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected static string GetBody(object dataItem)
        {
            FeedbackItem feedbackItem = (FeedbackItem)dataItem;
            if (feedbackItem.FeedbackType != FeedbackType.PingTrack)
            {
                return feedbackItem.Body;
            }
            return string.Format(CultureInfo.InvariantCulture, "{0}<br /><a target=\"_blank\" title=\"view: {1}\"  href=\"{2}\">Pingback/TrackBack</a>", feedbackItem.Body, feedbackItem.Title, feedbackItem.SourceUrl);
        }

        /// <summary>
        /// Returns the author during data binding. If the author specified 
        /// an email address, includes that.
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected static string GetAuthor(object dataItem)
        {
            FeedbackItem feedbackItem = (FeedbackItem)dataItem;
            return string.Format(@"<span title=""{0}"">{1}</span>", feedbackItem.IpAddress, feedbackItem.Author);
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="dataItem">The data item.</param>
        /// <returns></returns>
        protected static string GetTitle(object dataItem)
        {
            FeedbackItem feedbackItem = (FeedbackItem)dataItem;
            if (feedbackItem.DisplayUrl != null)
            {
                return string.Format(@"<a href=""{0}"" title=""{0}"">{1}</a>", feedbackItem.DisplayUrl, feedbackItem.Title);
            }

            return feedbackItem.Title;
        }

        /// <summary>
        /// Returns the author during data binding. If the author specified 
        /// an email address, includes that.
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected static string GetAuthorInfo(object dataItem)
        {
            FeedbackItem feedback = (FeedbackItem)dataItem;
            string authorInfo = string.Empty;

            if (feedback.Email != null && feedback.Email.Length > 0 && feedback.Email.IndexOf("@") > 0)
            {
                string mailToUrl = feedback.Email 
                    + "&subject=re:" + HttpUtility.UrlEncode(feedback.Title) 
                    + "&body=----------" + Environment.NewLine + HttpUtility.UrlEncode(feedback.Body);
                authorInfo += string.Format(@"<a href=""mailto:{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" class=""email"" /></a>", mailToUrl, HttpHelper.ExpandTildePath("~/images/email.gif"));
            }

            if (feedback.SourceUrl != null)
            {
                authorInfo += string.Format(@"<a href=""{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" /></a>", feedback.SourceUrl, HttpHelper.ExpandTildePath("~/images/permalink.gif"));
            }

            return authorInfo;
        }

        private void BindCounts()
        {
            FeedbackCounts counts = FeedbackItem.GetFeedbackCounts();
            SetCount(btnViewApprovedComments, counts.ApprovedCount);
            SetCount(btnViewModerateComments, counts.NeedsModerationCount);
            SetCount(btnViewSpam, counts.FlaggedAsSpamCount);
            SetCount(btnViewTrash, counts.DeletedCount);
        }

        static void SetCount(LinkButton button, int count)
        {
            button.Text = StringHelper.LeftBefore(button.Text, "(") + "(" + count + ")";
        }

        private void BindList()
        {
            rprSelectionList.Visible = true;
            noCommentsMessage.Visible = false;
            Edit.Visible = false;
            headerLiteral.Visible = true;
            if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

            if (this._hasViewChanged)
                this.pageIndex = 0;

            this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&status=" + (int)FeedbackStatusFilter;
            this.resultsPager.PageSize = Preferences.ListingItemCount;
            this.resultsPager.PageIndex = this.pageIndex;

            FeedbackStatusFlag excludeFilter = ~this.FeedbackStatusFilter;
            //Approved is a special case.  If a feedback has the approved bit set, 
            //it is approved no matter what other bits are set.
            if (this.FeedbackStatusFilter == FeedbackStatusFlag.Approved)
                excludeFilter = FeedbackStatusFlag.None;

            //Likewise, deleted is a special case.  If a feedback has the deleted 
            //bit set, it is in the trash no matter what other bits are set.
            if (this.FeedbackStatusFilter == FeedbackStatusFlag.Deleted)
                excludeFilter = FeedbackStatusFlag.Approved;

            //13-nov-06 mountain_sf
            //For version 1: checkbox to show only comments (obsolete)
            //IPagedCollection<FeedbackItem> selectionList = FeedbackItem.GetPagedFeedback(this.pageIndex, this.resultsPager.PageSize, this.FeedbackStatusFilter, excludeFilter, cbShowOnlyComments.Checked?FeedbackType.Comment:FeedbackType.None);
            //
            //For version 2: a filter that can show all, show only comments, or show only pingtracks.
            IPagedCollection<FeedbackItem> selectionList = FeedbackItem.GetPagedFeedback(this.pageIndex, this.resultsPager.PageSize, this.FeedbackStatusFilter, excludeFilter, Preferences.ParseFeedbackItemFilter(this.rbFeedbackFilter.SelectedValue));
            //

            if (selectionList.Count > 0)
            {
                this.resultsPager.Visible = true;
                this.resultsPager.ItemCount = selectionList.MaxItems;
                rprSelectionList.DataSource = selectionList;
                rprSelectionList.DataBind();
            }
            else
            {
                this.resultsPager.Visible = false;

                //TODO: This is a prime example of where the state pattern comes in.
                switch (this.FeedbackStatusFilter)
                {
                    case FeedbackStatusFlag.NeedsModeration:
                        noCommentsMessage.Text = "<em>No Entries Need Moderation.</em>";
                        break;

                    case FeedbackStatusFlag.FlaggedAsSpam:
                        noCommentsMessage.Text = "<em>No Entries Flagged as SPAM.</em>";
                        break;

                    case FeedbackStatusFlag.Deleted:
                        noCommentsMessage.Text = "<em>No Entries in the Trash.</em>";
                        break;

                    default:
                        Debug.Assert(this.FeedbackStatusFilter == FeedbackStatusFlag.Approved, "This is an impossible value for FeedbackStatusFilter '" + FeedbackStatusFilter + "'");
                        noCommentsMessage.Text = "<em>There are no approved comments to display.</em>";
                        break;
                }

                this.rprSelectionList.Controls.Clear();
                noCommentsMessage.Visible = true;
                
                this.btnDelete.Visible = false;
                this.btnApprove.Visible = false;
                if (this.FeedbackStatusFilter == FeedbackStatusFlag.Deleted)
                    this.btnDestroy.Visible = true;
                else
                    this.btnDestroy.Visible = false;
                this.btnConfirmSpam.Visible = false;
                this.btnEmpty.Visible = false;
            }
            BindCounts();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (btnViewApprovedComments != null)
                HtmlHelper.RemoveCssClass(this.btnViewApprovedComments, "active");
            if (btnViewModerateComments != null)
                HtmlHelper.RemoveCssClass(this.btnViewModerateComments, "active");
            if (btnViewSpam != null)
                HtmlHelper.RemoveCssClass(btnViewSpam, "active");
            if (btnViewTrash != null)
                HtmlHelper.RemoveCssClass(btnViewTrash, "active");

            switch (this.FeedbackStatusFilter)
            {
                case FeedbackStatusFlag.NeedsModeration:
                    if (btnViewModerateComments != null)
                        HtmlHelper.AppendCssClass(this.btnViewModerateComments, "active");
                    break;

                case FeedbackStatusFlag.FlaggedAsSpam:
                    if (btnViewSpam != null)
                        HtmlHelper.AppendCssClass(this.btnViewSpam, "active");
                    break;

                case FeedbackStatusFlag.Deleted:
                    if (btnViewTrash != null)
                        HtmlHelper.AppendCssClass(this.btnViewTrash, "active");
                    break;

                default:
                    Debug.Assert(this.FeedbackStatusFilter == FeedbackStatusFlag.Approved, "This is an impossible value for FeedbackStatusFilter '" + FeedbackStatusFilter + "'");
                    if (btnViewApprovedComments != null)
                        HtmlHelper.AppendCssClass(this.btnViewApprovedComments, "active");
                    break;
            }
            base.OnPreRender(e);
        }

        public string CheckHiddenStyle()
        {
            if (_isListHidden)
                return Constants.CSSSTYLE_HIDDEN;
            else
                return String.Empty;
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

        }
        #endregion

        protected void OnEmptyClick(object sender, EventArgs e)
        {
            FeedbackItem.Destroy(FeedbackStatusFilter);
            BindList();
        }

        /// <summary>
        /// Event handler for the approve button click event. 
        /// Approves the checked comments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnApproveClick(object sender, EventArgs e)
        {
            if (ApplyActionToCheckedFeedback(FeedbackItem.Approve) == 0)
            {
                Messages.ShowMessage("Nothing was selected to be approved.", true);
                return;
            }

            BindList();
        }

        /// <summary>
        /// Event handler for the Delete button Click event.  Deletes 
        /// the checked comments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDeleteClick(object sender, EventArgs e)
        {
            if (ApplyActionToCheckedFeedback(FeedbackItem.Delete) == 0)
            {
                Messages.ShowMessage("Nothing was selected to be deleted.", true);
                return;
            }
            BindList();
        }

        /// <summary>
        /// Called when the confirm spam button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void OnConfirmSpam(object sender, EventArgs e)
        {
            if (ApplyActionToCheckedFeedback(FeedbackItem.ConfirmSpam) == 0)
            {
                Messages.ShowMessage("Nothing was selected as spam.", true);
                return;
            }
            BindList();
        }

        /// <summary>
        /// Event handler for the Destroy button Click event.  Deletes 
        /// the checked comments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDestroyClick(object sender, EventArgs e)
        {
            if (ApplyActionToCheckedFeedback(FeedbackItem.Destroy) == 0)
            {
                Messages.ShowMessage("Nothing was selected to be destroyed.", true);
                return;
            }
            BindList();
        }

        private int ApplyActionToCheckedFeedback(FeedbackAction action)
        {
            int actionsApplied = 0;
            foreach (RepeaterItem item in this.rprSelectionList.Items)
            {
                // Get the checkbox from the item or the alternating item.
                CheckBox deleteCheck = item.FindControl("chkDelete") as CheckBox;
                if (deleteCheck == null)
                {
                    deleteCheck = item.FindControl("chkDeleteAlt") as CheckBox;
                }

                if (deleteCheck != null && deleteCheck.Checked)
                {
                    // Get the FeedbackId from the item or the alternating item.
                    HtmlInputHidden feedbackId = item.FindControl("FeedbackId") as HtmlInputHidden;
                    if (feedbackId == null)
                    {
                        feedbackId = item.FindControl("FeedbackIdAlt") as HtmlInputHidden;
                    }

                    int id;
                    if (feedbackId != null && int.TryParse(feedbackId.Value, out id))
                    {
                        FeedbackItem feedbackItem = FeedbackItem.Get(id);
                        if (feedbackItem != null)
                        {
                            actionsApplied++;
                            action(feedbackItem);
                        }
                    }
                }
            }
            return actionsApplied;
        }

        delegate void FeedbackAction(FeedbackItem feedback);

        protected void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "edit":
                    FeedbackID = Convert.ToInt32(e.CommandArgument);
                    BindFeedbackEdit();
                    break;

                default:
                    break;
            }
        }

        public int FeedbackID
        {
            get
            {
                if (ViewState[VSKEY_FEEDBACKID] != null)
                    return (int)ViewState[VSKEY_FEEDBACKID];
                else
                    return NullValue.NullInt32;
            }
            set { ViewState[VSKEY_FEEDBACKID] = value; }
        }
        protected void richTextEditor_Error(object sender, RichTextEditorErrorEventArgs e)
        {
            this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", e.Exception.Message), false);
        }

        private void SetConfirmation()
        {
            ConfirmationPage confirmPage = (ConfirmationPage)this.Page;
            confirmPage.IsInEdit = true;
            confirmPage.Message = "You will lose any unsaved content";

            this.lkbPost.Attributes.Add("OnClick", BypassFunctionName);
            this.lkbCancel.Attributes.Add("OnClick", BypassFunctionName);
        }

        private void BindFeedbackEdit()
        {
            FeedbackItem currentFeedback = FeedbackItem.Get(FeedbackID);
            if (currentFeedback == null)
            {
                Response.Redirect("Feedback.aspx");
                return;
            }
            SetConfirmation();
            rbFeedbackFilter.Visible = false;
            headerLiteral.Visible = false;
            rprSelectionList.Visible = false;
            Edit.Visible = true;
            rbFeedbackFilter.SelectedIndex = -1;
            lblName.Text = currentFeedback.Author;
            lblEmail.Text = currentFeedback.Email;
            if (currentFeedback.Email.Length > 0)
                hlAuthorEmail.NavigateUrl = "mailto:" + currentFeedback.Email;

            hlEntryLink.NavigateUrl = currentFeedback.DisplayUrl.ToString();
            hlEntryLink.Text = currentFeedback.DisplayUrl.ToString();
            if (currentFeedback.SourceUrl != null)
                txbWebsite.Text = currentFeedback.SourceUrl.ToString();


            txbTitle.Text = currentFeedback.Title;

            richTextEditor.Text = currentFeedback.Body;
        }
        protected void lkbPost_Click(object sender, EventArgs e)
        {
            UpdateFeedback();
        }


        private void UpdateFeedback()
        {
            Uri feedbackWebsite = null;
            if (txbWebsite.Text.Length > 0)
            {
                valtxbWebsite.IsValid = Uri.TryCreate(txbWebsite.Text, UriKind.RelativeOrAbsolute, out feedbackWebsite);
            }
            else
            {
                valtxbWebsite.IsValid = true;
            }

            if (Page.IsValid)
            {
                try
                {
                    FeedbackItem updatedFeedback = FeedbackItem.Get(FeedbackID);
                    updatedFeedback.Title = txbTitle.Text;
                    updatedFeedback.Body = richTextEditor.Text;
                    if (feedbackWebsite != null)
                        updatedFeedback.SourceUrl = feedbackWebsite;
                    //Plugins are not supported in this version
                    //FeedbackEventArgs e = new FeedbackEventArgs(updatedFeedback, ObjectState.Update);
                    //SubtextEvents.OnCommentUpdating(this, e);
                    FeedbackItem.Update(updatedFeedback);
                    //Plugins are not supported in this version
                    //SubtextEvents.OnCommentUpdated(this, new FeedbackEventArgs(updatedFeedback, ObjectState.Update));

                    if (ReturnToOriginalPost)
                    {
                        if (updatedFeedback != null)
                        {
                            Response.Redirect(updatedFeedback.DisplayUrl.ToString());
                            return;
                        }
                    }

                    this.Messages.ShowMessage(Constants.RES_SUCCESSEDIT, false);
                }
                finally
                {
                }
            }
        }

        protected void lkbCancel_Click(object sender, EventArgs e)
        {
            if (FeedbackID > -1 && ReturnToOriginalPost)
            {
                // We came from outside the post, let's go there.
                FeedbackItem updatedFeedback = FeedbackItem.Get(FeedbackID);
                if (updatedFeedback != null)
                {
                    Response.Redirect(updatedFeedback.DisplayUrl.ToString());
                    return;
                }
            }

            ResetFeedbackEdit(false);
        }

        private bool ReturnToOriginalPost
        {
            get
            {
                if (ViewState["ReturnToOriginalPost"] != null)
                    return (bool)ViewState["ReturnToOriginalPost"];
                return false;
            }
            set
            {
                ViewState["ReturnToOriginalPost"] = value;
            }
        }
        public void ResetFeedbackEdit(bool showEdit)
        {
            FeedbackID = NullValue.NullInt32;
            Edit.Visible = showEdit;
            headerLiteral.Visible = !showEdit;
            rbFeedbackFilter.Visible = !showEdit;
            rprSelectionList.Visible = true;
        }
    }
}





