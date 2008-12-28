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

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Admin Page used to set syndication settings.
	/// </summary>
	public partial class Syndication : AdminOptionsPage
	{
		// abstract out at a future point for i18n
		private const string RES_SUCCESS = "Your syndication settings were successfully updated.";
		private const string RES_FAILURE = "Syndication settings update failed.";

		protected override void Page_Load(object sender, EventArgs e)
		{
			base.Page_Load(sender, e);
			if (!IsPostBack)
			{
				PopulateForm();
			}
			ManageHiddenSettings();
		}
		
		private void PopulateForm()
		{
			Blog info = Config.CurrentBlog;
			
			this.chkEnableSyndication.Checked = info.IsAggregated;
			this.chkUseDeltaEncoding.Checked = info.RFC3229DeltaEncodingEnabled;
			this.chkUseSyndicationCompression.Checked = info.UseSyndicationCompression;
			this.txtFeedBurnerName.Text = info.FeedBurnerName;
			this.txtLicenseUrl.Text = info.LicenseUrl;
		}

		private void ManageHiddenSettings()
		{
			this.chkEnableSyndication.Attributes["onclick"] = "toggleHideOnCheckbox(this, 'otherSettings');";
	
			string startupScript = "<script type=\"text/javascript\">"
				+  Environment.NewLine + "var checkbox = document.getElementById('" + this.chkEnableSyndication.ClientID + "');"
				+  Environment.NewLine + " toggleHideOnCheckbox(checkbox, 'otherSettings');"
				+  Environment.NewLine +  "</script>";
	
			Type ctype = this.GetType();
			Page.ClientScript.RegisterStartupScript(ctype,"startupScript", startupScript);
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
			Blog info = Config.CurrentBlog;
			
			info.IsAggregated = this.chkEnableSyndication.Checked;
			info.UseSyndicationCompression = this.chkUseSyndicationCompression.Checked;
			info.RFC3229DeltaEncodingEnabled = this.chkUseDeltaEncoding.Checked;
			info.FeedBurnerName = this.txtFeedBurnerName.Text;
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
			this.Page.Load += new EventHandler(Page_Load);
		}
		#endregion

		protected void lkbPost_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}
	}
}
