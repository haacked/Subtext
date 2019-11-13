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
    public interface ICache : IEnumerable
    {
        object this[string key] { get; set; }
        void Insert(string key, object value);
        void Insert(string key, object value, CacheDependency dependency);

        void Insert(string key, object value, CacheDependency dependency, DateTime absoluteExpiration,
                    TimeSpan slidingExpiration);

        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
                    TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);

        void Remove(string key);
    }
}