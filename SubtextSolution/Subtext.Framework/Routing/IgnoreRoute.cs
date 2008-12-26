using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    /// <summary>
    /// When ignoring routes, we also want to ignore for rendering 
    /// the virtual path. Unfortunately, routing doesn't do this 
    /// yet.
    /// </summary>
    public class IgnoreRoute : Route {
        public IgnoreRoute(string url) : base(url, new StopRoutingHandler()) {
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            return null;
        }
    }
}
