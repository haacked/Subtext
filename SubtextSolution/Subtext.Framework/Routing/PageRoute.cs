using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    public class PageRoute : SubtextRoute
    {
        internal const string ControlNamesKey = "controls";

        public PageRoute(string url, string virtualPath, IEnumerable<string> controls)
            : base(url, new PageRouteHandler(virtualPath))
        {
            DataTokens = new RouteValueDictionary();
            DataTokens.Add(ControlNamesKey, controls.AsEnumerable());
        }
    }
}