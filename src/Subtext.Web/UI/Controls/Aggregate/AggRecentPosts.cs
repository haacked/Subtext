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
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    public class AggRecentPosts : AggregateUserControl
    {
        readonly string _fullUrl = HttpContext.Current.Request.Url.Scheme + "://{0}{1}{2}/";
        private string _appPath;
        protected Repeater RecentPosts;

        /// <summary>
        /// Prroperty to limit the number of posts displayed. Default is 35.
        /// </summary>
        public int Count { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            int? groupId = GetGroupIdFromQueryString();

            ICollection<Entry> entries = Repository.GetRecentEntries(HostInfo.AggregateBlog.Host, groupId,
                                                                     Count);
            RecentPosts.DataSource = entries;
            RecentPosts.DataBind();
            base.OnLoad(e);
        }

        protected string EntryUrl(object item)
        {
            var entry = item as Entry;
            return Url.EntryUrl(entry, entry.Blog);
        }

        protected string GetFullUrl(string host, string app)
        {
            if (_appPath == null)
            {
                _appPath = HttpContext.Current.Request.ApplicationPath;
                if (!_appPath.ToLower(CultureInfo.InvariantCulture).EndsWith("/"))
                {
                    _appPath += "/";
                }
            }

            if (Request.Url.Port != 80)
            {
                host += ":" + Request.Url.Port;
            }

            return string.Format(_fullUrl, host, _appPath, app);
        }
    }
}