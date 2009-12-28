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
using System.IO;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Abstract base class used to write RSS feeds.
    /// </summary>
    public abstract class BaseRssWriter<T> : GenericRssWriter<T> where T : IIdentifiable
    {
        protected BaseRssWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding,
                                ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
        }

        protected override string GetAggBugUrl(T item)
        {
            return UrlHelper.AggBugUrl(item.Id).ToFullyQualifiedUrl(Blog).ToString();
        }

        protected override string GetCommentApiUrl(T item)
        {
            return UrlHelper.CommentApiUrl(item.Id).ToFullyQualifiedUrl(Blog).ToString();
        }

        protected override string GetCommentRssUrl(T item)
        {
            return UrlHelper.CommentRssUrl(item.Id).ToFullyQualifiedUrl(Blog).ToString();
        }

        protected override string GetTrackBackUrl(T item)
        {
            return UrlHelper.TrackbacksUrl(item.Id).ToFullyQualifiedUrl(Blog).ToString();
        }
    }
}