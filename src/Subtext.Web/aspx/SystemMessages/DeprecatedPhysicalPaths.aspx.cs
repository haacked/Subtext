using System;
using System.Collections.ObjectModel;
using System.Web.UI;

namespace Subtext.Web.pages.SystemMessages
{
    public partial class DeprecatedPhysicalPaths : Page
    {
        public bool IsAdminOrHostAdmin
        {
            get { return Page.User != null && (Page.User.IsInRole("Admins") || Page.User.IsInRole("HostAdmins")); }
        }

        public ReadOnlyCollection<string> DeprecatedPaths { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            DeprecatedPaths = (Context.ApplicationInstance as SubtextApplication).DeprecatedPhysicalPaths;

            base.OnLoad(e);
        }
    }
}