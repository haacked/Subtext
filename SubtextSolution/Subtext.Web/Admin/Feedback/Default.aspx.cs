using System;
using System.Globalization;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.Feedback
{
    public partial class Default : ConfirmationPage
    {
        FeedbackStatusFlag feedbackStatusFilter;
        FeedbackState uiState;
        int pageIndex = 0;

        public Default()
        {
            this.TabSectionId = "Feedback";
        }

        protected override void OnLoad(EventArgs e)
        {
            feedbackStatusFilter = Master.FeedbackStatus;
            uiState = FeedbackState.GetUiState(feedbackStatusFilter);
            filterTypeDropDown.SelectedValue = Master.FeedbackType.ToString();
            
            BindUserInterface();
            if (!IsPostBack)
            {
                BindList();
            }
            base.OnLoad(e);
        }

        private void BindUserInterface()
        {
            this.headerLiteral.InnerText = uiState.HeaderText;
            this.btnApprove.Visible = uiState.Approvable;
            this.btnApprove.Text = uiState.ApproveText;
            this.btnDestroy.Visible = uiState.Destroyable;
            this.btnDelete.Visible = uiState.Deletable;
            this.btnDelete.ToolTip = uiState.DeleteToolTip;
            this.btnConfirmSpam.Visible = uiState.Spammable;
            this.btnEmpty.Visible = uiState.Emptyable;
            this.btnEmpty.ToolTip = uiState.EmptyToolTip;
        }

        private void BindList()
        {
            noCommentsMessage.Visible = false;
            if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

            this.resultsPager.UrlFormat = "Default.aspx?pg={0}&status=" + feedbackStatusFilter;
            this.resultsPager.PageSize = Preferences.ListingItemCount;
            this.resultsPager.PageIndex = this.pageIndex;

            FeedbackStatusFlag excludeFilter = ~feedbackStatusFilter;

            //Approved is a special case.  If a feedback has the approved bit set, 
            //it is approved no matter what other bits are set.
            if (feedbackStatusFilter == FeedbackStatusFlag.Approved)
                excludeFilter = FeedbackStatusFlag.None;

            //Likewise, deleted is a special case.  If a feedback has the deleted 
            //bit set, it is in the trash no matter what other bits are set.
            if (feedbackStatusFilter == FeedbackStatusFlag.Deleted)
                excludeFilter = FeedbackStatusFlag.Approved;

            IPagedCollection<FeedbackItem> selectionList = FeedbackItem.GetPagedFeedback(this.pageIndex
                , this.resultsPager.PageSize
                , feedbackStatusFilter
                , excludeFilter
                , Master.FeedbackType);

            if (selectionList.Count > 0)
            {
                resultsPager.Visible = true;

                resultsPager.ItemCount = selectionList.MaxItems;
                feedbackRepeater.DataSource = selectionList;
                feedbackRepeater.DataBind();
            }
            else
            {
                this.resultsPager.Visible = false;
                noCommentsMessage.Text = uiState.NoCommentsHtml;
                feedbackRepeater.Controls.Clear();
                noCommentsMessage.Visible = true;

                this.btnDelete.Visible = false;
                this.btnApprove.Visible = false;
                if (feedbackStatusFilter == FeedbackStatusFlag.Deleted)
                {
                    this.btnDestroy.Visible = true;
                }
                else
                {
                    this.btnDestroy.Visible = false;
                }
                this.btnConfirmSpam.Visible = false;
                this.btnEmpty.Visible = false;
            }
            Master.BindCounts();
        }

        protected new FeedbackMaster Master
        {
            get
            {
                return base.Master as FeedbackMaster;
            }
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
        protected string GetTitle(object dataItem)
        {
            FeedbackItem feedbackItem = (FeedbackItem)dataItem;
            string feedbackUrl = Url.FeedbackUrl(feedbackItem);
            if (!String.IsNullOrEmpty(feedbackUrl)) { 
                return string.Format(@"<a href=""{0}"" title=""{0}"">{1}</a>", feedbackUrl, feedbackItem.Title);
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
            string safeEmail = HttpUtility.HtmlAttributeEncode(feedback.Email);

            if (feedback.Email != null && feedback.Email.Length > 0 && feedback.Email.IndexOf("@") > 0)
            {
                string safeAuthor = StringHelper.MailToEncode(feedback.Author);
                string safeTitle = StringHelper.MailToEncode(feedback.Title);
                string safeBody = StringHelper.MailToBodyEncode(feedback.Body);

                string mailToUrl = safeEmail
                    + "&subject=re:" + safeTitle
                    + "&body=----------%0A"
                        + "From: " + StringHelper.MailToEncode(feedback.Author) + " (" + safeEmail + ")%0A"
                        + "Sent: " + StringHelper.MailToEncode(feedback.DateCreated.ToString()) + "%0A"
                        + "Subject: " + StringHelper.MailToEncode(safeTitle).Replace("+", " ") + "%0A%0A"
                        + safeBody;
                authorInfo += string.Format(@"<a href=""mailto:{0}"" title=""{1}""><img src=""{2}"" alt=""{1}"" border=""0"" class=""email"" /></a>", mailToUrl, safeEmail, HttpHelper.ExpandTildePath("~/images/email.gif"));
            }

            if (feedback.SourceUrl != null)
            {
                authorInfo += string.Format(@"<a href=""{0}"" title=""{1}""><img src=""{2}"" alt=""{1}"" border=""0"" /></a>", feedback.SourceUrl, safeEmail, HttpHelper.ExpandTildePath("~/images/permalink.gif"));
            }

            return authorInfo;
        }

        protected void OnEmptyClick(object sender, EventArgs e)
        {
            FeedbackItem.Destroy(feedbackStatusFilter);
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
            foreach (RepeaterItem item in feedbackRepeater.Items)
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
    }
}
