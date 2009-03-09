using System;
using System.Collections;
using System.Web.Caching;

namespace Subtext.Framework.Data
{
    public class SubtextCache : ICache
    {
        public SubtextCache(Cache cache) { 
            Cache = cache;
        }

        protected Cache Cache {
            get;
            set;
        }

        public object this[string key]
        {
            get {
                return Cache[key];
            }
            set {
                Cache[key] = value;
            }
        }

        public void Insert(string key, object value) {
            Cache.Insert(key, value);
        }

        public void Insert(string key, object value, CacheDependency dependency) {
            Cache.Insert(key, value, dependency);
        }

        public void Insert(string key, object value, CacheDependency dependency, System.DateTime absoluteExpiration, System.TimeSpan slidingExpiration) {
            Cache.Insert(key, value, dependency, absoluteExpiration, slidingExpiration);
        }

        public void Remove(string key) {
            Cache.Remove(key);
        }


        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public IEnumerator GetEnumerator()
        {
            return Cache.GetEnumerator();
        }
    }
}
