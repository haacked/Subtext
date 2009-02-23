using System.Web;
using System.Web.Routing;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Routing;

namespace Subtext.Web
{
    public partial class _Default : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            //Workaround for Cassini issue with request to /
            //In IIS7, Default.aspx can be deleted.
            var route = new RootRoute(HostInfo.Instance.BlogAggregationEnabled);
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = route.GetRouteData(httpContext);
            RequestContext requestContext = new RequestContext(httpContext, routeData);
            string originalPath = Request.Path;
            HttpContext.Current.RewritePath(Request.ApplicationPath, false);
            IRouteHandler routeHandler = new PageRouteHandler(HostInfo.Instance.BlogAggregationEnabled ? "~/AggDefault.aspx" : "~/Dtp.aspx");
            IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
            httpHandler.ProcessRequest(HttpContext.Current);
            HttpContext.Current.RewritePath(originalPath);
        }
    }
}
