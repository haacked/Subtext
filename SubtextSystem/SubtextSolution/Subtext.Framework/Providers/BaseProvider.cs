using System;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using Subtext.Framework.Util;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Abstract base class for all providers.
	/// </summary>
	public abstract class BaseProvider
	{
		private string _type;

		[XmlAttribute("type")]
		public string ProviderType
		{
			get	{	return _type;	}
			set	{ _type = value;	}
		}

		/// <summary>
		/// Returns an instance of this provider.
		/// </summary>
		/// <returns></returns>
		public object Instance()
		{
			return Activator.CreateInstance(System.Type.GetType(this.ProviderType));
		}

		/// <summary>
		/// Saves the specified object into the <see cref="BlogCache"/> 
		/// using the current <see cref="HttpContext"/>.
		/// </summary>
		/// <remarks>
		/// The object is saved with a cache dependency on its assembly.
		/// </remarks>
		/// <param name="cacheKey">Cache key.</param>
		/// <param name="obj">Obj.</param>
		public static void SaveCache(string cacheKey, object obj)
		{
			SaveCache(cacheKey, obj, HttpContext.Current);
		}

		/// <summary>
		/// Saves the specified object into the <see cref="BlogCache"/> 
		/// using the specified <see cref="HttpContext"/>.
		/// </summary>
		/// <remarks>
		/// The object is saved with a cache dependency on its assembly.
		/// </remarks>
		/// <param name="cacheKey">Cache key.</param>
		/// <param name="obj">Obj.</param>
		/// <param name="context">Context.</param>
		public static void SaveCache(string cacheKey, object obj, HttpContext context)
		{
			if(cacheKey != null && obj != null)
			{
				CacheDependency cacheDependency = new CacheDependency(obj.GetType().Assembly.Location);
				BlogCache.Cache(context).Insert(cacheKey, obj, cacheDependency);
			}
		}
	}
}
