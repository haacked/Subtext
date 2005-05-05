using System;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for BaseSyndicationHandler.
	/// </summary>
	public abstract class BaseSyndicationHandler : System.Web.IHttpHandler
	{
		public BaseSyndicationHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected BlogConfig CurrentBlog = null;
		protected HttpContext Context = null;
		protected CachedFeed Feed = null;

		protected string LastModifiedHeader
		{
			get
			{
				return Context.Request.Headers["If-Modified-Since"] as string;
			}
		}

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

		protected virtual bool IsHttpCacheOK()
		{
			Feed = Context.Cache[this.CacheKey()] as CachedFeed;
			if(Feed == null)
			{
				return false;
			}
			return Feed.LastModified == ConvertLastUpdatedDate(CurrentBlog.LastUpdated);
		}

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

		protected virtual void WriteFeed()
		{
			if(Feed != null)
			{
				Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
				Context.Response.ContentType = "text/xml";
				Context.Response.Cache.SetCacheability(HttpCacheability.Public);
				Context.Response.Cache.SetLastModified(Feed.LastModified);
				Context.Response.Cache.SetETag(Feed.Etag);
				Context.Response.Write(Feed.Xml);
			}
		}


		public void ProcessRequest(HttpContext context)
		{
			CurrentBlog = Config.CurrentBlog(context);
			Context = context;

			ProcessFeed();

		}

		public bool IsReusable
		{
			get
			{
				// TODO:  Add BaseSyndicationHandler.IsReusable getter implementation
				return false;
			}
		}


	}
}
