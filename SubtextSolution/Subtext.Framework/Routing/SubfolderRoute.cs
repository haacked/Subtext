#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System.Web;
using System.Web.Routing;
using System;

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

        public RouteData GetRouteData(HttpContextBase httpContext, string subfolder) {
            var routeData = GetRouteData(httpContext);
            if (routeData != null) {
                if (!String.Equals(subfolder, routeData.Values["subfolder"] as string, StringComparison.OrdinalIgnoreCase)) {
                    return null;
                }
            }
            return routeData;
        }

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
