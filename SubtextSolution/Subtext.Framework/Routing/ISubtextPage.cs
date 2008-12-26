using System.Collections.Generic;
using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    public interface ISubtextPage : IHttpHandler, IRouteableHandler
    {
        void SetControls(IEnumerable<string> controls);
    }
}
