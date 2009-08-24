#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web.Routing;

namespace Subtext.Framework.Util
{
    public static class RequestExtensions
    {
        public static DateTime GetDateFromRequest(this RequestContext requestContext)
        {
            string yearText = requestContext.RouteData.Values["year"] as string;
            string monthText = requestContext.RouteData.Values["month"] as string;
            string dayText = requestContext.RouteData.Values["day"] as string ?? "1";

            int year = Convert.ToInt32(yearText);
            int month = Convert.ToInt32(monthText);
            int day = Convert.ToInt32(dayText);

            return new DateTime(year, month, day);
        }
    }
}
