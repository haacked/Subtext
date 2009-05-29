using System.Security.Principal;
using System.Web.Routing;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Framework
{
    public interface ISubtextContext
    {
        Blog Blog { get; }
        ObjectProvider Repository { get; }
        RequestContext RequestContext { get; }
        UrlHelper UrlHelper { get; }
        IPrincipal User { get; }
        ICache Cache { get; }
    }
}
