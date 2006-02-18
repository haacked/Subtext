using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Subtext.Framework
{
	/// <summary>
	/// This is a replacement for the <see cref="Cache"/> object. 
	/// Use this when caching content.  This ensures content is 
	/// cached according to key and Locale.
	/// </summary>
	public class ContentCache : IEnumerable
	{
		Cache cache;

		/// <summary>
		/// Instantiates the specified content cache from the specific <see cref="HttpContext"/>. 
		/// At some point, we might consider replacing HttpContext with a type we can extend.
		/// </summary>
		/// <returns></returns>
		public static ContentCache Instantiate()
		{
			//Check per-request cache.
			ContentCache cache = HttpContext.Current.Items["ContentCache"] as ContentCache;
			if(cache != null)
				return cache;

			cache = new ContentCache(HttpContext.Current.Cache);
			//Per-Request Cache.
			HttpContext.Current.Items["ContentCache"] = cache;
			return cache;
		}

		private ContentCache() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentCache"/> class. 
		/// The specified <see cref="Cache"/> instance is wrapped by this instance.
		/// </summary>
		/// <param name="cache">The cache.</param>
		private ContentCache(Cache cache)
		{
			this.cache = cache;
		}

		//Returns a language aware cache key.
		private string GetCacheKey(string key)
		{
			return key + ":" + Thread.CurrentThread.CurrentCulture.LCID.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Gets or sets the <see cref="Object"/> with the specified key.
		/// </summary>
		/// <value></value>
		public object this[string key]
		{
			get
			{
				return this.cache[GetCacheKey(key)];
			}
			set
			{
				this.cache.Insert(GetCacheKey(key), value);
			}
		}

		/// <summary>
		/// Inserts the specified object to the <see cref="System.Web.Caching.Cache"/> object 
		/// with a cache key to reference its location and using default values provided by 
		/// the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Insert(string key, object value)
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot cache a null object.");
			this.cache.Insert(GetCacheKey(key), value);
		}

		/// <summary>
		/// <para>Inserts the specified object to the <see cref="System.Web.Caching.Cache"/> object 
		/// with a cache key to reference its location and using default values provided by 
		/// the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration.
		/// </para>
		/// <para>
		/// Allows specifying a general cache duration.
		/// </para>
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheDuration">The cache duration.</param>
		public void Insert(string key, object value, CacheDuration cacheDuration)
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot cache a null object.");
			
			this.cache.Insert(GetCacheKey(key), value, null, DateTime.Now.AddSeconds((int)cacheDuration), TimeSpan.Zero, CacheItemPriority.Normal, null);
		}

		/// <summary>
		/// <para>Inserts the specified object to the <see cref="System.Web.Caching.Cache"/> object 
		/// with a cache key to reference its location and using default values provided by 
		/// the <see cref="System.Web.Caching.CacheItemPriority"/> enumeration.
		/// </para>
		/// <para>
		/// Allows specifying a <see cref="CacheDependency"/>
		/// </para>
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="cacheDependency">The cache dependency.</param>
		public void Insert(string key, object value, CacheDependency cacheDependency)
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot cache a null object.");
			
			this.cache.Insert(GetCacheKey(key), value, cacheDependency);
		}

		/// <summary>
		/// Retrieves the specified item from the <see cref="System.Web.Caching.Cache"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object Get(string key)
		{
			return this.cache.Get(GetCacheKey(key));
		}

		/// <summary>
		/// Removes the specified item from the cache.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public object Remove(string key)
		{
			return this.cache.Remove(GetCacheKey(key));
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/>
		/// that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return this.cache.GetEnumerator();
		}
	}

	/// <summary>
	/// Low granularity Cache Duration.
	/// </summary>
	public enum CacheDuration
	{
		None = 0,
		Short = 10,
		Medium = 20,
		Long = 30
	};
}
