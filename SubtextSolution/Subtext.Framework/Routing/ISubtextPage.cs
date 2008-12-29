using System.Collections.Generic;
using System.Web;

namespace Subtext.Framework.Routing
{
    public interface ISubtextPage : IHttpHandler, IRouteableHandler {
        void SetControls(IEnumerable<string> controls);
        ISubtextContext SubtextContext { get; set; }
    }
}
