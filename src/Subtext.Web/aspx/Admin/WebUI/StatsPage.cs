#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web.UI.WebControls;
using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.WebUI
{
    public class StatsPage : AdminPage
    {
        public StatsPage()
        {
            TabSectionId = "Stats";
        }

        protected virtual void BindLocalUI()
        {
            HyperLink lnkReferrals = Utilities.CreateHyperLink("Referrals", "Referrers.aspx");
            HyperLink lnkViews = Utilities.CreateHyperLink("Views", "StatsView.aspx");
            HyperLink lnkErrorLog = Utilities.CreateHyperLink("Error Log", "ErrorLog.aspx");
            HyperLink lnkRssFeed = Utilities.CreateHyperLink("Custom Feed", "RssFeeds.aspx");

            // Add the buttons to the PageContainer.
            AdminMasterPage.AddToActions(lnkReferrals, Url.AdminRssUrl("Referrers"));
            AdminMasterPage.AddToActions(lnkViews);
            AdminMasterPage.AddToActions(lnkErrorLog, Url.AdminRssUrl("Errors"));
            AdminMasterPage.AddToActions(lnkRssFeed);
        }

        protected override void OnLoad(EventArgs e)
        {
            BindLocalUI();
            base.OnLoad(e);
        }
    }
}