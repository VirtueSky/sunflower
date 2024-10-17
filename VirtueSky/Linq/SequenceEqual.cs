using System.Collections.Generic;
using System;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>     
        public static bool SequenceEqual<T>(this T[] first, T[] second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Length != second.Length) return false;
            if (first == second) return true;

            for (int i = 0; i < first.Length; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>An array of integers, where the value corresponds to IComparer.Compare indicating less than, greater than, or equals</returns>     
        public static int[] SequenceCompare<T>(this T[] first, T[] second, IComparer<T> comparer = null)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (comparer == null)
            {
                comparer = Comparer<T>.Default;
            }

            if (first.Length != second.Length) throw new NotSupportedException();

            var result = new int[first.Length];
            for (int i = 0; i < first.Length; i++)
            {
                result[i] = comparer.Compare(first[i], second[i]);
            }

            return result;
        }


        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>
        public static bool SequenceEqual<T>(this T[] first, List<T> second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Length != second.Count) return false;

            for (int i = 0; i < first.Length; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>
        public static bool SequenceEqual<T>(this List<T> first, T[] second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Count != second.Length) return false;

            for (int i = 0; i < first.Count; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }

        /*---- Spans ----*/
#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>     
        public static bool SequenceEqual<T>(this Span<T> first, Span<T> second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Length != second.Length) return false;
            if (first == second) return true;

            for (int i = 0; i < first.Length; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }


        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>
        public static bool SequenceEqual<T>(this Span<T> first, List<T> second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Length != second.Count) return false;

            for (int i = 0; i < first.Length; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>
        public static bool SequenceEqual<T>(this List<T> first, Span<T> second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Count != second.Length) return false;

            for (int i = 0; i < first.Count; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }
#endif

        /* ------------ List ---------------- */

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements by using the 
        /// provided comparer or the default equality comparer for their type if none is provided.
        /// </summary>        
        /// <param name="first">A sequence to compare to second.</param>
        /// <param name="second">A sequence to compare to first.</param>
        /// <param name="comparer">An optional Comparer to use for the comparison.</param>
        /// <returns>true of the two sources are of equal length and their corresponding 
        /// elements are equal according to the equality comparer. Otherwise, false.</returns>
        public static bool SequenceEqual<T>(this List<T> first, List<T> second, IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<T>.Default;
            }

            if (first == null) throw new ArgumentNullException(nameof(first));

            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.Count != second.Count) return false;
            if (first == second) return true;

            for (int i = 0; i < first.Count; i++)
            {
                if (!comparer.Equals(first[i], second[i])) return false;
            }

            return true;
        }
    }
}