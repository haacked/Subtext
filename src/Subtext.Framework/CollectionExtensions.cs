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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Subtext.Framework
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || !items.Any();
        }

        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> elements)
        {
            elements.ForEach(source.Add);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        public static bool GetBoolean(this NameValueCollection source, string name)
        {
            bool result;
            return bool.TryParse(source[name], out result) && result;
        }

        public static TEnumType GetEnum<TEnumType>(this NameValueCollection source, string name)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), source[name], true /* ignoreCase */);
        }

        public static void Accumulate<TContainer, TKey, TItem>(this IEnumerable<TContainer> containers, IEnumerable<TItem> items, Func<TContainer, TKey> keySelector, Func<TItem, TKey> itemKeySelector, Action<TContainer, TItem> accumulator)
        {
            // Assumes that items are sorted by item key in the order corresponding to container key
            var groupedItems = from item in items
                               group item by itemKeySelector(item) into groupedByKey
                               select groupedByKey;

            foreach (var itemGroup in groupedItems)
            {
                foreach (var container in containers)
                {
                    if (!keySelector(container).Equals(itemGroup.Key))
                    {
                        continue;
                    }
                    foreach (var item in itemGroup)
                    {
                        accumulator(container, item);
                    }
                }
            }
        }

    }
}