using System;
using System.Web;
using System.Web.Routing;
using System.IO;

namespace Subtext.Framework.Routing
{
    public class DirectoryRouteHandler : RouteHandlerBase
    {
        public DirectoryRouteHandler() : this(new SubtextPageBuilder()) { 
        }

        public DirectoryRouteHandler(ISubtextPageBuilder pageBuilder) {
            _builder = pageBuilder;
        }
        
        private ISubtextPageBuilder _builder;

        protected override IHttpHandler GetHandler(RequestContext requestContext)
        {
            var routeData = requestContext.RouteData;
            var route = routeData.Route as DirectoryRoute;
            if (route == null)
            {
                throw new InvalidOperationException("DirectoryRouteHandler only works with DirectoryRoutes");
            }

            string virtualPath = "~/" + route.DirectoryName + "/" + routeData.Values["pathinfo"];
            if (String.IsNullOrEmpty(Path.GetExtension(virtualPath)))
            {
                if (!virtualPath.EndsWith("/"))
                {
                    virtualPath += "/";
                }
                virtualPath += "Default.aspx";
            }
            return _builder.CreateInstanceFromVirtualPath(virtualPath, typeof(IHttpHandler)) as IHttpHandler;
        }
    }
}
