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

	/// <summary>
	///	Code behind class for the MyLinks section.
	/// </summary>
	public class MyLinks : BaseControl
	{
		protected System.Web.UI.WebControls.HyperLink Admin;
		protected System.Web.UI.WebControls.HyperLink XMLLink;
		protected System.Web.UI.WebControls.HyperLink AtomLink;
		protected System.Web.UI.WebControls.HyperLink Syndication;
		protected System.Web.UI.WebControls.HyperLink HomeLink;
		protected System.Web.UI.WebControls.HyperLink Archives;
		protected System.Web.UI.WebControls.HyperLink ContactLink;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(Context != null)
			{			
				if(HomeLink != null)
				{
					HomeLink.NavigateUrl = CurrentBlog.VirtualUrl;
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
					if(Request.IsAuthenticated && Security.IsAdmin)
					{
						Admin.Text = "Admin";
						Admin.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}admin/default.aspx", CurrentBlog.VirtualUrl);
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
					Syndication.NavigateUrl = XMLLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}Rss.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(Syndication, "Subscribe to this feed.");
				}

				if (AtomLink != null)
				{
					AtomLink.NavigateUrl = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}Atom.aspx", CurrentBlog.VirtualUrl);
					ControlHelper.SetTitleIfNone(AtomLink, "Subscribe to this feed.");
				}
			}
		}

		
	}
}

