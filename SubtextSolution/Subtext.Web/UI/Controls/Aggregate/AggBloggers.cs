using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Subtext.Web.UI.Controls
{
    public class AggBloggers : BaseControl
    {
        protected Repeater Bloggers;

        public int BlogGroup
        {
            get;
            set;
        }

        public bool ShowGroups
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.QueryString["GroupID"] != null)
            {
            	int blogGroupId;
                if (Int32.TryParse(Request.QueryString["GroupID"], out blogGroupId))
                {
                    BlogGroup = blogGroupId;
                }
                else {
                    BlogGroup = 0;
                }
            }
        }

        private string appPath;
        readonly string fullUrl = HttpContext.Current.Request.Url.Scheme + "://{0}{1}{2}/";
        protected string GetFullUrl(string host, string app)
        {
            if (appPath == null)
            {
                appPath = HttpContext.Current.Request.ApplicationPath;
                if (!appPath.ToLower(CultureInfo.InvariantCulture).EndsWith("/"))
                {
                    appPath += "/";
                }
            }

            if (Request.Url.Port != 80)
            {
                host += ":" + Request.Url.Port;
            }

            return string.Format(fullUrl, host, appPath, app);

        }
    }
}
