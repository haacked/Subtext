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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using Subtext.Framework.Web;
using Subtext.Web.Admin;
using Subtext.Web.Admin.WebUI.Controls;
using Subtext.Web.Properties;

namespace Subtext.Web.HostAdmin.UserControls
{
    /// <summary>
    ///	User control used to create, edit and delete blogs.  
    ///	This only provides a few options for editing blogs. 
    ///	For the full options, one should visit the individual 
    ///	blog's admin tool.
    /// </summary>
    public partial class BlogsEditor : BaseUserControl
    {
        int _pageIndex;

        protected Button AddNewBlogButton = new Button();

        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value></value>
        public int BlogId
        {
            get
            {
                if (ViewState["BlogId"] != null)
                {
                    return (int)ViewState["BlogId"];
                }
                return NullValue.NullInt32;
            }
            set { ViewState["BlogId"] = value; }
        }

        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value></value>
        public int AliasId
        {
            get
            {
                if (ViewState["AliasId"] != null)
                {
                    return (int)ViewState["AliasId"];
                }
                return NullValue.NullInt32;
            }
            set { ViewState["AliasId"] = value; }
        }

        /// <summary>
        /// Gets or sets the number of blogs 
        /// currently in the system.
        /// </summary>
        /// <value></value>
        public int CurrentBlogCount
        {
            get
            {
                if (ViewState["CurrentBlogCount"] != null)
                {
                    return (int)ViewState["CurrentBlogCount"];
                }
                return NullValue.NullInt32;
            }
            set { ViewState["CurrentBlogCount"] = value; }
        }

        bool CreatingBlog
        {
            get { return BlogId == NullValue.NullInt32; }
        }

