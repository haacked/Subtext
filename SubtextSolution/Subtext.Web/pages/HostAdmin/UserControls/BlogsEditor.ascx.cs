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
		const string VSKEY_BLOGID = "VS_BLOGID";
		const string VSKEY_ALIASID = "VSKEY_ALIAS";
		
		int pageIndex = 0;

		#region Declared Controls
		protected Button btnAddNewBlog = new Button();
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddNewBlog.Click += btnAddNewBlog_Click;
			
			btnAddNewBlog.CssClass = "button";
			btnAddNewBlog.Text = Resources.BlogsEditor_NewBlogLabel;
			((HostAdminTemplate)this.Page.Master).AddSidebarControl(btnAddNewBlog);

            //Paging...
            if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
                this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]); 
            
            if (!IsPostBack)
			{
				resultsPager.PageSize = Preferences.ListingItemCount;
				resultsPager.PageIndex = this.pageIndex;
				pnlResults.Collapsible = false;
				this.chkShowInactive.Checked = false;
			}
			BindList();
		}

        public string GetBlogUrl(object dataItem) {
            Blog blog = dataItem as Blog;
            return Url.BlogUrl(blog);
        }

        public UrlHelper Url {
            get {
                if (_urlHelper == null) {
                    _urlHelper = new UrlHelper(null, null);
                }
                return _urlHelper;
            }
        }
        UrlHelper _urlHelper = null;

        private void BindGroups()
        {
            ddlGroups.DataSource = Config.ListBlogGroups(false);
            ddlGroups.DataBind();
        }

		private void BindList()
		{
			this.pnlResults.Visible = true;
			this.pnlEdit.Visible = false;

            IPagedCollection<Blog> blogs; 
			
			ConfigurationFlags configFlags = this.chkShowInactive.Checked ? ConfigurationFlags.None : ConfigurationFlags.IsActive;
			
			blogs = Blog.GetBlogs(this.pageIndex, resultsPager.PageSize, configFlags);
			
			if (blogs.Count > 0)
			{
				this.resultsPager.Visible = true;
				this.resultsPager.ItemCount = blogs.MaxItems;
				this.rprBlogsList.Visible = true;
				this.rprBlogsList.DataSource = blogs;
				this.rprBlogsList.DataBind();
				this.lblNoMessages.Visible = false;
			}
			else
			{
				this.resultsPager.Visible = false;
				this.rprBlogsList.Visible = false;
				this.resultsPager.Visible = false;
				this.lblNoMessages.Visible = true;	
			}

			CurrentBlogCount = blogs.MaxItems;
		}

		void BindEdit()
		{
			this.pnlResults.Visible = false;
			this.pnlEdit.Visible = true;

            BindGroups();
			
			BindEditHelp();

			Blog blog;
            if (!CreatingBlog)
            {
                blog = Blog.GetBlogById(BlogId);
                this.txtApplication.Text = blog.Subfolder;
                this.txtHost.Text = blog.Host;
                this.txtUsername.Text = blog.UserName;
                this.txtTitle.Text = blog.Title;
                IPagedCollection<BlogAlias> aliases = blog.GetBlogAliases(0, int.MaxValue);
                rprBlogAliasList.DataSource = aliases;
                rprBlogAliasList.DataBind();
                ddlGroups.Items.FindByValue(blog.BlogGroupId.ToString()).Selected = true;
            }
            else
            {
                ListItem item = ddlGroups.Items.FindByValue("1");
                if (item != null)
                    item.Selected = true;
            }
			this.txtTitle.Visible = true;

			string onChangeScript = string.Format(CultureInfo.InvariantCulture, "onPreviewChanged('{0}', '{1}', '{2}', false);", this.txtHost.ClientID, this.txtApplication.ClientID, this.virtualDirectory.ClientID);
			string onBlurScript = string.Format(CultureInfo.InvariantCulture, "onPreviewChanged('{0}', '{1}', '{2}', true);", this.txtHost.ClientID, this.txtApplication.ClientID, this.virtualDirectory.ClientID);

			if(!Page.ClientScript.IsStartupScriptRegistered("SetUrlPreview"))
			{
				string startupScript = "<script type=\"text/javascript\">" 
					+ Environment.NewLine 
					+ onBlurScript 
					+ Environment.NewLine 
					+ "</script>";

				Type ctype = this.GetType();
				Page.ClientScript.RegisterStartupScript(ctype,"SetUrlPreview", startupScript);
			}

			this.txtApplication.Attributes["onkeyup"] = onChangeScript;
			this.txtApplication.Attributes["onblur"] = onBlurScript;
			this.txtHost.Attributes["onkeyup"] = onChangeScript;
			this.txtHost.Attributes["onblur"] = onBlurScript;

			this.virtualDirectory.Value = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);
		}

		// Contains the various help strings
		void BindEditHelp()
		{
            this.blogEditorHelp.HelpText = Resources.BlogsEditor_HelpText;
            this.hostDomainHelpTip.HelpText = Resources.BlogsEditor_HostDomainHelpText;
            this.applicationHelpTip.HelpText = Resources.BlogsEditor_ApplicationHelpTip;
		}

		/// <summary>
		/// Gets or sets the blog id.
		/// </summary>
		/// <value></value>
		public int BlogId
		{
			get
			{
				if (ViewState[VSKEY_BLOGID] != null)
					return (int)ViewState[VSKEY_BLOGID];
				else
					return NullValue.NullInt32;
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
				if (ViewState[VSKEY_ALIASID] != null)
					return (int)ViewState[VSKEY_ALIASID];
				else
					return NullValue.NullInt32;
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
					return (int)ViewState["VS_CurrentBlogCount"];
				else
					return NullValue.NullInt32;
			}
			set { ViewState["VS_CurrentBlogCount"] = value; }
		}

		bool CreatingBlog
		{
			get
			{
				return BlogId == NullValue.NullInt32;
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

		protected void chkShowInactive_CheckedChanged(object sender, EventArgs e)
		{
			BindList();
		}

		private void btnAddNewBlog_Click(object sender, EventArgs e)
		{
			this.BlogId = NullValue.NullInt32;
			this.txtTitle.Text = string.Empty;
			this.txtApplication.Text = string.Empty;
			this.txtHost.Text = string.Empty;
			this.txtUsername.Text = string.Empty;
			BindEdit();
		}

		protected void rprBlogsList_ItemCommand(object source, RepeaterCommandEventArgs e)
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
					this.messagePanel.ShowError(e.Message);
				}
			}
			BindEdit();
		}

		// Saves a new blog.  Any exceptions are propagated up to the caller.
		void SaveNewBlog()
		{
			if(Config.CreateBlog(this.txtTitle.Text, this.txtUsername.Text, this.txtPassword.Text, this.txtHost.Text, this.txtApplication.Text, Int32.Parse(ddlGroups.SelectedValue)) > 0)
			{
				this.messagePanel.ShowMessage(Resources.BlogsEditor_BlogCreated);
			}
			else
			{
                this.messagePanel.ShowError(Resources.Message_UnexpectedError);
			}		
		}

		// Saves changes to a blog.  Any exceptions are propagated up to the caller.
		void SaveBlogEdits()
		{
			Blog blog = Blog.GetBlogById(BlogId);
			
			if(blog == null)
				throw new ArgumentNullException("BlogId");

			blog.Title = this.txtTitle.Text;
			blog.Host = this.txtHost.Text;
			blog.Subfolder = this.txtApplication.Text;
			blog.UserName = this.txtUsername.Text;
            blog.BlogGroupId = Int32.Parse(ddlGroups.SelectedValue);

			if(this.txtPassword.Text.Length > 0)
			{
				blog.Password = SecurityHelper.HashPassword(this.txtPassword.Text);
			}
			
            try
            {
			    Config.UpdateConfigData(blog);
                this.messagePanel.ShowMessage(Resources.BlogsEditor_BlogSaved);
			}
            catch(Exception)
            {
				this.messagePanel.ShowError(Resources.Message_UnexpectedError);
			}
		}

		protected virtual bool PageIsValid
		{
			get
			{
				bool isValidSoFar = true;

				if(CreatingBlog)
				{
					if(IsTextBoxEmpty(this.txtPassword))
					{
						isValidSoFar = false;
						this.messagePanel.ShowError(Resources.BlogsEditor_PasswordRequired + "<br />", true);
					}
					isValidSoFar = isValidSoFar && ValidateRequiredField(this.txtTitle, "Title");
				}

				if(this.txtPassword.Text != this.txtPasswordConfirm.Text)
				{
					isValidSoFar = false;
					this.messagePanel.ShowError(Resources.BlogsEditor_PasswordsDoNotMatch + "<br />", false);	
				}

				// Use of single & is intentional to stop short cirtuited evaluation.
				return isValidSoFar 
					& ValidateRequiredField(this.txtHost, "Host Domain") 
					& ValidateRequiredField(this.txtUsername, "Username")
					& ValidateFieldLength(this.txtHost, "Host Domain", 100)
					& ValidateFieldLength(this.txtApplication, "Subfolder", 50)
					& ValidateFieldLength(this.txtUsername, "Username", 50)
					& ValidateFieldLength(this.txtPassword, "Password", 50)
					& ValidateFieldLength(this.txtTitle, "Title", 100);
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
				this.messagePanel.ShowError(String.Format(CultureInfo.InvariantCulture, Resources.BlogsEditor_FieldRequired, fieldName) + "<br />", false);
				return false;
			}
			return true;
		}

		bool ValidateFieldLength(ITextControl textbox, string fieldName, int maxLength)
		{
			if(textbox.Text.Length > maxLength)
			{
				this.messagePanel.ShowError(String.Format(Resources.BlogsEditor_ValueTooLong, fieldName, maxLength) + "<br />", false);
				return false;
			}
			return true;
		}

		protected static string ToggleActiveString(bool active)
		{
			if(active)
				return Resources.Label_Deactivate;
			else
				return Resources.Label_Activate;
		}

		void ToggleActive()
		{
			Blog blog = Blog.GetBlogById(BlogId);
			blog.IsActive = !blog.IsActive;
			try
			{
				Config.UpdateConfigData(blog);
				if(blog.IsActive) {
					this.messagePanel.ShowMessage(Resources.BlogsEditor_BlogActivated);
				}
				else {
					this.messagePanel.ShowMessage(Resources.BlogsEditor_BlogDeactivated);
				}
			}
			catch(BaseBlogConfigurationException e)
			{
				this.messagePanel.ShowError(e.Message);
			}

			BindList();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.messagePanel.ShowMessage(Resources.BlogsEditor_UpdateCancelled);
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
			CommandEventArgs args = (CommandEventArgs)e;
			BlogAlias alias = Config.GetBlogAlias(Convert.ToInt32(args.CommandArgument));
			if (args.CommandName == "EditAlias")
			{

				AliasId = alias.Id;
				BindEdit();
				SetAliasEdit(true);
				txtAliasHost.Text = alias.Host;
				txtAliasApplication.Text = alias.Subfolder;
				cbAliasActive.Checked = alias.IsActive;
				
				Config.UpdateBlogAlias(alias);
			}

			if (args.CommandName == "DeleteAlias")
			{
				AliasId = NullValue.NullInt32;
				Config.DeleteBlogAlias(alias);
				BindEdit();
				SetAliasEdit(false);
			}

		}
		protected void btnAliasSave_Click(object sender, EventArgs e)
		{

			BlogAlias alias = new BlogAlias();
			if (AliasId != NullValue.NullInt32)
				alias.Id = AliasId;

			alias.Host = txtAliasHost.Text;
			alias.Subfolder = txtAliasApplication.Text;
			alias.BlogId = BlogId;
			alias.IsActive = cbAliasActive.Checked;

			if (AliasId == NullValue.NullInt32)
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
