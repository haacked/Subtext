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
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	public class EditPreferences : AdminOptionsPage
	{
		protected System.Web.UI.WebControls.DropDownList ddlPageSize;
		protected System.Web.UI.WebControls.DropDownList ddlPublished;
		protected System.Web.UI.WebControls.DropDownList ddlExpandAdvanced;
		protected System.Web.UI.WebControls.Button lkbUpdate;
		protected System.Web.UI.WebControls.CheckBox chkAutoGenerate;
		protected Subtext.Web.Controls.HelpToolTip Helptooltip4;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				BindLocalUI();
			}		
		}

		private void BindLocalUI()
		{
			ddlPageSize.SelectedIndex = -1;
			ddlPageSize.Items.FindByValue(Preferences.ListingItemCount.ToString(CultureInfo.InvariantCulture)).Selected = true;

			ddlPublished.SelectedIndex = -1;
			ddlPublished.Items.FindByValue(Preferences.AlwaysCreateIsActive ? "true" : "false").Selected = true;

			ddlExpandAdvanced.SelectedIndex = -1;
			ddlExpandAdvanced.Items.FindByValue(Preferences.AlwaysExpandAdvanced ? "true" : "false").Selected = true;

			this.chkAutoGenerate.Checked = Config.CurrentBlog.AutoFriendlyUrlEnabled;
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
			this.lkbUpdate.Click += new System.EventHandler(this.lkbUpdate_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lkbUpdate_Click(object sender, System.EventArgs e)
		{
			int pageSize = Int32.Parse(ddlPageSize.SelectedItem.Value);
			if (pageSize > 0)
				Preferences.ListingItemCount = pageSize;	
	
			bool published = Boolean.Parse(ddlPublished.SelectedItem.Value);
			Preferences.AlwaysCreateIsActive = published;

			bool alwaysExpand = Boolean.Parse(ddlExpandAdvanced.SelectedItem.Value);
			Preferences.AlwaysExpandAdvanced = alwaysExpand;

			BlogInfo info  = Config.CurrentBlog;
			info.ItemCount = pageSize;
			info.AutoFriendlyUrlEnabled = this.chkAutoGenerate.Checked;
			Config.UpdateConfigData(info);
		}
	}
}

