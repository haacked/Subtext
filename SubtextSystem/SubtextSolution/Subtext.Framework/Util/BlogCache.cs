using System;
using System.Web;
using System.Web.Caching;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Allows caching in application in applications hosted inside and out of IIS
	/// </summary>
	public class BlogCache
	{
		private BlogCache(){}


		public static Cache Cache(HttpContext context)
		{
			return GetCache(context);
		}

		public static Cache Cache()
		{
			return Cache(HttpContext.Current);
		}

		public static void AssemblyCacheDependency(string cacheKey, object obj)
		{
			AssemblyCacheDependency(cacheKey,obj,HttpContext.Current);
		}

		public static void AssemblyCacheDependency(string cacheKey, object obj, HttpContext context)
		{
			if(cacheKey != null && obj != null)
			{
				CacheDependency cacheDependency = new CacheDependency(obj.GetType().Assembly.Location);
				BlogCache.Cache().Insert(cacheKey,obj,cacheDependency);
			}
		}

		private static Cache GetCache(HttpContext context)
		{
			if(context != null)
			{
				return context.Cache;
			}
			else
			{
				return HttpRuntime.Cache;
			}
		}
	}
}
