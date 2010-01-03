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
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Subtext.Framework.XmlRpc;

namespace Subtext.Framework.Routing
{
    public static class SubtextRouteExtensions
    {
        public static IEnumerable<string> GetControlNames(this RouteData routeData)
        {
            if(routeData.DataTokens == null)
            {
                return null;
            }
            return routeData.DataTokens[PageRoute.ControlNamesKey] as IEnumerable<string>;
        }

        public static void MapDirectory(this SubtextRouteMapper routes, string directoryName)
        {
            routes.Add(directoryName, new DirectoryRoute(directoryName, routes.ServiceLocator));
        }

        public static void MapSystemDirectory(this SubtextRouteMapper routes, string directoryName)
        {
            routes.Add(directoryName, new SystemDirectoryRoute(directoryName, routes.ServiceLocator));
        }

        public static void MapControls(this SubtextRouteMapper routes, string url, object constraints,
                                       IEnumerable<string> controls)
        {
            routes.MapControls(null, url, ToRouteValueDictionary(constraints), controls, null);
        }

        public static void MapControls(this SubtextRouteMapper routes, string url, RouteValueDictionary constraints,
                                       IEnumerable<string> controls)
        {
            routes.MapControls(null, url, constraints, controls, null);
        }

        public static void MapControls(this SubtextRouteMapper routes, string name, string url, object constraints,
                                       IEnumerable<string> controls)
        {
            routes.MapControls(name, url, ToRouteValueDictionary(constraints), controls, null);
        }

        public static void MapControls(this SubtextRouteMapper routes, string name, string url,
                                       RouteValueDictionary constraints, IEnumerable<string> controls)
        {
            routes.MapControls(name, url, constraints, controls, null);
        }

        public static void MapControls(this SubtextRouteMapper routes, string name, string url,
                                       RouteValueDictionary constraints, IEnumerable<string> controls, RouteValueDictionary defaults)
        {
            var pageRoute = 
            new PageRoute(url, "~/aspx/Dtp.aspx", controls, routes.ServiceLocator)
            {
                Constraints = constraints,
                Defaults = defaults
            };
            routes.Add(name, pageRoute);
        }

        public static void MapControls(this SubtextRouteMapper routes, string url, IEnumerable<string> controls)
        {
            routes.MapControls(url, null, controls);
        }

        public static void MapPageToControl(this SubtextRouteMapper routes, string controlName)
        {
            routes.MapControls(controlName, controlName + ".aspx", null, new[] {controlName});
        }

        /// <summary>
        /// We need special handling here because of Aggregate blogs.
        /// </summary>
        /// <param name="routes"></param>
        public static void MapRoot(this SubtextRouteMapper routes)
        {
            routes.Add("root",
                       new RootRoute(
                           String.Equals(ConfigurationManager.AppSettings["AggregateEnabled"], "true",
                                         StringComparison.OrdinalIgnoreCase), routes.ServiceLocator));
        }

        public static void MapPage(this SubtextRouteMapper routes, string name)
        {
            string url = string.Format("{0}.aspx", name);
            routes.Add(name, new SubtextRoute(url, new PageRouteHandler(string.Format("~/aspx/{0}", url), routes.ServiceLocator.GetService<ISubtextPageBuilder>(), routes.ServiceLocator)));
        }

        public static void MapSystemPage(this SubtextRouteMapper routes, string name)
        {
            string url = string.Format("{0}.aspx", name);
            routes.Add(name,
                       new Route(url,
                                 new PageRouteHandler(string.Format("~/aspx/{0}", url), routes.ServiceLocator.GetService<ISubtextPageBuilder>(),
                                                      routes.ServiceLocator)));
        }

        public static void MapHttpHandler<THttpHandler>(this SubtextRouteMapper routes, string name, string url)
            where THttpHandler : IHttpHandler
        {
            routes.Add(name, new SubtextRoute(url, new HttpRouteHandler<THttpHandler>(routes.ServiceLocator)));
        }

        public static void MapHttpHandler<THttpHandler>(this SubtextRouteMapper routes, string url)
            where THttpHandler : IHttpHandler
        {
            routes.MapHttpHandler<THttpHandler>(null, url);
        }

        public static void MapXmlRpcHandler<TXmlRpcHandler>(this SubtextRouteMapper routes, string url,
                                                            object constraints)
            where TXmlRpcHandler : SubtextXmlRpcService
        {
            routes.Add(new SubtextRoute(url, new XmlRpcRouteHandler<TXmlRpcHandler>(routes.ServiceLocator)));
        }

        public static void MapXmlRpcHandler<TXmlRpcHandler>(this SubtextRouteMapper routes, string name, string url,
                                                            object constraints)
            where TXmlRpcHandler : SubtextXmlRpcService
        {
            routes.Add(name, new SubtextRoute(url, new XmlRpcRouteHandler<TXmlRpcHandler>(routes.ServiceLocator)));
        }

        public static void MapHttpHandler<THttpHandler>(this SubtextRouteMapper routes, string name, string url,
                                                        object constraints) where THttpHandler : IHttpHandler
        {
            var route = new SubtextRoute(url, new HttpRouteHandler<THttpHandler>(routes.ServiceLocator))
            {
                Constraints = ToRouteValueDictionary(constraints)
            };
            routes.Add(name, route);
        }

        public static void MapHttpHandler<THttpHandler>(this SubtextRouteMapper routes, string url, object constraints)
            where THttpHandler : IHttpHandler
        {
            routes.MapHttpHandler<THttpHandler>(null, url, constraints);
        }

        public static void MapImageRoute(this SubtextRouteMapper routes, string routeName, string url)
        {
            routes.Add(routeName, new ImageRoute(url));
        }
        
        public static void MapRoute(this SubtextRouteMapper routes, string routeName, string url, object defaults)
        {
            routes.MapRoute(routeName, url, defaults, null);
        }

        public static void MapRoute(this SubtextRouteMapper routes, string routeName, string url, object defaults,
                                    object constraints)
        {
            routes.Add(routeName, new SubtextRoute(url, new MvcRouteHandler())
            {
                Defaults = ToRouteValueDictionary(defaults),
                Constraints = ToRouteValueDictionary(constraints)
            });
        }

        public static void Ignore(this SubtextRouteMapper routes, string url)
        {
            routes.Add(new IgnoreRoute(url));
        }

        public static string GetSubfolder(this RouteData routeData)
        {
            return routeData.Values["subfolder"] as string;
        }

        private static RouteValueDictionary ToRouteValueDictionary(object anonymousDictionary)
        {
            if(anonymousDictionary == null)
            {
                return null;
            }
            if(anonymousDictionary is RouteValueDictionary)
            {
                return (RouteValueDictionary)anonymousDictionary;
            }
            return new RouteValueDictionary(anonymousDictionary);
        }

        public static Uri ToFullyQualifiedUrl(this VirtualPath virtualPath, Blog blog)
        {
            if(virtualPath == null)
            {
                return null;
            }

            if(blog == null)
            {
                throw new ArgumentNullException("blog");
            }

            var builder = new UriBuilder {Scheme = "http", Host = blog.Host};
            return new Uri(builder.Uri, virtualPath);
        }
    }
}