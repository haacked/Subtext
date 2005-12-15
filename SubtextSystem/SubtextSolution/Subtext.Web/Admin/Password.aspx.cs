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
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Summary description for Password.
	/// </summary>
	public class Password : AdminOptionsPage
	{
		protected TextBox tbPasswordConfirm;
		protected Label Message;
		protected CompareValidator CompareValidator1;
		protected RequiredFieldValidator RequiredFieldValidator6;
		protected TextBox tbCurrent;
		protected RequiredFieldValidator RequiredFieldValidator5;
		protected ValidationSummary ValidationSummary1;
		protected LinkButton btnSave;
		protected RequiredFieldValidator RequiredFieldValidator1;
		protected MessagePanel Messages;
		protected TextBox tbPassword;
	
		private void Page_Load(object sender, EventArgs e)
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
			this.btnSave.Click += new EventHandler(this.btnSave_Click);
			this.Load += new EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, EventArgs e)
		{
			const string failureMessage = "Your password can not be updated";
			if(Page.IsValid)
			{
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

