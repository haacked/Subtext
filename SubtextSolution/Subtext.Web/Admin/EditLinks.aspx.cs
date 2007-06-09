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
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	// TODO: import - reconcile duplicates
	// TODO: CheckAll client-side, confirm bulk delete (add cmd)

	public partial class EditLinks : AdminPage
	{
		private const string VSKEY_LINKID = "LinkID";

		private int filterCategoryID
		{
			get
			{
				if(ViewState["filterCategoryID"] == null)
				{
					return NullValue.NullInt32;
				}
				else
				{
					return (int)ViewState["filterCategoryID"];
				}
			}
			set
			{
				ViewState["filterCategoryID"] = value;
			}
		}
		private int resultsPageNumber;
	
		#region Accessors
		public int LinkId
		{
			get
			{
				if(ViewState[VSKEY_LINKID] != null)
					return (int)ViewState[VSKEY_LINKID];
				else
					return NullValue.NullInt32;
			}
			set { ViewState[VSKEY_LINKID] = value; }
		}
	
		#endregion
	    
	    public EditLinks()
	    {
            this.TabSectionId = "Links";
	    }

		protected void Page_Load(object sender, EventArgs e)
		{
			BindLocalUI();

			if (!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
					this.resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				if (null != Request.QueryString[Keys.QRYSTR_CATEGORYID])
					this.filterCategoryID = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);

				this.resultsPager.PageSize = Preferences.ListingItemCount;
				this.resultsPager.PageIndex = this.resultsPageNumber;
				Results.Collapsible = false;

				if (NullValue.NullInt32 != this.filterCategoryID)
					this.resultsPager.UrlFormat += string.Format(CultureInfo.InvariantCulture, "&{0}={1}", Keys.QRYSTR_CATEGORYID, 
						this.filterCategoryID);
				
				BindList();
			}
		}

		private void BindLocalUI()
		{
			LinkButton lkbNewLink = Utilities.CreateLinkButton("New Link");
			lkbNewLink.Click += new EventHandler(lkbNewLink_Click);
			lkbNewLink.CausesValidation =false;
			AdminMasterPage.AddToActions(lkbNewLink);
            HyperLink lnkEditCategories = Utilities.CreateHyperLink("Edit Categories",
                string.Format(CultureInfo.InvariantCulture, "{0}?{1}={2}", Constants.URL_EDITCATEGORIES, Keys.QRYSTR_CATEGORYTYPE, CategoryType.LinkCollection));
            AdminMasterPage.AddToActions(lnkEditCategories);
		}

		private void BindList()
		{
			Edit.Visible = false;

            IPagedCollection<Link> selectionList = Links.GetPagedLinks(this.filterCategoryID, this.resultsPageNumber, this.resultsPager.PageSize, true);
			
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
			Link currentLink = Links.GetSingleLink(LinkId);
		
			Results.Collapsed = true;
			Results.Collapsible = true;
//			ImportExport.Visible = false;
			Edit.Visible = true;

			lblLinkId.Visible = true;
			lblEntryID.Text = currentLink.Id.ToString(CultureInfo.InvariantCulture);
			txbTitle.Text = currentLink.Title;
			txbUrl.Text = currentLink.Url;
			txbRss.Text = currentLink.Rss;
		
			chkNewWindow.Checked = currentLink.NewWindow;
			ckbIsActive.Checked = currentLink.IsActive;

			BindLinkCategories();
			ddlCategories.Items.FindByValue(currentLink.CategoryID.ToString(CultureInfo.InvariantCulture)).Selected = true;

			if(AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
			{	
				string title = string.Format(CultureInfo.InvariantCulture, "Editing Link \"{0}\"", currentLink.Title);

				AdminMasterPage.BreadCrumb.AddLastItem(title);
                AdminMasterPage.Title = title;
			}
		}

		public void BindLinkCategories()
		{
            ICollection<LinkCategory> selectionList = Links.GetCategories(CategoryType.LinkCollection, ActiveFilter.None);
			if(selectionList != null && selectionList.Count != 0)
			{
				ddlCategories.DataSource = selectionList;
				ddlCategories.DataValueField = "Id";
				ddlCategories.DataTextField = "Title";
				ddlCategories.DataBind();

				if (!NullValue.IsNull(filterCategoryID))
					ddlCategories.SelectedValue = filterCategoryID.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				this.Messages.ShowError("You need to add a category before you can add links! Click \"Edit Categories\"");
				Edit.Visible = false;
			}

		}

		private void UpdateLink()
		{					
			string successMessage = Constants.RES_SUCCESSNEW;

			try
			{
				Link link = new Link();

				link.Title = txbTitle.Text;				
				link.Url = txbUrl.Text;
				link.Rss = txbRss.Text;
				link.IsActive = ckbIsActive.Checked;
				link.CategoryID = Convert.ToInt32(ddlCategories.SelectedItem.Value);
				link.NewWindow = chkNewWindow.Checked;
				link.Id = Config.CurrentBlog.Id;
				
				if (LinkId > 0)
				{
					successMessage = Constants.RES_SUCCESSEDIT;
					link.Id = LinkId;
					Links.UpdateLink(link);
				}
				else
				{
					LinkId = Links.CreateLink(link);
				}

				if (LinkId > 0)
				{			
					BindList();
					this.Messages.ShowMessage(successMessage);
				}
				else
					this.Messages.ShowError(Constants.RES_FAILUREEDIT 
						+ " There was a baseline problem posting your link.");
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
			LinkId = NullValue.NullInt32;

			Results.Collapsible = showEdit;
			Results.Collapsed = showEdit;
			//ImportExport.Visible = !showEdit;
			Edit.Visible = showEdit;

			lblLinkId.Visible = false;
			lblEntryID.Text = String.Empty;
			txbTitle.Text = String.Empty;
			txbUrl.Text = String.Empty;
			txbRss.Text = String.Empty;
			chkNewWindow.Checked = false;

			ckbIsActive.Checked = Preferences.AlwaysCreateIsActive;

			ddlCategories.SelectedIndex = -1;

			if (showEdit)
				BindLinkCategories();
		}

		private void ConfirmDelete(int linkID, string linkTitle)
		{
			this.Command = new DeleteLinkCommand(linkID, linkTitle);
			this.Command.RedirectUrl = Request.Url.ToString();
			Server.Transfer(Constants.URL_CONFIRM);
		}

		private void ImportOpml()
		{
			if(OpmlImportFile.PostedFile.FileName.Trim().Length > 0)
			{
				OpmlItemCollection importedLinks = OpmlProvider.Import(OpmlImportFile.PostedFile.InputStream);
				
				if (importedLinks.Count > 0)
				{
					this.Command = new ImportLinksCommand(importedLinks,Int32.Parse(this.ddlImportExportCategories.SelectedItem.Value));
					this.Command.RedirectUrl = Request.Url.ToString();
					Server.Transfer(Constants.URL_CONFIRM);
				}

				BindList();
			}
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
			this.rprSelectionList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rprSelectionList_ItemCommand);

		}
		#endregion 

		protected void lkbImportOpml_Click(object sender, EventArgs e)
		{
			if (Page.IsValid) ImportOpml();
		}

		private void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			switch (e.CommandName.ToLower(CultureInfo.InvariantCulture)) 
			{
				case "edit" :
					LinkId = Convert.ToInt32(e.CommandArgument);
					BindLinkEdit();
					break;
				case "delete" :
					int id = Convert.ToInt32(e.CommandArgument);
					Link link = Links.GetSingleLink(id);
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

