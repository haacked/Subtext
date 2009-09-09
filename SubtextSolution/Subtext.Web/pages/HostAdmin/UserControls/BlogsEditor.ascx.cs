#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
using Subtext.Framework.Format;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;
using Subtext.Web.Admin;
using Subtext.Web.Properties;

namespace Subtext.Web.HostAdmin.UserControls
{
    /// <summary>
    ///	User control used to create, edit and delete blogs.  
    ///	This only provides a few options for editing blogs. 
    ///	For the full options, one should visit the individual 
    ///	blog's admin tool.
    /// </summary>
    public partial class BlogsEditor : UserControl
    {
        const string VSKEY_ALIASID = "VSKEY_ALIAS";
        const string VSKEY_BLOGID = "VS_BLOGID";
        UrlHelper _urlHelper = null;

        int pageIndex = 0;

        #region Declared Controls

        protected Button btnAddNewBlog = new Button();

        #endregion

        public UrlHelper Url
        {
            get
            {
                if(_urlHelper == null)
                {
                    _urlHelper = new UrlHelper(null, null);
                }
                return _urlHelper;
            }
        }

        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value></value>
        public int BlogId
        {
            get
            {
                if(ViewState[VSKEY_BLOGID] != null)
                {
                    return (int)ViewState[VSKEY_BLOGID];
                }
                else
                {
                    return NullValue.NullInt32;
                }
            }
            set { ViewState[VSKEY_BLOGID] = value; }
        }

        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value></value>
        public int AliasId
        {
            get
            {
                if(ViewState[VSKEY_ALIASID] != null)
                {
                    return (int)ViewState[VSKEY_ALIASID];
                }
                else
                {
                    return NullValue.NullInt32;
                }
            }
            set { ViewState[VSKEY_ALIASID] = value; }
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
                if(ViewState["VS_CurrentBlogCount"] != null)
                {
                    return (int)ViewState["VS_CurrentBlogCount"];
                }
                else
                {
                    return NullValue.NullInt32;
                }
            }
            set { ViewState["VS_CurrentBlogCount"] = value; }
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

                if(CreatingBlog)
                {
                    if(IsTextBoxEmpty(txtPassword))
                    {
                        isValidSoFar = false;
                        messagePanel.ShowError(Resources.BlogsEditor_PasswordRequired + "<br />", true);
                    }
                    isValidSoFar = isValidSoFar && ValidateRequiredField(txtTitle, "Title");
                }

                if(txtPassword.Text != txtPasswordConfirm.Text)
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddNewBlog.Click += btnAddNewBlog_Click;

            btnAddNewBlog.CssClass = "button";
            btnAddNewBlog.Text = Resources.BlogsEditor_NewBlogLabel;
            ((HostAdminTemplate)Page.Master).AddSidebarControl(btnAddNewBlog);

