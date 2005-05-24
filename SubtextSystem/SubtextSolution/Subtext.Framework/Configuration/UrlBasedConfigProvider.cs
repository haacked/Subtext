using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// Provides a BlogConfig instance based on the URL.
	/// </summary>
	public class UrlBasedConfigProvider
	{
		static UrlBasedConfigProvider _singletonInstance = new UrlBasedConfigProvider();
		
		/// <summary>
		/// Returns a singleton instance of the UrlConfigProvider.
		/// </summary>
		/// <value></value>
		public static UrlBasedConfigProvider Instance
		{
			get
			{
				return _singletonInstance;
			}
		}
		protected const string adminPath = "/{0}/{1}";
		protected const string cacheKey = "BlogConfig-";

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
		/// <summary>
		/// Gets the blog configuration based on the current http context.
		/// </summary>
		/// <returns></returns>
		public BlogConfig GetConfig()
		{
			return GetConfig(HttpContext.Current);
		}

		private int _blogID;
		/// <summary>
		/// Gets or sets the blog ID.
		/// </summary>
		/// <value></value>
		public int BlogID
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

		private string _host;
		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		/// <value></value>
		public string Host
		{
			get {return this._host;}
			set {this._host = value;}
		}

		private string _application;
		/// <summary>
		/// Gets or sets the application.
		/// </summary>
		/// <value></value>
		public string Application
		{
			get {return this._application;}
			set {this._application = value;}
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
		/// Returns a BlogConfig instance for the current blog. 
		/// The object first checks the context for an existing object. 
		/// It will next check the cache.
		/// </summary>
		/// <returns></returns>
		public virtual BlogConfig GetConfig(HttpContext context)
		{
			// First check the context for an existing BlogConfig. This saves us the trouble
			// of having to figure out which blog we are at.
			BlogConfig config = (BlogConfig)context.Items[cacheKey];
			if(config == null)
			{
				string app = UrlFormats.GetBlogAppFromRequest(context.Request.RawUrl.ToLower(), context.Request.ApplicationPath);
				//BlogConfig was not found in the context. It could be in the current cache.
				string mCacheKey = cacheKey + app;

				//check the cache.
				config = (BlogConfig)context.Cache[mCacheKey];
				if(config == null)
				{
					//Not found in the cache

					if(Host == null)
					{
						// for example: haacked.com
						//				localhost
						Host = GetCurrentHost(context.Request);
					}

					config = Subtext.Framework.Configuration.Config.GetConfig(Host, app);
					if(config == null)
					{
						throw new BlogDoesNotExistException(String.Format("A blog matching the location you requested was not found. Host = [{0}], Application = [{1}]", Host, app));
					}

					BlogConfigurationSettings settings = Subtext.Framework.Configuration.Config.Settings;

					string appPath = Globals.FormatApplicationPath(string.Format("{0}/{1}", context.Request.ApplicationPath, app));

					string formattedHost = GetFormattedHost(Host,settings.UseWWW);

					config.FullyQualifiedUrl = formattedHost + appPath;


					if(!app.EndsWith("/"))
					{
						app += "/";
					}

					string virtualPath = string.Format("/images/{0}/{1}/", Regex.Replace(Host,@"\:|\.","_"), app);

					config.ImagePath = string.Format("{0}{1}{2}", formattedHost, app, virtualPath);
					config.ImageDirectory = context.Server.MapPath("~" + virtualPath);

					CacheConfig(context.Cache,config,mCacheKey);
					context.Items.Add(cacheKey,config);
				}
				else
				{
					context.Items.Add(cacheKey, config);
				}
			}
			return config;
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
				host  += ":" + Request.Url.Port.ToString();
			}

			if(StringHelper.StartsWith(host, "www.", true))
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
		/// <param name="config">Config.</param>
		/// <param name="cacheKEY">Cache KEY.</param>
		protected void CacheConfig(Cache cache, BlogConfig config, string cacheKEY)
		{
			cache.Insert(cacheKEY, config, null, DateTime.Now.AddSeconds(CacheTime), TimeSpan.Zero, CacheItemPriority.High, null);
		}
	}
}