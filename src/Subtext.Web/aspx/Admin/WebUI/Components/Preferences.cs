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
using System.Web;
using log4net;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;

namespace Subtext.Web.Admin
{
    // This is sort of experimental right now. Not sure it's clean enough/performant enough.
    public static class Preferences
    {
        const int COOKIE_EXPIRY_MONTHS = 6;
        private const string COOKIES_CREATE_ISACTIVE = "AlwaysCreateItemsAsActive";
        private const string COOKIES_EXPAND_ADVANCED = "AdminCookieAlwaysExpandAdvanced";
        private const string COOKIES_FEEDBACK_FILTER = "AdminCookieFeedbackFilter";
        private const string COOKIES_FEEDBACK_SHOWONLYCOMMENTS = "AdminCookieFeedbackShowOnlyComments"; //obsolete
        private const string COOKIES_PAGE_SIZE_DEFAULT = "AdminCookieListingItemCount";
        private const string COOKIES_USE_PLAIN_HTML_EDITOR = "UsePlainHtmlEditor";

        private readonly static ILog log = new Log();

        public static int ListingItemCount
        {
            get
            {
                if (null != HttpContext.Current.Request.Cookies[COOKIES_PAGE_SIZE_DEFAULT])
                {
                    return Int32.Parse(HttpContext.Current.Request.Cookies[COOKIES_PAGE_SIZE_DEFAULT].Value);
                }
                else
                {
                    return Constants.PAGE_SIZE_DEFAULT;
                }
            }
            set
            {
                if (value > 0)
                {
                    CreateCookie(COOKIES_PAGE_SIZE_DEFAULT, value, CookieExpiry);
                }
            }
        }

        public static bool AlwaysExpandAdvanced
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIES_EXPAND_ADVANCED];
                if (null != cookie)
                {
                    return String.Equals(cookie.Value, "true", StringComparison.OrdinalIgnoreCase)
                               ? true
                               : false;
                }
                else
                {
                    return Constants.ALWAYS_EXPAND_DEFAULT;
                }
            }
            set { CreateCookie(COOKIES_EXPAND_ADVANCED, value, CookieExpiry); }
        }

        public static bool AlwaysCreateIsActive
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIES_CREATE_ISACTIVE];
                if (null != cookie)
                {
                    return String.Equals(cookie.Value, "true", StringComparison.OrdinalIgnoreCase)
                               ? true
                               : false;
                }
                else
                {
                    return Constants.CREATE_ISACTIVE_DEFAULT;
                }
            }
            set
            {
                //				if (value != Constants.CREATE_ISACTIVE_DEFAULT)
                CreateCookie(COOKIES_CREATE_ISACTIVE, value, CookieExpiry);
            }
        }

        public static bool UsePlainHtmlEditor
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIES_USE_PLAIN_HTML_EDITOR];
                if (null != cookie)
                {
                    return String.Equals(cookie.Value, "true", StringComparison.OrdinalIgnoreCase)
                               ? true
                               : false;
                }
                else
                {
                    return Constants.USE_PLAIN_HTML_EDITOR_DEFAULT;
                }
            }
            set
            {
                CreateCookie(COOKIES_USE_PLAIN_HTML_EDITOR, value, CookieExpiry);
            }
        }

        static DateTime CookieExpiry
        {
            get { return DateTime.UtcNow.AddMonths(COOKIE_EXPIRY_MONTHS); }
        }

        public static string GetFeedbackItemFilter(FeedbackStatusFlag currentView)
        {
            string cookieName = COOKIES_FEEDBACK_FILTER + currentView;
            if (null != HttpContext.Current.Request.Cookies[cookieName])
            {
                return HttpContext.Current.Request.Cookies[cookieName].Value;
            }
            return FeedbackType.None.ToString();
        }

        public static void SetFeedbackItemFilter(string value, FeedbackStatusFlag currentView)
        {
            string cookieName = COOKIES_FEEDBACK_FILTER + currentView;

            if (Enum.IsDefined(typeof(FeedbackType), value))
            {
                CreateCookie(cookieName, value, CookieExpiry);
            }
            else
            {
                log.Warn("Could not set FeedbackType value: " + value);
            }
        }

        //This is a helper. Maybe it should go in another class? 13-nov-06 mountain_sf
        public static FeedbackType ParseFeedbackItemFilter(string value)
        {
            try
            {
                return (FeedbackType)Enum.Parse(typeof(FeedbackType), value, true);
            }
            catch (ArgumentNullException ane)
            {
                log.Warn("Could not parse FeedbackType value. Value was null.", ane);
                return FeedbackType.None;
            }
            catch (ArgumentException ae)
            {
                log.Warn("Could not parse FeedbackType value.", ae);
                return FeedbackType.None;
            }
        }

        static void CreateCookie(string name, object value, DateTime expiry)
        {
            var addingCookie = new HttpCookie(name);
            addingCookie.Value = value.ToString();
            addingCookie.Expires = expiry;
            HttpContext.Current.Response.Cookies.Add(addingCookie);
        }
    }
}