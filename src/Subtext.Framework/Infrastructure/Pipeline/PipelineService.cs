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

using System.Web;
using System.Web.Routing;
using Subtext.Framework.Routing;

namespace Subtext.Infrastructure
{
    public class PipelineService
    {
        public PipelineService(HttpContextBase httpContext, IServiceLocator serviceProvider)
        {
            HttpContext = httpContext;
            ServiceProvider = serviceProvider;
        }

        protected HttpContextBase HttpContext { get; private set; }
        protected IServiceLocator ServiceProvider { get; private set; }

        public void ProcessRootRequest(RootRoute route, IRouteHandler routeHandler)
        {
            // todo: unit test this method
            var request = HttpContext.Request;
            RouteData routeData = route.GetRouteData(HttpContext);
            var requestContext = new RequestContext(HttpContext, routeData);
            string originalPath = request.Path;
            HttpContext.RewritePath(request.ApplicationPath, false);
            IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
            httpHandler.ProcessRequest(System.Web.HttpContext.Current);
            HttpContext.RewritePath(originalPath);
        }
    }
}
