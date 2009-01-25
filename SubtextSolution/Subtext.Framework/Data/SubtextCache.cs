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
        
        public void Insert(string key, object value, CacheDependency dependency) {
            Cache.Insert(key, value, dependency);
            
        }


        public void Insert(string key, object value, CacheDependency dependency, System.DateTime absoluteExpiration, System.TimeSpan slidingExpiration) {
            Cache.Insert(key, value, dependency, absoluteExpiration, slidingExpiration);
        }

        public void Remove(string key) {
            Cache.Remove(key);
        }
    }
}
