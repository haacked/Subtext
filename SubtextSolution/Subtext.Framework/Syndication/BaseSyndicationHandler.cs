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
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;
using Subtext.Framework.Syndication.Compression;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Abstract base class used to respond to requests for 
	/// syndicated feeds such as RSS and ATOM.
	/// </summary>
	public abstract class BaseSyndicationHandler<T> : IHttpHandler
	{
		const int HTTP_IM_USED = 226;
		const int HTTP_MOVED_PERMANENTLY = 301;

        protected Blog Blog
        {
            get {
                return SubtextContext.Blog;
            }
        }

        protected ISubtextContext SubtextContext {
            get;
            private set;
        }

		protected HttpContextBase HttpContext {
            get {
                return SubtextContext.RequestContext.HttpContext;
            }
        }

        protected CachedFeed Feed {
            get;
            set;
        }

        protected virtual bool RequiresAdminRole
        {
            get;
            private set;
        }

        protected virtual bool RequiresHostAdminRole
        {
            get;
            private set;
        }

		/// <summary>
		/// Returns the "If-Modified-Since" HTTP header.  This indicates 
		/// the last time the client requested data and is used to 
		/// determine whether new data is to be sent.
		/// </summary>
		/// <value></value>
		protected string LastModifiedHeader
		{
			get
			{
				return HttpContext.Request.Headers["If-Modified-Since"];
			}
		}

		/// <summary>
		/// Returns the "If-None-Match" HTTP header.  This is used to indicate 
		/// a conditional GET and is used to implement RFC3229 with feeds 
		/// <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html"/>.
		/// </summary>
		/// <value></value>
		protected string IfNonMatchHeader
		{
			get
			{
				return HttpContext.Request.Headers["If-None-Match"];	
			}
		}

		/// <summary>
		/// Gets the Publish Date of the last feed item received by the client. 
		/// This is used to determine whether the client is up to date 
		/// or whether the client is ready to receive new feed items. 
		/// We will then send just the difference.
		/// </summary>
		/// <value></value>
		protected DateTime PublishDateOfLastFeedItemReceived
		{
			get
			{
				if(IfNonMatchHeader != null && IfNonMatchHeader.Length > 0) {
					try {
						return DateTime.Parse(IfNonMatchHeader);
					}
					catch(FormatException) {
						//Swallow it.
					}
				}
				return NullValue.NullDateTime;
			}
		}

		/// <summary>
		/// Gets a value indicating whether use delta encoding within this request.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if use delta encoding; otherwise, <c>false</c>.
		/// </value>
		protected bool UseDeltaEncoding
		{
			get {
				return Blog.RFC3229DeltaEncodingEnabled && AcceptDeltaEncoding;	
			}
		}

		/// <summary>
		/// Compares the requesting clients <see cref="LastModifiedHeader"/> against 
		/// the date the feed was last updated.  If the feed hasn't been updated, then 
		/// it sends a 304 HTTP header indicating such.
		/// </summary>
		/// <returns></returns>
		protected virtual bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;
			if(dt != null) {
				try
				{
					DateTime feedDT = DateTime.Parse(dt);
					DateTime lastUpdated = ConvertLastUpdatedDate(Blog.LastUpdated); 
					TimeSpan ts = feedDT - lastUpdated;
					
					//We need to allow some margin of error.
					return Math.Abs(ts.TotalMilliseconds) <= 500;
				}
				catch(FormatException) {
					//TODO: Review
                    //swallow it for now.
					//Some browsers send a funky last modified header.
					//We don't want to throw an exception in those cases.
				}

			}
			return false;
		}

		/// <summary>
		/// Returns whether or not the http cache is OK.
		/// </summary>
		/// <returns></returns>
		protected virtual bool IsHttpCacheOK() {
            if (HttpContext.Cache == null) {
                Feed = null;
                return false;
            }

            Feed = HttpContext.Cache[this.CacheKey(this.PublishDateOfLastFeedItemReceived)] as CachedFeed;
			if(Feed == null) {
				return false;
			}
			return Feed.LastModified == ConvertLastUpdatedDate(Blog.LastUpdated);
		}

		/// <summary>
		/// Send the HTTP status code 304 to the response this instance.
		/// </summary>
		private void Send304() {
			HttpContext.Response.StatusCode = 304;
		}

		/// <summary>
		/// Convert a date to the server time.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		protected DateTime ConvertLastUpdatedDate(DateTime dateTime) {
            DateTime utc = Blog.TimeZone.ToUniversalTime(dateTime);
			return TimeZone.CurrentTimeZone.ToLocalTime(utc);
		}

		/// <summary>
		/// Processs the feed. Responds to the incoming request with the 
		/// contents of the feed.
		/// </summary>
		protected virtual void ProcessFeed() {
			if(RedirectToFeedBurnerIfNecessary())
				return;
			
			// Checks Last Modified Header.
			if(IsLocalCacheOK()) {
				Send304();
				return;
			}
		
			// Checks our cache against last modified header.
			if(!IsHttpCacheOK()) {
				Feed = BuildFeed();
				if(Feed != null) {
					if(UseDeltaEncoding && Feed.ClientHasAllFeedItems) {
						Send304();
						return;
					}
					Cache(Feed);
				}
			}

			WriteFeed();
		}

		/// <summary>
		/// Returns the key used to cache this feed.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Date last viewed feed item published.</param>
		/// <returns></returns>
		protected abstract string CacheKey(DateTime dateLastViewedFeedItemPublished);
		protected abstract void Cache(CachedFeed feed);
		
		/// <summary>
		/// Gets the syndication writer.
		/// </summary>
		/// <returns></returns>
		protected abstract BaseSyndicationWriter SyndicationWriter{ get; }

		protected virtual CachedFeed BuildFeed() {
			CachedFeed feed = new CachedFeed();
			feed.LastModified = ConvertLastUpdatedDate(Blog.LastUpdated);
			BaseSyndicationWriter writer = SyndicationWriter;
			feed.Xml = writer.Xml;
			feed.ClientHasAllFeedItems = writer.ClientHasAllFeedItems;
			feed.Etag = writer.DateLastViewedFeedItemPublished.ToString(CultureInfo.InvariantCulture);
			feed.LatestFeedItemPublishDate = writer.DateLastViewedFeedItemPublished;

			return feed;
		}

		/// <summary>
		/// Writes the feed to the response.
		/// </summary>
		protected virtual void WriteFeed()
		{
			string encoding = null;

			if(Feed != null)
			{
				if(Blog.UseSyndicationCompression && this.AcceptGzipCompression)
				{
					// We're GZip Encoding!
					SyndicationCompressionFilter filter = SyndicationCompressionHelper.GetFilterForScheme(this.AcceptEncoding, HttpContext.Response.Filter);
	
					if(filter != null)
					{
						encoding = filter.ContentEncoding;
						HttpContext.Response.Filter = filter.Filter;
					}
				}

				if(encoding == null) {
					HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
				}

				HttpContext.Response.ContentType = "text/xml";
				HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
				HttpContext.Response.Cache.SetLastModified(Feed.LastModified);
				HttpContext.Response.Cache.SetETag(Feed.Etag);
				
                if(AcceptGzipCompression)
				{
					HttpContext.Response.AddHeader("IM", "feed, gzip");
				}
				else
				{
					HttpContext.Response.AddHeader("IM", "feed");
				}
				if(this.UseDeltaEncoding)
					HttpContext.Response.StatusCode = HTTP_IM_USED; //IM Used
				else
					HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

				HttpContext.Response.Write(Feed.Xml);
			}
		}

		/// <summary>
		/// Processs the request and sends the feed to the response.
		/// </summary>
		/// <param name="context">Context.</param>
		void IHttpHandler.ProcessRequest(HttpContext context)
		{
            if ((RequiresAdminRole && !SecurityHelper.IsAdmin) || (RequiresHostAdminRole && !SecurityHelper.IsHostAdmin)) {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                return;
            }

            var httpContext = new HttpContextWrapper(context);
            var requestContext = new RequestContext(httpContext, new RouteData());
            var subtextContext = new SubtextContext(Config.CurrentBlog, requestContext, new UrlHelper(requestContext, null), ObjectProvider.Instance());
			ProcessRequest(subtextContext);
		}

        public virtual void ProcessRequest(ISubtextContext context) {
            SubtextContext = context;
            ProcessFeed();
        }

		/// <summary>
		/// Gets a value indicating whether this handler is reusable.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if it is reusable; otherwise, <c>false</c>.
		/// </value>
		public bool IsReusable
		{
			get;
            private set;
		}

		/// <summary>
		/// Returns the "Accept-Encoding" value from the HTTP Request header. 
		/// This is a list of encodings that may be sent to the browser.
		/// </summary>
		/// <remarks>
		/// Specifically we're looking for gzip.
		/// </remarks>
		/// <value></value>
		protected string AcceptEncoding
		{
			get
			{
				string header = HttpContext.Request.Headers["Accept-Encoding"];
				if(header != null)
					return header;
				else
					return string.Empty;
			}
		}

		/// <summary>
		/// Gets the accept IM header from the request.
		/// </summary>
		/// <value></value>
		protected string AcceptIMHeader
		{
			get
			{
				string header = HttpContext.Request.Headers["A-IM"];
				if(header != null)
					return header;
				else
					return string.Empty;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the client accepts 
		/// <see href="http://bobwyman.pubsub.com/main/2004/09/using_rfc3229_w.html">RFC3229 Feed Delta 
		/// Encoding</see>. 
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [accepts delta encoding]; otherwise, <c>false</c>.
		/// </value>
		protected bool AcceptDeltaEncoding
		{
			get
			{
				return AcceptIMHeader.IndexOf("feed") >= 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the client accepts gzip compression.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if accepts gzip compression; otherwise, <c>false</c>.
		/// </value>
		protected bool AcceptGzipCompression
		{
			get
			{
				return AcceptEncoding.IndexOf("gzip") >= 0 || 
					AcceptIMHeader.IndexOf("gzip") >= 0;
			}
		}
		
		// Adapted from DasBlog
		private bool RedirectToFeedBurnerIfNecessary()
		{
			//If we are using FeedBurner, only allow them to get our feed...
			if (!String.IsNullOrEmpty(Blog.RssProxyUrl))
			{
				string userAgent = HttpContext.Request.UserAgent;
				if (!String.IsNullOrEmpty(userAgent))
				{
					// If they aren't FeedBurner and they aren't asking for a category or comment rss, redirect them!
					if (!userAgent.StartsWith("FeedBurner") && IsMainfeed)
					{
                        HttpContext.Response.StatusCode = HTTP_MOVED_PERMANENTLY;
                        HttpContext.Response.Status = HTTP_MOVED_PERMANENTLY + " Moved Permanently";
                        HttpContext.Response.RedirectLocation = SubtextContext.UrlHelper.RssProxyUrl(SubtextContext.Blog).ToString();
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if the feed is the main feed.  False for category feeds and comment feeds.
		/// </summary>
		protected abstract bool IsMainfeed { get;}
	}
}
