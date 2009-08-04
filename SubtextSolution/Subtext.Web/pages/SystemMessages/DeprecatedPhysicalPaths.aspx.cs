using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Subtext.Web.pages.SystemMessages
{
    public partial class DeprecatedPhysicalPaths : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            DeprecatedPaths = (Context.ApplicationInstance as SubtextApplication).DeprecatedPhysicalPaths;

            base.OnLoad(e);
        }

        public bool IsAdminOrHostAdmin {
            get { 
                return Page.User != null && (Page.User.IsInRole("Admins") || Page.User.IsInRole("HostAdmins"));
            }
        }

        public ReadOnlyCollection<string> DeprecatedPaths
        {
            get;
            private set;
        }
    }
}
