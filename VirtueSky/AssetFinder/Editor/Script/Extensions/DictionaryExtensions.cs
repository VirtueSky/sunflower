using System;
using System.Collections.Generic;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class DictionaryExtensions
    {
        #if UNITY_2021_2_OR_NEWER
        // GetValueOrDefault exists in newer Unity versions
        #else
        internal static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
        }
        #endif
        
        internal static TValue TryGetValueOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary.TryGetValue(key, out TValue value))
                return value;
            
            dictionary[key] = defaultValue;
            return defaultValue;
        }
    }
} 