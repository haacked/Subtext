#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Summary description for Password.
	/// </summary>
	public class Password : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox tbPasswordConfirm;
		protected System.Web.UI.WebControls.Label Message;
		protected System.Web.UI.WebControls.CompareValidator CompareValidator1;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator6;
		protected System.Web.UI.WebControls.TextBox tbCurrent;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator5;
		protected System.Web.UI.WebControls.ValidationSummary ValidationSummary1;
		protected System.Web.UI.WebControls.LinkButton btnSave;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
		protected System.Web.UI.WebControls.TextBox tbPassword;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			#if WANRelease
				Response.Redirect("http://asp.net/Forums/User/ChangePassword.aspx?tabindex=1");
			#endif			
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
			const string failureMessage = "Your password can not be updated";
			if(Page.IsValid)
			{
				BlogConfig config = Config.CurrentBlog();
				bool useHash = Config.Settings.UseHashedPasswords;
				
				if(Security.IsValidPassword(tbCurrent.Text))
				{
					if(tbPassword.Text == tbPasswordConfirm.Text)
					{
						Security.UpdatePassword(tbPassword.Text);

						Messages.ShowMessage("Your password has been updated");
					}
					else
					{
						Messages.ShowError(failureMessage);
					}
				}
				else
				{
					Messages.ShowError(failureMessage);
				}
			}
			else
			{
				Messages.ShowError(failureMessage);
			}
		}
	}
}

