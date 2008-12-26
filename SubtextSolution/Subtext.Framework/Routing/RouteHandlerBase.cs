using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    public abstract class RouteHandlerBase : IRouteHandler
    {
        protected abstract IHttpHandler GetHandler(RequestContext requestContext);

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) {
            IHttpHandler handler = GetHandler(requestContext);
            var routableHandler = handler as IRouteableHandler;
            if (routableHandler != null)
            {
                routableHandler.RequestContext = requestContext;
            }
            return handler;
        }
    }
}
