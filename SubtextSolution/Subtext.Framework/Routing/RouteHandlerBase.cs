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

using System.Web;
using System.Web.Routing;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Routing
{
    public abstract class RouteHandlerBase : IRouteHandler
    {
        protected abstract IHttpHandler GetHandler(RequestContext requestContext);

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) {
            IHttpHandler handler = GetHandler(requestContext);
            var subtextHandler = handler as ISubtextHandler;
            if (subtextHandler != null) {
                subtextHandler.SubtextContext = new SubtextContext(Config.CurrentBlog, requestContext, new UrlHelper(requestContext, RouteTable.Routes), ObjectProvider.Instance());
            }
            else {
                var routableHandler = handler as IRoutableHandler;
                if (routableHandler != null) {
                    routableHandler.RequestContext = requestContext;
                }
            }

            return handler;
        }
    }
}
