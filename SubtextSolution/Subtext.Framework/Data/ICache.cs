using System;
using System.Collections;
using System.Web.Caching;

namespace Subtext.Framework.Data
{
    public interface ICache : IEnumerable
    {
        object this[string key] { get; set; }
        void Insert(string key, object value);
        void Insert(string key, object value, CacheDependency dependency);
        void Insert(string key, object value, CacheDependency dependency, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);
        void Remove(string key);
    }
}
