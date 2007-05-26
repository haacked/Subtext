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
using System.Web.UI.WebControls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Web.Admin;

namespace Subtext.Web.HostAdmin.UserControls
{
	/// <summary>
	///	User control used to create, edit and delete blogs.  
	///	This only provides a few options for editing blogs. 
	///	For the full options, one should visit the individual 
	///	blog's admin tool.
	/// </summary>
	public partial class BlogsList : System.Web.UI.UserControl
	{
		int pageIndex;

		protected void Page_Load(object sender, EventArgs e)
		{
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

		public event EventHandler<BlogEditEventArgs> BlogEdit;

		protected void OnCreateNewBlogClick(object sender, EventArgs e)
		{
			Response.Redirect("EditBlog.aspx");
		}

		protected virtual void OnBlogEdit(int blogId)
		{
			EventHandler<BlogEditEventArgs> editBlogEvent = BlogEdit;
			if(editBlogEvent != null)
			{
				editBlogEvent(this, new BlogEditEventArgs(blogId));
			}
		}

		private void BindList()
		{
			this.pnlResults.Visible = true;

            IPagedCollection<BlogInfo> blogs; 
			
			ConfigurationFlags configFlag = this.chkShowInactive.Checked ? ConfigurationFlags.None : ConfigurationFlags.IsActive;
			
			blogs = BlogInfo.GetBlogs(this.pageIndex, resultsPager.PageSize, configFlag);
			
			if (blogs.Count > 0)
			{
				this.resultsPager.Visible = true;
				this.resultsPager.ItemCount = blogs.MaxItems;
				this.rprBlogsList.Visible = true;
				this.rprBlogsList.DataSource = blogs;
				this.rprBlogsList.DataBind();
			}
			else
			{
				this.resultsPager.Visible = false;
				this.rprBlogsList.Visible = false;
				this.resultsPager.Visible = false;
			}

			CurrentBlogCount = blogs.MaxItems;
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

		protected void OnActiveChanged(object sender, EventArgs e)
		{
			BindList();
		}

		protected void OnItemCommand(object source, RepeaterCommandEventArgs e)
		{
			int blogId;
			int.TryParse((string)e.CommandArgument, out blogId);
			switch (e.CommandName.ToLower(CultureInfo.InvariantCulture)) 
			{
				case "edit":
					OnBlogEdit(blogId);
					break;
				
				case "toggleactive":
					ToggleActive(blogId);
					break;
				
				default:
					break;
			}
		}

		protected static string ToggleActiveString(bool active)
		{
            if (active)
            {
                return "Deactivate";
            }
            else
            {
                return "Activate";
            }
		}

		void ToggleActive(int blogId)
		{
			BlogInfo blog = BlogInfo.GetBlogById(blogId);
			blog.IsActive = !blog.IsActive;
			try
			{
				Config.UpdateConfigData(blog);
				
				if(blog.IsActive)
				{
					ShowMessage("Blog Activated and ready to go.");
				}
				else
				{
					ShowMessage("Blog Inactivated and sent to a retirement community.");
				}				
			}
			catch(BaseBlogConfigurationException e)
			{
				ShowError(e.Message);
			}

			BindList();
		}

		public void ShowMessage(string message)
		{
			this.messagePanel.ShowMessage(message);
		}

		public void ShowError(string message)
		{
			this.messagePanel.ShowError(message);
		}
	}

	public class BlogEditEventArgs : EventArgs
	{
		public BlogEditEventArgs(int blogId)
		{
			this.blogId = blogId;
		}

		public int BlogId
		{
			get { return this.blogId; }
		}

		private int blogId;
	}
}