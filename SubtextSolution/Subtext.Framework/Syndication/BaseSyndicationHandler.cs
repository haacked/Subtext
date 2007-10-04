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
using log4net;
using Subtext.Extensibility.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Syndication.Compression;
using Subtext.Framework.Security;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Abstract base class used to respond to requests for 
	/// syndicated feeds such as RSS and ATOM.
	/// </summary>
	public abstract class BaseSyndicationHandler<T> : BaseHttpHandler
	{
		private ILog Log = new Log();
		const int HTTP_IM_USED = 226;
		const int HTTP_MOVED_PERMANENTLY = 301;

		protected BlogInfo CurrentBlog;
		protected HttpContext Context = null;
		protected CachedFeed Feed = null;

		protected virtual bool RequiresAdminRole
		{
			get { return false; }
		}

		protected virtual bool RequiresHostAdminRole
		{
			get { return false; }
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
				return Context.Request.Headers["If-Modified-Since"];
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
				return Context.Request.Headers["If-None-Match"];
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
				if (IfNonMatchHeader != null && IfNonMatchHeader.Length > 0)
				{
					try
					{
						return DateTime.Parse(IfNonMatchHeader);
					}
					catch(FormatException e)
					{
						Log.Info("Format Exception occured Grabbing PublishDateOfLastFeedItemReceived", e);
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
			get
			{
				return CurrentBlog.RFC3229DeltaEncodingEnabled && AcceptDeltaEncoding;
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
			if (dt != null)
			{
				try
				{
					DateTime feedDT = DateTime.Parse(dt);
					DateTime lastUpdated = ConvertLastUpdatedDate(CurrentBlog.LastUpdated);
					TimeSpan ts = feedDT - lastUpdated;

					//We need to allow some margin of error.
					return Math.Abs(ts.TotalMilliseconds) <= 500;
				}
				catch(FormatException)
				{
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
		protected virtual bool IsHttpCacheOK()
		{
			Feed = Context.Cache[this.CacheKey(this.PublishDateOfLastFeedItemReceived)] as CachedFeed;
			if (Feed == null)
			{
				return false;
			}
			return Feed.LastModified == ConvertLastUpdatedDate(CurrentBlog.LastUpdated);
		}

		/// <summary>
		/// Send the HTTP status code 304 to the response this instance.
		/// </summary>
		private void Send304()
		{
			Context.Response.StatusCode = 304;
		}

		/// <summary>
		/// Convert a date to the server time.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		protected static DateTime ConvertLastUpdatedDate(DateTime dt)
		{
			DateTime utc = Config.CurrentBlog.TimeZone.ToUniversalTime(dt);
			return TimeZone.CurrentTimeZone.ToLocalTime(utc);
		}

		/// <summary>
		/// Processs the feed. Responds to the incoming request with the 
		/// contents of the feed.
		/// </summary>
		protected virtual void ProcessFeed()
		{
			if (RedirectToFeedBurnerIfNecessary())
				return;

			// Checks Last Modified Header.
			if (IsLocalCacheOK())
			{
				Send304();
				return;
			}

			// Checks our cache against last modified header.
			if (!IsHttpCacheOK())
			{
				Feed = BuildFeed();
				if (Feed != null)
				{
					if (UseDeltaEncoding && Feed.ClientHasAllFeedItems)
					{
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

		protected virtual CachedFeed BuildFeed()
		{
			CachedFeed feed = new CachedFeed();
			feed.LastModified = ConvertLastUpdatedDate(CurrentBlog.LastUpdated);
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

			if (Feed != null)
			{
				if (Config.CurrentBlog.UseSyndicationCompression && this.AcceptGzipCompression)
				{
					// We're GZip Encoding!
					SyndicationCompressionFilter filter = SyndicationCompressionHelper.GetFilterForScheme(this.AcceptEncoding, Context.Response.Filter);

					if (filter != null)
					{
						encoding = filter.ContentEncoding;
						Context.Response.Filter = filter.Filter;
					}
				}

				if (encoding == null)
				{
					Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
				}

				Context.Response.ContentType = "text/xml";
				Context.Response.Cache.SetCacheability(HttpCacheability.Public);
				Context.Response.Cache.SetLastModified(Feed.LastModified);
				Context.Response.Cache.SetETag(Feed.Etag);
				if (AcceptGzipCompression)
				{
					Context.Response.AddHeader("IM", "feed, gzip");
				}
				else
				{
					Context.Response.AddHeader("IM", "feed");
				}
				if (this.UseDeltaEncoding)
					Context.Response.StatusCode = HTTP_IM_USED; //IM Used
				else
					Context.Response.StatusCode = (int)HttpStatusCode.OK;

				Context.Response.Write(Feed.Xml);
			}
		}

		/// <summary>
		/// Processs the request and sends the feed to the response.
		/// </summary>
		/// <param name="context">Context.</param>
		public override void HandleRequest(HttpContext context)
		{
			if ((RequiresAdminRole && !SecurityHelper.IsAdmin) || (RequiresHostAdminRole && !SecurityHelper.IsHostAdmin))
			{
				System.Web.Security.FormsAuthentication.RedirectToLoginPage();
				return;
			}

			CurrentBlog = Config.CurrentBlog;
			Context = context;

			ProcessFeed();
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
				string header = Context.Request.Headers["Accept-Encoding"];
				if (header != null)
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
				string header = Context.Request.Headers["A-IM"];
				if (header != null)
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
			if (!String.IsNullOrEmpty(Config.CurrentBlog.FeedBurnerName))
			{
				HttpContext current = HttpContext.Current;
				string userAgent = current.Request.UserAgent;
				if (!String.IsNullOrEmpty(userAgent))
				{
					// If they aren't FeedBurner and they aren't asking for a category or comment rss, redirect them!
					if (!userAgent.StartsWith("FeedBurner") && IsMainfeed)
					{
						current.Response.StatusCode = HTTP_MOVED_PERMANENTLY;
						current.Response.Status = HTTP_MOVED_PERMANENTLY + " Moved Permanently";
						current.Response.RedirectLocation = Config.CurrentBlog.UrlFormats.FeedBurnerUrl.ToString();
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

		/// <summary>
		/// Gets the content MIME type.
		/// </summary>
		/// <value></value>
		public override string ContentMimeType
		{
			get { return "text/xml"; }
		}

		/// <summary>
		/// Gets a value indicating whether this handler
		/// requires users to be authenticated.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if authentication is required
		/// otherwise, <c>false</c>.
		/// </value>
		public override bool RequiresAuthentication
		{
			get { return false; }
		}

		/// <summary>
		/// Validates the parameters.  Inheriting classes must
		/// implement this and return true if the parameters are
		/// valid, otherwise false.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns>
		/// 	<c>true</c> if the parameters are valid,
		/// otherwise <c>false</c>
		/// </returns>
		public override bool ValidateParameters(HttpContext context)
		{
			return true;
		}
	}
}
