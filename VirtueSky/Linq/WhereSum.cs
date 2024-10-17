using System;
using System.Collections.Generic;


namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  ARRAYS  --------------------------------------------

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static int FilterSum(this int[] source, Func<int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += v;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static int FilterSum<T>(this T[] source, Func<T, bool> predicate, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += selector(v);
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static long FilterSum(this long[] source, Func<long, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += v;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static long FilterSum<T>(this T[] source, Func<T, bool> predicate, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += selector(v);
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static float FilterSum(this float[] source, Func<float, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;

            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += v;
                }
            }

            return (float)sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>        
        /// <returns>The sum of the transformed elements.</returns>
        public static float FilterSum<T>(this T[] source, Func<T, bool> predicate, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += selector(v);
                }
            }

            return (float)sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static double FilterSum(this double[] source, Func<double, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static double FilterSum<T>(this T[] source, Func<T, bool> predicate, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static decimal FilterSum(this decimal[] source, Func<decimal, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;

            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static decimal FilterSum<T>(this T[] source, Func<T, bool> predicate, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        // --------------------------  SPANS  --------------------------------------------

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static int FilterSum(this Span<int> source, Func<int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += v;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static int FilterSum<T>(this Span<T> source, Func<T, bool> predicate, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += selector(v);
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static long FilterSum(this Span<long> source, Func<long, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += v;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static long FilterSum<T>(this Span<T> source, Func<T, bool> predicate, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    if (predicate(v))
                    {
                        sum += selector(v);
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static float FilterSum(this Span<float> source, Func<float, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;

            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += v;
                }
            }

            return (float)sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>        
        /// <returns>The sum of the transformed elements.</returns>
        public static float FilterSum<T>(this Span<T> source, Func<T, bool> predicate, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += selector(v);
                }
            }

            return (float)sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static double FilterSum(this Span<double> source, Func<double, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static double FilterSum<T>(this Span<T> source, Func<T, bool> predicate, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static decimal FilterSum(this Span<decimal> source, Func<decimal, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;

            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static decimal FilterSum<T>(this Span<T> source, Func<T, bool> predicate, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal sum = 0;
            foreach (var v in source)
            {
                if (predicate(v))
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

#endif
        // --------------------------  LISTS  --------------------------------------------

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static int FilterSum(this List<int> source, Func<int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var s = source[i];
                    if (predicate(s))
                    {
                        sum += s;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static int FilterSum<T>(this List<T> source, Func<T, bool> predicate, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var s = source[i];
                    if (predicate(s))
                    {
                        sum += selector(s);
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static long FilterSum(this List<long> source, Func<long, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var s = source[i];
                    if (predicate(s))
                    {
                        sum += s;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static long FilterSum<T>(this List<T> source, Func<T, bool> predicate, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var s = source[i];
                    if (predicate(s))
                    {
                        sum += selector(s);
                    }
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static float FilterSum(this List<float> source, Func<float, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                var s = source[i];
                if (predicate(s))
                {
                    sum += s;
                }
            }

            return (float)sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static float FilterSum<T>(this List<T> source, Func<T, bool> predicate, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                var s = source[i];
                if (predicate(s))
                {
                    sum += selector(s);
                }
            }

            return (float)sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static double FilterSum(this List<double> source, Func<double, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                var s = source[i];
                if (predicate(s))
                {
                    sum += s;
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static double FilterSum<T>(this List<T> source, Func<T, bool> predicate, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                var s = source[i];
                if (predicate(s))
                {
                    sum += selector(s);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds the values in the sequence that match the where predicate.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <returns>The sum of the sequence.</returns>
        public static decimal FilterSum(this List<decimal> source, Func<decimal, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                var s = source[i];
                if (predicate(s))
                {
                    sum += s;
                }
            }

            return sum;
        }

        /// <summary>
        /// Performs a filter with the where predicate, then sums the transformed values.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="predicate">A function to filter the sequence with before summing.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static decimal FilterSum<T>(this List<T> source, Func<T, bool> predicate, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                var s = source[i];
                if (predicate(s))
                {
                    sum += selector(s);
                }
            }

            return sum;
        }
    }
}