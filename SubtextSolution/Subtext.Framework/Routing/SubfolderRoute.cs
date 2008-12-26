using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace Subtext.Framework.Routing
{
    class SubfolderRoute : Route
    {
        public SubfolderRoute(Route parent) : base("{subfolder}/" + parent.Url, parent.RouteHandler) {
            _parent = parent;
            this.Constraints = parent.Constraints;
            this.Defaults = parent.Defaults;
            this.DataTokens = parent.DataTokens;
        }

        Route _parent;

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData != null) {
                routeData.Route = _parent;
            }
            return routeData;
        }
    }
}
