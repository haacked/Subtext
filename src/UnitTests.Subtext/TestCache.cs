using System;
using System.Collections.Specialized;
using System.Web.Caching;
using Subtext.Infrastructure;

namespace UnitTests.Subtext
{
    internal class TestCache : NameObjectCollectionBase, ICache
    {
        public object this[string key]
        {
            get { return BaseGet(key); }
            set { BaseSet(key, value); }
        }

        public void Insert(string key, object value, CacheDependency dependency)
        {
            this[key] = value;
        }

        public void Insert(string key, object value, CacheDependency dependency, DateTime absoluteExpiration,
                           TimeSpan slidingExpiration)
        {
            this[key] = value;
        }

        public void Remove(string key)
        {
            BaseRemove(key);
        }

        public void Insert(string key, object value)
        {
            this[key] = value;
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
                           TimeSpan slidingExpiration, CacheItemPriority priority,
                           CacheItemRemovedCallback onRemoveCallback)
        {
            this[key] = value;
        }
    }
}