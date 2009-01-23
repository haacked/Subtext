#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

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
