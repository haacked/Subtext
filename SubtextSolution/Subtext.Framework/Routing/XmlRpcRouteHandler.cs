using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using Subtext.Framework.XmlRpc;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Routing
{
    public class XmlRpcRouteHandler<THandler> : IRouteHandler where THandler : SubtextXmlRpcService
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext) {
            ISubtextContext context = new SubtextContext(Config.CurrentBlog, requestContext, new UrlHelper(requestContext, RouteTable.Routes), ObjectProvider.Instance());

            var ctor = typeof(THandler).GetConstructor(new Type[] { typeof(ISubtextContext) });
            var service = ctor.Invoke(new object[] { context }) as IHttpHandler;
            return service;
        }
    }
}
