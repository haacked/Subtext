using System.Configuration;
using System.Web;
using System.Web.Routing;

namespace Subtext.Framework.Routing
{
    /// <summary>
    /// Aggregate blogs enablement must match this constraint for the route to match.
    /// </summary>
    public class AggregateEnabledConstraint : IRouteConstraint
    {
        public AggregateEnabledConstraint(bool matchWhenAggregateBlogsEnabled) {
            MatchWhenAggregateBlogsEnabled = matchWhenAggregateBlogsEnabled;
        }

        public bool MatchWhenAggregateBlogsEnabled {
            get;
            private set;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration) {
                return true;
            }
            return Match(ConfigurationManager.AppSettings["AggregateEnabled"] == "true");
        }

        public bool Match(bool aggregateBlogsEnabled) { 
            return (aggregateBlogsEnabled == MatchWhenAggregateBlogsEnabled);
        }
    }
}
