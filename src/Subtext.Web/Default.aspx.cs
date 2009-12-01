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
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Web
{
    public class _Default : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            //Workaround for Cassini issue with request to /
            //In IIS7, Default.aspx can be deleted.
            var serviceLocator = Bootstrapper.ServiceLocator;
            var route = new RootRoute(HostInfo.Instance.BlogAggregationEnabled, serviceLocator);
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = route.GetRouteData(httpContext);
            var requestContext = new RequestContext(httpContext, routeData);
            string originalPath = Request.Path;
            HttpContext.Current.RewritePath(Request.ApplicationPath, false);
            IRouteHandler routeHandler =
                new PageRouteHandler(
                    HostInfo.Instance.BlogAggregationEnabled ? "~/pages/AggDefault.aspx" : "~/pages/Dtp.aspx",
                    serviceLocator.GetService<ISubtextPageBuilder>(), serviceLocator);
            IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
            httpHandler.ProcessRequest(HttpContext.Current);
            HttpContext.Current.RewritePath(originalPath);
        }
    }
}