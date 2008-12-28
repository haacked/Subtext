using CookComputing.XmlRpc;
using Subtext.Framework.Routing;

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
    }
}
