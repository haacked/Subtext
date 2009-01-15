using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;
using Subtext.Framework.Security;
using Subtext.Framework.Routing;

namespace Subtext.Web.Admin.WebUI
{
	/// <summary>
	/// Code behind for the admin master template.
	/// </summary>
	public partial class AdminPageTemplate : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

        public UrlHelper Url 
        {
            get 
            {
                return _urlHelper;
            }
        }
        UrlHelper _urlHelper = new UrlHelper(null, null);

		/// <summary>
		/// Adds a link button to the list of possible actions.
		/// </summary>
		/// <param name="button"></param>
		public void AddToActions(LinkButton button)
		{
			AddToActions(button, "");
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
				HyperLink rssLink = new HyperLink();
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
			AddToActions(link, "");
		}

		public void AddToActions(HyperLink link, string rssFeed)
		{
			LinksActions.Items.Add(link);
			if(!String.IsNullOrEmpty(rssFeed))
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

		/// <summary>
		/// The breadcrumb control that shows the user where 
		/// in the admin he or she is.
		/// </summary>
		public BreadCrumbs BreadCrumb
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The title of the page.
		/// </summary>
		public string Title
		{
			get { return this.Page.Title; }
			set { this.Page.Title = value; }
		}

		/// <summary>
		/// Attaches the logout button event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			this.LogoutLink.Click += OnLogoutClick;
			base.OnInit(e);
		}

		void OnLogoutClick(object sender, EventArgs e)
		{
			SecurityHelper.LogOut();
			HttpContext.Current.Response.Redirect(Config.CurrentBlog.HomeVirtualUrl);
		}
	}
}
