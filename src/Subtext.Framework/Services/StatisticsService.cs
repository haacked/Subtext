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
using System.Data.SqlClient;
using System.Web;
using log4net;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;
using Subtext.Framework.Routing;
using Subtext.Framework.Web;

namespace Subtext.Framework.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly static ILog Log = new Log();

        public StatisticsService(ISubtextContext context, Configuration.Tracking settings)
        {
            SubtextContext = context;
            Settings = settings;
        }

        public ISubtextContext SubtextContext { get; private set; }

        public Configuration.Tracking Settings { get; private set; }

        public void RecordAggregatorView(EntryView entryView)
        {
            if (!Settings.EnableAggBugs || SubtextContext.HttpContext.Request.HttpMethod == "POST")
            {
                return;
            }

            entryView.PageViewType = PageViewType.AggView;

            try
            {
                SubtextContext.Repository.TrackEntry(entryView);
            }
            catch (SqlException e)
            {
                Log.Error("Could not record Aggregator view", e);
            }
        }

        public void RecordWebView(EntryView entryView)
        {
            if (!Settings.EnableWebStats || SubtextContext.HttpContext.Request.HttpMethod == "POST")
            {
                return;
            }

            entryView.ReferralUrl = GetReferral(SubtextContext);

            entryView.PageViewType = PageViewType.WebView;
            try
            {
                SubtextContext.Repository.TrackEntry(entryView);
            }
            catch (Exception e)
            {
                // extra precautions for web view because it's not done via image bug.
                Log.Error("Could not record Web view", e);
            }
        }

        private static string GetReferral(ISubtextContext context)
        {
            HttpRequestBase request = context.HttpContext.Request;
            Uri uri = request.GetUriReferrerSafe();

            if (uri == null)
            {
                return null;
            }

            string referrerDomain = Blog.StripWwwPrefixFromHost(uri.Host);
            string blogDomain =
                Blog.StripWwwPrefixFromHost(context.UrlHelper.BlogUrl().ToFullyQualifiedUrl(context.Blog).Host);

            if (String.Equals(referrerDomain, blogDomain, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (referrerDomain.Length == 0)
            {
                Log.Warn("Somehow the referral was an empty string and not null.");
                return null;
            }

            return uri.ToString();
        }
    }
}