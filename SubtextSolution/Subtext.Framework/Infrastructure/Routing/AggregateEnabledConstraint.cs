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
    /// Aggregate blogs enablement must match this constraint for the route to match.
    /// </summary>
    public class AggregateEnabledConstraint : IRouteConstraint
    {
        public AggregateEnabledConstraint(HostInfo host, bool matchWhenAggregateBlogsEnabled)
        {
            MatchWhenAggregateBlogsEnabled = matchWhenAggregateBlogsEnabled;
            Host = host;
        }

        protected HostInfo Host { get; private set; }

        public bool MatchWhenAggregateBlogsEnabled { get; private set; }

        //Should always return true for non root requests...

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if(routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            return Match(Host.BlogAggregationEnabled);
        }

        #endregion

        public bool Match(bool aggregateBlogsEnabled)
        {
            return (aggregateBlogsEnabled == MatchWhenAggregateBlogsEnabled);
        }
    }
}