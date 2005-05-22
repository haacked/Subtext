using System;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Threading;

namespace Subtext.Common.Config
{
	/// <summary>
	/// A sample implementation of IConfig. This class will return the BlogConfig 
	/// found at the current host/application, or the one found by configuring the 
	/// Providers Host and Application values.
	/// </summary>
	public class SingleBlogConfig : BaseBlogConfig
	{
		private  static readonly object ConfigLock = new object();

		/// <summary>
		/// Gets the configuration based on the specified <see cref="HttpContext"/>. 
		/// Must be implemented by configuration handlers.
		/// </summary>
		/// <remarks>
		/// Will look for the configuration in the cache first using the 
		/// key "BlogConfig-".
		/// </remarks>
		/// <param name="context">Context.</param>
		/// <returns></returns>
		public override BlogConfig GetConfig(HttpContext context)
		{
			BlogConfig config = (BlogConfig)context.Cache[cacheKey];
			if(config == null)
			{
				using(TimedLock.Lock(ConfigLock))
				{
					config = (BlogConfig)context.Cache[cacheKey];
					if(config == null)
					{
						config = this.LoadSingleConfig(context);

						this.CacheConfig(context.Cache, config, cacheKey);
					}
				}
			}
			return config;
		}

		#region LoadSingleConfig
		private BlogConfig LoadSingleConfig(HttpContext context)
		{
			if(Host == null)
			{
				Host = GetCurrentHost(context.Request);
			}

			if(Application == null)
			{
				string appPath = context.Request.ApplicationPath;
				if(appPath.StartsWith("/"))
				{
					appPath = appPath.Remove(0, 1);
				}
				if(appPath.EndsWith("/"))
				{
					appPath = appPath.Remove(appPath.Length - 1, 1);
				}
				Application = appPath;
			}

			BlogConfig config = Subtext.Framework.Configuration.Config.GetConfig(Host, Application);
			if(config == null)
			{
				throw new BlogDoesNotExistException(String.Format("A blog matching the location you requested was not found. Host = [{0}], Application = [{1}]",
					Host, 
					Application));
			}
			config.Host = Host;
			config.Application = Application;

			BlogConfigurationSettings settings = Subtext.Framework.Configuration.Config.Settings;

			if(settings.UseWWW)
			{
				config.FullyQualifiedUrl = "http://www." + config.Host + config.Application;
			}
			else
			{
				config.FullyQualifiedUrl = "http://" + config.Host + config.Application;
			}

			config.ImageDirectory = context.Server.MapPath("~/images");
			config.ImagePath = string.Format("{0}images/",config.FullyQualifiedUrl);

			return config;
		}
		#endregion

	}
}
