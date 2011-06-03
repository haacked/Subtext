#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;
using Subtext.Web.Admin.Pages;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;
using Subtext.Web.Properties;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.Admin.UserControls
{
    public partial class EntryEditor : BaseControl
    {
        private const string CategoryTypeViewStateKey = "CategoryType";
        int? _postId;

        /// <summary>
        /// Gets or sets the type of the entry.
        /// </summary>
        /// <value>The type of the entry.</value>
        public PostType EntryType
        {
            get
            {
                if (ViewState["PostType"] != null)
                {
                    return (PostType)ViewState["PostType"];
                }
                return PostType.None;
            }
            set { ViewState["PostType"] = value; }
        }

        public int? PostId
        {
            get
            {
                if (_postId == null)
                {
                    string postIdText = Request.QueryString["PostId"];
                    int postId;
                    if (int.TryParse(postIdText, out postId))
                    {
                        _postId = postId;
                    }
                }
                return _postId;
            }
        }

        public CategoryType CategoryType
        {
            get
            {
                if (ViewState[CategoryTypeViewStateKey] != null)
                {
                    return (CategoryType)ViewState[CategoryTypeViewStateKey];
                }
                throw new InvalidOperationException(Resources.InvalidOperation_CategoryTypeNotSet);
            }
            set { ViewState[CategoryTypeViewStateKey] = value; }
        }

        //This is true if we came from a pencil edit link while viewing the post 
        //from outside the admin tool.
        private bool ReturnToOriginalPost
        {
            get { return (Request.QueryString["return-to-post"] == "true"); }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategoryList();
                SetEditorMode();

                if (PostId != null)
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

        private void BindCategoryList()
        {
            cklCategories.DataSource = Repository.GetCategories(CategoryType, ActiveFilter.None);
            cklCategories.DataValueField = "Id";
            cklCategories.DataTextField = "Title";
            cklCategories.DataBind();
        }

        private void SetConfirmation()
        {
            var confirmPage = (ConfirmationPage)Page;
            confirmPage.IsInEdit = true;
            confirmPage.Message = Resources.Message_YouWillLoseUnsavedContent;

            lkbPost.Attributes.Add("OnClick", ConfirmationPage.BypassFunctionName);
            lkUpdateCategories.Attributes.Add("OnClick", ConfirmationPage.BypassFunctionName);
            lkbCancel.Attributes.Add("OnClick", ConfirmationPage.BypassFunctionName);
        }

        private void BindPostCreate()
        {
            txbTitle.Text = string.Empty;
            richTextEditor.Text = string.Empty;

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
            Debug.Assert(PostId != null, "PostId Should not be null when we call this");

            SetConfirmation();

            Entry entry = GetEntryForEditing(PostId.Value);
            if (entry == null)
            {
                ReturnToOrigin(null);
                return;
            }

            txbTitle.Text = entry.Title;
            if (!entry.DatePublishedUtc.IsNull() && entry.DatePublishedUtc > DateTime.UtcNow)
            {
                txtPostDate.Text = entry.DateSyndicated.ToString(CultureInfo.CurrentCulture);
            }

            VirtualPath entryUrl = Url.EntryUrl(entry);
            if (entryUrl != null)
            {
                hlEntryLink.NavigateUrl = entryUrl;
                hlEntryLink.Text = entryUrl.ToFullyQualifiedUrl(Config.CurrentBlog).ToString();
                hlEntryLink.Attributes.Add("title", "view: " + entry.Title);
            }
            else
                hlEntryLink.Text = "This post has not been published yet, so it doesn't have an URL";

            PopulateMimeTypeDropDown();
            //Enclosures
            if (entry.Enclosure != null)
            {
                Enclosure.Collapsed = false;
                txbEnclosureTitle.Text = entry.Enclosure.Title;
                txbEnclosureUrl.Text = entry.Enclosure.Url;
                txbEnclosureSize.Text = entry.Enclosure.Size.ToString();
                if (ddlMimeType.Items.FindByText(entry.Enclosure.MimeType) != null)
                {
                    ddlMimeType.SelectedValue = entry.Enclosure.MimeType;
                }
                else
                {
                    ddlMimeType.SelectedValue = "other";
                    txbEnclosureOtherMimetype.Text = entry.Enclosure.MimeType;
                }
                ddlAddToFeed.SelectedValue = entry.Enclosure.AddToFeed.ToString().ToLower();
                ddlDisplayOnPost.SelectedValue = entry.Enclosure.ShowWithPost.ToString().ToLower();
            }

            chkComments.Checked = entry.AllowComments;
            chkCommentsClosed.Checked = entry.CommentingClosed;
            SetCommentControls();
            if (entry.CommentingClosedByAge)
            {
                chkCommentsClosed.Enabled = false;
            }

            chkDisplayHomePage.Checked = entry.DisplayOnHomePage;
            chkMainSyndication.Checked = entry.IncludeInMainSyndication;
            chkSyndicateDescriptionOnly.Checked = entry.SyndicateDescriptionOnly;
            chkIsAggregated.Checked = entry.IsAggregated;

            // Advanced Options
            txbEntryName.Text = entry.EntryName;
            txbExcerpt.Text = entry.Description;

            SetEditorText(entry.Body);

            ckbPublished.Checked = entry.IsActive;

            BindCategoryList();
            for (int i = 0; i < cklCategories.Items.Count; i++)
            {
                cklCategories.Items[i].Selected = false;
            }

            ICollection<Link> postCategories = Repository.GetLinkCollectionByPostId(PostId.Value);
            if (postCategories.Count > 0)
            {
                foreach (Link postCategory in postCategories)
                {
                    ListItem categoryItem =
                        cklCategories.Items.FindByValue(postCategory.CategoryId.ToString(CultureInfo.InvariantCulture));
                    if (categoryItem == null)
                    {
                        throw new InvalidOperationException(
                            string.Format(Resources.EntryEditor_CouldNotFindCategoryInList, postCategory.CategoryId,
                                          cklCategories.Items.Count));
                    }
                    categoryItem.Selected = true;
                }
            }

            SetEditorMode();
            Advanced.Collapsed = !Preferences.AlwaysExpandAdvanced;

            var adminMasterPage = Page.Master as AdminPageTemplate;
            if (adminMasterPage != null)
            {
                string title = string.Format(CultureInfo.InvariantCulture, Resources.EntryEditor_EditingTitle,
                                             CategoryType == CategoryType.StoryCollection
                                                 ? Resources.Label_Article
                                                 : Resources.Label_Post, entry.Title);
                adminMasterPage.Title = title;
            }

            if (entry.HasEntryName)
            {
                Advanced.Collapsed = false;
                txbEntryName.Text = entry.EntryName;
            }
        }

        private void PopulateMimeTypeDropDown()
        {
            ddlMimeType.Items.Add(new ListItem(Resources.Label_Choose, "none"));
            foreach (string key in MimeTypesMapper.Mappings.List)
            {
                ddlMimeType.Items.Add(new ListItem(MimeTypesMapper.Mappings.List[key],
                                                   MimeTypesMapper.Mappings.List[key]));
            }
            ddlMimeType.Items.Add(new ListItem(Resources.Label_Other, "other"));
        }

        private void SetCommentControls()
        {
            if (!Blog.CommentsEnabled)
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
            if (ReturnToOriginalPost && PostId != null)
            {
                // We came from outside the post, let's go there.
                Entry updatedEntry = Repository.GetEntry(PostId.Value, true /*activeOnly*/, false /*includeCategories*/);
                if (updatedEntry != null)
                {
                    Response.Redirect(Url.EntryUrl(updatedEntry));
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

            vCustomPostDate.IsValid = string.IsNullOrEmpty(txtPostDate.Text) || DateTime.TryParse(txtPostDate.Text, out postDate);

            EnableEnclosureValidation(EnclosureEnabled());

            if (Page.IsValid)
            {
                string successMessage = Constants.RES_SUCCESSNEW;

                try
                {
                    Entry entry;
                    if (PostId == null)
                    {
                        ValidateEntryTypeIsNotNone(EntryType);
                        entry = new Entry(EntryType);
                    }
                    else
                    {
                        entry = GetEntryForEditing(PostId.Value);
                        if (entry.PostType != EntryType)
                        {
                            EntryType = entry.PostType;
                        }
                    }

                    entry.Title = txbTitle.Text;
                    entry.Body = richTextEditor.Xhtml;
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
                        enc.MimeType = ddlMimeType.SelectedValue.Equals("other") ? txbEnclosureOtherMimetype.Text : ddlMimeType.SelectedValue;
                        long size;
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
                    entry.EntryName = txbEntryName.Text.NullIfEmpty();
                    entry.Description = txbExcerpt.Text.NullIfEmpty();
                    entry.Categories.Clear();
                    ReplaceSelectedCategoryNames(entry.Categories);

                    if (!postDate.IsNull())
                    {
                        entry.DatePublishedUtc = Blog.TimeZone.ToUtc(postDate);
                    }

                    if (PostId != null)
                    {
                        successMessage = Constants.RES_SUCCESSEDIT;
                        entry.DateModifiedUtc = DateTime.UtcNow;
                        entry.Id = PostId.Value;

                        var entryPublisher = SubtextContext.ServiceLocator.GetService<IEntryPublisher>();
                        entryPublisher.Publish(entry);

                        if (entry.Enclosure == null && enclosureId != 0)
                        {
                            Repository.DeleteEnclosure(enclosureId);
                        }
                        else if (entry.Enclosure != null && entry.Enclosure.Id != 0)
                        {
                            Repository.Update(entry.Enclosure);
                        }
                        else if (entry.Enclosure != null && entry.Enclosure.Id == 0)
                        {
                            entry.Enclosure.EntryId = entry.Id;
                            Repository.Create(entry.Enclosure);
                        }

                        UpdateCategories();
                    }
                    else
                    {
                        var entryPublisher = SubtextContext.ServiceLocator.GetService<IEntryPublisher>();
                        _postId = entryPublisher.Publish(entry);
                        NotificationServices.Run(entry, Blog, Url);

                        if (entry.Enclosure != null)
                        {
                            entry.Enclosure.EntryId = PostId.Value;
                            Repository.Create(entry.Enclosure);
                        }

                        UpdateCategories();
                        AddCommunityCredits(entry);
                    }
                }
                catch (Exception ex)
                {
                    Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
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
            Debug.Assert(entryType != PostType.None, "The entry type is none. This should be impossible!");
        }

        private bool EnclosureEnabled()
        {
            if (!String.IsNullOrEmpty(txbEnclosureUrl.Text))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(txbEnclosureTitle.Text))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(txbEnclosureSize.Text))
            {
                return true;
            }
            return ddlMimeType.SelectedIndex > 0;
        }

        private void EnableEnclosureValidation(bool enabled)
        {
            valEncSizeRequired.Enabled = enabled;
            valEncUrlRequired.Enabled = enabled;
            valEncMimeTypeRequired.Enabled = enabled;

            valEncOtherMimetypeRequired.Enabled = enabled && ddlMimeType.SelectedValue.Equals("other");
        }

        private void ReplaceSelectedCategoryNames(ICollection<string> sc)
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
                if (PostId != null)
                {
                    string successMessage = Constants.RES_SUCCESSCATEGORYUPDATE;
                    var al = new List<int>();

                    foreach (ListItem item in cklCategories.Items)
                    {
                        if (item.Selected)
                        {
                            al.Add(int.Parse(item.Value));
                        }
                    }
                    Repository.SetEntryCategoryList(PostId.Value, al);
                    return successMessage;
                }

                Messages.ShowError(Constants.RES_FAILURECATEGORYUPDATE + Resources.EntryEditor_ProblemEditingPostCategories);
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
                                                 Constants.RES_FAILUREEDIT, ex.Message));
            }
            return null;
        }

        private void SetEditorMode()
        {
            if (CategoryType == CategoryType.StoryCollection)
            {
                chkDisplayHomePage.Visible = false;
                chkIsAggregated.Visible = false;
                chkMainSyndication.Visible = false;
                chkSyndicateDescriptionOnly.Visible = false;
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
            lkbPost.Click += OnUpdatePostClick;
            lkUpdateCategories.Click += OnUpdateCategoriesClick;
            lkbCancel.Click += OnCancelClick;
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            if (PostId != null && ReturnToOriginalPost)
            {
                // We came from outside the post, let's go there.
                Entry updatedEntry = Repository.GetEntry(PostId.Value, true /* activeOnly */, false
                    /* includeCategories */);
                if (updatedEntry != null)
                {
                    Response.Redirect(Url.EntryUrl(updatedEntry));
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
            Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "TODO...", e.Exception.Message));
        }

        private void AddCommunityCredits(Entry entry)
        {
            try
            {
                CommunityCreditNotification.AddCommunityCredits(entry, Url, Blog);
            }
            catch (CommunityCreditNotificationException ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
                                                 Resources.EntryEditor_ErrorSendingToCommunityCredits, ex.Message));
            }
            catch (Exception ex)
            {
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
                                                 Resources.EntryEditor_ErrorSendingToCommunityCredits, ex.Message));
            }
        }

        private Entry GetEntryForEditing(int id)
        {
            var entry = Repository.GetEntry(id, false /*activeOnly*/, false /*includeCategories*/);
            entry.Blog = Blog;
            return entry;
        }
    }
}