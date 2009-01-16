using System.Collections.Generic;
using System.Web;

namespace Subtext.Framework.Routing
{
    public interface ISubtextPage : IHttpHandler, IRoutableHandler {
        void SetControls(IEnumerable<string> controls);
        ISubtextContext SubtextContext { get; set; }
    }
}
