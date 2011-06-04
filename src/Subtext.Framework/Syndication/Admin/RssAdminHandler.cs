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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Syndication.Admin
{
    public class RssAdminHandler : EntryCollectionHandler<object>
    {
        int _count;
        string[] _filters;
        string _rssType = "";
        string _title = "";

        public RssAdminHandler(ISubtextContext subtextContext)
            : base(subtextContext)
        {
        }

        protected override bool RequiresAdminRole
        {
            get { return true; }
        }

        protected override BaseSyndicationWriter SyndicationWriter
        {
            get
            {
                IList feed = GetFeedEntriesSimple();
                if (feed is ICollection<FeedbackItem>)
                {
                    //TODO: Test the admin feeds
                    var entry = new Entry(PostType.None) { Title = _title, Body = string.Empty };

                    var feedback = (ICollection<FeedbackItem>)feed;
                    return new CommentRssWriter(new StringWriter(), feedback, entry, SubtextContext);
                }
                if (feed is ICollection<Referrer>)
                {
                    var referrers = (ICollection<Referrer>)feed;
                    DateTime lastReferrer = NullValue.NullDateTime;
                    if (referrers.Count > 0)
                    {
                        lastReferrer = referrers.First().LastReferDate;
                    }
                    return new ReferrerRssWriter(new StringWriter(), referrers, lastReferrer, UseDeltaEncoding,
                                                 SubtextContext);
                }
                if (feed is ICollection<LogEntry>)
                {
                    var entries = (ICollection<LogEntry>)feed;
                    return new LogRssWriter(new StringWriter(), entries, UseDeltaEncoding, SubtextContext);
                }
                return null;
            }
        }

        protected override bool IsMainfeed
        {
            get { return false; }
        }

        protected override bool IsLocalCacheOk()
        {
            string dt = LastModifiedHeader;

            if (dt != null)
            {
                IList ec = GetFeedEntriesSimple();

                if (ec != null && ec.Count > 0)
                {
                    //Get the first entry.
                    object entry = default(object);

                    //TODO: Probably change GetFeedEntries to return ICollection<Entry>
                    foreach (object en in ec)
                    {
                        entry = en;
                        break;
                    }
                    return
                        DateTime.Compare(DateTime.Parse(dt, CultureInfo.InvariantCulture),
                                         GetItemCreatedDateUtc(entry)) == 0;
                }
            }
            return false;
        }

        protected void SetOptions()
        {
            if (!Int32.TryParse(HttpContext.Request.QueryString["Count"], out _count))
            {
                _count = Config.Settings.ItemCount;
            }

            //TODO: Use route data instead.
            if (Regex.IsMatch(HttpContext.Request.Url.PathAndQuery, "ModeratedCommentRss", RegexOptions.IgnoreCase))
            {
                _title = "Comments requiring your approval.";
                _filters = new[] { "NeedsModeration" };
                _rssType = "Comment";
                return;
            }

            if (Regex.IsMatch(HttpContext.Request.Url.PathAndQuery, "ReferrersRss", RegexOptions.IgnoreCase))
            {
                _title = "Referrals";
                _rssType = "Referral";
                return;
            }

            if (Regex.IsMatch(HttpContext.Request.Url.PathAndQuery, "ErrorsRss", RegexOptions.IgnoreCase))
            {
                _title = "Errors";
                _rssType = "Log";
                return;
            }

            _title = HttpContext.Request["Title"];
            _rssType = HttpContext.Request.QueryString["Type"];

            string qryFilters = HttpContext.Request.QueryString["Filter"];
            _filters = String.IsNullOrEmpty(qryFilters) ? new string[] { } : qryFilters.Split('+');
        }

        protected override void ProcessFeed()
        {
            SetOptions();
            base.ProcessFeed();
        }

        protected override ICollection<object> GetFeedEntries()
        {
            throw new NotImplementedException();
        }

        protected IList GetFeedEntriesSimple()
        {
            if (String.IsNullOrEmpty(_rssType))
            {
                throw new InvalidOperationException("rssType is empty or null");
            }

            switch (_rssType)
            {
                case "Comment":
                    FeedbackStatusFlag flags = FeedbackStatusFlag.None;

                    foreach (string filter in _filters)
                    {
                        if (Enum.IsDefined(typeof(FeedbackStatusFlag), filter))
                        {
                            flags |= (FeedbackStatusFlag)Enum.Parse(typeof(FeedbackStatusFlag), filter, true);
                        }
                    }
                    ICollection<FeedbackItem> moderatedFeedback = Repository.GetPagedFeedback(0, _count, flags,
                                                                                              FeedbackStatusFlag.None,
                                                                                              FeedbackType.None);
                    return (IList)moderatedFeedback;

                case "Referral":
                    //TODO: Fix!
                    ICollection<Referrer> referrers = Repository.GetPagedReferrers(0, _count, NullValue.NullInt32);
                    return (IList)referrers;

                case "Log":
                    ICollection<LogEntry> entries = LoggingProvider.Instance().GetPagedLogEntries(0, _count);
                    return (IList)entries;
            }

            return null;
        }

        protected override DateTime GetItemCreatedDateUtc(object item)
        {
            if (item is FeedbackItem)
            {
                return ((FeedbackItem)item).DateCreatedUtc;
            }
            if (item is Referrer)
            {
                return ((Referrer)item).LastReferDate;
            }
            if (item is LogEntry)
            {
                return ((LogEntry)item).Date;
            }
            return DateTime.UtcNow;
        }
    }
}