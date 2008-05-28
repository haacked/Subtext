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
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Web.Admin;
using Subtext.Framework.Security;
using Subtext.Framework.Providers;
using System.Data.SqlClient;

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
			btnAddNewBlog.Text = "New Blog";
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

        private void BindGroups()
        {
            ddlGroups.DataSource = Config.ListBlogGroups(false);
            ddlGroups.DataBind();
        }

		private void BindList()
		{
			this.pnlResults.Visible = true;
			this.pnlEdit.Visible = false;

            IPagedCollection<BlogInfo> blogs; 
			
			ConfigurationFlags configFlags = this.chkShowInactive.Checked ? ConfigurationFlags.None : ConfigurationFlags.IsActive;
			
			blogs = BlogInfo.GetBlogs(this.pageIndex, resultsPager.PageSize, configFlags);
			
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

			BlogInfo blog;
            if (!CreatingBlog)
            {
                blog = BlogInfo.GetBlogById(BlogId);
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
			#region Help Tool Tip Text
			this.blogEditorHelp.HelpText = "<p>Use this page to manage the blogs installed on this server. " 
				+ "For more information on configuring blogs, see the <a href=\'http://www.subtextproject.com/Home/Docs/Configuration/tabid/112/Default.aspx\' target=\'_blank\'>configuration docs</a> (opens a new window)."
				+ "</p>";

			this.hostDomainHelpTip.HelpText = "<p><strong>Host Domain</strong> is the domain name for this blog. "
				+ "If you never plan on setting up another blog on this server, then you do not have " 
				+ "to worry about this setting.  However, if you decide to add another blog at a later "
				+ "time, it&#8217;s important to update this setting for your initial blog.</p>"
				+ "<p>For example, if you are hosting this blog at http://www.example.com/, the Host Domain "
				+ "would be &#8220;www.example.com&#8221;.</p><p>If you are trying to set this up on your " 
				+ "own machine for testing purposes (i.e. it&#8217;s not publicly viewable, you might try " 
				+ "&#8220;localhost&#8221; for the host domain.</p>" 
				+ "<p><strong>Important:</strong>If you are setting up multiple blogs on the same server, "
				+ "multiple blogs may have the same Host Domain name if they don&#8217;t also have the same " 
				+ "Subfolder name.  However, two blogs with different Host Domains may have the same Subfolder "
				+ "name (though that&#8217;s not recommended)."
				+ "</p><p>Also, if there are multiple blogs with the same Host Domain Name, they must all "
				+ "have a non-empty Subfolder name defined.  For more detailed coverage " 
				+ ", please visit <a href=\'http://www.subtextproject.com/Developer/UrlToBlogMappings/tabid/119/Default.aspx\' target=\'_blank\'>the multiple blog configuration docs</a> (opens a new window)."
				+ "</p>";

			this.applicationHelpTip.HelpText = "<p>"
				+ "<strong>This sets the subfolder of the host domain that the blog will appear to be located in.</strong>"
				+ "</p>"
				+ "<p>For example, if you enter &#8220;MyBlog&#8221; " 
				+ "(sans quotes of course) for the application, then the root URL to your blog "
				+ "would be <em>http://[HOSTDOMAIN]/MyBlog/Default.aspx</em>"
				+ "</p>"
				+ "<p>"
				+ "Leave this value blank if you wish to host your blog in the root of your website."
				+ "</p>"
				+ "<p><strong>NOTE:</strong> If you specify a sub-folder, you do not need to set up a virtual directory "
				+ "corresponding to the subfolder. But not doing so will require that the blog url include the trailing " 
				+ "&#8220;Default.aspx&#8221; or you will have to <a href=\'http://www.subtextproject.com/Configuration/ConfiguringACustom404Page/tabid/121/Default.aspx\' target=\'_blank\'>configure a Custom 404 page</a> (opens a new window).</p>" 
				+ "<p>In the above example, if you want the URL to be <em>http://[HOSTDOMAIN]/MyBlog/</em> then you "
				+ "would simply set up a virtual directory (not application) named &#8220;MyBlog&#8221; pointing to the same location as the webroot.</p>" 
				+ "<p>For more information, please view <a href=\'http://www.subtextproject.com/Home/Docs/Configuration/tabid/112/Default.aspx\' target=\'_blank\'>the configuration docs</a> (opens a new window).</p>";
			#endregion
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
			if(Config.CreateBlog(this.txtTitle.Text, this.txtUsername.Text, this.txtPassword.Text, this.txtHost.Text, this.txtApplication.Text, Int32.Parse(ddlGroups.SelectedValue)))
			{
				this.messagePanel.ShowMessage("Blog Created.");
			}
			else
			{
				this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Sorry.");
			}		
		}

		// Saves changes to a blog.  Any exceptions are propagated up to the caller.
		void SaveBlogEdits()
		{
			BlogInfo blog = BlogInfo.GetBlogById(BlogId);
			
			if(blog == null)
				throw new ArgumentNullException("BlogId", "Ok, somehow the blog you were editing is now null.  This is very odd.");

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
		        this.messagePanel.ShowMessage("Blog Saved.");
			}
            catch(Exception)
            {
				this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Sorry.");
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
						this.messagePanel.ShowError("A  password is required when creating a blog.  Pick a good one.<br />", true);
					}
					isValidSoFar = isValidSoFar && ValidateRequiredField(this.txtTitle, "Title");
				}

				if(this.txtPassword.Text != this.txtPasswordConfirm.Text)
				{
					isValidSoFar = false;
					this.messagePanel.ShowError("The Password and Confirmation do not match.  Try retyping your password in both fields.<br />", false);	
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
				this.messagePanel.ShowError("Emptiness is quite Zen. Still, please enter a value for " + fieldName + ".<br />", false);
				return false;
			}
			return true;
		}

		bool ValidateFieldLength(ITextControl textbox, string fieldName, int maxLength)
		{
			if(textbox.Text.Length > maxLength)
			{
				this.messagePanel.ShowError("Brevity is rewarded.  " + fieldName + " may only have " + maxLength + " characters.<br />", false);
				return false;
			}
			return true;
		}

		protected static string ToggleActiveString(bool active)
		{
			if(active)
				return "Deactivate";
			else
				return "Activate";
		}

		void ToggleActive()
		{
			BlogInfo blog = BlogInfo.GetBlogById(BlogId);
			blog.IsActive = !blog.IsActive;
			try
			{
				Config.UpdateConfigData(blog);
				if(blog.IsActive)
				{
					this.messagePanel.ShowMessage("Blog Activated and ready to go.");
				}
				else
				{
					this.messagePanel.ShowMessage("Blog Inactivated and sent to a retirement community.");
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
			this.messagePanel.ShowMessage("Blog Update Cancelled. Nothing to see here.");
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
			txtAliasApplication.Text = "";
			txtAliasHost.Text = "";
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
