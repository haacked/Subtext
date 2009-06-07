﻿using CookComputing.XmlRpc;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using System.Web.Routing;

namespace Subtext.Framework.XmlRpc
{
    public abstract class SubtextXmlRpcService : XmlRpcService
    {
        public SubtextXmlRpcService(ISubtextContext context) {
            SubtextContext = context;
        }

        protected ISubtextContext SubtextContext {
            get;
            private set;
        }

        protected Blog Blog {
            get {
                return SubtextContext.Blog;
            }
        }

        protected UrlHelper Url
        {
            get {
                return SubtextContext.UrlHelper;
            }
        }

        protected ObjectProvider Repository {
            get {
                return SubtextContext.Repository;
            }
        }

        protected RouteValueDictionary RouteValues {
            get {
                return SubtextContext.RequestContext.RouteData.Values;
            }
        }
    }
}