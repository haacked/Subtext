using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Ninject;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Framework
{
    public class SubtextContext : ISubtextContext
    {
        public SubtextContext(Blog blog, RequestContext requestContext, UrlHelper urlHelper, ObjectProvider repository,
                              IPrincipal user, ICache cache, IKernel kernel)
        {
            Blog = blog;
            RequestContext = requestContext;
            UrlHelper = urlHelper;
            Repository = repository;
            User = user ?? requestContext.HttpContext.User;
            Cache = cache ?? new SubtextCache(requestContext.HttpContext.Cache);
            Kernel = kernel;
        }

        protected IKernel Kernel { get; private set; }

        public Blog Blog { get; private set; }

        public RequestContext RequestContext { get; private set; }

        public HttpContextBase HttpContext
        {
            get { return RequestContext.HttpContext; }
        }

        public UrlHelper UrlHelper { get; private set; }

        public ObjectProvider Repository { get; private set; }

        public IPrincipal User { get; private set; }

        public ICache Cache { get; private set; }

        public TService GetService<TService>()
        {
            return Kernel.Get<TService>();
        }
    }
}