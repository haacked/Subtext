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
using System.Linq;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Web.HttpModules;
using Subtext.Infrastructure;

namespace Subtext.Framework.Routing
{
    // We need special handling of requests for the root due 
    // to the whole aggregate blog situation.
    public class RootRoute : RouteBase
    {
        Route _subfolderAppRootRoute;
        Route _subfolderDefaultRoute;

        public RootRoute(bool blogAggregationEnabled, IServiceLocator serviceLocator)
            : this(blogAggregationEnabled, null, null, serviceLocator)
        {
        }

        public RootRoute(bool blogAggregationEnabled, IRouteHandler normalRouteHandler, IRouteHandler aggRouteHandler,
                         IServiceLocator serviceLocator)
        {
            BlogAggregationEnabled = blogAggregationEnabled;
            NormalRouteHandler = normalRouteHandler ??
                                 new PageRouteHandler("~/pages/Dtp.aspx", serviceLocator.GetService<ISubtextPageBuilder>(), serviceLocator);
            AggregateRouteHandler = aggRouteHandler ??
                                    new PageRouteHandler("~/pages/AggDefault.aspx", serviceLocator.GetService<ISubtextPageBuilder>(), serviceLocator);
        }

        protected bool BlogAggregationEnabled { get; private set; }

        private Route SubfolderDefaultRoute
        {
            get
            {
                if(_subfolderDefaultRoute == null)
                {
                    _subfolderDefaultRoute = new Route("{subfolder}/default.aspx", NormalRouteHandler)
                    {
                        DataTokens =
                            new RouteValueDictionary {{PageRoute.ControlNamesKey, new[] {"homepage"}.AsEnumerable()}}
                    };
                }
                return _subfolderDefaultRoute;
            }
        }

        private Route SubfolderAppRootRoute
        {
            get
            {
                if(_subfolderAppRootRoute == null)
                {
                    _subfolderAppRootRoute = new Route("{subfolder}", NormalRouteHandler)
                    {
                        DataTokens =
                            new RouteValueDictionary {{PageRoute.ControlNamesKey, new[] {"homepage"}.AsEnumerable()}}
                    };
                }
                return _subfolderAppRootRoute;
            }
        }

        public IRouteHandler AggregateRouteHandler { get; private set; }

        public IRouteHandler NormalRouteHandler { get; private set; }

        private IRouteHandler GetHandler()
        {
            return BlogAggregationEnabled ? AggregateRouteHandler : NormalRouteHandler;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string appExecutionPath = httpContext.Request.AppRelativeCurrentExecutionFilePath;

            if(appExecutionPath == "~/" ||
               String.Equals(appExecutionPath, "~/default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                var appRootRouteData = new RouteData
                {
                    Route = this,
                    RouteHandler = GetHandler(),
                };
                if(!BlogAggregationEnabled)
                {
                    appRootRouteData.DataTokens.Add(PageRoute.ControlNamesKey, new[] {"homepage"}.AsEnumerable());
                }
                return appRootRouteData;
            }

            var blogRequest = httpContext.Items[BlogRequest.BlogRequestKey] as BlogRequest;
            if(blogRequest == null || String.IsNullOrEmpty(blogRequest.Subfolder))
            {
                return null;
            }

            RouteData routeData = SubfolderAppRootRoute.GetRouteData(httpContext) ??
                                  SubfolderDefaultRoute.GetRouteData(httpContext);
            if(routeData != null)
            {
                routeData.Route = this;
                if(!String.Equals(blogRequest.Subfolder, routeData.GetSubfolder(), StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            object subfolderValue;
            if(values == null || !values.TryGetValue("subfolder", out subfolderValue))
            {
                requestContext.RouteData.Values.TryGetValue("subfolder", out subfolderValue);
            }

            var subfolder = subfolderValue as string;

            if(!String.IsNullOrEmpty(subfolder))
            {
                VirtualPathData vpd = SubfolderAppRootRoute.GetVirtualPath(requestContext,
                                                                           new RouteValueDictionary(new {subfolder}));
                vpd.Route = this;
                return vpd;
            }

            if(values == null || values.Count == 0)
            {
                return new VirtualPathData(this, string.Empty);
            }
            return null;
        }
    }
}