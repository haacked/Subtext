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
using Subtext.Framework.Security;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Summary description for Password.
	/// </summary>
	public partial class Security : AdminOptionsPage
	{
		protected Label Message;
		protected ValidationSummary ValidationSummary1;
	
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

		}
		#endregion

        protected override void BindLocalUI()
        {
            tbOpenIDURL.Text = Config.CurrentBlog.OpenIDUrl;
            tbOpenIDServer.Text = Config.CurrentBlog.OpenIDServer;
            tbOpenIDDelegate.Text = Config.CurrentBlog.OpenIDDelegate;
            base.BindLocalUI();
        }

        protected void btnSaveOptions_Click(object sender, EventArgs e)
        {
            BlogInfo info = Config.CurrentBlog;
            info.OpenIDUrl = tbOpenIDURL.Text;
            info.OpenIDServer = tbOpenIDServer.Text;
            info.OpenIDDelegate = tbOpenIDDelegate.Text;
            Config.UpdateConfigData(info);
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
		{
			const string failureMessage = "Your password can not be updated";
			if(Page.IsValid)
			{
				if(SecurityHelper.IsValidPassword(tbCurrent.Text))
				{
					if(tbPassword.Text == tbPasswordConfirm.Text)
					{
						SecurityHelper.UpdatePassword(tbPassword.Text);

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

