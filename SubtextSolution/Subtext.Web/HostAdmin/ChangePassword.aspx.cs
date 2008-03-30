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
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;

namespace Subtext.Web.HostAdmin
{
	/// <summary>
	/// Allows the user to change the host admin password.
	/// </summary>
	public partial class ChangePassword : HostAdminPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lblSuccess.Visible = false;
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
			vldCurrent.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(vldCurrent_ServerValidate);
		}
		#endregion

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			if(Page.IsValid)
			{
				HostInfo.SetHostPassword(HostInfo.Instance, txtNewPassword.Text);
				HostInfo.UpdateHost(HostInfo.Instance);
				lblSuccess.Visible = true;
			}
		}

		private void vldCurrent_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
		{
			string password = txtCurrentPassword.Text;
			if(Config.Settings.UseHashedPasswords)
				password = SecurityHelper.HashPassword(password, HostInfo.Instance.Salt);

			args.IsValid = password == HostInfo.Instance.Password;
		}
	}
}
