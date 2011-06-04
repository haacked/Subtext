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
using Subtext.Framework.Security;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Code behind class for the MyLinks section.  Hooks up links within 
    ///	MyLinks.ascx to their appropriate URL.
    /// </summary>
    public class MyLinks : BaseControl
    {
        protected HyperLink Admin;
        protected HyperLink Archives;
        protected HyperLink ContactLink;
        protected HyperLink HomeLink;
        protected HyperLink Syndication;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context != null)
            {
                if (HomeLink != null)
                {
                    HomeLink.NavigateUrl = Url.BlogUrl();
                    ControlHelper.SetTitleIfNone(HomeLink, "Link to the home page.");
                }

                if (ContactLink != null)
                {
                    ContactLink.NavigateUrl = Url.ContactFormUrl();
                    ControlHelper.SetTitleIfNone(ContactLink, "Contact form.");
                }

                if (Archives != null)
                {
                    Archives.NavigateUrl = Url.ArchivesUrl();
                    ControlHelper.SetTitleIfNone(Archives, "View Archives.");
                }

                if (Admin != null)
                {
                    if (Request.IsAuthenticated && User.IsAdministrator())
                    {
                        Admin.Text = "Admin";
                        Admin.NavigateUrl = AdminUrl.Home();
                        ControlHelper.SetTitleIfNone(Admin, "Admin Section.");
                    }
                    else
                    {
                        Admin.Text = "Login";
                        Admin.NavigateUrl = Url.LoginUrl();
                        ControlHelper.SetTitleIfNone(Admin, "Login Form.");
                    }
                }

                if (Syndication != null)
                {
                    Syndication.NavigateUrl = Url.RssUrl(Blog).ToString();
                    if (Syndication.ImageUrl.StartsWith("~/"))
                    {
                        Syndication.ImageUrl = Url.ResolveUrl(Syndication.ImageUrl);
                    }
                    ControlHelper.SetTitleIfNone(Syndication, "Subscribe to this feed.");
                }
            }
        }
    }
}