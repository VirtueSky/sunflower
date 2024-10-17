using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  ARRAYS  --------------------------------------------
        /// <summary>
        /// Returns a flattened sequence that contains the concatenation of all the nested sequences' elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of sequences to be flattened.</param>
        /// <returns>The concatenation of all the nested sequences' elements.</returns>
        public static TSource[] Flatten<TSource>(this TSource[][] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = new List<TSource>();

            foreach (var array in source)
            {
                result.AddRange(array);
            }

            return result.ToArray();
        }


        // --------------------------  LISTS  --------------------------------------------

        // --------------------------  ARRAYS  --------------------------------------------
        /// <summary>
        /// Returns a flattened sequence that contains the concatenation of all the nested sequences' elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of sequences to be flattened.</param>
        /// <returns>The concatenation of all the nested sequences' elements.</returns>
        public static List<TSource> Flatten<TSource>(this List<List<TSource>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = new List<TSource>();

            foreach (var array in source)
            {
                result.AddRange(array);
            }

            return result;
        }

        /// <summary>
        /// Returns a flattened sequence that contains the concatenation of all the nested sequences' elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of sequences to be flattened.</param>
        /// <returns>The concatenation of all the nested sequences' elements.</returns>
        public static List<TSource> Flatten<TSource>(this List<TSource[]> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = new List<TSource>();

            foreach (var array in source)
            {
                result.AddRange(array);
            }

            return result;
        }
    }
}