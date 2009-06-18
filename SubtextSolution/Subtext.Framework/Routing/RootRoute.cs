using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Ninject;
using Subtext.Framework.Web.HttpModules;
using Subtext.Infrastructure;

namespace Subtext.Framework.Routing
{
    // We need special handling of requests for the root due 
    // to the whole aggregate blog situation.
    public class RootRoute : RouteBase
    {
        Route _subfolderAppRootRoute = null;
        Route _subfolderDefaultRoute = null;

        public RootRoute(bool blogAggregationEnabled) : this(blogAggregationEnabled, null, null) { 
        }

        public RootRoute(bool blogAggregationEnabled, IRouteHandler normalRouteHandler, IRouteHandler aggRouteHandler) {
            BlogAggregationEnabled = blogAggregationEnabled;
            NormalRouteHandler = normalRouteHandler ?? new PageRouteHandler("~/Dtp.aspx", Bootstrapper.Kernel.Get<ISubtextPageBuilder>());
            AggregateRouteHandler = aggRouteHandler ?? new PageRouteHandler("~/AggDefault.aspx", Bootstrapper.Kernel.Get<ISubtextPageBuilder>());
        }

        protected bool BlogAggregationEnabled {
            get;
            private set;
        }

        private IRouteHandler GetHandler() {
            return BlogAggregationEnabled ? AggregateRouteHandler : NormalRouteHandler;
        }

        private Route SubfolderDefaultRoute {
            get {
                if (_subfolderDefaultRoute == null) {
                    _subfolderDefaultRoute = new Route("{subfolder}/default.aspx", NormalRouteHandler) { DataTokens = new RouteValueDictionary { { PageRoute.ControlNamesKey, new[] { "homepage" }.AsEnumerable() } } };
                }
                return _subfolderDefaultRoute;
            }
        }

        private Route SubfolderAppRootRoute {
            get {
                if (_subfolderAppRootRoute == null) {
                    _subfolderAppRootRoute = new Route("{subfolder}", NormalRouteHandler) { DataTokens = new RouteValueDictionary { { PageRoute.ControlNamesKey, new[] { "homepage" }.AsEnumerable() } } };
                }
                return _subfolderAppRootRoute;
            }
        }

        public IRouteHandler AggregateRouteHandler {
            get;
            private set;
        }
        
        public IRouteHandler NormalRouteHandler {
            get;
            private set;
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

            var routeData = SubfolderAppRootRoute.GetRouteData(httpContext) ?? SubfolderDefaultRoute.GetRouteData(httpContext);
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
                var vpd = SubfolderAppRootRoute.GetVirtualPath(requestContext, new RouteValueDictionary(new { subfolder = subfolder}));
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
