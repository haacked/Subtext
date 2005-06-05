#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;

namespace Subtext.Web.Admin
{
	// This is sort of experimental right now. Not sure it's clean enough/performant enough.

	internal class Preferences
	{
		private const string COOKIES_PAGE_SIZE_DEFAULT = "AdminCookieListingItemCount";
		private const string COOKIES_EXPAND_ADVANCED = "AdminCookieAlwaysExpandAdvanced";
		private const string COOKIES_CREATE_ISACTIVE = "AlwaysCreateItemsAsActive";

		protected const int COOKIE_EXPIRY_MONTHS = 6;

		internal static int ListingItemCount 
		{
			get 
			{ 		
				if (null != HttpContext.Current.Request.Cookies[COOKIES_PAGE_SIZE_DEFAULT])
					return Int32.Parse(HttpContext.Current.Request.Cookies[COOKIES_PAGE_SIZE_DEFAULT].Value);
				else
					return Constants.PAGE_SIZE_DEFAULT;
			}
			set 
			{ 
				if (value > 0)
					CreateCookie(COOKIES_PAGE_SIZE_DEFAULT, value, CookieExpiry);
			}
		}

		internal static bool AlwaysExpandAdvanced 
		{
			get 
			{ 		
				if (null != HttpContext.Current.Request.Cookies[COOKIES_EXPAND_ADVANCED])
				{
					return HttpContext.Current.Request.Cookies[COOKIES_EXPAND_ADVANCED].Value.ToLower(System.Globalization.CultureInfo.InvariantCulture) == "true" ? true : false;
				}
				else
					return Constants.ALWAYS_EXPAND_DEFAULT;
			}
			set 
			{ 
				CreateCookie(COOKIES_EXPAND_ADVANCED, value, CookieExpiry);
			}
		}

		internal static bool AlwaysCreateIsActive 
		{
			get 
			{ 		
				if (null != HttpContext.Current.Request.Cookies[COOKIES_CREATE_ISACTIVE])
				{
					return HttpContext.Current.Request.Cookies[COOKIES_CREATE_ISACTIVE].Value.ToLower(System.Globalization.CultureInfo.InvariantCulture) == "true" ? true : false;
				}
				else
					return Constants.CREATE_ISACTIVE_DEFAULT;
			}
			set 
			{ 
//				if (value != Constants.CREATE_ISACTIVE_DEFAULT)
				CreateCookie(COOKIES_CREATE_ISACTIVE, value, CookieExpiry);
			}
		}

		protected static DateTime CookieExpiry
		{
			get { return DateTime.Now.AddMonths(COOKIE_EXPIRY_MONTHS); }
		}

		protected static void CreateCookie(string name, object value, DateTime expiry)
		{
			HttpCookie addingCookie = new HttpCookie(name);
			addingCookie.Value = value.ToString();
			addingCookie.Expires = expiry;
			HttpContext.Current.Response.Cookies.Add(addingCookie);	
		}
	}
}

