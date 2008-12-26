using System.Web.Routing;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Framework
{
    public interface ISubtextContext
    {
        BlogInfo Blog { get; }
        ObjectProvider Repository { get; }
        RequestContext RequestContext { get; }
        UrlHelper UrlHelper { get; }
    }
}
