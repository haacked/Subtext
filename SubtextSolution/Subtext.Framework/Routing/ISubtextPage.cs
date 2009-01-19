using System.Collections.Generic;
using System.Web;

namespace Subtext.Framework.Routing
{
    public interface ISubtextPage : IHttpHandler, ISubtextHandler {
        void SetControls(IEnumerable<string> controls);
    }
}
