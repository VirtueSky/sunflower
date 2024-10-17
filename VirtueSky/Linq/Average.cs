using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  ARRAYS  --------------------------------------------

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static double Average(this int[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += source[i];
                }
            }

            return (double)sum / source.Length;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this T[] source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return (double)sum / source.Length;
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static double Average(this long[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += source[i];
                }
            }

            return (double)sum / source.Length;
        }


        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this T[] source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return (double)sum / source.Length;
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static float Average(this float[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return (float)(sum / source.Length);
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static float Average<T>(this T[] source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return (float)(sum / source.Length);
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static double Average(this double[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum / source.Length;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this T[] source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum / source.Length;
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static decimal Average(this decimal[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum / source.Length;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static decimal Average<T>(this T[] source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));


            decimal sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum / source.Length;
        }

        // --------------------------  this SpanS  --------------------------------------------

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static double Average(this Span<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += source[i];
                }
            }

            return (double)sum / source.Length;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this Span<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return (double)sum / source.Length;
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static double Average(this Span<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += source[i];
                }
            }

            return (double)sum / source.Length;
        }


        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this Span<T> source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Length; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return (double)sum / source.Length;
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static float Average(this Span<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return (float)(sum / source.Length);
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static float Average<T>(this Span<T> source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return (float)(sum / source.Length);
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static double Average(this Span<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum / source.Length;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this Span<T> source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum / source.Length;
        }

        /// <summary>
        /// Computes the average of an array
        /// </summary>
        /// <param name="source">The array to calculate the average of.</param>
        /// <returns>The average of the array.</returns>
        public static decimal Average(this Span<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum / source.Length;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static decimal Average<T>(this Span<T> source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));


            decimal sum = 0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum / source.Length;
        }
#endif

        // --------------------------  Lists  --------------------------------------------

        /// <summary>
        /// Computes the average of a list.
        /// </summary>
        /// <param name="source">The list to calculate the average of.</param>
        /// <returns>The average of the list.</returns>
        public static double Average(this List<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += source[i];
                }
            }

            return (double)sum / source.Count;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this List<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return (double)sum / source.Count;
        }

        /// <summary>
        /// Computes the average of a list.
        /// </summary>
        /// <param name="source">The list to calculate the average of.</param>
        /// <returns>The average of the list.</returns>
        public static double Average(this List<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += source[i];
                }
            }

            return (double)sum / source.Count;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this List<T> source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return (double)sum / source.Count;
        }

        /// <summary>
        /// Computes the average of a list.
        /// </summary>
        /// <param name="source">The list to calculate the average of.</param>
        /// <returns>The average of the list.</returns>
        public static float Average(this List<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += source[i];
            }

            return (float)(sum / source.Count);
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static float Average<T>(this List<T> source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += selector(source[i]);
            }

            return (float)(sum / source.Count);
        }

        /// <summary>
        /// Computes the average of a list.
        /// </summary>
        /// <param name="source">The list to calculate the average of.</param>
        /// <returns>The average of the list.</returns>
        public static double Average(this List<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += source[i];
            }

            return sum / source.Count;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static double Average<T>(this List<T> source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += selector(source[i]);
            }

            return sum / source.Count;
        }

        /// <summary>
        /// Computes the average of a list.
        /// </summary>
        /// <param name="source">The list to calculate the average of.</param>
        /// <returns>The average of the list.</returns>
        public static decimal Average(this List<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += source[i];
            }

            return sum / source.Count;
        }

        /// <summary>
        /// Computes the average of values obtained by invoking a transform function on
        /// each element of the input array.
        /// </summary>
        /// <param name="source">The array to calculate the transformed average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The average of the array.</returns>
        public static decimal Average<T>(this List<T> source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));


            decimal sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += selector(source[i]);
            }

            return sum / source.Count;
        }
    }
}