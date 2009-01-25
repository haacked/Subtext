using System.Web.Caching;
using System;

namespace Subtext.Framework.Data
{
    public interface ICache
    {
        object this[string key] { get; set; }
        void Insert(string key, object value, CacheDependency dependency);
        void Insert(string key, object value, CacheDependency dependency, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        void Remove(string key);
    }
}
