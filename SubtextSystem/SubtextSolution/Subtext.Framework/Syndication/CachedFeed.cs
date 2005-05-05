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

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// The CachedFeed is a cacheable container for our rss feed(s). Instead of requesting the cache data, processing it,
	/// and creating an XML document on each request, we will store the actually Rss document as a cached string.
	/// 
	/// Generally, it will be returned to the client by calling Response.Write(feed.Xml)
	/// </summary>
	public class CachedFeed
	{
		public CachedFeed()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private DateTime lastModified;
		//Need to figure out what happens to the date when we set LastModified date. Returned data usually does not match 
		//what we sent!
		public DateTime LastModified
		{
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

		private string etag;
		public string Etag
		{
			get
			{
				if(etag == null)
				{
					//if we did not set the etag, just use the LastModified Date
					etag = this.LastModified.ToString();
				}
				return etag;
			}
			set
			{
				etag = value;
			}
		}

		private string xml;
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

		/// <summary>
		/// Checks to see if the Requesting client has the most recent version of the feed
		/// </summary>
		/// <param name="CurrentEtag">Etag header sent with Response (If-None-Match)</param>
		/// <param name="CurrentLastModifiedDate">LastModified Data header sent with Response (If-Modified-Since)</param>
		/// <returns></returns>
		public bool IsCacheOK(string CurrentEtag, string CurrentLastModifiedDate)
		{
			bool isCache = false;

			if (CurrentLastModifiedDate != null)
			{
				string lastModified = this.LastModified.ToString("r");
				isCache = (lastModified == CurrentLastModifiedDate);					
			}

			if(!isCache)
			{
				if(CurrentEtag != null)
				{
					isCache = (this.Etag == CurrentEtag);
				}
			}

			return isCache;
		}

	}
}