        protected virtual bool PageIsValid
        {
            get
            {
                bool isValidSoFar = true;

                if (CreatingBlog)
                {
                    if (IsTextBoxEmpty(txtPassword))
                    {
                        isValidSoFar = false;
                        messagePanel.ShowError(Resources.BlogsEditor_PasswordRequired + "<br />", true);
                    }
                    isValidSoFar = isValidSoFar && ValidateRequiredField(txtTitle, "Title");
                }

                if (txtPassword.Text != txtPasswordConfirm.Text)
                {
                    isValidSoFar = false;
                    messagePanel.ShowError(Resources.BlogsEditor_PasswordsDoNotMatch + "<br />", false);
                }

                // Use of single & is intentional to stop short cirtuited evaluation.
                return isValidSoFar
                       & ValidateRequiredField(txtHost, "Host Domain")
                       & ValidateRequiredField(txtUsername, "Username")
                       & ValidateFieldLength(txtHost, "Host Domain", 100)
                       & ValidateFieldLength(txtApplication, "Subfolder", 50)
                       & ValidateFieldLength(txtUsername, "Username", 50)
                       & ValidateFieldLength(txtPassword, "Password", 50)
                       & ValidateFieldLength(txtTitle, "Title", 100);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AddNewBlogButton.Click += OnAddNewBlogClick;

            AddNewBlogButton.CssClass = "button";
            AddNewBlogButton.Text = Resources.BlogsEditor_NewBlogLabel;
            ((HostAdminTemplate)Page.Master).AddSidebarControl(AddNewBlogButton);

            //Paging...
            if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
            {
                _pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            if (!IsPostBack)
            {
                resultsPager.PageSize = Preferences.ListingItemCount;
                resultsPager.PageIndex = _pageIndex;
                pnlResults.Collapsible = false;
                chkShowInactive.Checked = false;
            }
            BindList();
        }

        public string GetBlogUrl(object dataItem)
        {
            var blog = dataItem as Blog;
            return Url.BlogUrl(blog);
        }

        private void BindGroups()
        {
            ddlGroups.DataSource = Repository.ListBlogGroups(false);
            ddlGroups.DataBind();
        }

        private void BindList()
        {
            pnlResults.Visible = true;
            pnlEdit.Visible = false;

            ConfigurationFlags configFlags = chkShowInactive.Checked
                                                 ? ConfigurationFlags.None
                                                 : ConfigurationFlags.IsActive;

            IPagedCollection<Blog> blogs = Repository.GetBlogs(_pageIndex, resultsPager.PageSize, configFlags);

            if (blogs.Count > 0)
            {
                resultsPager.Visible = true;
                resultsPager.ItemCount = blogs.MaxItems;
                rprBlogsList.Visible = true;
                rprBlogsList.DataSource = blogs;
                rprBlogsList.DataBind();
                lblNoMessages.Visible = false;
            }
            else
            {
                resultsPager.Visible = false;
                rprBlogsList.Visible = false;
                resultsPager.Visible = false;
                lblNoMessages.Visible = true;
            }

            CurrentBlogCount = blogs.MaxItems;
        }

        void BindEdit()
        {
            pnlResults.Visible = false;
            pnlEdit.Visible = true;

            BindGroups();

            BindEditHelp();

            Blog blog;
            if (!CreatingBlog)
            {
                blog = Repository.GetBlogById(BlogId);
                txtApplication.Text = blog.Subfolder;
                txtHost.Text = blog.Host;
                txtUsername.Text = blog.UserName;
                txtPassword.Text = txtPasswordConfirm.Text = string.Empty;
                txtTitle.Text = blog.Title;
                IPagedCollection<BlogAlias> aliases = blog.GetBlogAliases(Repository, 0, int.MaxValue);
                blogAliasListRepeater.DataSource = aliases;
                blogAliasListRepeater.DataBind();
                ddlGroups.Items.FindByValue(blog.BlogGroupId.ToString()).Selected = true;
            }
            else
            {
                ListItem item = ddlGroups.Items.FindByValue("1");
                if (item != null)
                {
                    item.Selected = true;
                }
            }
            txtTitle.Visible = true;

            string onChangeScript = string.Format(CultureInfo.InvariantCulture,
                                                  "onPreviewChanged('{0}', '{1}', '{2}', false);", txtHost.ClientID,
                                                  txtApplication.ClientID, virtualDirectory.ClientID);
            string onBlurScript = string.Format(CultureInfo.InvariantCulture,
                                                "onPreviewChanged('{0}', '{1}', '{2}', true);", txtHost.ClientID,
                                                txtApplication.ClientID, virtualDirectory.ClientID);

            if (!Page.ClientScript.IsStartupScriptRegistered("SetUrlPreview"))
            {
                string startupScript = "<script type=\"text/javascript\">"
                                       + Environment.NewLine
                                       + onBlurScript
                                       + Environment.NewLine
                                       + "</script>";

                Type ctype = GetType();
                Page.ClientScript.RegisterStartupScript(ctype, "SetUrlPreview", startupScript);
            }

            txtApplication.Attributes["onkeyup"] = onChangeScript;
            txtApplication.Attributes["onblur"] = onBlurScript;
            txtHost.Attributes["onkeyup"] = onChangeScript;
            txtHost.Attributes["onblur"] = onBlurScript;

            virtualDirectory.Value = HttpHelper.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
        }

        // Contains the various help strings
        void BindEditHelp()
        {
            blogEditorHelp.HelpText = Resources.BlogsEditor_HelpText;
            hostDomainHelpTip.HelpText = Resources.BlogsEditor_HostDomainHelpText;
            applicationHelpTip.HelpText = Resources.BlogsEditor_ApplicationHelpTip;
        }

        protected void chkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            BindList();
        }

        private void OnAddNewBlogClick(object sender, EventArgs e)
        {
            BlogId = NullValue.NullInt32;
            txtTitle.Text = string.Empty;
            txtApplication.Text = string.Empty;
            txtHost.Text = string.Empty;
            txtUsername.Text = string.Empty;
            BindEdit();
        }

        protected void OnBlogItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "edit":
                    BlogId = Convert.ToInt32(e.CommandArgument);
                    BindEdit();
                    break;

                case "toggleactive":
                    BlogId = Convert.ToInt32(e.CommandArgument);
                    ToggleActive();
                    break;

                default:
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            if (PageIsValid)
            {
                try
                {
                    if (BlogId != NullValue.NullInt32)
                    {
                        SaveBlogEdits();
                    }
                    else
                    {
                        SaveNewBlog();
                    }
                    BindList();
                    return;
                }
                catch (BaseBlogConfigurationException e)
                {
                    messagePanel.ShowError(e.Message);
                }
            }
            BindEdit();
        }

        // Saves a new blog.  Any exceptions are propagated up to the caller.
        void SaveNewBlog()
        {
            if (
                Repository.CreateBlog(txtTitle.Text, txtUsername.Text, txtPassword.Text, txtHost.Text, txtApplication.Text,
                                  Int32.Parse(ddlGroups.SelectedValue)) > 0)
            {
                messagePanel.ShowMessage(Resources.BlogsEditor_BlogCreated);
            }
            else
            {
                messagePanel.ShowError(Resources.Message_UnexpectedError);
            }
        }

        // Saves changes to a blog.  Any exceptions are propagated up to the caller.
        void SaveBlogEdits()
        {
            Blog blog = Repository.GetBlogById(BlogId);

            if (blog == null)
            {
                throw new InvalidOperationException("BlogId not valid");
            }

            blog.Title = txtTitle.Text;
            blog.Host = txtHost.Text;
            blog.Subfolder = txtApplication.Text;
            blog.UserName = txtUsername.Text;
            blog.BlogGroupId = Int32.Parse(ddlGroups.SelectedValue);

            if (txtPassword.Text.Length > 0)
            {
                blog.Password = SecurityHelper.HashPassword(txtPassword.Text);
            }

            try
            {
                Repository.UpdateConfigData(blog);
                messagePanel.ShowMessage(Resources.BlogsEditor_BlogSaved);
            }
            catch (Exception)
            {
                messagePanel.ShowError(Resources.Message_UnexpectedError);
            }
        }

