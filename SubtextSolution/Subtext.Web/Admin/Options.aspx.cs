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

namespace Subtext.Web.Admin.Pages
{
	public class AdminOptionsPage : AdminPage
	{
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
	
	    public AdminOptionsPage()
	    {
            TabSectionId = "Options";
	    }
	    
		private void Page_Load(object sender, System.EventArgs e)
		{
			BindLocalUI();
		}
		
		private void BindLocalUI()
		{
			HyperLink lnkConfigure = Utilities.CreateHyperLink("Configure", "Configure.aspx");
			HyperLink lnkSyndication = Utilities.CreateHyperLink("Syndication", "Syndication.aspx");
			HyperLink lnkComments = Utilities.CreateHyperLink("Comments", "Comments.aspx");
			HyperLink linkKeyWords = Utilities.CreateHyperLink("Key Words", "EditKeyWords.aspx");
			HyperLink lnkPasswords = Utilities.CreateHyperLink("Password", "Password.aspx");
			HyperLink lnkPreferences = Utilities.CreateHyperLink("Preferences", "Preferences.aspx");

			// Add the buttons to the PageContainer.
            AdminMasterPage.AddToActions(lnkConfigure);
            AdminMasterPage.AddToActions(lnkSyndication);
            AdminMasterPage.AddToActions(lnkComments);
            AdminMasterPage.AddToActions(linkKeyWords);
            AdminMasterPage.AddToActions(lnkPasswords);
            AdminMasterPage.AddToActions(lnkPreferences);
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

