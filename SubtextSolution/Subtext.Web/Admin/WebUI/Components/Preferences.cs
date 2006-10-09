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

using System;
using System.Web;

namespace Subtext.Web.Admin
{
	// This is sort of experimental right now. Not sure it's clean enough/performant enough.
	internal sealed class Preferences
	{
		private const string COOKIES_PAGE_SIZE_DEFAULT = "AdminCookieListingItemCount";
		private const string COOKIES_EXPAND_ADVANCED = "AdminCookieAlwaysExpandAdvanced";
		private const string COOKIES_CREATE_ISACTIVE = "AlwaysCreateItemsAsActive";

		const int COOKIE_EXPIRY_MONTHS = 6;

		private Preferences()
		{
		}
		
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
				HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIES_EXPAND_ADVANCED];
				if (null != cookie)
				{
					return String.Equals(cookie.Value, "true", StringComparison.InvariantCultureIgnoreCase) ? true : false;
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
				HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIES_CREATE_ISACTIVE];
				if (null != cookie)
				{
					return String.Equals(cookie.Value, "true", StringComparison.InvariantCultureIgnoreCase) ? true : false;
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

		static DateTime CookieExpiry
		{
			get { return DateTime.Now.AddMonths(COOKIE_EXPIRY_MONTHS); }
		}

		static void CreateCookie(string name, object value, DateTime expiry)
		{
			HttpCookie addingCookie = new HttpCookie(name);
			addingCookie.Value = value.ToString();
			addingCookie.Expires = expiry;
			HttpContext.Current.Response.Cookies.Add(addingCookie);	
		}
	}
}

