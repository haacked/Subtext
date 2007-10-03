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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.UI.WebControls;
using StringHelper = Subtext.Framework.Text.StringHelper;
using Subtext.Extensibility.Plugins;

namespace Subtext.Web.Admin.UserControls
{
	public partial class EntryEditor : UserControl, INotifiableControl
	{
		private const string VSKEY_POSTID = "PostID";
		private const string VSKEY_CATEGORYTYPE = "CategoryType";

		private int categoryId = NullValue.NullInt32;
		private int pageIndex;
				
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
				this.resultsPager.UrlFormat = value;
			}
		}
		
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
				this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

			if (null != Request.QueryString[Keys.QRYSTR_CATEGORYID])
				this.categoryId = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_CATEGORYID]);

			this.resultsPager.PageSize = Preferences.ListingItemCount;
			this.resultsPager.PageIndex = this.pageIndex;
			Results.Collapsible = false;

			if (NullValue.NullInt32 != this.categoryId)
			{
				string catIdQueryString = string.Format(CultureInfo.InvariantCulture, "&{0}={1}", Keys.QRYSTR_CATEGORYID, this.categoryId);
				if(!this.resultsPager.UrlFormat.EndsWith(catIdQueryString))
					this.resultsPager.UrlFormat += catIdQueryString;
			}
			
			if (!IsPostBack)
			{
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

			if (categoryId != NullValue.NullInt32)
			{
				LinkCategory category = Links.GetLinkCategory(categoryId, false);
				if (category != null)
				{
					this.Results.HeaderText = "POSTS (" + category.Title + ")";
				}
			}
			
            IPagedCollection<Entry> selectionList = Entries.GetPagedEntries(this.EntryType, this.categoryId, this.pageIndex, this.resultsPager.PageSize);		

			if (selectionList.Count > 0)
			{
				resultsPager.ItemCount = selectionList.MaxItems;
				rprSelectionList.DataSource = selectionList;
				rprSelectionList.DataBind();
				NoMessagesLabel.Visible = false;
			}

			NoMessagesLabel.Visible = selectionList.Count <= 0;
			resultsPager.Visible = rprSelectionList.Visible  = selectionList.Count > 0;
			
			
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
			
			Entry currentPost = Entries.GetEntry(PostID, PostConfig.None, false);
			if(currentPost == null)
			{
				Response.Redirect("EditPosts.aspx");
				return;
			}
		
			Results.Visible = false;
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
            this.txtPostDate.Text = currentPost.DateSyndicated.ToString();
			
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
            DateTime postDate = NullValue.NullDateTime;

            if (string.IsNullOrEmpty(txtPostDate.Text))
            {
                vCustomPostDate.IsValid = true;
            }
            else
            {
                vCustomPostDate.IsValid = DateTime.TryParse(txtPostDate.Text, out postDate);
            }
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
						entry = Entries.GetEntry(PostID, PostConfig.None, false);
						if(entry.PostType != EntryType)
						{
							this.EntryType = entry.PostType;
						}
					}
					
					entry.Title = txbTitle.Text;
					entry.Body = HtmlHelper.StripRTB(richTextEditor.Xhtml, Request.Url.Host);
                    entry.Author = System.Web.Security.Membership.GetUser();
					entry.BlogId = Config.CurrentBlog.Id;

					// Advanced options
					entry.IsActive = ckbPublished.Checked;
					entry.AllowComments = chkComments.Checked;
					entry.CommentingClosed = chkCommentsClosed.Checked;
					entry.DisplayOnHomePage = chkDisplayHomePage.Checked;
					entry.IncludeInMainSyndication = chkMainSyndication.Checked;
					entry.SyndicateDescriptionOnly = chkSyndicateDescriptionOnly.Checked;
					entry.IsAggregated = chkIsAggregated.Checked;
					entry.EntryName = StringHelper.ReturnNullForEmpty(txbEntryName.Text);
					entry.Description = StringHelper.ReturnNullForEmpty(txbExcerpt.Text);
                    entry.Categories = GetSelectedCategoryNames();

                    if (!NullValue.IsNull(postDate))
                    {
                        entry.DateSyndicated = postDate;
                    }

					if (PostID != NullValue.NullInt32)
					{
						successMessage = Constants.RES_SUCCESSEDIT;
						entry.DateModified = Config.CurrentBlog.TimeZone.Now;
						entry.Id = PostID;
						
						//Raise event before updating a post
						CancellableEntryEventArgs e = new CancellableEntryEventArgs(entry, ObjectState.Update);
						SubtextEvents.OnEntryUpdating(this, e);

						if (!e.Cancel)
						{
							Entries.Update(entry);

							//Raise event after updating a post
							SubtextEvents.OnEntryUpdated(this, new EntryEventArgs(entry, ObjectState.Update));
						}
						else
							ShowMessage("Update cancelled by plugin");

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
						entry.DateCreated = Config.CurrentBlog.TimeZone.Now;

						//Raise event before creating a post
						CancellableEntryEventArgs e = new CancellableEntryEventArgs(entry, ObjectState.Create);
						SubtextEvents.OnEntryUpdating(this, e);

						if (!e.Cancel)
						{
							PostID = Entries.Create(entry);

							//Raise event after creating a post
							SubtextEvents.OnEntryUpdated(this, new EntryEventArgs(entry, ObjectState.Create));
						}
						else
						{
							ShowMessage("Post creation cancelled by plugin");
						}
					}
				}
				catch(Exception ex)
				{
					this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, 
						Constants.RES_FAILUREEDIT, ex.Message), false);
                    successMessage = string.Empty;
				}
				finally
				{
					Results.Collapsible = false;
				}
				this.Messages.ShowMessage(successMessage, false);
			}
		}


        private StringCollection GetSelectedCategoryNames()
        {
            StringCollection sc = new StringCollection();

            foreach (ListItem item in cklCategories.Items)
            {
                if (item.Selected)
                {
                    sc.Add(item.Text);
                }
            }

            return sc;
        }

		private void UpdateCategories()
		{ 
			try
			{
				if (PostID > 0)
				{
					string successMessage = Constants.RES_SUCCESSCATEGORYUPDATE;
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
					this.Messages.ShowMessage(successMessage, false);
					this.ResetPostEdit(false);
				}
				else
				{
					this.Messages.ShowError(Constants.RES_FAILURECATEGORYUPDATE
						+ " There was a baseline problem updating the post categories.", false);  
				}
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
					Constants.RES_FAILUREEDIT, ex.Message), false);
			}
			finally
			{
				Results.Collapsible = false;
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
			richTextEditor.Text = bodyValue;
		}

		private void ConfirmDelete(int postID)
		{
			AdminPage page = Page as AdminPage;
			if (page == null)
				throw new InvalidOperationException("Somehow the page is not an AdminPage.");
			
			page.Command = new DeletePostCommand(postID);
			page.Command.RedirectUrl = Request.Url.ToString();
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
			this.rprSelectionList.ItemCommand += new RepeaterCommandEventHandler(this.rprSelectionList_ItemCommand);
			this.lkbPost.Click += new EventHandler(this.lkbPost_Click);
			this.lkUpdateCategories.Click += new EventHandler(lkUpdateCategories_Click);
			this.lkbCancel.Click += new EventHandler(this.lkbCancel_Click);
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
			this.Messages.ResetMessages();
			UpdatePost();
		}

		private void lkUpdateCategories_Click(object sender, EventArgs e)
		{
			this.Messages.ResetMessages();
			UpdateCategories();
		}

		protected void richTextEditor_Error(object sender, RichTextEditorErrorEventArgs e)
		{
			this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", e.Exception.Message), false);
		}

		#region INotifiableControl Members

		public void ShowError(string message)
		{
			this.Messages.ShowError(message, false);
		}

		public void ShowMessage(string message)
		{
			this.Messages.ShowMessage(message, false);
		}

		#endregion
	}
}


