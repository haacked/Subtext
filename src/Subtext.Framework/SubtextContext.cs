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

using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Framework
{
    public class SubtextContext : ISubtextContext
    {
        public SubtextContext(Blog blog, RequestContext requestContext, UrlHelper urlHelper, ObjectProvider repository,
                              IPrincipal user, ICache cache, IServiceLocator serviceLocator)
        {
            Blog = blog;
            RequestContext = requestContext;
            UrlHelper = urlHelper;
            Repository = repository;
            User = user ?? requestContext.HttpContext.User;
            Cache = cache ?? new SubtextCache(requestContext.HttpContext.Cache);
            ServiceLocator = serviceLocator;
        }

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

        public IServiceLocator ServiceLocator
        {
            get; 
            private set;
        }
    }
}