using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Web.Admin.Pages;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

namespace Subtext.Web.Admin.UserControls
{
	using System;

	public class EntryEditor : System.Web.UI.UserControl
	{
		private const string VSKEY_POSTID = "PostID";
		private const string VSKEY_CATEGORYTYPE = "CategoryType";

		private int _filterCategoryID = Constants.NULL_CATEGORYID;
		private int _resultsPageNumber = 1;
		private PostType _entryType = PostType.BlogPost;
		private bool _isListHidden = false;

		#region Declared Controls
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected System.Web.UI.WebControls.Repeater rprSelectionList;
		protected System.Web.UI.HtmlControls.HtmlGenericControl NoMessagesLabel;
		protected Subtext.Web.Admin.WebUI.Pager ResultsPager;
		protected System.Web.UI.WebControls.HyperLink hlEntryLink;
		protected System.Web.UI.WebControls.TextBox txbTitle;
		protected System.Web.UI.WebControls.TextBox txbBody;
		protected System.Web.UI.WebControls.Button Post;
		protected System.Web.UI.WebControls.TextBox txbExcerpt;
		protected System.Web.UI.WebControls.TextBox txbTitleUrl;
		protected System.Web.UI.WebControls.TextBox Textbox1;
		protected System.Web.UI.WebControls.TextBox Textbox2;
		protected System.Web.UI.WebControls.CheckBox ckbPublished;
		protected System.Web.UI.WebControls.CheckBox chkComments;
		protected System.Web.UI.WebControls.CheckBox chkDisplayHomePage;
		protected System.Web.UI.WebControls.CheckBox chkMainSyndication;
		protected System.Web.UI.WebControls.CheckBox chkSyndicateDescriptionOnly;
		protected System.Web.UI.WebControls.CheckBox chkIsAggregated;

		protected System.Web.UI.WebControls.CheckBoxList cklCategories;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Advanced;
		protected System.Web.UI.WebControls.TextBox txbSourceName;
		protected System.Web.UI.WebControls.TextBox txbSourceUrl;
		protected System.Web.UI.WebControls.LinkButton lkbPost;
		protected System.Web.UI.WebControls.LinkButton lkUpdateCategories;
		protected System.Web.UI.WebControls.LinkButton lkbCancel;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected System.Web.UI.WebControls.RequiredFieldValidator valtbBodyRequired;
		protected System.Web.UI.WebControls.RequiredFieldValidator valTitleRequired;
		protected System.Web.UI.WebControls.LinkButton lkbNewPost;	
		protected System.Web.UI.WebControls.TextBox txbEntryName;
		#endregion

		#region Accessors
		// REFACTOR: are all of these still relevant when done?
		public PostType EntryType
		{
			get { return _entryType; }
			set { _entryType = value; }
		}

		public int PostID
		{
			get
			{
				if(ViewState[VSKEY_POSTID] != null)
					return (int)ViewState[VSKEY_POSTID];
				else
					return Constants.NULL_POSTID;
			}
			set { ViewState[VSKEY_POSTID] = value; }
		}

		public CategoryType CategoryType
		{
			get
			{
				if(ViewState[VSKEY_CATEGORYTYPE] != null)
					return (CategoryType)ViewState[VSKEY_CATEGORYTYPE];
				else
					throw new ApplicationException("CategoryType was not set");
			}
			set 
			{ 
				ViewState[VSKEY_CATEGORYTYPE] = value; 
			}
		}

		public bool IsListHidden
		{
			get { return _isListHidden; }
			set { _isListHidden = value; }
		}

		public string ResultsTitle 
		{
			get
			{
				return Results.HeaderText;
			}
			set 
			{ 
				Results.HeaderText = value; 
			}
		}

		public string ResultsUrlFormat
		{
			set
			{
				this.ResultsPager.UrlFormat = value;
			}
		}
		
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{	
			if (!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				if (null != Request.QueryString[Keys.QRYSTR_CATEGORYID])
					_filterCategoryID = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);

				ResultsPager.PageSize = Preferences.ListingItemCount;
				ResultsPager.PageIndex = _resultsPageNumber;
				Results.Collapsible = false;

				if (Constants.NULL_CATEGORYID != _filterCategoryID)
				{
					ResultsPager.UrlFormat += String.Format("&{0}={1}", Keys.QRYSTR_CATEGORYID, _filterCategoryID);
				}
				
				BindList();
				BindCategoryList();
				SetEditorMode();

				// We now allow direct links to edit a post.
				string postIdText = Request.QueryString["PostId"];
				int postId = int.MinValue;
				if(postIdText != null && postIdText.Length > 0)
				{
					try
					{
						postId = int.Parse(postIdText);
					}
					catch(System.FormatException)
					{
						//Swallow it. Gulp!
					}
				}
				if(postId > int.MinValue)
				{
					this.PostID = postId;
					BindPostEdit();
				}
			}			
		}
		
