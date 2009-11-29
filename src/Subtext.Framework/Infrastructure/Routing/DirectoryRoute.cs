#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using Ninject;

namespace Subtext.Framework.Routing
{
    public class DirectoryRoute : SubtextRoute, IDirectoryRoute
    {
        public DirectoryRoute(string directoryName, IKernel kernel) :
            base(directoryName + "/{*pathInfo}", new DirectoryRouteHandler(kernel.Get<ISubtextPageBuilder>(), kernel))
        {
            DirectoryName = directoryName;
        }

        #region IDirectoryRoute Members

        public string DirectoryName { get; private set; }

        #endregion
    }
}