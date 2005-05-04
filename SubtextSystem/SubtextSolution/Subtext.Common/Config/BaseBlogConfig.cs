using System;
using System.Web;
using System.Web.Caching;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;

namespace Subtext.Common.Config
{
	/// <summary>
	/// Abstract base class used to implement a blog configuration class.  
	/// This is implemented by <see cref="MultipleBlogConfig"/> and <see cref="SingleBlogConfig"/>.
	/// </summary>
	public abstract class BaseBlogConfig : IConfig
	{
		protected const string adminPath = "/{0}/{1}";
		protected const string cacheKey = "BlogConfig-";

		#region IConfig

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

		/// <summary>
		/// Gets the blog configuration based on the current http context.
		/// </summary>
		/// <returns></returns>
		public BlogConfig GetConfig()
		{
			return GetConfig(HttpContext.Current);
		}

		/// <summary>
		/// Gets the configuration based on the specified <see cref="HttpContext"/>. 
		/// Must be implemented by configuration handlers.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <returns></returns>
		public abstract BlogConfig GetConfig(HttpContext context);
	}
}
