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

using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    /// <summary>
    /// Special route for gallery images. It's used to generate URLs, but never to 
    /// match incoming requests.
    /// </summary>
    public class ImageRoute : Route
    {
        public ImageRoute(string url) : base(url, null)
        {
            Defaults = new RouteValueDictionary(new {filename = string.Empty, id = string.Empty});
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return null;
        }
    }
}