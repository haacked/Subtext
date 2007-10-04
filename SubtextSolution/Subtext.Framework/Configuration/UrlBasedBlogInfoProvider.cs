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
using Subtext.Framework.Text;
using Subtext.Framework.Web.HttpModules;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Provides a <see cref="BlogInfo"/> instance based on the URL.
	/// </summary>
	public class UrlBasedBlogInfoProvider
	{
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
			if(useWWW)
			{
				return HttpContext.Current.Request.Url.Scheme + "://www." +  host;
			}
			else
			{
                return HttpContext.Current.Request.Url.Scheme + "://" + host;
			}
		}

		#region IConfig
		private int _cacheTime;
		/// <summary>
		/// Gets or sets the cache time.
		/// </summary>
		/// <value></value>
		public int CacheTime
		{
			get {return this._cacheTime;}
			set {this._cacheTime = value;}
		}
		#endregion

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance for the current blog. 
		/// The object first checks the context for an existing object. 
		/// It will next check the cache.
		/// </summary>
		/// <returns></returns>
		public virtual BlogInfo GetBlogInfo()
		{
			if (HttpContext.Current == null)
				return null;

			// First check the context for an existing BlogConfig. This saves us the trouble
			// of having to figure out which blog we are at.
			BlogInfo info = (BlogInfo) HttpContext.Current.Items[cacheKey];
			
#if DEBUG
			if(info != null)
				log.DebugFormat("Info found in the HttpContext.Current.Items cache with cachekey {0}", cacheKey);
#endif
			
			if(info == null)
			{
				BlogRequest blogRequest = BlogRequest.Current;
				
				//BlogInfo was not found in the context. It could be in the current cache.
				string mCacheKey = cacheKey + blogRequest.Subfolder;

				//check the cache.
				info = (BlogInfo)HttpContext.Current.Cache[mCacheKey];
				if(info == null)
				{
					//Not found in the cache
					log.DebugFormat("Attempting to get blog info. Host: {0}, Subfolder: {1}", blogRequest.Host, blogRequest.Subfolder);
					
                    info = Config.GetBlogInfo(blogRequest.Host, blogRequest.Subfolder, false);
					if (info == null)
					{
						info = Config.GetBlogInfo(BlogInfo.GetAlternateHostAlias(blogRequest.Host), blogRequest.Subfolder, false);
						if (info == null)
						{
							log.DebugFormat("Attempting to get blog by domain alias. Host: {0}, Subfolder: {1}", blogRequest.Host, blogRequest.Subfolder);
							info = Config.GetBlogInfoFromDomainAlias(blogRequest.Host, blogRequest.Subfolder,false);							
						}
						if (info != null)
						{
							string url = BlogRequest.Current.RawUrl.ToString();
							UriBuilder uriBuilder = new UriBuilder(url);
							uriBuilder.Host = info.Host;
							if (blogRequest.Subfolder != info.Subfolder)
							{
								if (blogRequest.Subfolder.Length > 0)
									uriBuilder.Path = uriBuilder.Path.Remove(0, blogRequest.Subfolder.Length+1);
								if (info.Subfolder.Length > 0)
									uriBuilder.Path = "/" + info.Subfolder + uriBuilder.Path;

							}
							//string newUrl = HtmlHelper.ReplaceHost(url, info.Host);
							HttpContext.Current.Response.StatusCode = 301;
							HttpContext.Current.Response.Status = "301 Moved Permanently";
							HttpContext.Current.Response.RedirectLocation = uriBuilder.ToString();
							HttpContext.Current.Response.End();
						}
					}
					
					if(info == null)
					{
						log.InfoFormat("No active blog found for Host: {0}, Subfolder: {1}", blogRequest.Host, blogRequest.Subfolder);
						bool anyBlogsExist = Config.BlogCount > 0;

                        if (anyBlogsExist && ConfigurationManager.AppSettings["AggregateEnabled"] == "true")
						{
							return Config.AggregateBlog;
						}

						// When going thru the install for MultiBlogs there will be no blogs in the system,
						// so just return null... there must be a better way.
						// The same is true for requests to the HostAdmin directory.
						if(InstallationManager.IsOnLoginPage 
							|| (!anyBlogsExist && InstallationManager.IsInInstallDirectory) // may not need the anyBlogsExist check
							|| InstallationManager.IsInHostAdminDirectory)
						{
							return null;
						}

						throw new BlogDoesNotExistException(blogRequest.Host, blogRequest.Subfolder, anyBlogsExist);
					}

					if(!info.IsActive && !InstallationManager.IsInHostAdminDirectory && !InstallationManager.IsInSystemMessageDirectory && !InstallationManager.IsOnLoginPage)
					{
						throw new BlogInactiveException();
					}
			
					BlogConfigurationSettings settings = Config.Settings;

					// look here for issues with gallery images not showing up.
					string webApp = HttpContext.Current.Request.ApplicationPath;

					if(webApp.Length <= 1)
						webApp="";

					string formattedHost = GetFormattedHost(blogRequest.Host, settings.UseWWW) + webApp;

					string subfolder = blogRequest.Subfolder;
					if(!subfolder.EndsWith("/"))
					{
						subfolder += "/";
					}
					if(subfolder.Length > 1)
						subfolder = "/" + subfolder;
					
					string virtualPath = string.Format(CultureInfo.InvariantCulture, "images/{0}{1}", Regex.Replace(blogRequest.Host + webApp, @"\:|\.","_"), subfolder);

					// now put together the host + / + virtual path (url) to images
					info.ImagePath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", formattedHost, virtualPath);
					try
					{
						info.ImageDirectory = HttpContext.Current.Request.MapPath("~/" + virtualPath);
					}
					catch(ArgumentNullException nullException)
					{
						log.Warn("Could not map the image directory.", nullException);
					}

					CacheConfig(HttpContext.Current.Cache, info, mCacheKey);
					HttpContext.Current.Items.Add(cacheKey, info);

					if(!InstallationManager.IsInHostAdminDirectory)
					{
						// Set the BlogId context for the current request.
						Log.SetBlogIdContext(info.Id);
					}
					else
					{
						HttpContext.Current.Items[Security.SecurityHelper.ApplicationNameContextId] = "/";
						Log.ResetBlogIdContext();
					}
				}
				else
				{
					HttpContext.Current.Items.Add(cacheKey, info);
				}
			}

			//TODO: Use dependency injection or a provider. This'll do for now.
			
			return info;
		}

		private static BlogInfo GetAggregateBlog()
		{
			BlogInfo aggregateBlog = new BlogInfo();
            aggregateBlog.Title = ConfigurationManager.AppSettings["AggregateTitle"];
			aggregateBlog.Skin = SkinConfig.GetDefaultSkin();
            aggregateBlog.Host = ConfigurationManager.AppSettings["AggregateHost"];
			aggregateBlog.Subfolder = "";
			//TODO: aggregateBlog.UserName = HostInfo.Instance.HostUserName;
			
			return aggregateBlog;
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
		protected void CacheConfig(Cache cache, BlogInfo info, string cacheKEY)
		{
            if (cache == null)
                throw new ArgumentNullException("cache", Resources.ArgumentNull_Generic);

            if (info == null)
                throw new ArgumentNullException("info", Resources.ArgumentNull_Generic);

            if (cacheKEY == null)
                throw new ArgumentNullException("cacheKEY", Resources.ArgumentNull_Generic);

            if (cacheKEY.Length == 0)
                throw new ArgumentException(Resources.Argument_StringZeroLength, "cacheKEY");

			cache.Insert(cacheKEY, info, null, DateTime.Now.AddSeconds(CacheTime), TimeSpan.Zero, CacheItemPriority.High, null);
		}
	}
}
