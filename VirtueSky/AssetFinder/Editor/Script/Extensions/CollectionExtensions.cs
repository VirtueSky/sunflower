using System.Collections.Generic;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class CollectionExtensions
    {
        internal static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
        {
            var result = new HashSet<T>();
            if (collection == null) return result;

            foreach (T item in collection)
            {
                result.Add(item);
            }
            return result;
        }
    }
} 