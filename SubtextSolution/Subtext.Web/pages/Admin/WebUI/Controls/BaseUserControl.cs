using System.Web.UI;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web.Admin.WebUI.Controls
{
    public class BaseUserControl : UserControl
    {
        public UrlHelper Url {
            get {
                return SubtextPage.Url;
            }
        }

        public SubtextPage SubtextPage {
            get {
                return Page as SubtextPage;
            }
        }
    }
}
