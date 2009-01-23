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

using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;

namespace Subtext.Framework.Routing
{
    public class PageRouteHandler : RouteHandlerBase
    {
        public PageRouteHandler(string virtualPath) : this(virtualPath, new SubtextPageBuilder()) { 
        }

        public PageRouteHandler(string virtualPath, ISubtextPageBuilder pageBuilder) {
            VirtualPath = virtualPath;
            _builder = pageBuilder;
        }

        private ISubtextPageBuilder _builder;

        public string VirtualPath { 
            get; 
            private set; 
        }

        protected override IHttpHandler GetHandler(RequestContext requestContext) {
            var page = _builder.CreateInstanceFromVirtualPath(this.VirtualPath, typeof(Page)) as IHttpHandler;

            if (page != null) {
                var pageWithControls = page as IPageWithControls;
                if (pageWithControls != null) {
                    if (requestContext.RouteData.DataTokens != null) {
                        var controls = requestContext.RouteData.GetControlNames();
                        //TODO: Temporary hack to append .ascx
                        pageWithControls.SetControls(controls.Select(s => s += ".ascx"));
                    }
                }
            }
            return page;
        }
    }
}
