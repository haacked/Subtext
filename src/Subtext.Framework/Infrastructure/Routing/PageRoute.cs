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

using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Ninject;

namespace Subtext.Framework.Routing
{
    public class PageRoute : SubtextRoute
    {
        public const string ControlNamesKey = "controls";

        public PageRoute(string url, string virtualPath, IEnumerable<string> controls, IKernel kernel)
            : this(url, virtualPath, controls, kernel.Get<ISubtextPageBuilder>(), kernel)
        {
        }

        public PageRoute(string url, string virtualPath, IEnumerable<string> controls, ISubtextPageBuilder pageBuilder,
                         IKernel kernel)
            : base(url, new PageRouteHandler(virtualPath, pageBuilder, kernel))
        {
            DataTokens = new RouteValueDictionary {{ControlNamesKey, controls.AsEnumerable()}};
        }
    }
}