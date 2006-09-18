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
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Controls;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Displays comments posted to the blog and allows the 
	/// admin to delete comments.
	/// </summary>
	public partial class Feedback : AdminPage
	{
		private int pageIndex = 0;
		private bool _isListHidden = false;
		LinkButton btnViewActiveComments;
		LinkButton btnModerateComments;
		
		/// <summary>
		/// Constructs an image of this page. Sets the tab section to "Feedback".
		/// </summary>
	    public Feedback() : base()
	    {
            this.TabSectionId = "Feedback";
	    }
		
		/// <summary>
		/// Whether or not to moderate comments.
		/// </summary>
		protected bool ModerateComments
		{
			get
			{
				return ViewState["ModerateComments"] == null ? false : (bool)ViewState["ModerateComments"];
			}
			set
			{
				ViewState["ModerateComments"] = value;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Config.CurrentBlog.ModerationEnabled)
			{
				this.btnViewActiveComments = Utilities.CreateLinkButton("View Active Comments");
				this.btnViewActiveComments.ID = "btnViewActiveComments";
				this.btnViewActiveComments.CausesValidation = false;
				this.btnViewActiveComments.Click += OnViewActiveCommentsClick;
				AdminMasterPage.AddToActions(this.btnViewActiveComments);

				this.btnModerateComments = Utilities.CreateLinkButton("Moderate Comments");
				this.btnModerateComments.ID = "btnModerateComments";
				this.btnModerateComments.CausesValidation = false;
				this.btnModerateComments.Click += OnViewCommentsForModerationClick;
				AdminMasterPage.AddToActions(this.btnModerateComments);
			}
			
			if (!IsPostBack)
			{
				this.ModerateComments = Request.QueryString["moderate"] == "true";
				
				if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
					this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				this.resultsPager.PageSize = Preferences.ListingItemCount;
				this.resultsPager.PageIndex = this.pageIndex;
				Results.Collapsible = false;

				BindList();
			}			
		}

		void OnViewActiveCommentsClick(object sender, EventArgs e)
		{
			this.ModerateComments = false;
			this.Results.HeaderText = "Comments";
			HtmlHelper.AppendCssClass(this.btnViewActiveComments, "active");
			this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&moderate=false";
			BindList();
		}

		void OnViewCommentsForModerationClick(object sender, EventArgs e)
		{
			this.ModerateComments = true;
			this.Results.HeaderText = "Comments Pending Approval";
			HtmlHelper.AppendCssClass(this.btnModerateComments, "active");
			this.resultsPager.UrlFormat = "Feedback.aspx?pg={0}&moderate=true";
			BindList();
		}

		/// <summary>
		/// Gets the body of the feedback represented by the dataItem.
		/// </summary>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected string GetBody(object dataItem)
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
		protected string GetAuthor(object dataItem)
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
			if (feedbackItem.SourceUrl != null)
				return string.Format(@"<a href=""{0}"" title=""{0}"">{1}</a>", feedbackItem.SourceUrl, feedbackItem.Title);

			return feedbackItem.Title;
		}

		/// <summary>
		/// Returns the author during data binding. If the author specified 
		/// an email address, includes that.
		/// </summary>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected string GetAuthorInfo(object dataItem)
		{
			FeedbackItem feedback = (FeedbackItem)dataItem;
			string authorInfo = string.Empty;

			if (feedback.Email != null && feedback.Email.Length > 0 && feedback.Email.IndexOf("@") > 0)
			{
				authorInfo += string.Format(@"<a href=""mailto:{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" class=""email"" /></a>", feedback.Email, ControlHelper.ExpandTildePath("~/images/email.gif"));
			}

			if (feedback.SourceUrl == null)
			{
				authorInfo += string.Format(@"<a href=""{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" /></a>", feedback.SourceUrl, ControlHelper.ExpandTildePath("~/images/permalink.gif"));
			}

			return authorInfo;
		}

		private void BindList()
		{
			FeedbackStatusFlag statusFlag = this.ModerateComments ? FeedbackStatusFlag.NeedsModeration : FeedbackStatusFlag.None;
			IPagedCollection<FeedbackItem> selectionList = FeedbackItem.GetPagedFeedback(this.pageIndex, this.resultsPager.PageSize, statusFlag, FeedbackType.None);

			this.btnApprove.Visible = this.ModerateComments;
			
			if (selectionList.Count > 0)
			{
				this.resultsPager.Visible = true;
				this.resultsPager.ItemCount = selectionList.MaxItems;
				rprSelectionList.DataSource = selectionList;
				rprSelectionList.DataBind();
				this.btnDelete.Visible = true;
			}
			else
			{
				this.resultsPager.Visible = false;
				
				//No Comments To Show..
				Literal noComments = new Literal();
				if(this.ModerateComments)
					noComments.Text = "<em>No Entries Need Moderation.</em>";
				else
					noComments.Text = "<em>There are no comments to display.</em>";
				this.rprSelectionList.Controls.Clear();
				Results.Controls.Add(noComments);
				this.btnDelete.Visible = false;
			}
		}

		public string CheckHiddenStyle()
		{
			if (_isListHidden)
				return Constants.CSSSTYLE_HIDDEN;
			else
				return String.Empty;
		}

		private void ConfirmDeleteComment(IList<int> feedbackIds)
		{
			this.Command = new DeleteCommentsCommand(feedbackIds);
			this.Command.RedirectUrl = Request.Url.ToString();
			Server.Transfer(Constants.URL_CONFIRM);
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

		/// <summary>
		/// Event handler for the approve button click event. 
		/// Approves the checked comments.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnApproveClick(object sender, System.EventArgs e)
		{
			IList<int> itemsToDelete = GetCheckedFeedbackIds();

			if (itemsToDelete.Count == 0)
			{
				Messages.ShowMessage("Nothing was selected to be approved.", true);
				return;
			}

			foreach(int feedbackId in itemsToDelete)
			{
				FeedbackItem comment = FeedbackItem.Get(feedbackId);
				FeedbackItem.Approve(comment);
			}
			
			BindList();
		}
		
		/// <summary>
		/// Event handler for the Delete button Click event.  Deletes 
		/// the checked comments.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnDeleteClick(object sender, System.EventArgs e)
		{
			IList<int> itemsToDelete = GetCheckedFeedbackIds();

			if(itemsToDelete.Count == 0)
			{
				Messages.ShowMessage("Nothing was selected to be deleted.", true);
				return;
			}

			ConfirmDeleteComment(itemsToDelete);
		}

		private IList<int> GetCheckedFeedbackIds()
		{
			IList<int> feedbackIds = new List<int>();
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
					
					//Now add the item to the list of items to delete.
					if(feedbackId != null && feedbackId.Value != null && feedbackId.Value.Length > 0)
					{
						try
						{
							feedbackIds.Add(int.Parse(feedbackId.Value));
						}
						catch(System.FormatException)
						{
							//Swallow this one.
						}
					}
				}
			}
			return feedbackIds;
		}
	}
}

