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
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
	public class EditPreferences : AdminPage
	{
		protected System.Web.UI.WebControls.DropDownList ddlPageSize;
		protected System.Web.UI.WebControls.DropDownList ddlPublished;
		protected System.Web.UI.WebControls.DropDownList ddlExpandAdvanced;
		protected System.Web.UI.WebControls.LinkButton lkbUpdate;
		protected System.Web.UI.WebControls.LinkButton lkbCancel;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected System.Web.UI.WebControls.CheckBox EnableComments;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
//			if (!AreCookiesAllowed())
//			{
//				// TODO -- display an errormsg indicating cookies are required
//				// with a link to FinishCookieTest on the off chance this is the
//				// first page accessed and cookies are in fact on.
//				Controls.Add(new LiteralControl("Cookies NOT ALLOWED"));
//			}
//			else if (!IsPostBack)
//			{
				BindLocalUI();
//			}
			
		}

		private void BindLocalUI()
		{
			ddlPageSize.SelectedIndex = -1;
			ddlPageSize.Items.FindByValue(Preferences.ListingItemCount.ToString()).Selected = true;

			ddlPublished.SelectedIndex = -1;
			ddlPublished.Items.FindByValue(Preferences.AlwaysCreateIsActive ? "true" : "false").Selected = true;

			ddlExpandAdvanced.SelectedIndex = -1;
			ddlExpandAdvanced.Items.FindByValue(Preferences.AlwaysExpandAdvanced ? "true" : "false").Selected = true;

			this.EnableComments.Checked = Config.CurrentBlog(Context).EnableComments;

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
			this.lkbCancel.Click += new System.EventHandler(this.lkbCancel_Click);
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

			BlogConfig config  = Config.CurrentBlog(Context);
			config.EnableComments = this.EnableComments.Checked;
			Config.UpdateConfigData(config);


//			BindLocalUI();
		}

		private void lkbCancel_Click(object sender, System.EventArgs e)
		{
			BindLocalUI();
		}
	}
}

