using System.Web;
using System.Web.Routing;
using Subtext.BlogML;
using Subtext.BlogML.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.ImportExport;

namespace Subtext.Framework.ImportExport
{
    public class SubtextBlogMlHttpHandler : BlogMLHttpHandler, ISubtextHandler {
        public override IBlogMLProvider GetBlogMlProvider() {
            var handler = new SubtextBlogMLProvider(Config.ConnectionString, SubtextContext);
            handler.PageSize = 100;
            return handler;
        }

        public UrlHelper Url
        {
            get {
                return SubtextContext.UrlHelper;
            }
        }

        public ISubtextContext SubtextContext
        {
            get;
            set;
        }

        public RequestContext RequestContext
        {
            get {
                return SubtextContext.RequestContext;
            }
            set {
            }
        }
    }
}
