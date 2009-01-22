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
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using log4net;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Logging;
using Subtext.Framework.Web.HttpModules;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Provides a <see cref="Blog"/> instance based on the URL.
	/// </summary>
	public class UrlBasedBlogInfoProvider
	{
        public UrlBasedBlogInfoProvider() {
            CacheTime = 5;
        }

		private readonly static ILog log = new Log();

		static readonly UrlBasedBlogInfoProvider _singletonInstance = new UrlBasedBlogInfoProvider();
		
		/// <summary>
		/// Returns a singleton instance of the UrlConfigProvider.
		/// </summary>
		/// <value></value>
		public static UrlBasedBlogInfoProvider Instance
		{
			get
			{
				return _singletonInstance;
			}
		}
		protected const string adminPath = "/{0}/{1}";
		protected const string cacheKey = "BlogInfo-";

		/// <summary>
		/// Returns the host formatted correctly with "http://" or "https://" and "www." 
		/// if specified.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="useWWW">Use WWW.</param>
		/// <returns></returns>
		static protected string GetFormattedHost(string host, bool useWWW)
		{
			if(useWWW) {
				return HttpContext.Current.Request.Url.Scheme + "://www." +  host;
			}
			else {
                return HttpContext.Current.Request.Url.Scheme + "://" + host;
			}
		}

		/// <summary>
		/// Gets or sets the cache time.
		/// </summary>
		/// <value></value>
		public int CacheTime
		{
			get;
			set;
		}

		static T GetRequestCache<T>(string key)
		{
			return (T)HttpContext.Current.Items[key];
		}

		static void SetRequestCache<T>(string key, T value)
		{
			HttpContext.Current.Items[key] = value;
		}

		static T GetApplicationCache<T>(string key)
		{
			return (T) HttpContext.Current.Cache[key];
		}

		/// <summary>
		/// Returns a <see cref="Blog"/> instance for the current blog. 
		/// The object first checks the context for an existing object. 
		/// It will next check the cache.
		/// </summary>
		/// <returns></returns>
		public virtual Blog GetBlogInfo(BlogRequest blogRequest)
		{
            Blog info = blogRequest.Blog;

            if (info != null) {
                // look here for issues with gallery images not showing up.
                MapImageDirectory(blogRequest);
                SetBlogIdContextForLogging(blogRequest);
            }
			return info;
		}

		private static void SetBlogIdContextForLogging(BlogRequest blogRequest)
		{
            if (!blogRequest.IsHostAdminRequest) {
				// Set the BlogId context for the current request.
                Log.SetBlogIdContext(blogRequest.Blog.Id);
			}
			else {
				Log.ResetBlogIdContext();
			}
		}

		private static void MapImageDirectory(BlogRequest blogRequest)
		{
            Blog info = blogRequest.Blog;
			BlogConfigurationSettings settings = Config.Settings;
			string webApp = HttpContext.Current.Request.ApplicationPath;

			if(webApp.Length <= 1)
				webApp = string.Empty;

			string formattedHost = GetFormattedHost(blogRequest.Host, settings.UseWWW) + webApp;

			string subfolder = blogRequest.Subfolder;
			if(!subfolder.EndsWith("/"))
			{
				subfolder += "/";
			}
            if (subfolder.Length > 1) {
                subfolder = "/" + subfolder;
            }
					
			string virtualPath = string.Format(CultureInfo.InvariantCulture
                , "images/{0}{1}"
                , Regex.Replace(blogRequest.Host + webApp, @"\:|\." , "_")
                , subfolder);

			// now put together the host + / + virtual path (url) to images
			info.ImagePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", formattedHost, virtualPath);
			try {
				info.ImageDirectory = HttpContext.Current.Request.MapPath("~/" + virtualPath);
			}
			catch(ArgumentNullException nullException)
			{
				log.Warn("Could not map the image directory.", nullException);
			}
		}

		private Blog GetAggregateBlog(string mCacheKey)
		{
			Blog info;
			info = Blog.AggregateBlog;
			CacheConfig(HttpContext.Current.Cache, info, mCacheKey);
			HttpContext.Current.Items.Add(cacheKey, info);
			return info;
		}

		/// <summary>
		/// Gets the current host, stripping off the initial "www." if 
		/// found.
		/// </summary>
		/// <param name="Request">Request.</param>
		/// <returns></returns>
		protected static string GetCurrentHost(HttpRequest Request)
		{
			string host = Request.Url.Host;
			if(!Request.Url.IsDefaultPort)
			{
				host  += ":" + Request.Url.Port.ToString(CultureInfo.InvariantCulture);
			}

			if (host.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
			{
				host = host.Substring(4);
			}
			return host;
		}

		/// <summary>
		/// Stores the blog configuration in the cache using the specified cache key.
		/// </summary>
		/// <remarks>
		/// The config is stored with a high <see cref="CacheItemPriority"/>.
		/// No callback is registered for the removal of the blog item.
		/// </remarks>
		/// <param name="cache">Cache.</param>
		/// <param name="info">Config.</param>
		/// <param name="cacheKEY">Cache KEY.</param>
		protected void CacheConfig(Cache cache, Blog info, string cacheKey)
		{
			cache.Insert(cacheKey, info, null, DateTime.Now.AddSeconds(CacheTime), TimeSpan.Zero, CacheItemPriority.High, null);
		}
	}
}
