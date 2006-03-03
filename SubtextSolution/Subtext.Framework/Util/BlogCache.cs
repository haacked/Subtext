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
