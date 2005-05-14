using System;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Syndication.Compression;
using Subtext.Framework.Util;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for BaseSyndicationHandler.
	/// </summary>
	public abstract class BaseSyndicationHandler : System.Web.IHttpHandler
	{
		protected BlogConfig CurrentBlog = null;
		protected HttpContext Context = null;
		protected CachedFeed Feed = null;

		/// <summary>
		/// Gets the last modified header.
		/// </summary>
		/// <value></value>
		protected string LastModifiedHeader
		{
			get
			{
				return Context.Request.Headers["If-Modified-Since"] as string;
			}
		}

		/// <summary>
		/// Returns whether or not the local cache is OK.
		/// </summary>
		/// <returns></returns>
		protected virtual bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;
			if(dt != null)
			{
				try
				{
					DateTime feedDT = DateTime.Parse(dt);
					return DateTime.Compare(feedDT,ConvertLastUpdatedDate(CurrentBlog.LastUpdated)) == 0;
				}
				catch{}
			}
			return false;
		}

		/// <summary>
		/// Returns whether or not the http cache is OK.
		/// </summary>
		/// <returns></returns>
		protected virtual bool IsHttpCacheOK()
		{
			Feed = Context.Cache[this.CacheKey()] as CachedFeed;
			if(Feed == null)
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

		protected DateTime ConvertLastUpdatedDate(DateTime dt)
		{
			return BlogTime.ConvertToServerTime(dt,CurrentBlog.TimeZone);
		}

		protected virtual void ProcessFeed()
		{
			if(IsLocalCacheOK())
			{
				Send304();
			}
			else
			{
				if(!IsHttpCacheOK())
				{
					Feed = BuildFeed();
					if(Feed != null)
					{
						Cache(Feed);
					}
				}

				WriteFeed();
			}
		}

		protected abstract CachedFeed BuildFeed();
		protected abstract string CacheKey();
		protected abstract void Cache(CachedFeed feed);

		/// <summary>
		/// Writes the feed to the response.
		/// </summary>
		protected virtual void WriteFeed()
		{
			string encoding = null;

			if(Feed != null)
			{
				if(Config.CurrentBlog.UseSyndicationCompression && this.AcceptEncoding != null)
				{
					// We're GZip Encoding!
					SyndicationCompressionFilter filter = SyndicationCompressionHelper.GetFilterForScheme(this.AcceptEncoding, Context.Response.Filter);
	
					if(filter != null)
					{
						encoding = filter.ContentEncoding;
						Context.Response.Filter = filter.Filter;
						Context.Response.AppendHeader("Content-Encoding", encoding);
					}
				}

				if(encoding == null)
				{
					Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
				}

				Context.Response.ContentType = "text/xml";
				Context.Response.Cache.SetCacheability(HttpCacheability.Public);
				Context.Response.Cache.SetLastModified(Feed.LastModified);
				Context.Response.Cache.SetETag(Feed.Etag);
				Context.Response.Write(Feed.Xml);
			}
		}


		/// <summary>
		/// Processs the request and sends the feed to the response.
		/// </summary>
		/// <param name="context">Context.</param>
		public void ProcessRequest(HttpContext context)
		{
			CurrentBlog = Config.CurrentBlog;
			Context = context;

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
			get
			{
				return false;
			}
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
				return Context.Request.Headers["Accept-Encoding"];
			}
		}
	}
}
