using System;
using System.Security;
using Subtext.Framework;

namespace Subtext.Web.Install
{
	/// <summary>
	/// Page used to create an initial configuration for the blog.
	/// </summary>
	/// <remarks>
	/// This page will ONLY be displayed if there are no 
	/// blog configurations within the database.
	/// </remarks>
	public class Step02_ConfigureHost : InstallationBase
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
		protected System.Web.UI.WebControls.Literal ltlMessage;
		protected Subtext.Web.Controls.ContentRegion MPTitle;
		protected Subtext.Web.Controls.ContentRegion MPSubTitle;
		protected Subtext.Web.Controls.ContentRegion Content;
		protected Subtext.Web.Controls.MasterPage MPContainer;
		protected System.Web.UI.HtmlControls.HtmlTable tblConfigForm;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			//We need to make sure that the form is ONLY displayed 
			//if there really is no Host record.
			if(HostInfo.Instance != null)
			{
				// Ok, someone shouldn't be here. Redirect to the error page.
				throw new SecurityException("That page is forbidden.");
			}

			tblConfigForm.Visible = true;
			ltlMessage.Text = 
				"<p>" 
				+ "Welcome!  This is the first step towards successfully configuring " 
				+ "Subtext. "
				+ "</p>"
				+ "<p>" 
				+ "To get you started quickly, just specify a username and password "
				+ "for the special Host Administrator account. " 
				+ "This account can create blogs in this system. "
				+ "</p>";
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
				string userName = txtUserName.Text;
				string password = txtPassword.Text;
				
				// Create the HostInfo record.
				if(HostInfo.CreateHost(userName, password))
				{
					//TODO: Use a controller here to determine where to go next...
					Response.Redirect("Step03_CreateBlog.aspx");
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
