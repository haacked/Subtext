using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Globalization;
using Subtext.Framework.Configuration;
using System.Web.Mobile;

namespace Subtext.Framework.Services
{
    public class BrowserDetectionService : IHttpHandler
    {
        HttpContext context;
        int blogId;

        public BrowserDetectionService() : this(HttpContext.Current, Config.CurrentBlog.Id)
        { }

        public BrowserDetectionService(HttpContext context, int blogId)
        {
            this.context = context;
            this.blogId = blogId;
        }

        public BrowserInfo DetectBrowserCapabilities()
        {
            bool? isMobile = UserSpecifiedMobile();
            if (isMobile == null)
            {
                MobileCapabilities mobileCaps = this.context.Request.Browser as MobileCapabilities;
                isMobile = mobileCaps != null && mobileCaps.IsMobileDevice;
            }
            return new BrowserInfo(isMobile.Value);
        }

        bool? UserSpecifiedMobile()
        { 
            HttpCookie cookie = context.Request.Cookies.Get("MobileDeviceInfo_" + this.blogId);
            if (cookie == null)
                return null;
            return (cookie.Value == "True");
        }

        public void SetMobile(bool isMobile)
        {
            HttpCookie cookie = new HttpCookie("MobileDeviceInfo_" + this.blogId, isMobile.ToString(CultureInfo.InvariantCulture));
            cookie.Value = isMobile.ToString(CultureInfo.InvariantCulture);
            this.context.Response.Cookies.Add(cookie);
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string mobileQuery = context.Request.QueryString["mobile"];
            bool isMobile;
            if (!bool.TryParse(mobileQuery, out isMobile))
                isMobile = false;
            SetMobile(isMobile);

            string returnUrl = context.Request.QueryString["returnUrl"] ?? string.Empty;
            if (returnUrl.Length == 0)
                returnUrl = "~/";

            //Security so people can't use this for phishing.
            if (returnUrl.StartsWith("http:") 
                || returnUrl.StartsWith("https:")
                || (!returnUrl.StartsWith("/") && !returnUrl.StartsWith("~/")))
                returnUrl = "~/";
            context.Response.Redirect(returnUrl);
        }

        #endregion
    }
}
