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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;
using StringHelper = Subtext.Framework.Text.StringHelper;

namespace Subtext.Web.Admin.UserControls
{
	public class EntryEditor : UserControl
	{
		private const string VSKEY_POSTID = "PostID";
		private const string VSKEY_CATEGORYTYPE = "CategoryType";

		private int _filterCategoryID = NullValue.NullInt32;
		private int pageIndex = 0;
		private bool _isListHidden = false;
		
		#region Declared Controls
		protected MessagePanel Messages;
		protected Repeater rprSelectionList;
		protected HtmlGenericControl NoMessagesLabel;
		protected Pager ResultsPager;
		protected HyperLink hlEntryLink;
		protected TextBox txbTitle;
		protected Button Post;
		protected TextBox txbExcerpt;
		protected TextBox txbTitleUrl;
		protected TextBox Textbox1;
		protected TextBox Textbox2;
		protected CheckBox ckbPublished;
		protected CheckBox chkComments;
		protected CheckBox chkCommentsClosed;
		protected CheckBox chkDisplayHomePage;
		protected CheckBox chkMainSyndication;
		protected CheckBox chkSyndicateDescriptionOnly;
		protected CheckBox chkIsAggregated;

		protected CheckBoxList cklCategories;
		protected AdvancedPanel Results;
		protected AdvancedPanel Advanced;
		protected TextBox txbSourceName;
		protected TextBox txbSourceUrl;
		protected Button lkbPost;
		protected Button lkUpdateCategories;
		protected Button lkbCancel;
		protected AdvancedPanel Edit;
		protected RequiredFieldValidator valtbBodyRequired;
		protected RequiredFieldValidator valTitleRequired;
		protected Button lkbNewPost;	
		protected TextBox txbEntryName;
		protected RichTextEditor richTextEditor;
		#endregion

		#region Accessors
		/// <summary>
		/// Gets or sets the type of the entry.
		/// </summary>
		/// <value>The type of the entry.</value>
		public PostType EntryType
		{
			get
			{
				if(ViewState["PostType"] != null)
					return (PostType)ViewState["PostType"];
				return PostType.None;
			}
			set
			{
				ViewState["PostType"] = value;
			}
		}

