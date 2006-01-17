	using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class Step04_CreateBlog : InstallationBase
	{
		protected System.Web.UI.WebControls.Button btnQuickCreate;
		protected System.Web.UI.WebControls.Literal ltlMessage;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected System.Web.UI.WebControls.Button btnImportBlog;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected Subtext.Web.Controls.ContentRegion Content;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			bool _anyBlogsExist = true;
			BlogInfo info = null;
			try
			{
				info = Config.CurrentBlog;
			}
			catch(BlogDoesNotExistException exception)
			{
				_anyBlogsExist = exception.AnyBlogsExist;
			}

			if(!_anyBlogsExist)
			{
				//TODO:
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnQuickCreate.Click += new System.EventHandler(this.btnQuickCreate_Click);
			this.btnImportBlog.Click += new System.EventHandler(this.btnImportBlog_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnQuickCreate_Click(object sender, System.EventArgs e)
		{
			// Create the blog_config record using default values 
			// and the specified user info
			
			//Since the password is stored as a hash, let's not hash it again.
			const bool passwordAlreadyHashed = true;
			if(Config.CreateBlog("TEMPORARY BLOG NAME", HostInfo.Instance.HostUserName, HostInfo.Instance.Password, Request.Url.Host, string.Empty, passwordAlreadyHashed))
			{
				//We probably should have creating the blog authenticate the user 
				//automatically so this redirect doesn't require a login.
				InstallationManager.ResetInstallationStatusCache();
				Response.Redirect("~/Admin/Configure.aspx");
			}
			else
			{
				string errorMessage = "I'm sorry, but we had a problem creating your initial "
					+ "configuration. Please <a href=\"http://sourceforge.net/tracker/?group_id=137896&atid=739979\">report "
					+ "this issue</a> to the Subtext team.";
					
				//TODO: Pick a non-generic exception.
				throw new Exception(errorMessage);
			}
		}

		private void btnImportBlog_Click(object sender, System.EventArgs e)
		{
			// We need to get over to the Import pages... why do i keep getting redirected here?
			Response.Redirect("~/HostAdmin/Import/ImportStart.aspx");
		}
	}
}