		private void BindList()
		{
			Edit.Visible = false;

			PagedEntryCollection selectionList = Entries.GetPagedEntries(_entryType, _filterCategoryID, 
				_resultsPageNumber, ResultsPager.PageSize,true);		

			if (selectionList.Count > 0)
			{				
				ResultsPager.ItemCount = selectionList.MaxItems;
				rprSelectionList.DataSource = selectionList;
				rprSelectionList.DataBind();
				NoMessagesLabel.Visible = false;
			}

			NoMessagesLabel.Visible = selectionList.Count <= 0;
			ResultsPager.Visible = selectionList.Count > 0;
			
		}

		private void BindCategoryList()
		{
			cklCategories.DataSource = Links.GetCategories(CategoryType,false);
			cklCategories.DataValueField = "CategoryID";
			cklCategories.DataTextField = "Title";
			cklCategories.DataBind();
		}

		private void SetConfirmation()
		{
			ConfirmationPage confirmPage = (ConfirmationPage)this.Page;
			confirmPage.IsInEdit = true;
			confirmPage.Message = "You will lose any unsaved content";

			this.lkbPost.Attributes.Add("OnClick",ConfirmationPage.ByPassFuncationName);
			this.lkUpdateCategories.Attributes.Add("OnClick",ConfirmationPage.ByPassFuncationName);
			this.lkbCancel.Attributes.Add("OnClick",ConfirmationPage.ByPassFuncationName);
		}

		private void BindPostEdit()
		{
			SetConfirmation();
			
			Entry currentPost = Entries.GetEntry(PostID, false);
			if(currentPost == null)
			{
				Response.Redirect("EditPosts.aspx");
				return;
			}
		
			Results.Collapsed = true;
			Edit.Visible = true;
			this.lkUpdateCategories.Visible = true;
			txbTitle.Text = currentPost.Title;

			hlEntryLink.NavigateUrl = currentPost.Link;
			hlEntryLink.Text = currentPost.Link;
			hlEntryLink.Attributes.Add("title", "view: " + currentPost.Title);
			hlEntryLink.Visible = true;

			chkComments.Checked                    = currentPost.AllowComments;	
			chkDisplayHomePage.Checked             = currentPost.DisplayOnHomePage;
			chkMainSyndication.Checked             = currentPost.IncludeInMainSyndication;  
			chkSyndicateDescriptionOnly.Checked    = currentPost.SyndicateDescriptionOnly ; 
			chkIsAggregated.Checked                = currentPost.IsAggregated;

			// Advanced Options
			this.txbEntryName.Text = currentPost.EntryName;
			this.txbExcerpt.Text = currentPost.Description;
			if(currentPost.HasTitleUrl)
			{
				this.txbTitleUrl.Text = currentPost.TitleUrl;
			}
			this.txbSourceUrl.Text = currentPost.SourceUrl;
			this.txbSourceName.Text = currentPost.SourceName;
	
			SetEditorText(currentPost.Body);

			ckbPublished.Checked = currentPost.IsActive;

			for (int i =0; i < cklCategories.Items.Count;i++)
				cklCategories.Items[i].Selected = false;

			LinkCollection postCategories = Links.GetLinkCollectionByPostID(PostID);
			if (postCategories.Count > 0)
			{
				for (int i = 0; i < postCategories.Count; i++)
				{
					cklCategories.Items.FindByValue(postCategories[i].CategoryID.ToString()).Selected = true;
				}
			}

			SetEditorMode();
			Results.Collapsible = true;
			Advanced.Collapsed = !Preferences.AlwaysExpandAdvanced;

			Control container = Page.FindControl("PageContainer");
			if (null != container && container is Subtext.Web.Admin.WebUI.Page)
			{	
				Subtext.Web.Admin.WebUI.Page page = (Subtext.Web.Admin.WebUI.Page)container;
				string title = String.Format("Editing {0} \"{1}\"", 
					CategoryType == CategoryType.StoryCollection ? "Article" : "Post", currentPost.Title);

				page.BreadCrumbs.AddLastItem(title);
				page.Title = title;
			}

			if(currentPost.HasEntryName)
			{
				this.Advanced.Collapsed = false;
				txbEntryName.Text = currentPost.EntryName;
			}
		}

		public void EditNewEntry()
		{
			ResetPostEdit(true);
			SetConfirmation();
		}

