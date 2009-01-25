using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.Web.UI.Controls.Aggregate;

namespace Subtext.Web.UI.Controls
{
    public class AggRecentPosts : AggregateControl
    {
        protected Repeater RecentPosts;

        /// <summary>
        /// Prroperty to limit the number of posts displayed. Default is 35.
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int? groupId = GetGroupIdFromQueryString();

            var entries = ObjectProvider.Instance().GetRecentEntries(HostInfo.Instance.AggregateBlog.Host, groupId, Count);
            RecentPosts.DataSource = entries;
            RecentPosts.DataBind();
        }

        protected string GetEntryUrl(string host, string app, string entryName, DateTime dt)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}archive/{1:yyyy/MM/dd}/{2}.aspx", GetFullUrl(host, app), dt, entryName);
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
