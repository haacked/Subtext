using System;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.WebUI
{
	/// <summary>
	/// Code behind for the admin master template.
	/// </summary>
	public partial class AdminPageTemplate : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Adds a link button to the list of possible actions.
		/// </summary>
		/// <param name="button"></param>
		public void AddToActions(LinkButton button)
		{
			// HACK: one without the other doesn't seem to work. If I don't add this
			// to Items it doesn't render, if I don't add to controls it doesn't get
			// wired up. 
			LinksActions.Items.Add(button);
			LinksActions.Controls.Add(button);
		}

		/// <summary>
		/// Adds a hyperlink to the list of possible actions.
		/// </summary>
		/// <param name="link"></param>
		public void AddToActions(HyperLink link)
		{
			LinksActions.Items.Add(link);
		}

		/// <summary>
		/// The breadcrumb control that shows the user where 
		/// in the admin he or she is.
		/// </summary>
		public BreadCrumbs BreadCrumb
		{
			get
			{
				return this.breadCrumbs;
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
			Security.LogOut();
			HttpContext.Current.Response.Redirect(Config.CurrentBlog.HomeVirtualUrl);
		}
	}
}
