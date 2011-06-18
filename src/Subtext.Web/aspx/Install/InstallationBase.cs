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
using System.Web;
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web.Install
{
    /// <summary>
    /// Summary description for InstallationBase.
    /// </summary>
    public class InstallationBase : SubtextPage
    {
        [Inject]
        public IInstallationManager InstallationManager
        {
            get;
            set;
        }

        static readonly string[] WizardPages =
            {
                "Default.aspx"
                , "Step02_ConfigureHost.aspx"
                , "Step03_CreateBlog.aspx"
            };

        /// <summary>
        /// Gets the next step URL.
        /// </summary>
        /// <value></value>
        public static string NextStepUrl
        {
            get
            {
                for (int i = 0; i < WizardPages.Length; i++)
                {
                    if (IsOnPage(WizardPages[i]) && i < WizardPages.Length - 1)
                    {
                        return WizardPages[i + 1];
                    }
                }
                return "InstallationComplete.aspx";
            }
        }

        /// <summary>
        /// Ons the load.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnLoad(EventArgs e)
        {
            InstallationState status = InstallationManager.GetInstallationStatus(VersionInfo.CurrentAssemblyVersion);
            var repository = new DatabaseObjectProvider();
            switch (status)
            {
                case InstallationState.NeedsInstallation:
                case InstallationState.NeedsUpgrade:
                    EnsureInstallStep("Default.aspx", "Step02_ConfigureHost.aspx");
                    break;

                default:
                    HostInfo info = HostInfo.LoadHostInfoFromDatabase(repository, true /* suppressException */);
                    int blogCount = repository.GetBlogCount();

                    if (info == null)
                    {
                        EnsureInstallStep("Step02_ConfigureHost.aspx");
                    }
                    if (info != null && blogCount == 0)
                    {
                        EnsureInstallStep("Step03_CreateBlog.aspx");
                    }
                    if (info != null && blogCount > 0)
                    {
                        EnsureInstallStep("InstallationComplete.aspx");
                    }
                    break;
            }

            base.OnLoad(e);
        }

        //Make sure we're on this page.
        void EnsureInstallStep(string page)
        {
            EnsureInstallStep(page, "");
        }

        void EnsureInstallStep(params string[] pages)
        {
            if (pages.Length == 0)
            {
                return;
            }

            foreach (string page in pages)
            {
                if (!string.IsNullOrEmpty(page))
                {
                    if (IsOnPage(page))
                    {
                        return;
                    }
                }
            }

            Response.Redirect(pages[0], true);
        }

        static bool IsOnPage(string page)
        {
            return HttpContext.Current.Request.Path.IndexOf(page, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}