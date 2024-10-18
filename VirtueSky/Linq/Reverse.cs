using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>        
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        public static T[] Reverse<T>(this T[] source)
        {
            var result = new T[source.Length];
            int lenLessOne = source.Length - 1;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = source[lenLessOne - i];
            }

            return result;
        }


        /// <summary>
        /// Inverts the order of the elements in a sequence in place.
        /// The result will change itself <paramref name = "source" />
        /// </summary>        
        /// <param name="source">A sequence of values to reverse.</param>        
        public static void ReverseOrigin<T>(this T[] source)
        {
            Array.Reverse(source);
        }


#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>        
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        public static T[] Reverse<T>(this Span<T> source)
        {
            var result = new T[source.Length];
            int lenLessOne = source.Length - 1;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = source[lenLessOne - i];
            }

            return result;
        }

        /// <summary>
        /// Inverts the order of the elements in a sequence in place.
        /// The result will change itself <paramref name = "source" />
        /// </summary>        
        /// <param name="source">A sequence of values to reverse.</param>        
        public static void ReverseOrigin<T>(this Span<T> source)
        {
            MemoryExtensions.Reverse(source);
        }
#endif

        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>        
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        public static List<T> Reverse<T>(this List<T> source)
        {
            var result = new List<T>(source.Count);
            for (int i = source.Count - 1; i >= 0; i--)
            {
                result.Add(source[i]);
            }

            return result;
        }

        /// <summary>
        /// Inverts the order of the elements in a sequence in place.
        /// The result will change itself <paramref name = "source" />
        /// </summary>        
        /// <param name="source">A sequence of values to reverse.</param>        
        public static void ReverseOrigin<T>(this List<T> source)
        {
            source.Reverse();
        }
    }
}