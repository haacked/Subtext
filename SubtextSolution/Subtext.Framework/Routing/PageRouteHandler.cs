using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

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
                var subtextPage = page as ISubtextPage;
                if (subtextPage != null) {
                    if (requestContext.RouteData.DataTokens != null) {
                        var controls = requestContext.RouteData.GetControlNames();

                        //Todo: Temporary hack to append .ascx
                        subtextPage.SetControls(controls.Select(s => s += ".ascx"));
                    }
                    subtextPage.SubtextContext = new SubtextContext(Config.CurrentBlog, requestContext, new UrlHelper(requestContext, RouteTable.Routes), ObjectProvider.Instance());
                }
            }
            return page;
        }
    }
}
