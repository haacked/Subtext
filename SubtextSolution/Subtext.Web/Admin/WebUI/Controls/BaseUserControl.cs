using System.Web.UI;
using Subtext.Framework.Routing;

namespace Subtext.Web.Admin.WebUI.Controls
{
    public class BaseUserControl : UserControl
    {
        public UrlHelper Url
        {
            get
            {
                var routablePage = RoutablePage;
                if (routablePage != null) {
                    return routablePage.Url;
                }
                return null;
            }
        }

        public IRoutableHandler RoutablePage {
            get {
                return Page as IRoutableHandler;
            }
        }
    }
}
