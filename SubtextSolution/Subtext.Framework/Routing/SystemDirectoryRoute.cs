using System.Web.Routing;
using Ninject;
using Subtext.Infrastructure;

namespace Subtext.Framework.Routing
{
    public class SystemDirectoryRoute : Route, IDirectoryRoute
    {
        public SystemDirectoryRoute(string directoryName)
            : base(directoryName + "/{*pathInfo}", new DirectoryRouteHandler(Bootstrapper.Kernel.Get<ISubtextPageBuilder>())) {
                DirectoryName = directoryName;
        }

        public string DirectoryName {
            get;
            private set;
        }
    }
}
