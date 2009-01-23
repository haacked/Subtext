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

using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.XmlRpc;

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
            routes.Add(directoryName, new DirectoryRoute(directoryName));
        }

        public static void MapControls(this RouteCollection routes, string url, object constraints, IEnumerable<string> controls) {
            routes.MapControls(url, new RouteValueDictionary(constraints), controls);
        }

        public static void MapControls(this RouteCollection routes, string url, RouteValueDictionary constraints, IEnumerable<string> controls)
        {
            routes.Add(new PageRoute(url, "~/Dtp.aspx", controls) { Constraints = constraints });
        }

        public static void MapControls(this RouteCollection routes, string name, string url, object constraints, IEnumerable<string> controls)
        {
            routes.MapControls(name, url, new RouteValueDictionary(constraints), controls);
        }

        public static void MapControls(this RouteCollection routes, string name, string url, RouteValueDictionary constraints, IEnumerable<string> controls) {
            routes.Add(name, new PageRoute(url, "~/Dtp.aspx", controls) { Constraints = constraints });
        }

        public static void MapControls(this RouteCollection routes, string url, IEnumerable<string> controls) {
            routes.MapControls(url, new { }, controls);
        }

        public static void MapPageToControl(this RouteCollection routes, string controlName)
        {
            routes.MapControls(controlName, controlName + ".aspx", null, new string[] { controlName });
        }

        /// <summary>
        /// We need special handling here because of Aggregate blogs.
        /// </summary>
        /// <param name="routes"></param>
        public static void MapRoot(this RouteCollection routes) {
            var aggDisabledConstraints = new RouteValueDictionary();
            aggDisabledConstraints.Add("", new AggregateEnabledConstraint(false));
            routes.MapControls("Default.aspx", aggDisabledConstraints, new[] { "homepage" });
            routes.MapControls("root", string.Empty, aggDisabledConstraints, new[] { "homepage" });

            var aggEnabledConstraints = new RouteValueDictionary();
            aggEnabledConstraints.Add("", new AggregateEnabledConstraint(true));
            routes.Add(new SubtextRoute("Default.aspx", new PageRouteHandler("~/AggDefault.aspx")) { Constraints = aggEnabledConstraints });
            routes.Add(new SubtextRoute(string.Empty, new PageRouteHandler("~/AggDefault.aspx")) { Constraints = aggEnabledConstraints });
        }

        public static void MapPage(this RouteCollection routes, string name)
        {
            string url = name + ".aspx";
            routes.Add(name, new SubtextRoute(url, new PageRouteHandler("~/" + url)));
        }

        public static void MapPage(this RouteCollection routes, string name, string url, string virtualPath)
        {
            routes.Add(name, new SubtextRoute(url, new PageRouteHandler(virtualPath)));
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
            where TXmlRpcHandler : SubtextXmlRpcService
        {
            routes.Add(new SubtextRoute(url, new XmlRpcRouteHandler<TXmlRpcHandler>()));
        }

        public static void MapXmlRpcHandler<TXmlRpcHandler>(this RouteCollection routes, string url, object constraints)
            where TXmlRpcHandler : SubtextXmlRpcService
        {
            routes.Add(new SubtextRoute(url, new XmlRpcRouteHandler<TXmlRpcHandler>()));
        }

        public static void MapXmlRpcHandler<TXmlRpcHandler>(this RouteCollection routes, string name, string url, object constraints)
            where TXmlRpcHandler : SubtextXmlRpcService
        {
            routes.Add(name, new SubtextRoute(url, new XmlRpcRouteHandler<TXmlRpcHandler>()));
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
