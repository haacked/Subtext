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
using Subtext.Framework.Configuration;

namespace Subtext.Web.Install
{
    /// <summary>
    /// Summary description for InstallationComplete.
    /// </summary>
    public partial class InstallationComplete : InstallationBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InstallationManager.ResetInstallationStatusCache();

            if (paraBlogLink != null)
            {
                paraBlogLink.Visible = false;
            }
            if (paraBlogAdminLink != null)
            {
                paraBlogAdminLink.Visible = false;
            }
            if (paraBlogmlImport != null)
            {
                paraBlogmlImport.Visible = false;
            }

            if (Config.CurrentBlog != null)
            {
                if (lnkBlog != null && paraBlogLink != null)
                {
                    paraBlogLink.Visible = true;
                    lnkBlog.HRef = Url.BlogUrl();
                }

                if (lnkBlogAdmin != null && paraBlogAdminLink != null)
                {
                    paraBlogAdminLink.Visible = true;
                    lnkBlogAdmin.HRef = AdminUrl.Home();
                }

                if (lnkBlogMl != null && paraBlogmlImport != null)
                {
                    paraBlogmlImport.Visible = true;
                    lnkBlogMl.HRef = AdminUrl.ImportExport();
                }
            }
        }
    }
}