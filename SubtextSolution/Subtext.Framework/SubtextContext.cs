using System.Security.Principal;
using System.Web.Routing;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework
{
    public class SubtextContext : Subtext.Framework.ISubtextContext
    {
        public SubtextContext(Blog blog, RequestContext requestContext, UrlHelper urlHelper, ObjectProvider repository) {
            Blog = blog;
            RequestContext = requestContext;
            UrlHelper = urlHelper;
            Repository = repository;
            User = requestContext.HttpContext.User;
        }

        public Blog Blog
        {
            get;
            private set;
        }

        public RequestContext RequestContext {
            get;
            private set;
        }

        public UrlHelper UrlHelper {
            get;
            private set;
        }

        public ObjectProvider Repository {
            get;
            private set;
        }

        public IPrincipal User {
            get;
            private set;
        }
    }
}
