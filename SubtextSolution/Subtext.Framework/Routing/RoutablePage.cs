using System.Web;
using System.Web.Routing;
using System.Web.UI;

namespace Subtext.Framework.Routing
{
    public class RoutablePage : Page, IRoutableHandler
    {
        public RoutablePage() : this(null) { 
        }

        public RoutablePage(RouteCollection routes) {
            Routes = routes ?? RouteTable.Routes;
        }

        protected RouteCollection Routes {
            get;
            private set;
        }

        public RequestContext RequestContext {
            get {
                if (_requestContext == null) {
                    _requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());
                }
                return _requestContext;
            }
            set {
                _requestContext = value;
            }
        }

        private RequestContext _requestContext;

        public UrlHelper Url
        {
            get {
                if (_urlHelper == null) {
                    _urlHelper = new UrlHelper(RequestContext, Routes);
                }
                return _urlHelper;
            }
        }
        UrlHelper _urlHelper;

        public AdminUrlHelper AdminUrl
        {
            get
            {
                if (_adminUrlHelper == null)
                {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }
        AdminUrlHelper _adminUrlHelper;
    }
}
