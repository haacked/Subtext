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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Properties;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.Admin.Feedback
{
    public partial class Default : ConfirmationPage
    {
        FeedbackStatusFlag _feedbackStatusFilter;
        int _pageIndex;
        FeedbackState _uiState;

        public Default()
        {
            TabSectionId = "Feedback";
        }

        protected new FeedbackMaster Master
        {
            get { return base.Master as FeedbackMaster; }
        }

        public FeedbackState FeedbackState
        {
            get
            {
                return _uiState;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            _feedbackStatusFilter = Master.FeedbackStatus;
            _uiState = FeedbackState.GetUiState(_feedbackStatusFilter);
            filterTypeDropDown.SelectedValue = Master.FeedbackType.ToString();

            BindUserInterface();
            if (!IsPostBack)
            {
                if (!Contact.ShowContactMessages)
                {
                    filterTypeDropDown.Items.RemoveAt(3);
                }

                BindList();
            }
            base.OnLoad(e);
        }

        private void BindUserInterface()
        {
            headerLiteral.InnerText = _uiState.HeaderText;
            btnEmpty.Visible = _uiState.Emptyable;
            btnEmpty.ToolTip = _uiState.EmptyToolTip;
        }

        private void BindList()
        {
            noCommentsMessage.Visible = false;
            if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
            {
                _pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            resultsPager.UrlFormat = "Default.aspx?pg={0}&status=" + _feedbackStatusFilter;
            resultsPager.PageSize = Preferences.ListingItemCount;
            resultsPager.PageIndex = _pageIndex;

            // Deleted is a special case.  If a feedback has the deleted 
            // bit set, it is in the trash no matter what other bits are set.
            FeedbackStatusFlag excludeFilter = _feedbackStatusFilter == FeedbackStatusFlag.Deleted ? FeedbackStatusFlag.None : FeedbackStatusFlag.Deleted;

            IPagedCollection<FeedbackItem> selectionList = Repository.GetPagedFeedback(_pageIndex
                                                                                       , resultsPager.PageSize
                                                                                       , _feedbackStatusFilter
                                                                                       , excludeFilter
                                                                                       , Master.FeedbackType);

            if (selectionList.Count > 0)
            {
                resultsPager.Visible = true;

                resultsPager.ItemCount = selectionList.MaxItems;
                feedbackRepeater.DataSource = selectionList;
                feedbackRepeater.ItemCreated += FeedbackItemDataBound;
                feedbackRepeater.DataBind();
            }
            else
            {
                resultsPager.Visible = false;
                noCommentsMessage.Text = _uiState.NoCommentsHtml;
                feedbackRepeater.Controls.Clear();
                noCommentsMessage.Visible = true;

                btnEmpty.Visible = false;
            }
            Master.BindCounts();
        }

        void FeedbackItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Author = new FeedbackAuthorViewModel(e.Item.DataItem);
            }
        }

        protected FeedbackAuthorViewModel Author
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the body of the feedback represented by the dataItem.
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected static string GetBody(object dataItem)
        {
            var feedbackItem = (FeedbackItem)dataItem;
            if (feedbackItem.FeedbackType != FeedbackType.PingTrack)
            {
                return feedbackItem.Body;
            }
            return string.Format(CultureInfo.InvariantCulture,
                                 "{0}<br /><a target=\"_blank\" title=\"{3}: {1}\"  href=\"{2}\">Pingback/TrackBack</a>",
                                 feedbackItem.Body, feedbackItem.Title, feedbackItem.SourceUrl, Resources.Label_View);
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <param name="dataItem">The data item.</param>
        /// <returns></returns>
        protected string GetTitle(object dataItem)
        {
            var feedbackItem = (FeedbackItem)dataItem;
            string feedbackUrl = Url.FeedbackUrl(feedbackItem);
            if (!String.IsNullOrEmpty(feedbackUrl))
            {
                return string.Format(@"<a href=""{0}"" title=""{0}"">{1}</a>", feedbackUrl, feedbackItem.Title);
            }

            return feedbackItem.Title;
        }

        protected void OnEmptyClick(object sender, EventArgs e)
        {
            Repository.Destroy(_feedbackStatusFilter);
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
            if (ApplyActionToCheckedFeedback(Repository.Approve) == 0)
            {
                Messages.ShowMessage(Resources.Feedback_NothingToApprove, true);
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
            if (ApplyActionToCheckedFeedback((item, service) => Repository.Delete(item)) == 0)
            {
                Messages.ShowMessage(Resources.Feedback_NothingToDelete, true);
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
            if (ApplyActionToCheckedFeedback(Repository.ConfirmSpam) == 0)
            {
                Messages.ShowMessage(Resources.Feedback_NothingFlaggedAsSpam, true);
                return;
            }
            BindList();
        }

        private int ApplyActionToCheckedFeedback(Action<FeedbackItem, ICommentSpamService> action)
        {
            ICommentSpamService feedbackService = null;
            if (Blog.FeedbackSpamServiceEnabled)
            {
                feedbackService = new AkismetSpamService(Config.CurrentBlog.FeedbackSpamServiceKey, Config.CurrentBlog,
                                                         null, Url);
            }

            int actionsApplied = 0;
            foreach (RepeaterItem item in feedbackRepeater.Items)
            {
                // Get the checkbox from the item or the alternating item.
                var deleteCheck = item.FindControl("chkDelete") as CheckBox ?? item.FindControl("chkDeleteAlt") as CheckBox;

                if (deleteCheck != null && deleteCheck.Checked)
                {
                    // Get the FeedbackId from the item or the alternating item.
                    var feedbackId = item.FindControl("FeedbackId") as HtmlInputHidden ?? item.FindControl("FeedbackIdAlt") as HtmlInputHidden;

                    int id;
                    if (feedbackId != null && int.TryParse(feedbackId.Value, out id))
                    {
                        FeedbackItem feedbackItem = Repository.Get(id);
                        if (feedbackItem != null)
                        {
                            actionsApplied++;

                            action(feedbackItem, feedbackService);
                        }
                    }
                }
            }
            return actionsApplied;
        }
    }
}