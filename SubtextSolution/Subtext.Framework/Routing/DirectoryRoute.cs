using System.Web.Routing;
namespace Subtext.Framework.Routing
{
    public class DirectoryRoute : SubtextRoute
    {
        public DirectoryRoute(string directoryName) : 
            base(directoryName + "/{*pathInfo}", new DirectoryRouteHandler()) {
                this.DirectoryName = directoryName;
        }

        public string DirectoryName { 
            get; 
            private set; 
        }
    }
}
