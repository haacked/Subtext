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
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace Subtext.Web.Admin.Pages
{
	// TODO: import - reconcile duplicates
	// TODO: CheckAll client-side, confirm bulk delete (add cmd)

	public partial class EditKeyWords : AdminOptionsPage
	{
		private const string VSKEY_KEYWORDID = "LinkID";

		private int _resultsPageNumber = 0;
		private bool _isListHidden = false;
	
		#region Accessors

		public int KeyWordID
		{
			get
			{
				if(ViewState[VSKEY_KEYWORDID] != null)
					return (int)ViewState[VSKEY_KEYWORDID];
				else
					return NullValue.NullInt32;
			}
			set { ViewState[VSKEY_KEYWORDID] = value; }
		}
	
		#endregion

		private new void Page_Load(object sender, System.EventArgs e)
		{
			//BindLocalUI(); //no need to call

			if (!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				this.resultsPager.PageSize = Preferences.ListingItemCount;
				this.resultsPager.PageIndex = _resultsPageNumber;
				Results.Collapsible = false;

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

            IPagedCollection<KeyWord> selectionList = KeyWords.GetPagedKeyWords(_resultsPageNumber, this.resultsPager.PageSize);
			
			if (selectionList.Count > 0)
			{
				this.resultsPager.ItemCount = selectionList.MaxItems;
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
			KeyWord kw = KeyWords.GetKeyWord(KeyWordID);
		
			Results.Collapsed = true;
			Results.Collapsible = true;
			Edit.Visible = true;

			txbTitle.Text = kw.Title;
			txbUrl.Text = kw.Url;
			txbWord.Text = kw.Word;
			txbRel.Text = kw.Rel;
			txbText.Text = kw.Text;
			
		
			chkNewWindow.Checked = kw.OpenInNewWindow;
			chkFirstOnly.Checked = kw.ReplaceFirstTimeOnly;
			chkCaseSensitive.Checked = kw.CaseSensitive;

            if(AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
			{	
				string title = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Editing KeyWord \"{0}\"", kw.Title);

				AdminMasterPage.BreadCrumb.AddLastItem(title);
				AdminMasterPage.Title = title;
			}
		}


		private void UpdateLink()
		{					
			string successMessage = Constants.RES_SUCCESSNEW;

			try
			{
				KeyWord kw = new KeyWord();

				

				kw.Title = txbTitle.Text;				
				kw.Url = txbUrl.Text;
				kw.Text = txbText.Text;
				kw.OpenInNewWindow = chkNewWindow.Checked;
				kw.ReplaceFirstTimeOnly = chkFirstOnly.Checked;
				kw.CaseSensitive = chkCaseSensitive.Checked;
				kw.Rel = txbRel.Text;
				kw.Word = txbWord.Text;
				
				if (KeyWordID > 0)
				{
					successMessage = Constants.RES_SUCCESSEDIT;
					kw.Id = KeyWordID;
					KeyWords.UpdateKeyWord(kw);
				}
				else
				{
					KeyWordID = KeyWords.CreateKeyWord(kw);
				}

				if (KeyWordID > 0)
				{			
					BindList();
					this.Messages.ShowMessage(successMessage);
				}
				else
					this.Messages.ShowError(Constants.RES_FAILUREEDIT 
						+ " There was a baseline problem posting your KeyWord.");
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, 
					Constants.RES_FAILUREEDIT, ex.Message));
			}
			finally
			{
				Results.Collapsible = false;
			}
		}

		private void ResetPostEdit(bool showEdit)
		{
			KeyWordID = NullValue.NullInt32;

			Results.Collapsible = showEdit;
			Results.Collapsed = showEdit;
			Edit.Visible = showEdit;

			
			txbTitle.Text = string.Empty;
			txbText.Text = string.Empty;
			txbUrl.Text = string.Empty;
			txbRel.Text = string.Empty;
			txbWord.Text = string.Empty;
			chkNewWindow.Checked = false;
			chkFirstOnly.Checked = false;
			chkCaseSensitive.Checked = false;

		}

		private void ConfirmDelete(int kwID, string kwWord)
		{
			this.Command = new DeleteKeyWordCommand(kwID, kwWord);
			this.Command.RedirectUrl = Request.Url.ToString();
			Server.Transfer(Constants.URL_CONFIRM);
		}

		// REFACTOR
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion 


		protected void rprSelectionList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			switch (e.CommandName.ToLower(System.Globalization.CultureInfo.InvariantCulture)) 
			{
				case "edit" :
					KeyWordID = Convert.ToInt32(e.CommandArgument);
					BindLinkEdit();
					break;
				case "delete" :
					int id = Convert.ToInt32(e.CommandArgument);
					KeyWord kw = KeyWords.GetKeyWord(id);
					ConfirmDelete(id, kw.Word);
					break;
				default:
					break;
			}			
		}

		protected void lkbCancel_Click(object sender, System.EventArgs e)
		{
			ResetPostEdit(false);
		}

		protected void lkbPost_Click(object sender, System.EventArgs e)
		{
			UpdateLink();
		}

		protected void btnCreate_Click(object sender, System.EventArgs e)
		{
			ResetPostEdit(true);
		}
	}
}

