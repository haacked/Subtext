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
using System.IO;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Syndication.Admin
{
    public class LogRssWriter : GenericRssWriter<LogEntry>
    {
        public LogRssWriter(TextWriter writer, ICollection<LogEntry> logs, bool useDeltaEncoding,
                            ISubtextContext context)
            : base(writer, NullValue.NullDateTime, useDeltaEncoding, context)
        {
            Items = logs;
        }

        //TODO: Fix ISP Violation
        protected override string GetCommentRssUrl(LogEntry item)
        {
            return string.Empty;
        }

        protected override string GetGuid(LogEntry item)
        {
            return item.Message + item.Date.ToUniversalTime();
        }

        //TODO: Fix ISP violation
        protected override string GetTrackBackUrl(LogEntry item)
        {
            return string.Empty;
        }

        //TODO: Fix ISP violation
        protected override string GetCommentApiUrl(LogEntry item)
        {
            return string.Empty;
        }

        //TODO: Fix ISP violation
        protected override string GetAggBugUrl(LogEntry item)
        {
            return string.Empty;
        }

        protected override ICollection<string> GetCategoriesFromItem(LogEntry item)
        {
            IList<string> collection = new List<string> {item.Level};

            string path = item.Url.PathAndQuery.Split('?')[0];
            collection.Add(path);
            return collection;
        }

        protected override string GetTitleFromItem(LogEntry item)
        {
            return item.Message;
        }

        protected override string GetLinkFromItem(LogEntry item)
        {
            return UrlHelper.AdminUrl("ErrorLog.aspx");
        }

        protected override string GetBodyFromItem(LogEntry item)
        {
            return item.Exception;
        }

        protected override string GetAuthorFromItem(LogEntry item)
        {
            return item.Logger;
        }

        protected override DateTime GetPublishedDateUtc(LogEntry item)
        {
            return item.Date;
        }

        protected override bool ItemCouldContainComments(LogEntry item)
        {
            return false;
        }

        protected override bool ItemAllowsComments(LogEntry item)
        {
            return false;
        }

        protected override bool CommentsClosedOnItem(LogEntry item)
        {
            return true;
        }

        protected override int GetFeedbackCount(LogEntry item)
        {
            return 0;
        }

        protected override DateTime GetSyndicationDate(LogEntry item)
        {
            return item.Date;
        }

        protected override EnclosureItem GetEnclosureFromItem(LogEntry item)
        {
            return null;
        }
    }
}