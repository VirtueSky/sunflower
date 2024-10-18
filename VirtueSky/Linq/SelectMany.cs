using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  Arrays --------------------------------------------

        /// <summary>
        /// Projects each element of a sequence to another sequence and flattens the resulting sequences into one sequence.
        /// Yo dawg, I heard you like sequences.
        /// </summary>                    
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        public static TResult[] SelectMany<TSource, TResult>(this TSource[] source, Func<TSource, TResult[]> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new List<TResult>(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                var va = selector(source[i]);
                for (int j = 0; j < va.Length; j++)
                {
                    result.Add(va[j]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Projects each element of a sequence to another sequence and flattens the resulting sequences into one sequence
        /// utilizing the index of each element.
        /// </summary>                    
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element and it's index.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element and index of the input sequence.</returns>
        public static TResult[] SelectMany<TSource, TResult>(this TSource[] source, Func<TSource, int, TResult[]> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new List<TResult>(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                var va = selector(source[i], i);
                for (int j = 0; j < va.Length; j++)
                {
                    result.Add(va[j]);
                }
            }

            return result.ToArray();
        }

        // --------------------------  this Spans --------------------------------------------

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Projects each element of a sequence to another sequence and flattens the resulting sequences into one sequence.
        /// Yo dawg, I heard you like sequences.
        /// </summary>                    
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        public static TResult[] SelectMany<TSource, TResult>(this Span<TSource> source, Func<TSource, TResult[]> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new List<TResult>(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                var va = selector(source[i]);
                for (int j = 0; j < va.Length; j++)
                {
                    result.Add(va[j]);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Projects each element of a sequence to another sequence and flattens the resulting sequences into one sequence
        /// utilizing the index of each element.
        /// </summary>                    
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element and it's index.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element and index of the input sequence.</returns>
        public static TResult[] SelectMany<TSource, TResult>(this Span<TSource> source, Func<TSource, int, TResult[]> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new List<TResult>(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                var va = selector(source[i], i);
                for (int j = 0; j < va.Length; j++)
                {
                    result.Add(va[j]);
                }
            }

            return result.ToArray();
        }
#endif
        // --------------------------  LISTS --------------------------------------------

        /// <summary>
        /// Projects each element of a sequence to another sequence and flattens the resulting sequences into one sequence.
        /// Yo dawg, I heard you like sequences.
        /// </summary>                    
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        public static List<TResult> SelectMany<TSource, TResult>(this List<TSource> source, Func<TSource, List<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new List<TResult>(source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                var va = selector(source[i]);
                for (int j = 0; j < va.Count; j++)
                {
                    result.Add(va[j]);
                }
            }

            return result;
        }

        /// <summary>
        /// Projects each element of a sequence to another sequence and flattens the resulting sequences into one sequence
        /// utilizing the index of each element.
        /// </summary>                    
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element and it's index.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element and index of the input sequence.</returns>
        public static List<TResult> SelectMany<TSource, TResult>(this List<TSource> source, Func<TSource, int, List<TResult>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            var result = new List<TResult>(source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                var va = selector(source[i], i);
                for (int j = 0; j < va.Count; j++)
                {
                    result.Add(va[j]);
                }
            }

            return result;
        }
    }
}