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

namespace Subtext.Web.Admin.WebUI
{
    /// <summary>
    /// Code behind for the admin master template.
    /// </summary>
    public partial class AdminPageTemplate : AdminMasterPage
    {
        /// <summary>
        /// The title of the page.
        /// </summary>
        public string Title
        {
            get { return Page.Title; }
            set { Page.Title = value; }
        }

        /// <summary>
        /// Adds a link button to the list of possible actions.
        /// </summary>
        /// <param name="button"></param>
        public void AddToActions(LinkButton button)
        {
            AddToActions(button, string.Empty);
        }

        public void AddToActions(LinkButton button, string rssFeed)
        {
            // HACK: one without the other doesn't seem to work. If I don't add this
            // to Items it doesn't render, if I don't add to controls it doesn't get
            // wired up. 
            LinksActions.Items.Add(button);
            LinksActions.Controls.Add(button);
            if (!String.IsNullOrEmpty(rssFeed))
            {
                HyperLink rssLink = CreateAdminRssHyperlink(rssFeed);
                LinksActions.Items.Add(rssLink);
                LinksActions.Controls.Add(rssLink);
            }
        }

        private HyperLink CreateAdminRssHyperlink(string rssFeed)
        {
            var rssLink = new HyperLink();
            rssLink.NavigateUrl = rssFeed;
            rssLink.Text = "(rss)";
            return rssLink;
        }

        /// <summary>
        /// Adds a hyperlink to the list of possible actions.
        /// </summary>
        /// <param name="link"></param>
        public void AddToActions(HyperLink link)
        {
            AddToActions(link, string.Empty);
        }

        public void AddToActions(HyperLink link, string rssFeed)
        {
            LinksActions.Items.Add(link);
            if (!String.IsNullOrEmpty(rssFeed))
            {
                LinksActions.Items.Add(CreateAdminRssHyperlink(rssFeed));
            }
        }

        /// <summary>
        /// Clears the actions.
        /// </summary>
        public void ClearActions()
        {
            LinksActions.Items.Clear();
        }

        public string AjaxServicesUrl()
        {
            return AdminUrl.AjaxServices() + "?proxy";
        }
    }
}