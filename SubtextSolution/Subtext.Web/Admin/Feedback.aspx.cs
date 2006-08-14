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
		/// Gets the body of the entry represented by the dataItem.
		/// </summary>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected string GetBody(object dataItem)
		{
			Entry entry = (Entry)dataItem;
			if(entry.PostType == PostType.Comment)
			{
				return entry.Body;
			}
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}<br /><a target=\"_blank\" title=\"view: {1}\"  href=\"{2}\">Pingback/TrackBack</a>", entry.Body, entry.Title, entry.TitleUrl);
		}

		/// <summary>
		/// Returns the author during data binding. If the author specified 
		/// an email address, includes that.
		/// </summary>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected string GetAuthor(object dataItem)
		{
			Entry entry = (Entry)dataItem;
			return string.Format(@"<span title=""{0}"">{1}</span>", entry.SourceName, entry.Author);
		}

		/// <summary>
		/// Returns the author during data binding. If the author specified 
		/// an email address, includes that.
		/// </summary>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected string GetAuthorInfo(object dataItem)
		{
			Entry entry = (Entry)dataItem;
			string authorInfo = string.Empty;

			if (entry.Email != null && entry.Email.Length > 0 && entry.Email.IndexOf("@") > 0)
			{
				authorInfo += string.Format(@"<a href=""mailto:{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" class=""email"" /></a>", entry.Email, ControlHelper.ExpandTildePath("~/images/email.gif"));
			}


			if (!String.IsNullOrEmpty(entry.SourceUrl))
			{
				authorInfo += string.Format(@"<a href=""{0}"" title=""{0}""><img src=""{1}"" alt=""{0}"" border=""0"" /></a>", entry.SourceUrl, ControlHelper.ExpandTildePath("~/images/permalink.gif"));
			}

			return authorInfo;
		}

		private void BindList()
		{
			PostConfig postConfig = this.ModerateComments ? PostConfig.NeedsModeratorApproval : PostConfig.IsActive;
			IPagedCollection<Entry> selectionList = Entries.GetPagedFeedback(this.pageIndex, this.resultsPager.PageSize, postConfig);

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
			IList<int> itemsToDelete = GetCheckedEntryIds();

			if (itemsToDelete.Count == 0)
			{
				Messages.ShowMessage("Nothing was selected to be approved.", true);
				return;
			}

			foreach(int entryId in itemsToDelete)
			{
				Entry comment = Entries.GetEntry(entryId, PostConfig.None, false);
				Entries.Approve(comment);
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
			IList<int> itemsToDelete = GetCheckedEntryIds();

			if(itemsToDelete.Count == 0)
			{
				Messages.ShowMessage("Nothing was selected to be deleted.", true);
				return;
			}

			ConfirmDeleteComment(itemsToDelete);
		}

		private IList<int> GetCheckedEntryIds()
		{
			IList<int> entryIds = new List<int>();
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
					// Get the EntryId from the item or the alternating item.
					HtmlInputHidden entryId = item.FindControl("EntryID") as HtmlInputHidden;
					if(entryId == null)
					{
						entryId = item.FindControl("EntryIDAlt") as HtmlInputHidden;
					}
					
					//Now add the item to the list of items to delete.
					if(entryId != null && entryId.Value != null && entryId.Value.Length > 0)
					{
						try
						{
							entryIds.Add(int.Parse(entryId.Value));
						}
						catch(System.FormatException)
						{
							//Swallow this one.
						}
					}
				}
			}
			return entryIds;
		}
	}
}

