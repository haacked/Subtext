using System;
using System.Security;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Web.Controls;

namespace Subtext.Web
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class BlogNotConfiguredError : System.Web.UI.Page
	{
		bool _anyBlogsExist = false;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtConfirmPassword;
		protected System.Web.UI.WebControls.ValidationSummary vldSummary;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldUsernameRequired;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldPasswordRequired;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldConfirmPasswordRequired;
		protected System.Web.UI.WebControls.CompareValidator vldComparePasswords;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Literal ltlMessage;
		protected System.Web.UI.HtmlControls.HtmlTable tblConfigForm;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			//We need to make sure that the form is ONLY displayed 
			//when an actual error has happened AND the user is a 
			//local user.
			
			bool blogConfigured = true;
			BlogInfo info = null;
			try
			{
				info = Config.CurrentBlog;
			}
			catch(BlogDoesNotExistException exception)
			{
				blogConfigured = false;
				_anyBlogsExist = exception.AnyBlogsExist;
			}

			if(blogConfigured || info != null)
			{
				// Ok, someone shouldn't be here. Redirect to the error page.
				throw new SecurityException("That page is forbidden.");
			}

			if(_anyBlogsExist)
			{
				ltlMessage.Text = 
					"<p>" 
					+ "Welcome!  The Subtext Blogging Engine has been properly installed, " 
					+ "but the blog you&#8217;ve requested cannot be found."
					+ "</p>"
					+ "<p>"
					+ "Several blogs have been created on this system, but either the "
					+ "blog you are requesting hasn&#8217;t yet been created, " 
					+ "or the requesting URL does not match an existing blog." 
					+ "</p><p>"
					+ "If you are the Host Admin, visit the <a href=\"" + ControlHelper.ExpandTildePath("~/HostAdmin/") + "\">Host Admin</a> " 
					+ "Tool to view existing blogs and if necessary, correct settings."
					+ "</p>";
				tblConfigForm.Visible = false;
			}
			else
			{
				tblConfigForm.Visible = true;
				ltlMessage.Text = 
					"<p>" 
					+ "Welcome!  The Subtext Blogging Engine has been properly installed, " 
					+ "but there are currently no blogs created on this system."
					+ "</p>"
					+ "<p>" 
					+ "To get you started quickly, just specify an "
					+ "administrative username and password below and I&#8217;ll create your blog and "
					+ "send you to the admin section where you can finish updating settings for your blog." 
					+ "</p>"
					+ "<p>"
					+ "For future reference, use the <a href=\"" + ControlHelper.ExpandTildePath("~/HostAdmin/") + "\">Host Admin</a> tool to create a blog."
					+ "</p>" ;
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				bool persist = true;
				string title = "A Subtext Blog";
				string userName = txtUserName.Text;
				string password = txtPassword.Text;
							
				// Create the blog_config record using default values 
				// and the specified user info.
				if(Config.CreateBlog(title, userName, password, Request.Url.Host, UrlFormats.GetBlogApplicationNameFromRequest(Request.RawUrl, Request.ApplicationPath)))
				{
					if(Security.Authenticate(userName, password, !persist))
					{
						Response.Redirect("~/Admin/Configure.aspx");
					}
					else
					{
						throw new InvalidOperationException("Could not authenticate user we just created. That's really bad.");
					}
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
		}
	}
}
