using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// Unlike standard Linq NOT a stable sort.
        /// </summary>        
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">A Comparer to compare keys.</param>
        /// <returns>A sequence whose elements are ordered according to a key</returns>
        public static TSource[] OrderBy<TSource, TKey>(this TSource[] source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }

            var keys = new TKey[source.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = keySelector(source[i]);
            }

            var result = (TSource[])source.Clone();
            Array.Sort(keys, result, comparer);
            return result;
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// Unlike standard Linq NOT a stable sort.
        /// </summary>        
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">A Comparer to compare keys.</param>
        /// <returns>A sequence whose elements are ordered according to a key</returns>
        public static TSource[] OrderByDescending<TSource, TKey>(this TSource[] source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }

            var keys = new TKey[source.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = keySelector(source[i]);
            }

            var result = (TSource[])source.Clone();
            Array.Sort(keys, result, comparer.Reverse());
            return result;
        }


        // ---------------------- Lists

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// Unlike standard Linq NOT a stable sort.
        /// </summary>        
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">A Comparer to compare keys.</param>
        /// <returns>A sequence whose elements are ordered according to a key</returns>
        public static List<TSource> OrderBy<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }

            var result = new List<TSource>(source);
            var lambdaComparer = new LambdaComparer<TSource, TKey>(keySelector, comparer);
            result.Sort(lambdaComparer);
            return result;
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// Unlike standard Linq NOT a stable sort.
        /// </summary>        
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">A Comparer to compare keys.</param>
        /// <returns>A sequence whose elements are ordered according to a key</returns>
        public static List<TSource> OrderByDescending<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }

            var result = new List<TSource>(source);
            var lambdaComparer = new ReverseLambdaComparer<TSource, TKey>(keySelector, comparer);
            result.Sort(lambdaComparer);
            return result;
        }
    }
}