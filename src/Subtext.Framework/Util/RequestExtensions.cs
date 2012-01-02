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

using System;
using System.Globalization;
using System.Web.Routing;

namespace Subtext.Framework.Util
{
    public static class RequestExtensions
    {
        public static DateTime GetDateFromRequest(this RequestContext requestContext)
        {
            var yearText = requestContext.RouteData.Values["year"] as string;
            var monthText = requestContext.RouteData.Values["month"] as string;
            string dayText = requestContext.RouteData.Values["day"] as string ?? "1";

            int year = Convert.ToInt32(yearText, CultureInfo.InvariantCulture);
            int month = Convert.ToInt32(monthText, CultureInfo.InvariantCulture);
            int day = Convert.ToInt32(dayText, CultureInfo.InvariantCulture);

            return new DateTime(year, month, day);
        }

        public static string GetSlugFromRequest(this RequestContext requestContext)
        {
            return requestContext.RouteData.Values["slug"] as string;
        }

        public static int? GetIdFromRequest(this RequestContext requestContext)
        {
            RouteValueDictionary routeValues = requestContext.RouteData.Values;
            int id;
            if (!routeValues.ContainsKey("id") ||
               !int.TryParse((string)routeValues["id"], out id))
            {
                return null;
            }
            return id;
        }

        public static string GetQueryFromRequest(this RequestContext requestContext)
        {
            return requestContext.HttpContext.Request.QueryString["q"];
        }
    }
}