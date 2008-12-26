using System.Collections.Generic;
using System.Web.Routing;
using System.Web;
using CookComputing.XmlRpc;

namespace Subtext.Framework.Routing
{
    public static class SubtextRouteExtensions
    {
        public static IEnumerable<string> GetControlNames(this RouteData routeData) {
            if (routeData.DataTokens == null) {
                return null;
            }
            return routeData.DataTokens[PageRoute.ControlNamesKey] as IEnumerable<string>;
        }

        public static void MapDirectory(this RouteCollection routes, string directoryName) {
            routes.Add(new DirectoryRoute(directoryName));
        }

        public static void MapControls(this RouteCollection routes, string url, object constraints, IEnumerable<string> controls) {
            routes.Add(new PageRoute(url, "~/Dtp.aspx", controls) { Constraints = new RouteValueDictionary(constraints) });
        }

        public static void MapControls(this RouteCollection routes, string name, string url, object constraints, IEnumerable<string> controls)
        {
            routes.Add(name, new PageRoute(url, "~/Dtp.aspx", controls) { Constraints = new RouteValueDictionary(constraints ?? new { }) });
        }

        public static void MapControls(this RouteCollection routes, string url, IEnumerable<string> controls) {
            routes.MapControls(url, new { }, controls);
        }

        public static void MapPageToControl(this RouteCollection routes, string controlName)
        {
            routes.MapControls(controlName + ".aspx", new string[] { controlName });
        }

        public static void MapPage(this RouteCollection routes, string url, string virtualPath)
        {
            routes.Add(new SubtextRoute(url, new PageRouteHandler(virtualPath)));
        }

        public static void MapHttpHandler<THttpHandler>(this RouteCollection routes, string name, string url) where THttpHandler : IHttpHandler, new() {
            routes.Add(name, new SubtextRoute(url, new HttpRouteHandler<THttpHandler>()));
        }

        public static void MapHttpHandler<THttpHandler>(this RouteCollection routes, string url) 
            where THttpHandler : IHttpHandler, new() {
                routes.MapHttpHandler<THttpHandler>(null, url);
        }

        public static void MapXmlRpcHandler<TXmlRpcHandler>(this RouteCollection routes, string url)
            where TXmlRpcHandler : XmlRpcService, new()
        {
            routes.Add(new SubtextRoute(url, new HttpRouteHandler<TXmlRpcHandler>(new TXmlRpcHandler())));
        }

        public static void MapXmlRpcHandler<TXmlRpcHandler>(this RouteCollection routes, string url, object constraints)
            where TXmlRpcHandler : XmlRpcService, new()
        {
            routes.Add(new SubtextRoute(url, new HttpRouteHandler<TXmlRpcHandler>(new TXmlRpcHandler())));
        }

        public static void MapHttpHandler<THttpHandler>(this RouteCollection routes, string name, string url, object constraints) where THttpHandler : IHttpHandler, new() {
            var route = new SubtextRoute(url, new HttpRouteHandler<THttpHandler>(new THttpHandler()));
            route.Constraints = new RouteValueDictionary(constraints);
            routes.Add(name, route);
        }

        public static void MapHttpHandler<THttpHandler>(this RouteCollection routes, string url, object constraints)
            where THttpHandler : IHttpHandler, new()
        {
            routes.MapHttpHandler<THttpHandler>(null, url, constraints);
        }

        public static void Ignore(this RouteCollection routes, string url)
        {
            routes.Add(new IgnoreRoute(url));   
        }
    }
}
