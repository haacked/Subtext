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
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;
using StringHelper = Subtext.Framework.Text.StringHelper;
using Subtext.Framework.Tracking;
using System.Collections.Specialized;
using System.Web;

namespace Subtext.Web.Admin.UserControls
{
	public partial class EntryEditor : UserControl
	{
		private const string VSKEY_CATEGORYTYPE = "CategoryType";

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

		public int? PostID
		{
			get
			{
                if (_postId == null) 
                {
                    string postIdText = Request.QueryString["PostID"];
                    int postId;
                    if (int.TryParse(postIdText, out postId)) 
                    {
                        _postId = postId;
                    }
                }
                return _postId;
		    }
		}
        int? _postId = null;

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
		#endregion

        protected override void OnLoad(EventArgs e) 
        {
			if (!IsPostBack)
			{
				BindCategoryList();
				SetEditorMode();

                if (PostID != null) 
                {
                    BindPostEdit();
                }
                else 
                {
                    BindPostCreate();
                }
			}
            base.OnLoad(e);
		}

		//This is true if we came from a pencil edit link while viewing the post 
		//from outside the admin tool.
		private bool ReturnToOriginalPost
		{
			get
			{
                return (Request.QueryString["return-to-post"] == "true");
			}
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

			this.lkbPost.Attributes.Add("OnClick", ConfirmationPage.BypassFunctionName);
			this.lkUpdateCategories.Attributes.Add("OnClick", ConfirmationPage.BypassFunctionName);
			this.lkbCancel.Attributes.Add("OnClick", ConfirmationPage.BypassFunctionName);
		}

        private void BindPostCreate() 
        {
            if (PostID != null)
                throw new InvalidOperationException("Cannot create a post when we have an id.");

            this.txbTitle.Text = string.Empty;
            this.richTextEditor.Text = string.Empty;

            SetConfirmation();
            SetDefaultPublishOptions();
            PopulateMimeTypeDropDown();
        }

        private void SetDefaultPublishOptions() 
        {
            chkMainSyndication.Checked = true;
            ckbPublished.Checked = true;
            chkDisplayHomePage.Checked = true;
            chkComments.Checked = Config.CurrentBlog.CommentsEnabled;
        }

        private void BindPostEdit()
		{
            if (PostID == null)
                throw new InvalidOperationException("Cannot edit a null post");

			SetConfirmation();
			
			Entry entry = Entries.GetEntry(PostID.Value, PostConfig.None, false);
			if(entry == null)
			{
                ReturnToOrigin(null);
				return;
			}
		
			txbTitle.Text = entry.Title;
            if (!NullValue.IsNull(entry.DateSyndicated) && entry.DateSyndicated > Config.CurrentBlog.TimeZone.Now) {
                txtPostDate.Text = entry.DateSyndicated.ToString(CultureInfo.CurrentCulture);
            }

			hlEntryLink.NavigateUrl = entry.Url;
			hlEntryLink.Text = entry.FullyQualifiedUrl.ToString();
			hlEntryLink.Attributes.Add("title", "view: " + entry.Title);

		    PopulateMimeTypeDropDown();
		    //Enclosures
            if(entry.Enclosure != null)
            {
                Enclosure.Collapsed = false;
                txbEnclosureTitle.Text = entry.Enclosure.Title;
                txbEnclosureUrl.Text = entry.Enclosure.Url;
                txbEnclosureSize.Text = entry.Enclosure.Size.ToString();
                if (ddlMimeType.Items.FindByText(entry.Enclosure.MimeType) != null)
                    ddlMimeType.SelectedValue = entry.Enclosure.MimeType;
                else
                {
                    ddlMimeType.SelectedValue = "other";
                    txbEnclosureOtherMimetype.Text = entry.Enclosure.MimeType;
                }
                ddlAddToFeed.SelectedValue = entry.Enclosure.AddToFeed.ToString().ToLower();
                ddlDisplayOnPost.SelectedValue = entry.Enclosure.ShowWithPost.ToString().ToLower();
            }

			chkComments.Checked                    = entry.AllowComments;	
			chkCommentsClosed.Checked			   = entry.CommentingClosed;
			SetCommentControls();
			if (entry.CommentingClosedByAge)
				chkCommentsClosed.Enabled = false;

			chkDisplayHomePage.Checked             = entry.DisplayOnHomePage;
			chkMainSyndication.Checked             = entry.IncludeInMainSyndication;  
			chkSyndicateDescriptionOnly.Checked    = entry.SyndicateDescriptionOnly;
			chkIsAggregated.Checked                = entry.IsAggregated;

			// Advanced Options
			this.txbEntryName.Text = entry.EntryName;
			this.txbExcerpt.Text = entry.Description;
			
			SetEditorText(entry.Body);

			ckbPublished.Checked = entry.IsActive;

            BindCategoryList();
            for (int i = 0; i < cklCategories.Items.Count; i++)
            {
                cklCategories.Items[i].Selected = false;
            }

			IList<Link> postCategories = Links.GetLinkCollectionByPostID(PostID.Value);
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
			Advanced.Collapsed = !Preferences.AlwaysExpandAdvanced;

            AdminPageTemplate adminMasterPage = Page.Master as AdminPageTemplate;
            if (adminMasterPage != null && adminMasterPage.BreadCrumb != null)
			{	
				string title = string.Format(CultureInfo.InvariantCulture, "Editing {0} \"{1}\"", 
					CategoryType == CategoryType.StoryCollection ? "Article" : "Post", entry.Title);

                adminMasterPage.BreadCrumb.AddLastItem(title);
                adminMasterPage.Title = title;
			}

			if(entry.HasEntryName)
			{
				this.Advanced.Collapsed = false;
				txbEntryName.Text = entry.EntryName;
			}
		}

