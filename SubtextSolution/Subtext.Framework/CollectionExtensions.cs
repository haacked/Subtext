using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Subtext.Framework
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> elements) {
            elements.ForEach(item => source.Add(item));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source) {
                action(item);
            }
        }

        public static bool GetBoolean(this NameValueCollection source, string name) {
            bool result;
            if(bool.TryParse(source[name], out result)) {
                return result;
            }
            return false;
        }

        public static EnumType GetEnum<EnumType>(this NameValueCollection source, string name)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), source[name], true /* ignoreCase */);
        }
    }
}