		public int PostID
		{
			get
			{
				if(ViewState[VSKEY_POSTID] != null)
					return (int)ViewState[VSKEY_POSTID];
				else
					return NullValue.NullInt32;
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
					throw new InvalidOperationException("CategoryType was not set");
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

		private void Page_Load(object sender, EventArgs e)
		{	
			if (!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
					this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				if (null != Request.QueryString[Keys.QRYSTR_CATEGORYID])
					_filterCategoryID = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);

				ResultsPager.PageSize = Preferences.ListingItemCount;
				ResultsPager.PageIndex = this.pageIndex;
				Results.Collapsible = false;

				if (NullValue.NullInt32 != _filterCategoryID)
				{
					ResultsPager.UrlFormat += string.Format(CultureInfo.InvariantCulture, "&{0}={1}", Keys.QRYSTR_CATEGORYID, _filterCategoryID);
				}
				
				BindList();
				BindCategoryList();
				SetEditorMode();

				// We now allow direct links to edit a post.
				string postIdText = Request.QueryString["PostId"];
				int postId = NullValue.NullInt32;
				if(postIdText != null && postIdText.Length > 0)
				{
					try
					{
						postId = int.Parse(postIdText);
						//Ok, we came from outside the admin tool.
						ReturnToOriginalPost = true;
					}
					catch(FormatException)
					{
						//Swallow it. Gulp!
					}
				}
				if(postId > NullValue.NullInt32)
				{
					this.PostID = postId;
					BindPostEdit();
				}
			}			
		}

		//This is true if we came from a pencil edit link while viewing the post 
		//from outside the admin tool.
		private bool ReturnToOriginalPost
		{
			get
			{
				if(ViewState["ReturnToOriginalPost"] != null)
					return (bool)ViewState["ReturnToOriginalPost"];
				return false;
			}
			set
			{
				ViewState["ReturnToOriginalPost"] = value;
			}
		}
		
		private void BindList()
		{
			Edit.Visible = false;

            IPagedCollection<Entry> selectionList = Entries.GetPagedEntries(this.EntryType, _filterCategoryID, this.pageIndex, ResultsPager.PageSize);		

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
            cklCategories.DataSource = Links.GetCategories(CategoryType, ActiveFilter.None);
			cklCategories.DataValueField = "Id";
			cklCategories.DataTextField = "Title";
			cklCategories.DataBind();
		}

		private void SetConfirmation()
		{
			ConfirmationPage confirmPage = (ConfirmationPage)this.Page;
			confirmPage.IsInEdit = true;
			confirmPage.Message = "You will lose any unsaved content";

			this.lkbPost.Attributes.Add("OnClick",ConfirmationPage.BypassFunctionName);
			this.lkUpdateCategories.Attributes.Add("OnClick",ConfirmationPage.BypassFunctionName);
			this.lkbCancel.Attributes.Add("OnClick",ConfirmationPage.BypassFunctionName);
		}

		private void BindPostEdit()
		{
			SetConfirmation();
			
			Entry currentPost = Entries.GetEntry(PostID, PostConfig.IsActive, false);
			if(currentPost == null)
			{
				Response.Redirect("EditPosts.aspx");
				return;
			}
		
			Results.Collapsed = true;
			Edit.Visible = true;
			this.lkUpdateCategories.Visible = true;
			txbTitle.Text = currentPost.Title;

			hlEntryLink.NavigateUrl = currentPost.Url;
			hlEntryLink.Text = currentPost.FullyQualifiedUrl.ToString();
			hlEntryLink.Attributes.Add("title", "view: " + currentPost.Title);
			hlEntryLink.Visible = true;

			chkComments.Checked                    = currentPost.AllowComments;	
			chkCommentsClosed.Checked			   = currentPost.CommentingClosed;
			SetCommentControls();
			if (currentPost.CommentingClosedByAge)
				chkCommentsClosed.Enabled = false;

			chkDisplayHomePage.Checked             = currentPost.DisplayOnHomePage;
			chkMainSyndication.Checked             = currentPost.IncludeInMainSyndication;  
			chkSyndicateDescriptionOnly.Checked    = currentPost.SyndicateDescriptionOnly ; 
			chkIsAggregated.Checked                = currentPost.IsAggregated;

			// Advanced Options
			this.txbEntryName.Text = currentPost.EntryName;
			this.txbExcerpt.Text = currentPost.Description;
			if(currentPost.HasAlternativeTitleUrl)
			{
				this.txbTitleUrl.Text = currentPost.AlternativeTitleUrl;
			}
			this.txbSourceUrl.Text = currentPost.SourceUrl;
			this.txbSourceName.Text = currentPost.SourceName;
	
			SetEditorText(currentPost.Body);

			ckbPublished.Checked = currentPost.IsActive;

            BindCategoryList();
			for (int i = 0; i < cklCategories.Items.Count; i++)
				cklCategories.Items[i].Selected = false;

			ICollection<Link> postCategories = Links.GetLinkCollectionByPostID(PostID);
			if (postCategories.Count > 0)
			{
				foreach(Link postCategory in postCategories)
				{
					ListItem categoryItem = cklCategories.Items.FindByValue(postCategory.CategoryID.ToString(CultureInfo.InvariantCulture));
					if(categoryItem == null)
						throw new InvalidOperationException(string.Format("Could not find category id {0} in the Checkbox list which has {1} items.", postCategory.CategoryID, cklCategories.Items.Count));
					categoryItem.Selected = true;
				}
			}

			SetEditorMode();
			Results.Collapsible = true;
			Advanced.Collapsed = !Preferences.AlwaysExpandAdvanced;

            AdminPageTemplate adminMasterPage = Page.Master as AdminPageTemplate;
            if (adminMasterPage != null && adminMasterPage.BreadCrumb != null)
			{	
				string title = string.Format(CultureInfo.InvariantCulture, "Editing {0} \"{1}\"", 
					CategoryType == CategoryType.StoryCollection ? "Article" : "Post", currentPost.Title);

                adminMasterPage.BreadCrumb.AddLastItem(title);
                adminMasterPage.Title = title;
			}

			if(currentPost.HasEntryName)
			{
				this.Advanced.Collapsed = false;
				txbEntryName.Text = currentPost.EntryName;
			}
		}

		private void SetCommentControls()
		{
			if (!Config.CurrentBlog.CommentsEnabled)
			{
				chkComments.Enabled = false;
				chkCommentsClosed.Enabled = false;
			}
		}

		public void EditNewEntry()
		{
			ResetPostEdit(true);
			SetConfirmation();
		}

		private void ResetPostEdit(bool showEdit)
		{
			PostID = NullValue.NullInt32;

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
			chkCommentsClosed.Checked = false;
			SetCommentControls();
			chkDisplayHomePage.Checked = true;
			chkMainSyndication.Checked = true;
			chkSyndicateDescriptionOnly.Checked = false;
			chkIsAggregated.Checked = true;

//			txbBody.Text = String.Empty;
			richTextEditor.Text = String.Empty;

			for(int i =0; i < cklCategories.Items.Count;i++)
				cklCategories.Items[i].Selected = false;

			Advanced.Collapsed = !Preferences.AlwaysExpandAdvanced;

			if(!ReturnToOriginalPost)
			{
				SetEditorMode();
			}
			else
			{
				// We came from outside the post, let's go there.
				Entry updatedEntry = Entries.GetEntry(PostID, PostConfig.IsActive, false);
				if(updatedEntry != null)
				{
					Response.Redirect(updatedEntry.Url);
				}
			}
		}
	
		private void UpdatePost()
		{	
			if(Page.IsValid)
			{
				string successMessage = Constants.RES_SUCCESSNEW;

				try
				{
					Entry entry;
					if (PostID == NullValue.NullInt32)
					{
						if(EntryType == PostType.None)
						{
							throw new InvalidOperationException("The entry type is None. Impossible!");
						}
						entry = new Entry(EntryType);
					}
					else
					{
						entry = Entries.GetEntry(PostID, PostConfig.IsActive, false);
						if(entry.PostType != EntryType)
						{
							this.EntryType = entry.PostType;
						}
					}
					
					entry.Title = txbTitle.Text;
					entry.Body = HtmlHelper.StripRTB(richTextEditor.Xhtml, Request.Url.Host);
					entry.Author = Config.CurrentBlog.Author;
					entry.Email = Config.CurrentBlog.Email;
					entry.BlogId = Config.CurrentBlog.Id;

					// Advanced options
					/* Need to do some special checks for txb*.Text == "", b/c they get posted 
					 * by the page as String.Empty if there is nothing in them, but this 
					 * causes issues when getting entries out of the dataStore. For example, 
					 * when getting an "" for txbTitleUrl, we don't correctly write the url to
					 * the post. So we need to be sure to get NULL for these values. This also works
					 * to reset these fields in the dataStore for an "updated" post.
					 */
					entry.IsActive = ckbPublished.Checked;
					entry.AllowComments = chkComments.Checked;
					entry.CommentingClosed = chkCommentsClosed.Checked;
					entry.DisplayOnHomePage = chkDisplayHomePage.Checked;
					entry.IncludeInMainSyndication = chkMainSyndication.Checked;
					entry.SyndicateDescriptionOnly = chkSyndicateDescriptionOnly.Checked;
					entry.IsAggregated = chkIsAggregated.Checked;
					entry.EntryName = StringHelper.ReturnNullForEmpty(txbEntryName.Text);
					entry.Description = StringHelper.ReturnNullForEmpty(txbExcerpt.Text);
					entry.AlternativeTitleUrl = StringHelper.ReturnNullForEmpty(txbTitleUrl.Text);
					entry.SourceUrl = StringHelper.ReturnNullForEmpty(txbSourceUrl.Text);
					entry.SourceName = StringHelper.ReturnNullForEmpty(txbSourceName.Text);

					if (PostID != NullValue.NullInt32)
					{
						successMessage = Constants.RES_SUCCESSEDIT;
						entry.DateUpdated = BlogTime.CurrentBloggerTime;
						entry.Id = PostID;
						
						//TODO: Add here code to be called before updating a post
						
						Entries.Update(entry);
						
						//TODO: Add here code to be called after updating a post

						if(ReturnToOriginalPost)
						{
							// We came from outside the post, let's go there.
							Entry updatedEntry = Entries.GetEntry(PostID, PostConfig.IsActive, false);
							if(updatedEntry != null)
							{
								Response.Redirect(updatedEntry.Url);
								return;
							}
						}
					}
					else
					{
						entry.DateCreated = BlogTime.CurrentBloggerTime;
						
						//TODO: Add here code to be called before creating a post
						
						PostID = Entries.Create(entry);
						
						//TODO: Add here code to be called after creating a post
						AddCommunityCredits(entry);
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
				string successMessage;

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
//			txbBody.Text = bodyValue;
			richTextEditor.Text = bodyValue;
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
			this.rprSelectionList.ItemCommand += new RepeaterCommandEventHandler(this.rprSelectionList_ItemCommand);
			this.lkbPost.Click += new EventHandler(this.lkbPost_Click);
			this.lkUpdateCategories.Click += new EventHandler(lkUpdateCategories_Click);
			this.lkbCancel.Click += new EventHandler(this.lkbCancel_Click);
			//this.richTextEditor.Error+=new Subtext.Web.Controls.RichTextEditor.ErrorHandler(richTextEditor_Error);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

		private void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{				
			switch (e.CommandName.ToLower(CultureInfo.InvariantCulture)) 
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

		private void lkbCancel_Click(object sender, EventArgs e)
		{
			if(PostID > -1 && ReturnToOriginalPost)
			{
				// We came from outside the post, let's go there.
				Entry updatedEntry = Entries.GetEntry(PostID, PostConfig.IsActive, false);
				if(updatedEntry != null)
				{
					Response.Redirect(updatedEntry.Url);
					return;
				}
			}

			ResetPostEdit(false);
		}

		private void lkbPost_Click(object sender, EventArgs e)
		{
			UpdatePost();
		}

		private void lkUpdateCategories_Click(object sender, EventArgs e)
		{
			UpdateCategories();
		}

		protected void richTextEditor_Error(object sender, RichTextEditorErrorEventArgs e)
		{
			this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", e.Exception.Message));
		}

		private string AddCommunityCredits(Entry entry) 
		{
			string result=string.Empty;

			bool commCreditsEnabled;
			try
			{
                commCreditsEnabled = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CommCreditEnabled"]);
			}
			catch(Exception) 
			{
				commCreditsEnabled = false;
			}

			if(commCreditsEnabled.Equals("true")) 
			{
				com.community_credit.www.AffiliateServices wsCommunityCredit = new com.community_credit.www.AffiliateServices();
				string url=entry.FullyQualifiedUrl.ToString();
				string category=String.Empty;
				if(entry.PostType==PostType.BlogPost)
					category="Blog";
				else if (entry.PostType==PostType.Story)
					category="Article";
				string description = "Blogged about: " + entry.Title;
				BlogInfo info = Config.CurrentBlog;
				string firstName=string.Empty;
				string lastName=info.Author;
				string email=info.Email;
                string affiliateCode = System.Configuration.ConfigurationManager.AppSettings["CommCreditAffiliateCode"];
                string affiliateKey = System.Configuration.ConfigurationManager.AppSettings["CommCreditAffiliateKey"];
				
				try 
				{
					result=wsCommunityCredit.AddCommunityCredit(email,firstName,lastName,description,url,category,affiliateCode,affiliateKey);
				}
				catch(Exception ex) 
				{
					this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "Error during Community Credits submission (your post has been saved)", ex.Message));
				}	
			}
			return result;
		}
	}
}

