using System;
using System.Collections.Generic;
using System.Text;

namespace Vt.Platform.Utils
{
    public static class DictionaryExtensions
    {
        public static T GetValueOrDefault<T, TKey>(this IDictionary<TKey, T> dict, TKey key)
        {
            if (dict.TryGetValue(key, out var output))
            {
                return output;
            }
            return default(T);
        }
    }
}
