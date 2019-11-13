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
using System.IO;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Text;

namespace Subtext.Framework.Syndication.Admin
{
    public class ReferrerRssWriter : GenericRssWriter<Referrer>
    {
        public ReferrerRssWriter(TextWriter writer, ICollection<Referrer> referrers,
                                 DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding,
                                 ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
            Items = referrers;
        }

        protected override ICollection<string> GetCategoriesFromItem(Referrer item)
        {
            var strings = new List<string> { item.PostTitle, new Uri(item.ReferrerUrl).Host };
            return strings;
        }

        protected override string GetGuid(Referrer item)
        {
            return item.BlogId.ToString(CultureInfo.InvariantCulture) +
                   item.EntryId.ToString(CultureInfo.InvariantCulture) + item.ReferrerUrl;
        }

        protected override string GetTitleFromItem(Referrer item)
        {
            return item.PostTitle + " - " + item.ReferrerUrl.ShortenUrl(20);
        }

        protected override string GetLinkFromItem(Referrer item)
        {
            return UrlHelper.AdminUrl("Referrers.aspx");
        }

        protected override string GetBodyFromItem(Referrer item)
        {
            return String.Format(CultureInfo.InvariantCulture, Resources.Message_ReferrersForm, item.ReferrerUrl,
                                 item.Count);
        }

        protected override string GetAuthorFromItem(Referrer item)
        {
            return "";
        }

        protected override DateTime GetPublishedDateUtc(Referrer item)
        {
            return item.LastReferDate;
        }

        protected override bool ItemCouldContainComments(Referrer item)
        {
            return false;
        }

        protected override bool ItemAllowsComments(Referrer item)
        {
            return false;
        }

        protected override bool CommentsClosedOnItem(Referrer item)
        {
            return true;
        }

        protected override int GetFeedbackCount(Referrer item)
        {
            return item.Count;
        }

        protected override DateTime GetSyndicationDate(Referrer item)
        {
            return item.LastReferDate;
        }

        protected override string GetAggBugUrl(Referrer item)
        {
            return string.Empty;
        }

        protected override string GetCommentApiUrl(Referrer item)
        {
            return string.Empty;
        }

        protected override string GetCommentRssUrl(Referrer item)
        {
            return string.Empty;
        }

        protected override string GetTrackBackUrl(Referrer item)
        {
            return string.Empty;
        }

        protected override EnclosureItem GetEnclosureFromItem(Referrer item)
        {
            return null;
        }
    }
}