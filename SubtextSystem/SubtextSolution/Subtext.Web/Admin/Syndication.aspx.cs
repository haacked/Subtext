using System;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin
{
	/// <summary>
	/// Admin Page used to set syndication settings.
	/// </summary>
	public class Syndication : System.Web.UI.Page
	{
		// abstract out at a future point for i18n
		private const string RES_SUCCESS = "Your syndication settings were successfully updated.";
		private const string RES_FAILURE = "Syndication settings update failed.";

		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected System.Web.UI.WebControls.CheckBox chkEnableSyndication;
		protected System.Web.UI.WebControls.CheckBox chkUseSyndicationCompression;
		protected System.Web.UI.WebControls.TextBox txtLicenseUrl;
		protected System.Web.UI.WebControls.LinkButton lkbPost;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip1;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip2;
		protected Subtext.Web.Controls.HelpToolTip HelpToolTip3;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				PopulateForm();
			}
			ManageHiddenSettings();
		}
		
		private void PopulateForm()
		{
			BlogInfo info = Config.CurrentBlog;
			
			this.chkEnableSyndication.Checked = info.IsAggregated;
			
			this.chkUseSyndicationCompression.Checked = info.UseSyndicationCompression;
			this.txtLicenseUrl.Text = info.LicenseUrl;
		}

		private void ManageHiddenSettings()
		{
			this.chkEnableSyndication.Attributes["onclick"] = "toggleHideOnCheckbox(this, 'otherSettings');";
	
			string startupScript = "<script language=\"javascript\">"
				+  Environment.NewLine + "var checkbox = document.getElementById('" + this.chkEnableSyndication.ClientID + "');"
				+  Environment.NewLine + " toggleHideOnCheckbox(checkbox, 'otherSettings');"
				+  Environment.NewLine +  "</script>";
	
			Page.RegisterStartupScript("startupScript", startupScript);
		}

		private void SaveSettings()
		{
			try
			{
				UpdateConfiguration();
				this.Messages.ShowMessage(RES_SUCCESS);
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, RES_FAILURE, ex.Message));
			}
		}

		private void UpdateConfiguration()
		{
			BlogInfo info = Config.CurrentBlog;
			
			info.IsAggregated = this.chkEnableSyndication.Checked;
			info.UseSyndicationCompression = this.chkUseSyndicationCompression.Checked;
			info.LicenseUrl = this.txtLicenseUrl.Text;

			Config.UpdateConfigData(info);
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
			this.lkbPost.Click += new System.EventHandler(this.lkbPost_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lkbPost_Click(object sender, System.EventArgs e)
		{
			SaveSettings();
		}
	}
}
