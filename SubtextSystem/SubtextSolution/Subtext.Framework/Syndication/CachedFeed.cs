#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Globalization;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// <p>
	/// The CachedFeed is a cacheable container for our rss feed(s). Instead of 
	/// requesting the cache data, processing it, and creating an XML document 
	/// on each request, we will store the actual Rss document as a cached string.
	/// Generally, it will be returned to the client by calling Response.Write(feed.Xml)
	/// </p>
	/// </summary>
	public class CachedFeed
	{
		private DateTime lastModified;
		
		
		/// <summary>
		/// Gets or sets the date this feed was last modified.
		/// </summary>
		/// <value></value>
		public DateTime LastModified
		{
			//TODO: Need to figure out what happens to the date when we set LastModified date. 
			// Returned data usually does not match 
			// what we sent!
			get{return lastModified;}
			set
			{
				//Just incase the user changes timezones after a post
				if(value > DateTime.Now)
				{
					value = DateTime.Now;
				}
				lastModified = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether client has all feed items. 
		/// This is according to RFC3229 with feeds 
		/// <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html"/>.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the client has all feed items; otherwise, <c>false</c>.
		/// </value>
		public bool ClientHasAllFeedItems
		{
			get { return _clientHasAllFeedItems; }
			set { _clientHasAllFeedItems = value; }
		}

		bool _clientHasAllFeedItems;

		/// <summary>
		/// Gets or sets the latest feed item publish date. This is the date that the latest feed 
		/// item that will be sent to the client was published.
		/// </summary>
		/// <value></value>
		public DateTime LatestFeedItemPublishDate
		{
			get { return _latestFeedItemPublishDate; }
			set { _latestFeedItemPublishDate = value; }
		}

		DateTime _latestFeedItemPublishDate = NullValue.NullDateTime;


		private string etag;
		/// <summary>
		/// Provides the current value of the entity tag for the requested 
		/// variant (<see href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html#sec14.19"/>). 
		/// In our case, it should be the ID of the last feed item sent.
		/// </summary>
		/// <value></value>
		public string Etag
		{
			get
			{
				if(etag == null)
				{
					// if we did not set the etag, just use the 
					// LastModified Date
					etag = this.LastModified.ToString(CultureInfo.InvariantCulture);
				}
				return etag;
			}
			set
			{
				etag = value;
			}
		}

		private string xml;
		/// <summary>
		/// Gets or sets the contents of the feed.
		/// </summary>
		/// <value></value>
		public string Xml
		{
			get
			{
				return xml;
			}
			set
			{
				xml = value;
			}
		}
	}
}

