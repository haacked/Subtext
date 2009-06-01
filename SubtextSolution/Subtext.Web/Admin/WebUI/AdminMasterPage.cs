using System.Web.UI;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web.Admin.WebUI
{
    public class AdminMasterPage : MasterPage
    {
        public UrlHelper Url {
            get {
                return SubtextPage.Url;
            }
        }

        public AdminUrlHelper AdminUrl {
            get {
                return SubtextPage.AdminUrl;
            }
        }

        public SubtextPage SubtextPage {
            get {
                return Page as SubtextPage;
            }
        }
    }
}
