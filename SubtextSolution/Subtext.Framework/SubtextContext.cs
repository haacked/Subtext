using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Subtext.Framework.Routing;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
    public class SubtextContext : Subtext.Framework.ISubtextContext
    {
        public SubtextContext(Blog blog, RequestContext requestContext, UrlHelper urlHelper, ObjectProvider repository) {
            Blog = blog;
            RequestContext = requestContext;
            UrlHelper = urlHelper;
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
    }
}
