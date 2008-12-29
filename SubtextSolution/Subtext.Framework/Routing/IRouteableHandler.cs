using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    public interface IRouteableHandler : IHttpHandler
    {
        RequestContext RequestContext {
            get;
            set;
        }

        UrlHelper Url {
            get;
        }
    }
}
