using System;
using System.Text.RegularExpressions;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace Subtext.Common.Config
{
	/// <summary>
	/// A sample implementation of a class that implements IConfig 
	/// for a multiple blog system
	/// </summary>
	public class MultipleBlogConfig : BaseBlogConfig
	{
		/// <summary>
		/// Override abstract GetConfig() (also part of IConfig). Returns a BlogConfig 
		/// instance for the current blog. The object first checks the context for an 
		/// existing object. It will next check the cache.
		/// </summary>
		/// <returns></returns>
		public override BlogConfig GetConfig(HttpContext context)
		{
			//First check the context for an existing BlogConfig. This saves us the trouble of having to figure out which blog we are at.
			BlogConfig config = (BlogConfig)context.Items[cacheKey];
			if(config == null)
			{
				string app = context.Request.ApplicationPath.ToLower();
				//BlogConfig was not found in the context. It could be in the current cache.
				string mCacheKey = cacheKey +  Globals.GetBlogAppFromRequest(context.Request.RawUrl.ToLower(), app);

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

					string appFromRequest = Globals.GetBlogAppFromRequest(context.Request.RawUrl.ToLower(), app);

					config = Subtext.Framework.Configuration.Config.GetConfig(Host, appFromRequest);

					//kind of hacky. Not sure if this value should be persisted in the db or not
					config.IsVirtual = true;

					BlogConfigurationSettings settings = Subtext.Framework.Configuration.Config.Settings;

					string appPath = Globals.FormatApplicationPath(string.Format("{0}/{1}",context.Request.ApplicationPath, appFromRequest));

					string formattedHost = GetFormattedHost(Host,settings.UseWWW);

					config.FullyQualifiedUrl = formattedHost + appPath;


					if(!app.EndsWith("/"))
					{
						app += "/";
					}

					string virtualPath = string.Format("/images/{0}/{1}/",Regex.Replace(Host,@"\:|\.","_"),appFromRequest);

					config.ImagePath = string.Format("{0}{1}{2}",formattedHost,app,virtualPath);
					config.ImageDirectory = context.Server.MapPath("~" + virtualPath);

					CacheConfig(context.Cache,config,mCacheKey);
					context.Items.Add(cacheKey,config);
				}
				else
				{
					context.Items.Add(cacheKey,config);
				}
			}
			return config;
		}
	}
}
