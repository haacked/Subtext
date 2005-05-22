using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin;
using Subtext.Web.Controls;
using Keys = Subtext.Web.Admin.Keys;

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

			BlogConfigCollection blogs = null; 
			
			int totalBlogs;
			if(this.chkShowInactive.Checked)
			{
				blogs = BlogConfig.GetBlogs(_resultsPageNumber, resultsPager.PageSize, false);	
				totalBlogs = blogs.Count;
			}
			else
			{
				blogs = BlogConfig.GetActiveBlogs(_resultsPageNumber, resultsPager.PageSize, false, out totalBlogs);			
			}

			if (blogs.Count > 0)
			{
				this.resultsPager.Visible = true;
				this.resultsPager.ItemCount = blogs.MaxItems;
				this.rprBlogsList.DataSource = blogs;
				this.rprBlogsList.DataBind();
				this.lblNoMessages.Visible = false;
			}
			else
			{
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

			BlogConfig blog;
			if(BlogId != Constants.NULL_INTEGER)
			{
				blog = BlogConfig.GetBlogById(BlogId);
				this.lblTitle.Text = blog.Title;
				this.txtApplication.Text = blog.Application;
				this.txtHost.Text = blog.Host;
				this.txtUsername.Text = blog.UserName;
			}
			else
			{
				blog = new BlogConfig();
				this.lblTitle.Text = string.Empty;
				this.txtApplication.Text = string.Empty;
				this.txtHost.Text = string.Empty;
				this.txtUsername.Text = string.Empty;
			}

			string onChangeScript = string.Format("onPreviewChanged('{0}', '{1}', '{2}');", this.txtHost.ClientID, this.txtApplication.ClientID, this.virtualDirectory.ClientID);

			if(!Page.IsStartupScriptRegistered("SetUrlPreview"))
			{
				string startupScript = "<script type=\"text/javascript\">" 
					+ Environment.NewLine 
					+ onChangeScript 
					+ Environment.NewLine 
					+ "</script>";
				Page.RegisterStartupScript("SetUrlPreview", startupScript);
			}

			this.txtApplication.Attributes["onkeyup"] = onChangeScript;
			this.txtHost.Attributes["onkeyup"] = onChangeScript;

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
					return Constants.NULL_INTEGER;
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
					return Constants.NULL_INTEGER;
			}
			set { ViewState["VS_CurrentBlogCount"] = value; }
		}

		bool CreatingBlog
		{
			get
			{
				return BlogId == Constants.NULL_INTEGER;
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
			this.BlogId = Constants.NULL_INTEGER;
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
				BlogConfig blog;
				if(BlogId != Constants.NULL_INTEGER)
				{
					blog = BlogConfig.GetBlogById(BlogId);
				}
				else
				{
					blog = new BlogConfig();
				}
			
				blog.Host = this.txtHost.Text;
				blog.Application = this.txtApplication.Text;
			
				if(Config.UpdateConfigData(blog))
				{
					this.messagePanel.ShowMessage("Blog Saved.");
				}
				else
				{
					this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Sorry.");
				}
				BindList();
			}
			else
			{
				BindEdit();
			}
		}

		protected virtual bool PageIsValid
		{
			get
			{
				bool isValidSoFar = true;

				if(CreatingBlog && IsTextBoxEmpty(this.txtPassword))
				{
					isValidSoFar = false;
					this.messagePanel.ShowError("When creating a blog, specifying a valid password is required.  Pick a good one.");
				}

				if(this.txtPassword.Text != this.txtPasswordConfirm.Text)
				{
					isValidSoFar = false;
					this.messagePanel.ShowError("Oh dear. The Password and the Password Confirmation do not match.");	
				}

				return isValidSoFar 
					&& ValidateRequiredField(this.txtApplication, "Application") 
					&& ValidateRequiredField(this.txtHost, "Host Domain") 
					&& ValidateRequiredField(this.txtUsername, "Username");
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
				this.messagePanel.ShowError("Please do not leave " + fieldName + " empty.");
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
			BlogConfig blog = BlogConfig.GetBlogById(BlogId);
			blog.IsActive = !blog.IsActive;
			if(Config.UpdateConfigData(blog))
			{
				if(blog.IsActive)
				{
					this.messagePanel.ShowMessage("Blog Activated.");
				}
				else
				{
					this.messagePanel.ShowMessage("Blog Inactivated.");
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
			this.messagePanel.ShowMessage("Blog Update Cancelled.");
			BindList();
		}
	}
}