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

using System;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.XmlRpc;

namespace Subtext.Framework.Routing
{
    public class XmlRpcRouteHandler<THandler> : IRouteHandler where THandler : SubtextXmlRpcService {
        public IHttpHandler GetHttpHandler(RequestContext requestContext) {
            ISubtextContext context = new SubtextContext(Config.CurrentBlog, requestContext, new UrlHelper(requestContext, RouteTable.Routes), ObjectProvider.Instance());

            var ctor = typeof(THandler).GetConstructor(new Type[] { typeof(ISubtextContext) });
            var service = ctor.Invoke(new object[] { context }) as IHttpHandler;
            return service;
        }
    }
}