        static bool IsTextBoxEmpty(ITextControl textbox)
        {
            return textbox.Text.Length == 0;
        }

        bool ValidateRequiredField(ITextControl textbox, string fieldName)
        {
            if (IsTextBoxEmpty(textbox))
            {
                messagePanel.ShowError(
                    String.Format(CultureInfo.InvariantCulture, Resources.BlogsEditor_FieldRequired, fieldName) +
                    "<br />", false);
                return false;
            }
            return true;
        }

        bool ValidateFieldLength(ITextControl textbox, string fieldName, int maxLength)
        {
            if (textbox.Text.Length > maxLength)
            {
                messagePanel.ShowError(
                    String.Format(Resources.BlogsEditor_ValueTooLong, fieldName, maxLength) + "<br />", false);
                return false;
            }
            return true;
        }

        protected static string ToggleActiveString(bool active)
        {
            if (active)
            {
                return Resources.Label_Deactivate;
            }
            return Resources.Label_Activate;
        }

        void ToggleActive()
        {
            Blog blog = Repository.GetBlogById(BlogId);
            blog.IsActive = !blog.IsActive;
            try
            {
                Repository.UpdateConfigData(blog);
                if (blog.IsActive)
                {
                    messagePanel.ShowMessage(Resources.BlogsEditor_BlogActivated);
                }
                else
                {
                    messagePanel.ShowMessage(Resources.BlogsEditor_BlogDeactivated);
                }
            }
            catch (BaseBlogConfigurationException e)
            {
                messagePanel.ShowError(e.Message);
            }

            BindList();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            messagePanel.ShowMessage(Resources.BlogsEditor_UpdateCancelled);
            BindList();
        }

        protected void SetAliasEdit(bool editing)
        {
            tdAliasHost.Visible = editing;
            btnAliasSave.Visible = editing;
            btnAliasCancel.Visible = editing;
            tdAliasApplication.Visible = editing;
            tbAliasActive.Visible = editing;

            tdAliasList.Visible = !editing;
            btnSave.Visible = !editing;
            btnCancel.Visible = !editing;
            txtHost.Enabled = !editing;
            txtPassword.Enabled = !editing;
            txtPasswordConfirm.Enabled = !editing;
            txtApplication.Enabled = !editing;
            txtTitle.Enabled = !editing;
            txtUsername.Enabled = !editing;
            blogAliasListRepeater.Visible = !editing;
        }

        protected void OnAddAliasOnClick(object sender, EventArgs e)
        {
            BindEdit();
            txtAliasApplication.Text = string.Empty;
            txtAliasHost.Text = string.Empty;
            cbAliasActive.Checked = true;
            SetAliasEdit(true);
        }

        protected void btnAliasCancel_Click(object sender, EventArgs e)
        {
            BindEdit();
            AliasId = NullValue.NullInt32;
            SetAliasEdit(false);
        }

        protected void OnItemCommand(object sender, EventArgs e)
        {
            var args = (CommandEventArgs)e;
            BlogAlias alias = Repository.GetBlogAlias(Convert.ToInt32(args.CommandArgument));
            if (args.CommandName == "EditAlias")
            {
                AliasId = alias.Id;
                BindEdit();
                SetAliasEdit(true);
                txtAliasHost.Text = alias.Host;
                txtAliasApplication.Text = alias.Subfolder;
                cbAliasActive.Checked = alias.IsActive;

                Repository.UpdateBlogAlias(alias);
            }

            if (args.CommandName == "DeleteAlias")
            {
                AliasId = NullValue.NullInt32;
                Repository.DeleteBlogAlias(alias);
                BindEdit();
                SetAliasEdit(false);
            }
        }

        protected void btnAliasSave_Click(object sender, EventArgs e)
        {
            var alias = new BlogAlias();
            if (AliasId != NullValue.NullInt32)
            {
                alias.Id = AliasId;
            }

            alias.Host = txtAliasHost.Text;
            alias.Subfolder = txtAliasApplication.Text;
            alias.BlogId = BlogId;
            alias.IsActive = cbAliasActive.Checked;

            if (AliasId == NullValue.NullInt32)
            {
                Repository.AddBlogAlias(alias);
            }
            else
            {
                Repository.UpdateBlogAlias(alias);
            }

            AliasId = NullValue.NullInt32;
            BindEdit();
            SetAliasEdit(false);
        }
    }
}