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
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Displays comments posted to the blog and allows the 
	/// admin to delete comments.
	/// </summary>
	public class Feedback : AdminPage
	{
		private int _resultsPageNumber = 1;
		private bool _isListHidden = false;
		protected System.Web.UI.WebControls.Repeater rprSelectionList;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected Subtext.Web.Admin.WebUI.Pager ResultsPager;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
	

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				ResultsPager.PageSize = Preferences.ListingItemCount;
				ResultsPager.PageIndex = _resultsPageNumber;
				Results.Collapsible = false;
				
				BindList();
			}			
		}

		protected string GetBody(object dataItem)
		{
			Entry entry = (Entry)dataItem;
			if(entry.PostType == PostType.Comment)
			{
				return entry.Body;
			}
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}<br /><a target=\"_blank\" title=\"view: {1}\"  href=\"{2}\">Pingback/TrackBack</a>", entry.Body,entry.Title, entry.TitleUrl);
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
			if(entry.Email != null && entry.Email.Length > 0 && entry.Email.IndexOf("@") > 0)
			{
				return string.Format("<a href=\"mailto:{0}\" title=\"Email Address\">{1}</a>", entry.Email, entry.Author);
			}
			else
			{
				return entry.Author;
			}
		}

		private void BindList()
		{
            IPagedCollection<Entry> selectionList = Entries.GetPagedFeedback(_resultsPageNumber, ResultsPager.PageSize, true);		

			if (selectionList.Count > 0)
			{
				ResultsPager.ItemCount = selectionList.MaxItems;
				rprSelectionList.DataSource = selectionList;
				rprSelectionList.DataBind();
			}
			else
			{
				//No Comments To Show..
				Literal noComments = new Literal();
				noComments.Text = "<em>There are no comments to display.</em>";
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

		private void ConfirmDeleteComment(int[] feedbackIDs)
		{
			this.Command = new DeleteCommentsCommand(feedbackIDs);
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
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			ArrayList itemsToDelete = new ArrayList();
			
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
							itemsToDelete.Add(int.Parse(entryId.Value));
						}
						catch(System.FormatException)
						{
							//Swallow this one.
						}
					}
				}
			}
			
			if(itemsToDelete.Count == 0)
			{
				Messages.ShowMessage("Nothing was selected to be deleted.", true);
				return;
			}

			ConfirmDeleteComment((int[])itemsToDelete.ToArray(typeof(int)));
		}
	}
}

