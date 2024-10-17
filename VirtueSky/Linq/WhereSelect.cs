using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  ARRAYS --------------------------------------------


        /// <summary>
        /// Combined Where and Select for optimal performance.
        /// </summary>        
        /// <param name="source">The input sequence to filter then transform.</param>
        /// <param name="predicate">A function to use to filter the sequence.</param>
        /// <param name="selector">A function to transform the filtered elements.</param>
        /// <returns>A sequence of filtered and transformed elements.</returns>
        public static TResult[] FilterMap<T, TResult>(this T[] source, Func<T, bool> predicate, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new TResult[source.Length];
            int idx = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                {
                    result[idx] = selector(source[i]);
                    idx++;
                }
            }

            Array.Resize(ref result, idx);
            return result;
        }

        /// <summary>
        /// Combined Where and Select for optimal performance that uses the index in the 
        /// predicate and selector.
        /// </summary>        
        /// <param name="source">The input sequence to filter then transform.</param>
        /// <param name="predicate">A function to use to filter the sequence.</param>
        /// <param name="selector">A function to transform the filtered elements.</param>
        /// <returns>A sequence of filtered and transformed elements.</returns>
        public static TResult[] FilterMap<T, TResult>(this T[] source, Func<T, int, bool> predicate, Func<T, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new TResult[source.Length];
            int idx = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i], i))
                {
                    result[idx] = selector(source[i], idx);
                    idx++;
                }
            }

            Array.Resize(ref result, idx);
            return result;
        }


        // --------------------------  SPANS --------------------------------------------


#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Combined Where and Select for optimal performance.
        /// </summary>        
        /// <param name="source">The input sequence to filter then transform.</param>
        /// <param name="predicate">A function to use to filter the sequence.</param>
        /// <param name="selector">A function to transform the filtered elements.</param>
        /// <returns>A sequence of filtered and transformed elements.</returns>
        public static TResult[] FilterMap<T, TResult>(this Span<T> source, Func<T, bool> predicate, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new TResult[source.Length];
            int idx = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                {
                    result[idx] = selector(source[i]);
                    idx++;
                }
            }

            Array.Resize(ref result, idx);
            return result;
        }

        /// <summary>
        /// Combined Where and Select for optimal performance that uses the index in the 
        /// predicate and selector.
        /// </summary>        
        /// <param name="source">The input sequence to filter then transform.</param>
        /// <param name="predicate">A function to use to filter the sequence.</param>
        /// <param name="selector">A function to transform the filtered elements.</param>
        /// <returns>A sequence of filtered and transformed elements.</returns>
        public static TResult[] FilterMap<T, TResult>(this Span<T> source, Func<T, int, bool> predicate, Func<T, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new TResult[source.Length];
            int idx = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i], i))
                {
                    result[idx] = selector(source[i], idx);
                    idx++;
                }
            }

            Array.Resize(ref result, idx);
            return result;
        }
#endif


        // --------------------------  LISTS --------------------------------------------

        /// <summary>
        /// Combined Where and Select for optimal performance.
        /// </summary>        
        /// <param name="source">The input sequence to filter then transform.</param>
        /// <param name="predicate">A function to use to filter the sequence.</param>
        /// <param name="selector">A function to transform the filtered elements.</param>
        /// <returns>A sequence of filtered and transformed elements.</returns>
        public static List<TResult> FilterMap<T, TResult>(this List<T> source, Func<T, bool> predicate, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var r = new List<TResult>();
            for (int i = 0; i < source.Count; i++)
            {
                if (predicate(source[i])) r.Add(selector(source[i]));
            }

            return r;
        }

        /// <summary>
        /// Combined Where and Select for optimal performance that uses the index in the 
        /// predicate and selector.
        /// </summary>        
        /// <param name="source">The input sequence to filter then transform.</param>
        /// <param name="predicate">A function to use to filter the sequence.</param>
        /// <param name="selector">A function to transform the filtered elements.</param>
        /// <returns>A sequence of filtered and transformed elements.</returns>
        public static List<TResult> FilterMap<T, TResult>(this List<T> source, Func<T, int, bool> predicate, Func<T, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var r = new List<TResult>();
            int idx = 0;
            for (int i = 0; i < source.Count; i++)
            {
                if (predicate(source[i], i))
                {
                    r.Add(selector(source[i], idx));
                    idx++;
                }
            }


            return r;
        }
    }
}