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
            if (String.IsNullOrEmpty(blogRequest.Subfolder)) {
                routeData = base.GetRouteData(httpContext);
                if (routeData != null) {
                    //Add current subfolder info.
                    routeData.Values.Add("subfolder", string.Empty);
                }
            }
            else {
                routeData = RouteForSubfolder.GetRouteData(httpContext, blogRequest.Subfolder);
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
            if (String.IsNullOrEmpty(requestContext.RouteData.Values["subfolder"] as string)) {
                values["subfolder"] = requestContext.RouteData.Values["subfolder"];
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
