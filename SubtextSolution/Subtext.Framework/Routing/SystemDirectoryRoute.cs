using System.Web.Routing;
using Ninject;

namespace Subtext.Framework.Routing
{
    public class SystemDirectoryRoute : Route, IDirectoryRoute
    {
        public SystemDirectoryRoute(string directoryName, IKernel kernel)
            : base(directoryName + "/{*pathInfo}", new DirectoryRouteHandler(kernel.Get<ISubtextPageBuilder>(), kernel))
        {
            DirectoryName = directoryName;
        }

        public string DirectoryName
        {
            get;
            private set;
        }
    }
}
