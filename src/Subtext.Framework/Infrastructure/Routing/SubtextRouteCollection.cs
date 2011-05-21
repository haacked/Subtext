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

using System.Web.Mvc;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    public class SubtextRouteMapper
    {
        public SubtextRouteMapper(RouteCollection routes, IDependencyResolver serviceLocator)
        {
            Routes = routes;
            ServiceLocator = serviceLocator;
        }

        public IDependencyResolver ServiceLocator
        {
            get;
            private set;
        }

        protected RouteCollection Routes { get; private set; }

        public void Add(string routeName, RouteBase route)
        {
            Routes.Add(routeName, route);
        }

        public void Add(RouteBase route)
        {
            Routes.Add(route);
        }
    }
}