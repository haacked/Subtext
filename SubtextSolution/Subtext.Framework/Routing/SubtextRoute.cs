using System;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Routing
{
    public class SubtextRoute : Route
    {
        public SubtextRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler) { 
        }

        public virtual RouteData GetRouteData(HttpContextBase httpContext, BlogRequest blogRequest)
        {
            RouteData routeData = null;
            if (String.IsNullOrEmpty(blogRequest.Subfolder))
            {
                routeData = base.GetRouteData(httpContext);
                if (routeData != null)
                {
                    //Add current subfolder info.
                    routeData.Values.Add("subfolder", string.Empty);
                }
            }
            else
            {
                routeData = RouteForSubfolder.GetRouteData(httpContext);
            }

            return routeData;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            BlogRequest request = (BlogRequest)httpContext.Items[BlogRequest.BlogRequestKey];
            return GetRouteData(httpContext, request);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (String.IsNullOrEmpty(requestContext.RouteData.Values["subfolder"] as string))
            {
                return base.GetVirtualPath(requestContext, values);
            }
            else {
                return RouteForSubfolder.GetVirtualPath(requestContext, values);
            }
        }

        private SubfolderRoute RouteForSubfolder
        {
            get
            {
                var subfolderRoute = _subfolderRoute;
                //Not going to lock...
                if (subfolderRoute == null)
                {
                    subfolderRoute = new SubfolderRoute(this);
                    _subfolderRoute = subfolderRoute;
                }
                return subfolderRoute;
            }
        }
        SubfolderRoute _subfolderRoute;
    }
}
