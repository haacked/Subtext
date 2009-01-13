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
					HomeLink.NavigateUrl = Blog.HomeVirtualUrl;
					ControlHelper.SetTitleIfNone(HomeLink, "Link to the home page.");
				}
				
				if(ContactLink != null)
				{
					ContactLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}contact.aspx", Blog.VirtualUrl);
					ControlHelper.SetTitleIfNone(ContactLink, "Contact form.");
				}

				if(Archives != null)
				{
					Archives.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}archives.aspx", Blog.VirtualUrl);
					ControlHelper.SetTitleIfNone(Archives, "View Archives.");
				}

				if (Admin != null)
				{
					if(Request.IsAuthenticated && SecurityHelper.IsAdmin)
					{
						Admin.Text = "Admin";
						Admin.NavigateUrl = Blog.AdminHomeVirtualUrl;
						ControlHelper.SetTitleIfNone(Admin, "Admin Section.");
					}
					else
					{
						Admin.Text = "Login";
						Admin.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}login.aspx", Blog.VirtualUrl);
						ControlHelper.SetTitleIfNone(Admin, "Login Form.");
					}
				}

				if (Syndication != null)
				{
					Syndication.NavigateUrl = Url.RssUrl(Blog).ToString();
					ControlHelper.SetTitleIfNone(Syndication, "Subscribe to this feed.");
				}

				if(ArchivePostPageLink != null)
				{
					ArchivePostPageLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}ArchivePostPage.aspx", Blog.VirtualUrl);
					ControlHelper.SetTitleIfNone(ArchivePostPageLink, "Archives.");
				}

				if(LinkPageLink != null)
				{
					LinkPageLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}LinkPage.aspx", Blog.VirtualUrl);
					ControlHelper.SetTitleIfNone(LinkPageLink, "Links.");
				}

				if(ArticleCategoriesLink != null)
				{
					ArticleCategoriesLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}ArticleCategoriesPage.aspx", Blog.VirtualUrl);
					ControlHelper.SetTitleIfNone(ArticleCategoriesLink, "Article Categories.");
				}
				
			}
		}

		
	}
}

