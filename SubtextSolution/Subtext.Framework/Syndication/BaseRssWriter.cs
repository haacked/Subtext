#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Specialized;
using System.Globalization;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Text;
using Subtext.Framework.Tracking;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Abstract base class used to write RSS feeds.
	/// </summary>
	public abstract class BaseRssWriter<T> : GenericRssWriter<T> where T : IIdentifiable
	{
		/// <summary>
		/// Creates a new <see cref="BaseRssWriter"/> instance.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected BaseRssWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding)
			: base(dateLastViewedFeedItemPublished, useDeltaEncoding)
		{
		}

		protected override string GetAggBugUrl(T item, UrlFormats urlFormats)
		{
			return urlFormats.AggBugkUrl(item.Id);
		}

		protected override string GetCommentApiUrl(T item, UrlFormats urlFormats)
		{
			return urlFormats.CommentApiUrl(item.Id);
		}

		protected override string GetCommentRssUrl(T item, UrlFormats urlFormats)
		{
			return urlFormats.CommentRssUrl(item.Id);
		}
		protected override string GetTrackBackUrl(T item, UrlFormats urlFormats)
		{
			return urlFormats.TrackBackUrl(item.Id);
		}
	}
}
