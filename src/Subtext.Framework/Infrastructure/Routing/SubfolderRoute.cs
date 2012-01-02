#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    class SubfolderRoute : Route
    {
        readonly Route _parent;

        public SubfolderRoute(Route parent)
            : base("{subfolder}/" + parent.Url, parent.RouteHandler)
        {
            _parent = parent;
            Constraints = parent.Constraints;
            Defaults = parent.Defaults;
            DataTokens = parent.DataTokens;
        }

        public RouteData GetRouteData(HttpContextBase httpContext, string subfolder)
        {
            RouteData routeData = GetRouteData(httpContext);
            if (routeData != null)
            {
                if (!String.Equals(subfolder, routeData.GetSubfolder(), StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }
            return routeData;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData routeData = base.GetRouteData(httpContext);
            if (routeData != null)
            {
                routeData.Route = _parent;
            }
            return routeData;
        }
    }
}