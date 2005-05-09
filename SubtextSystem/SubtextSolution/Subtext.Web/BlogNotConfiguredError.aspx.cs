using System;
using System.Security;
using Subtext.Framework;
using Subtext.Framework.Configuration;

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
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtConfirmPassword;
		protected System.Web.UI.WebControls.ValidationSummary vldSummary;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldUsernameRequired;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldPasswordRequired;
		protected System.Web.UI.WebControls.RequiredFieldValidator vldConfirmPasswordRequired;
		protected System.Web.UI.WebControls.CompareValidator vldComparePasswords;
		protected System.Web.UI.WebControls.Button btnSave;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			//We need to make sure that the form is ONLY displayed 
			//when an actual error has happened AND the user is a 
			//local user.
			bool blogConfigured = true;
			try
			{
				Config.GetConfig("", "");
			}
			catch(BlogDoesNotExistException)
			{
				blogConfigured = false;
			}

			if(blogConfigured)
			{
				// Ok, someone shouldn't be here. Redirect to the error page.
				throw new SecurityException("That page is forbidden.");
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
				string userName = txtUserName.Text;
				string password = txtPassword.Text;
				string hashedPassword = string.Empty;
				
				if(Config.Settings.UseHashedPasswords)
				{
					hashedPassword = Security.HashPassword(password);
				}
				
				// Create the blog_config record using default values 
				// and the specified user info.
				if(Config.AddInitialBlog(userName, hashedPassword))
				{
					if(Security.Authenticate(userName, password, !persist))
					{
						Response.Redirect("~/Admin/Configure.aspx");
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
