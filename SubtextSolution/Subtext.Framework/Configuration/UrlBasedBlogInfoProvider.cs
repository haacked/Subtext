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
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Provides a <see cref="BlogInfo"/> instance based on the URL.
	/// </summary>
	public class UrlBasedBlogInfoProvider
	{
		private readonly static ILog log = new Subtext.Framework.Logging.Log();

		static UrlBasedBlogInfoProvider _singletonInstance = new UrlBasedBlogInfoProvider();
		
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
		/// Returns the host formatted correctly with "http://" and "www." 
		/// if specified.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="useWWW">Use WWW.</param>
		/// <returns></returns>
		protected string GetFormattedHost(string host, bool useWWW)
		{
			if(useWWW)
			{
				return "http://www." +  host;
			}
			else
			{
				return "http://" +  host;
			}
		}

		#region IConfig
		private int _blogID;
		/// <summary>
		/// Gets or sets the blog ID.
		/// </summary>
		/// <value></value>
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

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

		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value></value>
		public string Host
		{
			get
			{
				return this.GetCurrentHost(HttpContext.Current.Request);
			}
		}

		private string subfolder;
		/// <summary>
		/// Gets or sets the application.
		/// </summary>
		/// <value></value>
		public string Subfolder
		{
			get {return this.subfolder;}
			set {this.subfolder = value;}
		}

		private string _imageDirectory;
		/// <summary>
		/// Gets or sets the image directory.
		/// </summary>
		/// <value></value>
		public string ImageDirectory
		{
			get {return this._imageDirectory;}
			set {this._imageDirectory = value;}
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
			// First check the context for an existing BlogConfig. This saves us the trouble
			// of having to figure out which blog we are at.
			BlogInfo info = (BlogInfo)HttpContext.Current.Items[cacheKey];
			if(info == null)
			{
				string subFolder = UrlFormats.GetBlogSubfolderFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);

				if(!Config.IsValidSubfolderName(subFolder))
					subFolder = string.Empty;

				//BlogConfig was not found in the context. It could be in the current cache.
				string mCacheKey = cacheKey + subFolder;

				//check the cache.
				info = (BlogInfo)HttpContext.Current.Cache[mCacheKey];
				if(info == null)
				{
					//Not found in the cache
					bool strict = true; //strict implies 
					info = Subtext.Framework.Configuration.Config.GetBlogInfo(Host, subFolder, !strict);
					if(info == null)
					{
						int totalBlogs;
						BlogInfo.GetActiveBlogs(1, 10, true, out totalBlogs);
						bool anyBlogsExist = totalBlogs > 0;

						if(anyBlogsExist && ConfigurationSettings.AppSettings["AggregateEnabled"] == "true")
						{
							return GetAggregateBlog();
						}

						throw new BlogDoesNotExistException(Host, subFolder, anyBlogsExist);
					}

					if(!info.IsActive && !InstallationManager.IsInHostAdminDirectory && !InstallationManager.IsInSystemMessageDirectory)
					{
						throw new BlogInactiveException();
					}
			
					BlogConfigurationSettings settings = Subtext.Framework.Configuration.Config.Settings;

					// look here for issues with gallery images not showing up.
					string webApp = HttpContext.Current.Request.ApplicationPath;

					if(webApp.Length<=1)
						webApp="";

					string formattedHost = GetFormattedHost(Host, settings.UseWWW)+webApp;

					if(!subFolder.EndsWith("/"))
					{
						subFolder += "/";
					}
					if(subFolder.Length > 1)
						subFolder="/" + subFolder;
					
					Console.WriteLine("INSIDE GetBlog: subfolder: " + subfolder);

					string virtualPath = string.Format(System.Globalization.CultureInfo.InvariantCulture, "images/{0}{1}", Regex.Replace(Host+webApp,@"\:|\.","_"), subFolder);

					// now put together the host + / + virtual path (url) to images
					info.ImagePath = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}", formattedHost, virtualPath);
					try
					{
						info.ImageDirectory = HttpContext.Current.Request.MapPath("~/" + virtualPath);
					}
					catch(NullReferenceException nullException)
					{
						log.Warn("Could not map the image directory.", nullException);
					}

					CacheConfig(HttpContext.Current.Cache, info, mCacheKey);
					HttpContext.Current.Items.Add(cacheKey, info);

					if(!InstallationManager.IsInHostAdminDirectory)
					{
						// Set the BlogId context for the current request.
						Log.SetBlogIdContext(info.BlogId);
					}
					else
					{
						Log.ResetBlogIdContext();
					}
				}
				else
				{
					HttpContext.Current.Items.Add(cacheKey, info);
				}
			}
			else
			{
				Console.WriteLine("Blog was in cache.");
			}

			return info;
		}

		private BlogInfo GetAggregateBlog()
		{
			BlogInfo aggregateBlog = new BlogInfo();
			aggregateBlog.Title = System.Configuration.ConfigurationSettings.AppSettings["AggregateTitle"];
			aggregateBlog.Skin = SkinConfig.GetDefaultSkin();
			aggregateBlog.Host = System.Configuration.ConfigurationSettings.AppSettings["AggregateHost"];
			aggregateBlog.Subfolder = "";
			aggregateBlog.UserName = HostInfo.Instance.HostUserName;
			
			return aggregateBlog;
		}

		/// <summary>
		/// Gets the current host, stripping off the initial "www." if 
		/// found.
		/// </summary>
		/// <param name="Request">Request.</param>
		/// <returns></returns>
		protected string GetCurrentHost(HttpRequest Request)
		{
			string host = Request.Url.Host;
			if(!Request.Url.IsDefaultPort)
			{
				host  += ":" + Request.Url.Port.ToString(CultureInfo.InvariantCulture);
			}

			if(StringHelper.StartsWith(host, "www.", ComparisonType.CaseInsensitive))
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
		protected void CacheConfig(Cache cache, BlogInfo info, string cacheKEY)
		{
			cache.Insert(cacheKEY, info, null, DateTime.Now.AddSeconds(CacheTime), TimeSpan.Zero, CacheItemPriority.High, null);
		}
	}
}