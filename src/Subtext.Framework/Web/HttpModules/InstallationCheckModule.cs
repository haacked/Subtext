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
using Subtext.Framework.Infrastructure.Installation;

namespace Subtext.Framework.Web.HttpModules
{
    /// <summary>
    /// Checks to see if the blog needs an upgrade.
    /// </summary>
    public class InstallationCheckModule : IHttpModule
    {
        private HostInfo _hostInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationCheckModule"/> class.
        /// </summary>
        public InstallationCheckModule(IInstallationManager installationManager, HostInfo hostInfo)
        {
            InstallationManager = installationManager;
            _hostInfo = hostInfo;
        }

        public IInstallationManager InstallationManager { get; private set; }

        /// <summary>
        /// Initializes a module and prepares it to handle
        /// requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, 
        /// and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += HandleInstallationUpdates;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the
        /// module that implements <see langword="IHttpModule."/>
        /// </summary>
        public void Dispose()
        {
            //Do nothing.
        }

        /// <summary>
        /// Checks the installation status and redirects if necessary.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HandleInstallationUpdates(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            HandleInstallationStatus(context, BlogRequest.Current);
        }

        public void HandleInstallationStatus(HttpContextBase context, BlogRequest blogRequest)
        {
            string redirectUrl = GetInstallationRedirectUrl(blogRequest);
            if (!String.IsNullOrEmpty(redirectUrl))
            {
                context.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// Checks the installation status and redirects if necessary.
        /// </summary>
        public string GetInstallationRedirectUrl(BlogRequest blogRequest)
        {
            const string installUrl = "~/install/default.aspx";

            // Bypass for static files.
            if (blogRequest.RawUrl.IsStaticFileRequest())
            {
                return null;
            }

            if (_hostInfo == null && blogRequest.RequestLocation != RequestLocation.Installation)
            {
                return installUrl;
            }

            // Want to redirect to install if installation is required, 
            // or if we're missing a HostInfo record.
            if ((InstallationManager.InstallationActionRequired(VersionInfo.CurrentAssemblyVersion, null) || _hostInfo == null))
            {
                InstallationState state = InstallationManager.GetInstallationStatus(VersionInfo.CurrentAssemblyVersion);
                if (state == InstallationState.NeedsInstallation
                   && !blogRequest.IsHostAdminRequest
                   && blogRequest.RequestLocation != RequestLocation.Installation)
                {
                    return installUrl;
                }

                if (state == InstallationState.NeedsUpgrade)
                {
                    if (blogRequest.RequestLocation != RequestLocation.Upgrade
                       && blogRequest.RequestLocation != RequestLocation.LoginPage
                       && blogRequest.RequestLocation != RequestLocation.SystemMessages
                       && blogRequest.RequestLocation != RequestLocation.HostAdmin)
                    {
                        return "~/SystemMessages/UpgradeInProgress.aspx";
                    }
                }
            }
            return null;
        }
    }
}