using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Web.Admin;
using Subtext.Web.Controls;

namespace Subtext.Web.HostAdmin.UserControls
{
	/// <summary>
	///	User control used to create, edit and delete blogs.  
	///	This only provides a few options for editing blogs. 
	///	For the full options, one should visit the individual 
	///	blog's admin tool.
	/// </summary>
	public class BlogsEditor : System.Web.UI.UserControl
	{
		const string VSKEY_BLOGID = "VS_BLOGID";
		int _resultsPageNumber = 1;

		#region Declared Controls
		protected System.Web.UI.WebControls.Repeater rprBlogsList;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel pnlResults;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel pnlEdit;
		protected Subtext.Web.Admin.WebUI.MessagePanel messagePanel;
		protected Subtext.Web.Admin.WebUI.Pager resultsPager;
		protected System.Web.UI.WebControls.CheckBox chkShowInactive;
		protected System.Web.UI.WebControls.TextBox txtHost;
		protected System.Web.UI.WebControls.TextBox txtApplication;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblNoMessages;
		protected System.Web.UI.WebControls.Label lblTitle;
		protected System.Web.UI.WebControls.Button btnCancel;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtPasswordConfirm;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldHostRequired;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldApplicationRequired;
		protected System.Web.UI.HtmlControls.HtmlTableRow passwordRow;
		protected System.Web.UI.HtmlControls.HtmlTableRow passwordRowConfirm;
		protected System.Web.UI.HtmlControls.HtmlImage Img1;
		protected System.Web.UI.HtmlControls.HtmlImage Img2;
		protected System.Web.UI.HtmlControls.HtmlInputHidden virtualDirectory;
		protected Subtext.Web.Controls.HelpToolTip hostDomainHelpTip;
		protected Subtext.Web.Controls.HelpToolTip applicationHelpTip;
		protected System.Web.UI.HtmlControls.HtmlImage Img3;
		protected Subtext.Web.Controls.HelpToolTip blogEditorHelp;
		protected Subtext.Web.Controls.HelpToolTip Helptooltip1;
		protected Subtext.Web.Controls.HelpToolTip helpUsername;
		protected Subtext.Web.Controls.HelpToolTip helpPassword;
		protected System.Web.UI.WebControls.Button btnAddNewBlog = new System.Web.UI.WebControls.Button();
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddNewBlog.Click += new EventHandler(btnAddNewBlog_Click);
			ContentRegion sideBar = Page.FindControl("MPSideBar") as ContentRegion;
			if(sideBar != null)
			{
				btnAddNewBlog.CssClass = "button";
				btnAddNewBlog.Text = "New Blog";
				sideBar.Controls.Add(btnAddNewBlog);
			}

			if(!IsPostBack)
			{
				//Paging...
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

				resultsPager.PageSize = Preferences.ListingItemCount;
				resultsPager.PageIndex = _resultsPageNumber;
				pnlResults.Collapsible = false;
				this.chkShowInactive.Checked = false;
			}
			BindList();
		}

		private void BindList()
		{
			this.pnlResults.Visible = true;
			this.pnlEdit.Visible = false;

			BlogInfoCollection blogs = null; 
			
			int totalBlogs;
			if(this.chkShowInactive.Checked)
			{
				blogs = BlogInfo.GetBlogs(_resultsPageNumber, resultsPager.PageSize, false);	
				totalBlogs = blogs.Count;
			}
			else
			{
				blogs = BlogInfo.GetActiveBlogs(_resultsPageNumber, resultsPager.PageSize, false, out totalBlogs);			
			}

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

			CurrentBlogCount = totalBlogs;
		}

		void BindEdit()
		{
			this.pnlResults.Visible = false;
			this.pnlEdit.Visible = true;
			
			BindEditHelp();

			BlogInfo blog;
			if(!CreatingBlog)
			{
				this.lblTitle.Visible = true;
				this.txtTitle.Visible = false;
				blog = BlogInfo.GetBlogById(BlogId);
				this.lblTitle.Text = blog.Title;
				this.txtApplication.Text = blog.Application;
				this.txtHost.Text = blog.Host;
				this.txtUsername.Text = blog.UserName;
			}
			else //Creating a blog
			{
				this.lblTitle.Visible = false;
				this.txtTitle.Visible = true;
				blog = new BlogInfo();
			}

			string onChangeScript = string.Format(System.Globalization.CultureInfo.InvariantCulture, "onPreviewChanged('{0}', '{1}', '{2}', false);", this.txtHost.ClientID, this.txtApplication.ClientID, this.virtualDirectory.ClientID);
			string onBlurScript = string.Format(System.Globalization.CultureInfo.InvariantCulture, "onPreviewChanged('{0}', '{1}', '{2}', true);", this.txtHost.ClientID, this.txtApplication.ClientID, this.virtualDirectory.ClientID);

			if(!Page.IsStartupScriptRegistered("SetUrlPreview"))
			{
				string startupScript = "<script type=\"text/javascript\">" 
					+ Environment.NewLine 
					+ onBlurScript 
					+ Environment.NewLine 
					+ "</script>";
				Page.RegisterStartupScript("SetUrlPreview", startupScript);
			}

			this.txtApplication.Attributes["onkeyup"] = onChangeScript;
			this.txtApplication.Attributes["onblur"] = onBlurScript;
			this.txtHost.Attributes["onkeyup"] = onChangeScript;
			this.txtHost.Attributes["onblur"] = onBlurScript;

			this.virtualDirectory.Value = Request.ApplicationPath.Replace("/", string.Empty);
		}