		private void ResetPostEdit(bool showEdit)
		{
			PostID = Constants.NULL_POSTID;

			Results.Collapsible = showEdit;
			Results.Collapsed = showEdit;
			Edit.Visible = showEdit;
			
			this.lkUpdateCategories.Visible = false;

			hlEntryLink.NavigateUrl = String.Empty;
			hlEntryLink.Attributes.Clear();
			hlEntryLink.Visible = false;
			txbTitle.Text = String.Empty;
			txbExcerpt.Text = String.Empty;
			txbSourceUrl.Text = String.Empty;
			txbSourceName.Text = String.Empty;
			txbEntryName.Text = string.Empty;

			ckbPublished.Checked = Preferences.AlwaysCreateIsActive;
			chkComments.Checked = true;
			chkDisplayHomePage.Checked = true;
			chkMainSyndication.Checked = true;
			chkSyndicateDescriptionOnly.Checked = false;
			chkIsAggregated.Checked = true;

			txbBody.Text = String.Empty;

			for(int i =0; i < cklCategories.Items.Count;i++)
				cklCategories.Items[i].Selected = false;

			Advanced.Collapsed = !Preferences.AlwaysExpandAdvanced;

			SetEditorMode();
		}
	
		private void UpdatePost()
		{	
			if(Page.IsValid)
			{
				string successMessage = Constants.RES_SUCCESSNEW;

				try
				{
					Entry entry = new Entry(EntryType);

					entry.Title = txbTitle.Text;
					entry.Body = HtmlHelper.StripRTB(txbBody.Text, Request.Url.Host);
					entry.IsActive = ckbPublished.Checked;
					entry.SourceName = txbSourceName.Text;
					entry.Author = Config.CurrentBlog.Author;
					entry.Email = Config.CurrentBlog.Email;
					entry.SourceUrl = txbSourceUrl.Text;
					entry.Description = txbExcerpt.Text;
					entry.TitleUrl = txbTitleUrl.Text;

					entry.AllowComments = chkComments.Checked;
					entry.DisplayOnHomePage = chkDisplayHomePage.Checked;
					entry.IncludeInMainSyndication = chkMainSyndication.Checked;
					entry.SyndicateDescriptionOnly = chkSyndicateDescriptionOnly.Checked;
					entry.IsAggregated = chkIsAggregated.Checked;
					entry.EntryName = txbEntryName.Text;

					entry.BlogID = Config.CurrentBlog.BlogID;
				
					if (PostID > 0)
					{
						successMessage = Constants.RES_SUCCESSEDIT;
						entry.DateUpdated = BlogTime.CurrentBloggerTime;
						entry.EntryID = PostID;
						Entries.Update(entry);
					}
					else
					{
						entry.DateCreated = BlogTime.CurrentBloggerTime;
						PostID = Entries.Create(entry);
					}

					UpdateCategories();
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
				this.Messages.ShowMessage(successMessage);
			}
		}

		private void UpdateCategories()
		{ 
			if(Page.IsValid)
			{
				string successMessage = Constants.RES_SUCCESSCATEGORYUPDATE;

				try
				{
					if (PostID > 0)
					{
						successMessage = Constants.RES_SUCCESSCATEGORYUPDATE;
						ArrayList al = new ArrayList();

						foreach(ListItem item in cklCategories.Items)
						{
							if(item.Selected)
							{
								al.Add(int.Parse(item.Value));
							}
						}					

						int[] Categories = (int[])al.ToArray(typeof(int));
						Entries.SetEntryCategoryList(PostID,Categories);

						BindList();
						this.Messages.ShowMessage(successMessage);
						this.ResetPostEdit(false);
					}
					else
					{
						this.Messages.ShowError(Constants.RES_FAILURECATEGORYUPDATE
							+ " There was a baseline problem updating the post categories.");  
					}
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
		}

	
		private void SetEditorMode()
		{
			if(CategoryType == CategoryType.StoryCollection)
			{
				this.chkDisplayHomePage.Visible = false;
				this.chkIsAggregated.Visible = false;
				this.chkMainSyndication.Visible = false;
				this.chkSyndicateDescriptionOnly.Visible = false;
			}
		}

		private void SetEditorText(string bodyValue)
		{
			txbBody.Text = bodyValue;
		}

		private void ConfirmDelete(int postID)
		{
			(Page as AdminPage).Command = new DeletePostCommand(postID);
			(Page as AdminPage).Command.RedirectUrl = Request.Url.ToString();
			Server.Transfer(Constants.URL_CONFIRM);
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
			this.rprSelectionList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rprSelectionList_ItemCommand);
			this.lkbPost.Click += new System.EventHandler(this.lkbPost_Click);
			this.lkUpdateCategories.Click += new EventHandler(lkUpdateCategories_Click);
			this.lkbCancel.Click += new System.EventHandler(this.lkbCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void rprSelectionList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{				
			switch (e.CommandName.ToLower()) 
			{
				case "edit" :
					PostID = Convert.ToInt32(e.CommandArgument);
					BindPostEdit();
					break;
				case "delete" :
					ConfirmDelete(Convert.ToInt32(e.CommandArgument));
					break;
				default:
					break;
			}
		}

		private void lkbCancel_Click(object sender, System.EventArgs e)
		{
			ResetPostEdit(false);
		}

		private void lkbPost_Click(object sender, System.EventArgs e)
		{
			UpdatePost();
		}

		private void lkUpdateCategories_Click(object sender, EventArgs e)
		{
			UpdateCategories();
		}
	}
}

