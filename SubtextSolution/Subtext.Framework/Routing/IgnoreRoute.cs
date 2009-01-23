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

namespace Subtext.Framework.Routing {
    /// <summary>
    /// When ignoring routes, we also want to ignore for rendering 
    /// the virtual path. Unfortunately, routing doesn't do this 
    /// yet.
    /// </summary>
    public class IgnoreRoute : Route {
        public IgnoreRoute(string url) : base(url, new StopRoutingHandler()) {
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            return null;
        }
    }
}
