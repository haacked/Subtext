using System;
using Subtext.Framework;
using Subtext.Web.Controls;

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

namespace Subtext.Web.UI.Controls
{
	using System;
    using Subtext.Framework.Security;

	/// <summary>
	///	Code behind class for the MyLinks section.  Hooks up links within 
	///	MyLinks.ascx to their appropriate URL.
	/// </summary>
	public class MyLinks : BaseControl
	{
		#region Declared Controls
		protected System.Web.UI.WebControls.HyperLink Admin;
		protected System.Web.UI.WebControls.HyperLink XMLLink;
		protected System.Web.UI.WebControls.HyperLink AtomLink;
		protected System.Web.UI.WebControls.HyperLink Syndication;
		protected System.Web.UI.WebControls.HyperLink HomeLink;
		protected System.Web.UI.WebControls.HyperLink Archives;
		protected System.Web.UI.WebControls.HyperLink ContactLink;
		protected System.Web.UI.WebControls.HyperLink ArchivePostPageLink;
		protected System.Web.UI.WebControls.HyperLink LinkPageLink;
		protected System.Web.UI.WebControls.HyperLink ArticleCategoriesLink;
		#endregion

		protected override void OnLoad(EventArgs e)
		{
			//TODO: Make sure these urls use the UrlFormats class.

			base.OnLoad (e);
			if(Context != null)
			{
				if(HomeLink != null)
				{
					HomeLink.NavigateUrl = CurrentBlog.HomeVirtualUrl;
					ControlHelper.SetTitleIfNone(HomeLink, "Link to the home page.");
				}
				
				if(ContactLink != null)
				{
					ContactLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}contact.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(ContactLink, "Contact form.");
				}

				if(Archives != null)
				{
					Archives.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}archives.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(Archives, "View Archives.");
				}

				if (Admin != null)
				{
					if(Request.IsAuthenticated && SecurityHelper.IsAdmin)
					{
						Admin.Text = "Admin";
						Admin.NavigateUrl = CurrentBlog.AdminHomeVirtualUrl;
						ControlHelper.SetTitleIfNone(Admin, "Admin Section.");
					}
					else
					{
						Admin.Text = "Login";
						Admin.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}login.aspx", CurrentBlog.VirtualUrl);
						ControlHelper.SetTitleIfNone(Admin, "Login Form.");
					}
				}

				if (Syndication != null)
				{
					Syndication.NavigateUrl = CurrentBlog.UrlFormats.RssUrl.ToString();
					ControlHelper.SetTitleIfNone(Syndication, "Subscribe to this feed.");
				}

				if (XMLLink != null)
				{
					XMLLink.NavigateUrl = CurrentBlog.UrlFormats.RssUrl.ToString();
					ControlHelper.SetTitleIfNone(XMLLink, "Subscribe to this feed.");
				}

				if (AtomLink != null)
				{
					AtomLink.NavigateUrl = CurrentBlog.UrlFormats.AtomUrl.ToString();
					ControlHelper.SetTitleIfNone(AtomLink, "Subscribe to this feed.");
				}

				if(ArchivePostPageLink != null)
				{
					ArchivePostPageLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}ArchivePostPage.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(ArchivePostPageLink, "Archives.");
				}

				if(LinkPageLink != null)
				{
					LinkPageLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}LinkPage.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(LinkPageLink, "Links.");
				}

				if(ArticleCategoriesLink != null)
				{
					ArticleCategoriesLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}ArticleCategoriesPage.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(ArticleCategoriesLink, "Article Categories.");
				}
				
			}
		}

		
	}
}

