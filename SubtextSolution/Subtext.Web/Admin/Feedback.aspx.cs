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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Controls;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Displays comments posted to the blog and allows the 
	/// admin to delete comments.
	/// </summary>
	public partial class Feedback : AdminPage
	{
		private int pageIndex;
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
		protected FeedbackStatusFlags FeedbackStatusFilter
		{
			get
			{
				return (FeedbackStatusFlags)(ViewState["FeedbackStatusFilter"] ?? FeedbackStatusFlags.Approved);
			}
			set
			{
				ViewState["FeedbackStatusFilter"] = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnViewApprovedComments = AddFolderLink("Approved", "btnViewActiveComments", "Approved Comments", OnViewApprovedCommentsClick);
			this.btnViewModerateComments = AddFolderLink("Moderate", "btnModerateComments", "Comments in need of moderation", OnViewCommentsForModerationClick);
			this.btnViewModerateComments.Enabled = Config.CurrentBlog.ModerationEnabled;
			this.btnViewSpam = AddFolderLink("Flagged Spam", "btnViewSpam", "Comments Flagged As Spam By Filters", OnViewSpamClick);
			this.btnViewTrash = AddFolderLink("Trash", "btnViewTrash", "Comments In The Trash Bin (Confirmed Spam or Deleted Items)", OnViewTrashClick);
			
			if (!IsPostBack)
			{
				this.FeedbackStatusFilter = GetStatusFromQueryString();
				//this.cbShowOnlyComments.Checked = Preferences.FeedbackShowOnlyComments;
				this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
				BindList();
			}
		}
		
		FeedbackStatusFlags GetStatusFromQueryString()
		{
			string filter = Request.QueryString["status"] ?? "1";
			int filterId;
			if(!int.TryParse(filter, out filterId))
			{
				return FeedbackStatusFlags.Approved;
			}
			return (FeedbackStatusFlags)filterId;
		}

		private LinkButton AddFolderLink(string label, string id, string title, EventHandler handler)
		{
			LinkButton button = Utilities.CreateLinkButton(label);
			button.ID = id;
			button.CausesValidation = false;
			button.Click += handler;
			button.Attributes["title"] = title;
			AdminMasterPage.AddToActions(button);
			return button;
		}

		void OnViewApprovedCommentsClick(object sender, EventArgs e)
		{
            if (this.FeedbackStatusFilter != FeedbackStatusFlags.Approved)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlags.Approved;
                this._hasViewChanged = true;
            }

			this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
			this.Results.HeaderText = "Comments";
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
            if (this.FeedbackStatusFilter != FeedbackStatusFlags.NeedsModeration)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlags.NeedsModeration;
                this._hasViewChanged = true;
            }
			
			this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
			this.Results.HeaderText = "Comments Pending Moderator Approval";
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
            if (this.FeedbackStatusFilter != FeedbackStatusFlags.FlaggedAsSpam)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlags.FlaggedAsSpam;
                this._hasViewChanged = true;
            }
			
			this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
			HtmlHelper.AppendCssClass(this.btnViewSpam, "active");
			this.Results.HeaderText = "Comments Flagged As SPAM";
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
            if (this.FeedbackStatusFilter != FeedbackStatusFlags.Deleted)
            {
                this.FeedbackStatusFilter = FeedbackStatusFlags.Deleted;
                this._hasViewChanged = true;
            }
			this.rbFeedbackFilter.SelectedValue = Preferences.GetFeedbackItemFilter(FeedbackStatusFilter);
			HtmlHelper.AppendCssClass(this.btnViewTrash, "active");
			this.Results.HeaderText = "Comments In The Trash Bin";
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
			if(feedbackItem.FeedbackType != FeedbackType.PingTrack)
			{
				return feedbackItem.Body;
			}
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}<br /><a target=\"_blank\" title=\"view: {1}\"  href=\"{2}\">Pingback/TrackBack</a>", feedbackItem.Body, feedbackItem.Title, feedbackItem.SourceUrl);
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
				authorInfo += string.Format(@"<a href=""mailto:{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" class=""email"" /></a>", feedback.Email, HttpHelper.ExpandTildePath("~/images/email.gif"));
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
			if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
				this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

            if (this._hasViewChanged)
                this.pageIndex = 0;

			this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&status=" + (int)FeedbackStatusFilter;
			this.resultsPager.PageSize = Preferences.ListingItemCount;
			this.resultsPager.PageIndex = this.pageIndex;
			Results.Collapsible = false;
			
			FeedbackStatusFlags excludeFilter = ~this.FeedbackStatusFilter;
			//Approved is a special case.  If a feedback has the approved bit set, 
			//it is approved no matter what other bits are set.
			if (this.FeedbackStatusFilter == FeedbackStatusFlags.Approved)
				excludeFilter = FeedbackStatusFlags.None;

			//Likewise, deleted is a special case.  If a feedback has the deleted 
			//bit set, it is in the trash no matter what other bits are set.
			if (this.FeedbackStatusFilter == FeedbackStatusFlags.Deleted)
				excludeFilter = FeedbackStatusFlags.Approved;

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
				
				//No Comments To Show..
				Literal noComments = new Literal();
				
				//TODO: This is a prime example of where the state pattern comes in.
				switch(this.FeedbackStatusFilter)
				{
					case FeedbackStatusFlags.NeedsModeration:
						noComments.Text = "<em>No Entries Need Moderation.</em>";
						break;
						
					case FeedbackStatusFlags.FlaggedAsSpam:
						noComments.Text = "<em>No Entries Flagged as SPAM.</em>";
						break;
						
					case FeedbackStatusFlags.Deleted:
						noComments.Text = "<em>No Entries in the Trash.</em>";
						break;
					
					default:
						Debug.Assert(this.FeedbackStatusFilter == FeedbackStatusFlags.Approved, "This is an impossible value for FeedbackStatusFilter '" + FeedbackStatusFilter + "'");
						noComments.Text = "<em>There are no approved comments to display.</em>";
						break;
				}
				
				this.rprSelectionList.Controls.Clear();
				Results.Controls.Add(noComments);
				this.btnDelete.Visible = false;
				this.btnApprove.Visible = false;
                if (this.FeedbackStatusFilter == FeedbackStatusFlags.Deleted)
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
			HtmlHelper.RemoveCssClass(this.btnViewApprovedComments, "active");
			HtmlHelper.RemoveCssClass(this.btnViewModerateComments, "active");
			HtmlHelper.RemoveCssClass(btnViewSpam, "active");
			HtmlHelper.RemoveCssClass(btnViewTrash, "active");
			
			switch(this.FeedbackStatusFilter)
			{
				case FeedbackStatusFlags.NeedsModeration:
					HtmlHelper.AppendCssClass(this.btnViewModerateComments, "active");
					break;
					
				case FeedbackStatusFlags.FlaggedAsSpam:
					HtmlHelper.AppendCssClass(this.btnViewSpam, "active");
					break;
					
				case FeedbackStatusFlags.Deleted:
					HtmlHelper.AppendCssClass(this.btnViewTrash, "active");
					break;
				
				default:
					Debug.Assert(this.FeedbackStatusFilter == FeedbackStatusFlags.Approved, "This is an impossible value for FeedbackStatusFilter '" + FeedbackStatusFilter + "'");
					HtmlHelper.AppendCssClass(this.btnViewApprovedComments, "active");
					break;
			}
 			 base.OnPreRender(e);
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
			foreach(RepeaterItem item in this.rprSelectionList.Items)
			{
				// Get the checkbox from the item or the alternating item.
				CheckBox deleteCheck = item.FindControl("chkDelete") as CheckBox;
				if(deleteCheck == null)
				{
					deleteCheck = item.FindControl("chkDeleteAlt") as CheckBox;
				}

				if(deleteCheck != null && deleteCheck.Checked)
				{
					// Get the FeedbackId from the item or the alternating item.
					HtmlInputHidden feedbackId = item.FindControl("FeedbackId") as HtmlInputHidden;
					if(feedbackId == null)
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

