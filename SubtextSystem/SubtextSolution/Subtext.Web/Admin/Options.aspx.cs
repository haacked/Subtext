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

namespace Subtext.Web.Admin.Pages
{
	public class AdminOptionsPage : AdminPage
	{
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			BindLocalUI();
		}
		
		private void BindLocalUI()
		{
			HyperLink lnkConfigure = Utilities.CreateHyperLink("Configure", "Configure.aspx");
			HyperLink lnkSyndication = Utilities.CreateHyperLink("Syndication", "Syndication.aspx");
			HyperLink lnkComments = Utilities.CreateHyperLink("Comments", "Comments.aspx");
			HyperLink linkKeyWords		= Utilities.CreateHyperLink("Key Words", "EditKeyWords.aspx");
			HyperLink lnkPasswords		= Utilities.CreateHyperLink("Password", "Password.aspx");
			HyperLink lnkPreferences		= Utilities.CreateHyperLink("Preferences", "Preferences.aspx");

			// Add the buttons to the PageContainer.
			PageContainer.AddToActions(lnkConfigure);
			PageContainer.AddToActions(lnkSyndication);
			PageContainer.AddToActions(lnkComments);
			PageContainer.AddToActions(linkKeyWords);
			PageContainer.AddToActions(lnkPasswords);
			PageContainer.AddToActions(lnkPreferences);

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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}

