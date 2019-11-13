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
using Ninject;
using Subtext.Framework;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web
{
    /// <summary>
    /// Displays the blog not active message.
    /// </summary>
    public partial class UpgradeInProgress : SubtextPage
    {
        [Inject]
        public IInstallationManager InstallationManager
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            InstallationState state = InstallationManager.GetInstallationStatus(VersionInfo.CurrentAssemblyVersion);
            if (state == InstallationState.NeedsUpgrade)
            {
                plcUpgradeInProgressMessage.Visible = true;
                plcNothingToSeeHere.Visible = false;
            }
            else
            {
                plcUpgradeInProgressMessage.Visible = true;
                plcNothingToSeeHere.Visible = false;
                lnkBlog.HRef = Url.BlogUrl();
            }
            base.OnLoad(e);
        }
    }
}