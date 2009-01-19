using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subtext.Extensibility.Web;
using Subtext.Framework.Routing;
using System.Web.Routing;
using System.Web;

namespace Subtext.Framework.Web.Handlers
{
    public abstract class SubtextHttpHandlerBase : BaseHttpHandler, ISubtextHandler
    {
        public SubtextHttpHandlerBase() : this(null) { 
        }

        public SubtextHttpHandlerBase(RouteCollection routes) {
            Routes = routes ?? RouteTable.Routes;
        }

        public RequestContext RequestContext {
            get;
            set;
        }
        
        protected RouteCollection Routes {
            get;
            private set;
        }

        public UrlHelper Url
        {
            get {
                if (_urlHelper == null)
                {
                    _urlHelper = new UrlHelper(RequestContext, Routes);
                }
                return _urlHelper;
            }
        }
        UrlHelper _urlHelper;

        protected override void HandleRequest(HttpContext context)
        {
            HandleRequest(SubtextContext);
        }

        protected abstract void HandleRequest(ISubtextContext context);

        public ISubtextContext SubtextContext
        {
            get;
            set;
        }
    }
}
