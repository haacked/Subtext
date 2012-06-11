#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Web.Caching;

namespace Subtext.Infrastructure
{
    public class SubtextCache : ICache
    {
        public SubtextCache(Cache cache)
        {
            Cache = cache;
        }

        protected Cache Cache { get; set; }

        public object this[string key]
        {
            get { return Cache.Get(key); }
            set { Cache[key] = value; }
        }

        public void Insert(string key, object value)
        {
            Cache.Insert(key, value);
        }

        public void Insert(string key, object value, CacheDependency dependency)
        {
            Cache.Insert(key, value, dependency);
        }

        public void Insert(string key, object value, CacheDependency dependency, DateTime absoluteExpiration,
                           TimeSpan slidingExpiration)
        {
            Cache.Insert(key, value, dependency, absoluteExpiration, slidingExpiration);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
                           TimeSpan slidingExpiration, CacheItemPriority priority,
                           CacheItemRemovedCallback onRemoveCallback)
        {
            Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public IEnumerator GetEnumerator()
        {
            return Cache.GetEnumerator();
        }
    }
}