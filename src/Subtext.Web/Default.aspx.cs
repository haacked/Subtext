using System;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using Ninject;
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
            IKernel kernel = Bootstrapper.Kernel;
            var route = new RootRoute(HostInfo.Instance.BlogAggregationEnabled, kernel);
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = route.GetRouteData(httpContext);
            var requestContext = new RequestContext(httpContext, routeData);
            string originalPath = Request.Path;
            HttpContext.Current.RewritePath(Request.ApplicationPath, false);
            IRouteHandler routeHandler =
                new PageRouteHandler(
                    HostInfo.Instance.BlogAggregationEnabled ? "~/pages/AggDefault.aspx" : "~/pages/Dtp.aspx",
                    kernel.Get<ISubtextPageBuilder>(), kernel);
            IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
            httpHandler.ProcessRequest(HttpContext.Current);
            HttpContext.Current.RewritePath(originalPath);
        }
    }
}