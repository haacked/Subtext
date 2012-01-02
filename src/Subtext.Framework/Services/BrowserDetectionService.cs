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

using System.Globalization;
using System.Web;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Services
{
    public class BrowserDetectionService : IHttpHandler
    {
        private static int BlogId
        {
            get
            {
                if (Config.CurrentBlog != null)
                {
                    return Config.CurrentBlog.Id;
                }
                return 0;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string mobileQuery = context.Request.QueryString["mobile"];
            bool isMobile;
            if (!bool.TryParse(mobileQuery, out isMobile))
            {
                isMobile = false;
            }
            SetMobile(isMobile);

            string returnUrl = context.Request.QueryString["returnUrl"] ?? string.Empty;
            if (returnUrl.Length == 0)
            {
                returnUrl = "~/";
            }

            //Security so people can't use this for phishing.
            if (returnUrl.StartsWith("http:")
               || returnUrl.StartsWith("https:")
               || (!returnUrl.StartsWith("/") && !returnUrl.StartsWith("~/")))
            {
                returnUrl = "~/";
            }
            context.Response.Redirect(returnUrl);
        }

        public BrowserInfo DetectBrowserCapabilities(HttpRequestBase request)
        {
            bool? isMobile = UserSpecifiedMobile();
            if (isMobile == null)
            {
                var mobileCaps = request.Browser;
                isMobile = mobileCaps != null && mobileCaps.IsMobileDevice;
            }
            return new BrowserInfo(isMobile.Value);
        }

        static bool? UserSpecifiedMobile()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("MobileDeviceInfo_" + BlogId);
            if (cookie == null)
            {
                return null;
            }
            return (cookie.Value == "True");
        }

        public void SetMobile(bool isMobile)
        {
            var cookie = new HttpCookie("MobileDeviceInfo_" + BlogId, isMobile.ToString(CultureInfo.InvariantCulture)) { Value = isMobile.ToString(CultureInfo.InvariantCulture) };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}