	    private void PopulateMimeTypeDropDown()
	    {
            ddlMimeType.Items.Add(new ListItem("Choose...", "none"));
            foreach (string key in MimeTypesMapper.Mappings.List)
	        {
                ddlMimeType.Items.Add(new ListItem(MimeTypesMapper.Mappings.List[key], MimeTypesMapper.Mappings.List[key]));
	        }
            ddlMimeType.Items.Add(new ListItem("Other", "other"));
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
			SetConfirmation();
		}

        private void ReturnToOrigin(string message)
		{
            if (ReturnToOriginalPost && PostID != null) 
            {
                // We came from outside the post, let's go there.
                Entry updatedEntry = Entries.GetEntry(PostID.Value, PostConfig.IsActive, false);
                if (updatedEntry != null) 
                {
                    Response.Redirect(updatedEntry.Url);
                }
            }
            else 
            {
                string url = "Default.aspx";
                if (!String.IsNullOrEmpty(message)) 
                {
                    url += "?message=" + HttpUtility.UrlEncode(message);
                }
                Response.Redirect(url);
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

            EnableEnclosureValidation(EnclosureEnabled());

            if(Page.IsValid)
			{
				string successMessage = Constants.RES_SUCCESSNEW;

				try
				{
					Entry entry;
					if (PostID == null)
					{
                        ValidateEntryTypeIsNotNone(EntryType);
						entry = new Entry(EntryType);
					}
					else
					{
						entry = Entries.GetEntry(PostID.Value, PostConfig.None, false);
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

                    //Enclosure
				    int enclosureId = 0;
                    if (entry.Enclosure != null)
                    {
                        enclosureId = entry.Enclosure.Id;
                    }

                    if (EnclosureEnabled())
                    {
                        if (entry.Enclosure == null)
                        {
                            entry.Enclosure = new Enclosure();
                        }
                        Enclosure enc = entry.Enclosure;

                        enc.Title = txbEnclosureTitle.Text;
                        enc.Url = txbEnclosureUrl.Text;
                        if (ddlMimeType.SelectedValue.Equals("other"))
                        {
                            enc.MimeType = txbEnclosureOtherMimetype.Text;
                        }
                        else
                        {
                            enc.MimeType = ddlMimeType.SelectedValue;
                        }
                        long size = 0;
                        Int64.TryParse(txbEnclosureSize.Text, out size);
                        enc.Size = size;
                        enc.AddToFeed = Boolean.Parse(ddlAddToFeed.SelectedValue);
                        enc.ShowWithPost = Boolean.Parse(ddlDisplayOnPost.SelectedValue);
                    }
                    else
                    {
                        entry.Enclosure = null;
                    }

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
                    entry.Categories.Clear();
                    ReplaceSelectedCategoryNames(entry.Categories);

                    if (!NullValue.IsNull(postDate))
                    {
                        entry.DateSyndicated = postDate;
                    }

					if (PostID != null)
					{
						successMessage = Constants.RES_SUCCESSEDIT;
						entry.DateModified = Config.CurrentBlog.TimeZone.Now;
						entry.Id = PostID.Value;
						
						Entries.Update(entry);

                        if (entry.Enclosure == null && enclosureId != 0)
                        {
                            Enclosures.Delete(enclosureId);
                        }
                        else if (entry.Enclosure != null && entry.Enclosure.Id != 0)
                        {
                            Enclosures.Update(entry.Enclosure);
                        }
                        else if (entry.Enclosure != null && entry.Enclosure.Id == 0)
                        {
                            entry.Enclosure.EntryId = entry.Id;
                            Enclosures.Create(entry.Enclosure);
                        }

						UpdateCategories();
					}
					else
					{
						_postId = Entries.Create(entry);

                        if(entry.Enclosure != null)
                        {
                            entry.Enclosure.EntryId = PostID.Value;
                            Enclosures.Create(entry.Enclosure);
                        }

						UpdateCategories();
						AddCommunityCredits(entry);
					}
				}
				catch(Exception ex)
				{
					this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, 
						Constants.RES_FAILUREEDIT, ex.Message));
                    successMessage = string.Empty;
				}

                //Prepared success messages were reset in the catch block because of some error on posting the content
                if (!String.IsNullOrEmpty(successMessage))
                {
                    ReturnToOrigin(successMessage);
                }
			}
		}

