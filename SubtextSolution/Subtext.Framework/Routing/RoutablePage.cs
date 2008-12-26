using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.Routing;
using System.Web.Hosting;

namespace Subtext.Framework.Routing
{
    public class RoutablePage : Page, IRouteableHandler
    {
        public RoutablePage() : this(null) { 
        }

        public RoutablePage(RouteCollection routes) {
            _routes = routes ?? RouteTable.Routes;
        }
        RouteCollection _routes;

        public RequestContext RequestContext {
            get;
            set;
        }

        public UrlHelper Url
        {
            get {
                if (_urlHelper == null) {
                    _urlHelper = new UrlHelper(RequestContext, _routes);
                }
                return _urlHelper;
            }
        }
        UrlHelper _urlHelper;
    }
}
