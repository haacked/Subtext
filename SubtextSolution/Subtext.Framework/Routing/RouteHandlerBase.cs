﻿#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System.Web;
using System.Web.Routing;
using Ninject;
using Subtext.Infrastructure;

namespace Subtext.Framework.Routing
{
    public abstract class RouteHandlerBase : IRouteHandler
    {
        public RouteHandlerBase() : this(Bootstrapper.Kernel) { 
        }

        public RouteHandlerBase(IKernel kernel) {
            Kernel = kernel;
        }

        protected abstract IHttpHandler GetHandler(RequestContext requestContext);

        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext) {
            Bootstrapper.RequestContext = requestContext;
            IHttpHandler handler = GetHandler(requestContext);
            Kernel.Inject(handler);
            return handler;
        }

        public IKernel Kernel {
            get;
            private set;
        }
    }
}