        [CoverageExclude]
        private static void ValidateEntryTypeIsNotNone(PostType entryType)
        {
            if (entryType == PostType.None)
            {
                throw new InvalidOperationException("The entry type is None. Impossible!");
            }
        }

        private bool EnclosureEnabled()
        {
            if (!String.IsNullOrEmpty(txbEnclosureUrl.Text))
                return true;
            if (!String.IsNullOrEmpty(txbEnclosureTitle.Text))
                return true;
            if (!String.IsNullOrEmpty(txbEnclosureSize.Text))
                return true;
            if (ddlMimeType.SelectedIndex > 0)
                return true;

            return false;
        }

        private void EnableEnclosureValidation(bool enabled)
        {
            valEncSizeRequired.Enabled = enabled;
            valEncUrlRequired.Enabled = enabled;
            valEncMimeTypeRequired.Enabled = enabled;

            if(!enabled)
                valEncOtherMimetypeRequired.Enabled = false;
            else
            {
                if (ddlMimeType.SelectedValue.Equals("other"))
                    valEncOtherMimetypeRequired.Enabled = true;
                else
                    valEncOtherMimetypeRequired.Enabled = false;
            }
        }

        private void ReplaceSelectedCategoryNames(StringCollection sc)
        {
            sc.Clear();
            foreach (ListItem item in cklCategories.Items)
            {
                if (item.Selected)
                {
                    sc.Add(item.Text);
                }
            }
        }

		private string UpdateCategories()
		{ 
			try
			{
				if (PostID != null)
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
					Entries.SetEntryCategoryList(PostID.Value, Categories);

                    return successMessage;
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
            return null;
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

        override protected void OnInit(EventArgs e)
		{
			InitializeComponent();			
			base.OnInit(e);			
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    			
			this.lkbPost.Click += OnUpdatePostClick;
			this.lkUpdateCategories.Click += OnUpdateCategoriesClick;
			this.lkbCancel.Click += this.OnCancelClick;
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			if(PostID != null && ReturnToOriginalPost)
			{
				// We came from outside the post, let's go there.
				Entry updatedEntry = Entries.GetEntry(PostID.Value, PostConfig.IsActive, false);
				if(updatedEntry != null)
				{
					Response.Redirect(updatedEntry.Url);
					return;
				}
			}

            ReturnToOrigin(null);
		}

		private void OnUpdatePostClick(object sender, EventArgs e)
		{
			UpdatePost();
		}

		private void OnUpdateCategoriesClick(object sender, EventArgs e)
		{
			string successMessage = UpdateCategories();
            if (successMessage != null) 
            {
                ReturnToOrigin(successMessage);
            }
		}

		protected void richTextEditor_Error(object sender, RichTextEditorErrorEventArgs e)
		{
			this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", e.Exception.Message));
		}

		private void AddCommunityCredits(Entry entry) 
		{
			string result=string.Empty;

         try
         {
            CommunityCreditNotification.AddCommunityCredits(entry);
         }
         catch (CommunityCreditNotificationException ex)
         {
            this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "Error during Community Credits submission (your post has been saved)", ex.Message));
         }
         catch (Exception ex)
         {
            this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "Error during Community Credits submission (your post has been saved)", ex.Message));
         }
		}
	}
}