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
        private string _etag;
        private DateTime _lastModified;

        public CachedFeed()
        {
            LatestFeedItemPublishDate = NullValue.NullDateTime;
        }

        /// <summary>
        /// Gets or sets the date this feed was last modified.
        /// </summary>
        /// <value></value>
        public DateTime LastModified
        {
            //TODO: Need to figure out what happens to the date when we set LastModified date. 
            // Returned data usually does not match 
            // what we sent!
            get { return _lastModified; }
            set
            {
                //Just incase the user changes timezones after a post
                if(value > DateTime.Now)
                {
                    value = DateTime.Now;
                }
                _lastModified = value;
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
        public bool ClientHasAllFeedItems { get; set; }

        /// <summary>
        /// Gets or sets the latest feed item publish date. This is the date that the latest feed 
        /// item that will be sent to the client was published.
        /// </summary>
        /// <value></value>
        public DateTime LatestFeedItemPublishDate { get; set; }

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
                if(_etag == null)
                {
                    // if we did not set the etag, just use the 
                    // LastModified Date
                    _etag = LastModified.ToString(CultureInfo.InvariantCulture);
                }
                return _etag;
            }
            set { _etag = value; }
        }

        /// <summary>
        /// Gets or sets the contents of the feed.
        /// </summary>
        /// <value></value>
        public string Xml { get; set; }
    }
}