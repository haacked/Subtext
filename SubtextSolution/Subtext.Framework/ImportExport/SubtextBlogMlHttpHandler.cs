using Subtext.BlogML;
using Subtext.BlogML.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.ImportExport;
using System.Web.Routing;
using System.Web;
using Subtext.Framework.Routing;
using Subtext.Framework.Providers;

namespace Subtext.Framework.ImportExport
{
    public class SubtextBlogMlHttpHandler : BlogMLHttpHandler {
        public override IBlogMLProvider GetBlogMlProvider() {

            //TODO: RequestContext should come from routing.
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var requestContext = new RequestContext(httpContext, new RouteData());
            var urlHelper = new UrlHelper(requestContext, RouteTable.Routes);
            ISubtextContext context = new SubtextContext(Config.CurrentBlog, requestContext, urlHelper, ObjectProvider.Instance());
            var handler = new SubtextBlogMLProvider(Config.ConnectionString, context);
            handler.PageSize = 100;

            return handler;
        }
    }
}
