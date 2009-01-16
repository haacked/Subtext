using System.Web.UI;
using Subtext.Framework.Routing;

namespace Subtext.Web.Admin.WebUI
{
    public class AdminMasterPage : MasterPage
    {
        public UrlHelper Url
        {
            get
            {
                var page = this.Page as IRoutableHandler;
                if (page != null)
                {
                    return page.Url;
                }
                return null;
            }
        }

        public AdminUrlHelper AdminUrl {
            get {
                if (_adminUrlHelper == null) {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }
        AdminUrlHelper _adminUrlHelper;
    }
}