		// Contains the various help strings
		void BindEditHelp()
		{
			#region Help Tool Tip Text
			this.blogEditorHelp.HelpText = "<p>Use this page to manage the blogs installed on this server.</p>";

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
				+ "Application name.  However, two blogs with different Host Domains may have the same Application "
				+ "name (though that&#8217;s not recommended)."
				+ "</p><p>Also, if there are multiple blogs with the same Host Domain Name, they must all "
				+ "have a non-empty Application name defined.  For more detailed coverage " 
				+ ", please visit [//TODO: Enter URL HERE]."
				+ "</p>";

			this.applicationHelpTip.HelpText = "<p>"
				+ "Leave the application blank unless you are hosting multiple blogs on the "
				+ "same server.</p>"
				+ "<p>The Application is a &#8220;subdirectory&#8221; that will correspond "
				+ "to this blog.</p><p>For example, if you enter &#8220;MyBlog&#8221; " 
				+ "(sans quotes of course) for the application, then the root URL to your blog "
				+ "would be <em>http://[HOSTDOMAIN]/MyBlog/</em>"
				+ "</p>";
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
				if(ViewState[VSKEY_BLOGID] != null)
					return (int)ViewState[VSKEY_BLOGID];
				else
					return NullValue.NullInt32;
			}
			set { ViewState[VSKEY_BLOGID] = value; }
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
			this.chkShowInactive.CheckedChanged += new System.EventHandler(this.chkShowInactive_CheckedChanged);
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void chkShowInactive_CheckedChanged(object sender, System.EventArgs e)
		{
			BindList();
		}

		private void btnAddNewBlog_Click(object sender, EventArgs e)
		{
			this.BlogId = NullValue.NullInt32;
			this.txtTitle.Text = string.Empty;
			this.lblTitle.Text = string.Empty;
			this.txtApplication.Text = string.Empty;
			this.txtHost.Text = string.Empty;
			this.txtUsername.Text = string.Empty;
			BindEdit();
		}

		protected void rprBlogsList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
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

		private void btnSave_Click(object sender, System.EventArgs e)
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
			if(Config.CreateBlog(this.txtTitle.Text, this.txtUsername.Text, this.txtPassword.Text, this.txtHost.Text, this.txtApplication.Text))
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
				throw new ArgumentNullException("Blog Being Edited", "Ok, somehow the blog you were editing is now null.  This is very odd.");
			
			blog.Host = this.txtHost.Text;
			blog.Application = this.txtApplication.Text;
			blog.UserName = this.txtUsername.Text;

			if(this.txtPassword.Text.Length > 0)
			{
				blog.Password = this.txtPassword.Text;
			}
			
			if(Config.UpdateConfigData(blog))
			{
				this.messagePanel.ShowMessage("Blog Saved.");
			}
			else
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
					& ValidateFieldLength(this.txtApplication, "Application", 50)
					& ValidateFieldLength(this.txtUsername, "Username", 50)
					& ValidateFieldLength(this.txtPassword, "Password", 50)
					& ValidateFieldLength(this.txtTitle, "Title", 100);
			}
		}
		
		bool IsTextBoxEmpty(TextBox textbox)
		{
			return textbox.Text.Length == 0;
		}

		bool ValidateRequiredField(TextBox textbox, string fieldName)
		{
			if(IsTextBoxEmpty(textbox))
			{
				this.messagePanel.ShowError("Emptiness is quite Zen. Still, please enter a value for " + fieldName + ".<br />", false);
				return false;
			}
			return true;
		}

		bool ValidateFieldLength(TextBox textbox, string fieldName, int maxLength)
		{
			if(textbox.Text.Length > maxLength)
			{
				this.messagePanel.ShowError("Brevity is rewarded.  " + fieldName + " may only have " + maxLength + " characters.<br />", false);
				return false;
			}
			return true;
		}

		protected string ToggleActiveString(bool active)
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
			if(Config.UpdateConfigData(blog))
			{
				if(blog.IsActive)
				{
					this.messagePanel.ShowMessage("Blog Activated and ready to go.");
				}
				else
				{
					this.messagePanel.ShowMessage("Blog Inactivated and sent to a retirement community.");
				}
			}
			else
			{
				this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Sorry.");
			}
			BindList();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.messagePanel.ShowMessage("Blog Update Cancelled. Nothing to see here.");
			BindList();
		}
	}
}