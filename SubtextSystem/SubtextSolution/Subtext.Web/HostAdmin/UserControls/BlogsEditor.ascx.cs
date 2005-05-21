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
		private const string VSKEY_BLOGID = "VS_BLOGID";
		private int _resultsPageNumber = 1;

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
		protected System.Web.UI.WebControls.Button btnAddNewBlog = new System.Web.UI.WebControls.Button();
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
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
			
			if(this.chkShowInactive.Checked)
			{
				blogs = BlogConfig.GetBlogs(_resultsPageNumber, resultsPager.PageSize, false);	
			}
			else
			{
				blogs = BlogConfig.GetActiveBlogs(_resultsPageNumber, resultsPager.PageSize, false);
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
		}

		void BindEdit()
		{
			this.pnlResults.Visible = false;
			this.pnlEdit.Visible = true;
			
			BlogConfig blog;
			if(BlogId != Constants.NULL_INTEGER)
			{
				blog = BlogConfig.GetBlogById(BlogId);

				lblTitle.Text = blog.Title;
				this.txtApplication.Text = blog.Application;
				this.txtHost.Text = blog.Host;
				this.txtUsername.Text = blog.UserName;
			}
			else
			{
				blog = new BlogConfig();
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
					return (int)ViewState[VSKEY_BLOGID];
				else
					return Constants.NULL_INTEGER;
			}
			set { ViewState[VSKEY_BLOGID] = value; }
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
			this.btnAddNewBlog.Click += new EventHandler(btnAddNewBlog_Click);
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
