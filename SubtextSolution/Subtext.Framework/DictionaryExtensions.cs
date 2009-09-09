using System.Collections.Generic;

namespace Subtext.Framework
{
    public static class DictionaryExtensions
    {
        public static V ItemOrNull<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            V value;
            if(!dictionary.TryGetValue(key, out value))
            {
                return default(V);
            }
            return value;
        }
    }
}