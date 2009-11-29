using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;

namespace Subtext.Framework
{
    public interface ISubtextContext : IServiceLocator
    {
        Blog Blog { get; }
        ObjectProvider Repository { get; }
        RequestContext RequestContext { get; }
        HttpContextBase HttpContext { get; }
        UrlHelper UrlHelper { get; }
        IPrincipal User { get; }
        ICache Cache { get; }
    }
}