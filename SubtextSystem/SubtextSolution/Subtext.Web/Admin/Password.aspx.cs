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
		protected Button btnSave;
		protected RequiredFieldValidator RequiredFieldValidator1;
		protected MessagePanel Messages;
		/* Doesn't compile and since I guess there is no need to redefine them here
		 * I simply commented them out..
		 * Pardon me Phil if I broke some your idea, but.. doesn't compile
		 * 
		new protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		new protected Subtext.Web.Admin.WebUI.Page PageContainer;
		*/
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

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

