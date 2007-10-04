using System;
using System.Web.UI.WebControls;
using Subtext.Web.Admin.Pages;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.WebUI
{
    public class StatsPage : AdminPage
    {
        public StatsPage()
        {
            this.TabSectionId = "Stats";
        }

        protected virtual void BindLocalUI()
        {
			HyperLink lnkReferrals = Utilities.CreateHyperLink("Referrals", "Referrers.aspx");
			HyperLink lnkErrorLog = Utilities.CreateHyperLink("Error Log", "ErrorLog.aspx");
			HyperLink lnkRssFeed = Utilities.CreateHyperLink("Custom Feed", "RssFeeds.aspx");

            // Add the buttons to the PageContainer.
            AdminMasterPage.AddToActions(lnkReferrals, CreateAdminRssUrl("ReferrersRss.aspx"));
			AdminMasterPage.AddToActions(lnkErrorLog, CreateAdminRssUrl("ErrorsRss.aspx"));
			AdminMasterPage.AddToActions(lnkRssFeed);
        }

        protected override void OnLoad(EventArgs e)
        {
            BindLocalUI();
            base.OnLoad(e);
        }
    }
}
