using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        /// <summary>
        /// Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="selector">A function that specifies how to merge the elements from the two sequences.</param>
        /// <returns>A sequence that contains merged elements of two input sequences.</returns>
        public static TR[] ZipF<T, TU, TR>(this T[] first, TU[] second, Func<T, TU, TR> selector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            //maintain array bounds elision
            if (first.Length < second.Length)
            {
                var result = new TR[first.Length];
                for (int i = 0; i < first.Length; i++)
                {
                    result[i] = selector(first[i], second[i]);
                }

                return result;
            }
            else
            {
                var result = new TR[second.Length];
                for (int i = 0; i < second.Length; i++)
                {
                    result[i] = selector(first[i], second[i]);
                }

                return result;
            }
        }

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="selector">A function that specifies how to merge the elements from the two sequences.</param>
        /// <returns>A sequence that contains merged elements of two input sequences.</returns>
        public static TR[] ZipF<T, TU, TR>(this Span<T> first, Span<TU> second, Func<T, TU, TR> selector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            //maintain array bounds elision
            if (first.Length < second.Length)
            {
                var result = new TR[first.Length];
                for (int i = 0; i < first.Length; i++)
                {
                    result[i] = selector(first[i], second[i]);
                }

                return result;
            }
            else
            {
                var result = new TR[second.Length];
                for (int i = 0; i < second.Length; i++)
                {
                    result[i] = selector(first[i], second[i]);
                }

                return result;
            }
        }
#endif

        /// <summary>
        /// Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="selector">A function that specifies how to merge the elements from the two sequences.</param>
        /// <returns>A sequence that contains merged elements of two input sequences.</returns>
        public static List<TR> ZipF<T, TU, TR>(this List<T> first, List<TU> second, Func<T, TU, TR> selector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            //maintain array bounds elision
            if (first.Count < second.Count)
            {
                var result = new List<TR>(first.Count);
                for (int i = 0; i < first.Count; i++)
                {
                    result.Add(selector(first[i], second[i]));
                }

                return result;
            }
            else
            {
                var result = new List<TR>(second.Count);
                for (int i = 0; i < second.Count; i++)
                {
                    result.Add(selector(first[i], second[i]));
                }

                return result;
            }
        }
    }
}