using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Routing
{
    // We need special handling of requests for the root due 
    // to the whole aggregate blog situation.
    public class RootRoute : RouteBase
    {
        static IRouteHandler _aggRouteHandler = new PageRouteHandler("~/AggDefault.aspx");
        static IRouteHandler _normalRouteHandler = new PageRouteHandler("~/Dtp.aspx");
        static Route subfolderAppRootRoute = new Route("{subfolder}", _normalRouteHandler) { DataTokens = new RouteValueDictionary { { PageRoute.ControlNamesKey, new[] { "homepage" }.AsEnumerable() } } };
        static Route subfolderDefaultRoute = new Route("{subfolder}/default.aspx", _normalRouteHandler) { DataTokens = new RouteValueDictionary { { PageRoute.ControlNamesKey, new[] { "homepage" }.AsEnumerable() } } };

        public RootRoute(bool blogAggregationEnabled) {
            BlogAggregationEnabled = blogAggregationEnabled;
        }

        protected bool BlogAggregationEnabled {
            get;
            private set;
        }

        private IRouteHandler GetHandler() {
            return BlogAggregationEnabled ? _aggRouteHandler : _normalRouteHandler;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            string appExecutionPath = httpContext.Request.AppRelativeCurrentExecutionFilePath;

            if (appExecutionPath == "~/" || String.Equals(appExecutionPath, "~/default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                var appRootRouteData = new RouteData {
                    Route = this,
                    RouteHandler = GetHandler(), 
                };
                if (!BlogAggregationEnabled) {
                    appRootRouteData.DataTokens.Add(PageRoute.ControlNamesKey, new[] { "homepage" }.AsEnumerable());
                }
                return appRootRouteData;
            }

            var blogRequest = httpContext.Items[BlogRequest.BlogRequestKey] as BlogRequest;
            if (blogRequest == null || String.IsNullOrEmpty(blogRequest.Subfolder)) {
                return null;
            }

            var routeData = subfolderAppRootRoute.GetRouteData(httpContext) ?? subfolderDefaultRoute.GetRouteData(httpContext);
            if (routeData != null) {
                routeData.Route = this;
                if (!String.Equals(blogRequest.Subfolder, routeData.Values["subfolder"] as string, StringComparison.OrdinalIgnoreCase)) {
                    return null;
                } 
            }
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            object subfolderValue = null;
            if (!values.TryGetValue("subfolder", out subfolderValue)) {
                requestContext.RouteData.Values.TryGetValue("subfolder", out subfolderValue);
            }

            string subfolder = subfolderValue as string;

            if (!String.IsNullOrEmpty(subfolder)) {
                var vpd = subfolderAppRootRoute.GetVirtualPath(requestContext, new RouteValueDictionary(new { subfolder = subfolder}));
                vpd.Route = this;
                return vpd;
            }

            if (values.Count == 0) {
                return new VirtualPathData(this, string.Empty);
            }
            return null;
        }
    }
}
