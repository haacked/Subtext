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
using System.IO;
using System.Web;
using System.Web.Routing;
using Ninject;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Routing
{
    public class DirectoryRouteHandler : PageRouteHandler
    {
        public DirectoryRouteHandler(ISubtextPageBuilder pageBuilder, IKernel kernel)
            : base(null, pageBuilder, kernel)
        {
        }

        protected override IHttpHandler GetHandler(RequestContext requestContext)
        {
            RouteData routeData = requestContext.RouteData;
            var route = routeData.Route as IDirectoryRoute;
            if(route == null)
            {
                throw new InvalidOperationException(
                    Resources.InvalidOperation_DirectoryRouteHandlerWorksWithDirectoryRoutes);
            }

            string virtualPath = string.Format("~/pages/{0}/{1}", route.DirectoryName, routeData.Values["pathinfo"]);
            if(String.IsNullOrEmpty(Path.GetExtension(virtualPath)))
            {
                if(!virtualPath.EndsWith("/"))
                {
                    virtualPath += "/";
                }
                virtualPath += "Default.aspx";
            }
            VirtualPath = virtualPath;
            return base.GetHandler(requestContext);
        }
    }
}