            //Paging...
            if(null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
            {
                pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            if(!IsPostBack)
            {
                resultsPager.PageSize = Preferences.ListingItemCount;
                resultsPager.PageIndex = pageIndex;
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
            ddlGroups.DataSource = Config.ListBlogGroups(false);
            ddlGroups.DataBind();
        }

        private void BindList()
        {
            pnlResults.Visible = true;
            pnlEdit.Visible = false;

            IPagedCollection<Blog> blogs;

            ConfigurationFlags configFlags = chkShowInactive.Checked
                                                 ? ConfigurationFlags.None
                                                 : ConfigurationFlags.IsActive;

            blogs = Blog.GetBlogs(pageIndex, resultsPager.PageSize, configFlags);

            if(blogs.Count > 0)
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
            if(!CreatingBlog)
            {
                blog = Blog.GetBlogById(BlogId);
                txtApplication.Text = blog.Subfolder;
                txtHost.Text = blog.Host;
                txtUsername.Text = blog.UserName;
                txtTitle.Text = blog.Title;
                IPagedCollection<BlogAlias> aliases = blog.GetBlogAliases(0, int.MaxValue);
                rprBlogAliasList.DataSource = aliases;
                rprBlogAliasList.DataBind();
                ddlGroups.Items.FindByValue(blog.BlogGroupId.ToString()).Selected = true;
            }
            else
            {
                ListItem item = ddlGroups.Items.FindByValue("1");
                if(item != null)
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

            if(!Page.ClientScript.IsStartupScriptRegistered("SetUrlPreview"))
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

            virtualDirectory.Value = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
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

        private void btnAddNewBlog_Click(object sender, EventArgs e)
        {
            BlogId = NullValue.NullInt32;
            txtTitle.Text = string.Empty;
            txtApplication.Text = string.Empty;
            txtHost.Text = string.Empty;
            txtUsername.Text = string.Empty;
            BindEdit();
        }

        protected void rprBlogsList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch(e.CommandName.ToLower(CultureInfo.InvariantCulture))
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
            if(PageIsValid)
            {
                try
                {
                    if(BlogId != NullValue.NullInt32)
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
                catch(BaseBlogConfigurationException e)
                {
                    messagePanel.ShowError(e.Message);
                }
            }
            BindEdit();
        }

        // Saves a new blog.  Any exceptions are propagated up to the caller.
        void SaveNewBlog()
        {
            if(
                Config.CreateBlog(txtTitle.Text, txtUsername.Text, txtPassword.Text, txtHost.Text, txtApplication.Text,
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
            Blog blog = Blog.GetBlogById(BlogId);

            if(blog == null)
            {
                throw new ArgumentNullException("BlogId");
            }

            blog.Title = txtTitle.Text;
            blog.Host = txtHost.Text;
            blog.Subfolder = txtApplication.Text;
            blog.UserName = txtUsername.Text;
            blog.BlogGroupId = Int32.Parse(ddlGroups.SelectedValue);

            if(txtPassword.Text.Length > 0)
            {
                blog.Password = SecurityHelper.HashPassword(txtPassword.Text);
            }

            try
            {
                Config.UpdateConfigData(blog);
                messagePanel.ShowMessage(Resources.BlogsEditor_BlogSaved);
            }
            catch(Exception)
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
            if(IsTextBoxEmpty(textbox))
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
            if(textbox.Text.Length > maxLength)
            {
                messagePanel.ShowError(
                    String.Format(Resources.BlogsEditor_ValueTooLong, fieldName, maxLength) + "<br />", false);
                return false;
            }
            return true;
        }

        protected static string ToggleActiveString(bool active)
        {
            if(active)
            {
                return Resources.Label_Deactivate;
            }
            else
            {
                return Resources.Label_Activate;
            }
        }

        void ToggleActive()
        {
            Blog blog = Blog.GetBlogById(BlogId);
            blog.IsActive = !blog.IsActive;
            try
            {
                Config.UpdateConfigData(blog);
                if(blog.IsActive)
                {
                    messagePanel.ShowMessage(Resources.BlogsEditor_BlogActivated);
                }
                else
                {
                    messagePanel.ShowMessage(Resources.BlogsEditor_BlogDeactivated);
                }
            }
            catch(BaseBlogConfigurationException e)
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
            rprBlogAliasList.Visible = !editing;
        }

        protected void lbAddAlias_OnClick(object sender, EventArgs e)
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

        protected void rprBlogAliasList_ItemCommand(object sender, EventArgs e)
        {
            var args = (CommandEventArgs)e;
            BlogAlias alias = Config.GetBlogAlias(Convert.ToInt32(args.CommandArgument));
            if(args.CommandName == "EditAlias")
            {
                AliasId = alias.Id;
                BindEdit();
                SetAliasEdit(true);
                txtAliasHost.Text = alias.Host;
                txtAliasApplication.Text = alias.Subfolder;
                cbAliasActive.Checked = alias.IsActive;

                Config.UpdateBlogAlias(alias);
            }

            if(args.CommandName == "DeleteAlias")
            {
                AliasId = NullValue.NullInt32;
                Config.DeleteBlogAlias(alias);
                BindEdit();
                SetAliasEdit(false);
            }
        }

        protected void btnAliasSave_Click(object sender, EventArgs e)
        {
            var alias = new BlogAlias();
            if(AliasId != NullValue.NullInt32)
            {
                alias.Id = AliasId;
            }

            alias.Host = txtAliasHost.Text;
            alias.Subfolder = txtAliasApplication.Text;
            alias.BlogId = BlogId;
            alias.IsActive = cbAliasActive.Checked;

            if(AliasId == NullValue.NullInt32)
            {
                Config.AddBlogAlias(alias);
            }
            else
            {
                Config.UpdateBlogAlias(alias);
            }

            AliasId = NullValue.NullInt32;
            BindEdit();
            SetAliasEdit(false);
        }
    }
}