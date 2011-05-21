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

using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace Subtext.Framework.Routing
{
    public class PageRouteHandler : RouteHandlerBase
    {
        public PageRouteHandler(string virtualPath, ISubtextPageBuilder pageBuilder, IDependencyResolver serviceLocator)
            : base(serviceLocator)
        {
            VirtualPath = virtualPath;
            PageBuilder = pageBuilder;
        }

        protected ISubtextPageBuilder PageBuilder { get; set; }

        public string VirtualPath { get; protected set; }

        protected override IHttpHandler GetHandler(RequestContext requestContext)
        {
            var page = PageBuilder.CreateInstanceFromVirtualPath(VirtualPath, typeof(Page)) as IHttpHandler;

            if (page != null)
            {
                var pageWithControls = page as IPageWithControls;
                if (pageWithControls != null)
                {
                    if (requestContext.RouteData.DataTokens != null)
                    {
                        IEnumerable<string> controls = requestContext.RouteData.GetControlNames();
                        pageWithControls.SetControls(controls);
                    }
                }
            }
            return page;
        }
    }
}