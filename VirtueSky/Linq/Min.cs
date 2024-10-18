using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    //int, long, float,double, decimal
    public static partial class L
    {
        // --------------------------  ARRAYS  --------------------------------------------

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static T Min<T>(this T[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            Comparer<T> comparer = Comparer<T>.Default;
            T r = default(T);
            if (r == null)
            {
                r = source[0];
                for (int i = 1; i < source.Length; i++)
                {
                    if (source[i] != null && comparer.Compare(source[i], r) < 0) r = source[i];
                }
            }
            else
            {
                r = source[0];
                for (int i = 1; i < source.Length; i++)
                {
                    if (comparer.Compare(source[i], r) < 0) r = source[i];
                }
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static TResult Min<T, TResult>(this T[] source, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            Comparer<TResult> comparer = Comparer<TResult>.Default;
            TResult r = default(TResult);
            if (r == null)
            {
                r = selector(source[0]);
                for (int i = 1; i < source.Length; i++)
                {
                    var v = selector(source[i]);
                    if (v != null && comparer.Compare(v, r) < 0) r = v;
                }
            }
            else
            {
                r = selector(source[0]);
                for (int i = 1; i < source.Length; i++)
                {
                    var v = selector(source[i]);
                    if (comparer.Compare(v, r) < 0) r = v;
                }
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static int Min(this int[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            int r = int.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static int Min<T>(this T[] source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int r = int.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static long Min(this long[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long r = long.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static long Min<T>(this T[] source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long r = long.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static float Min(this float[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            float r = float.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
                else if (float.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static float Min<T>(this T[] source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            float r = float.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
                else if (float.IsNaN(v)) return v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static double Min(this double[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double r = double.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
                else if (double.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static double Min<T>(this T[] source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double r = double.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
                else if (double.IsNaN(v)) return v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static decimal Min(this decimal[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal r = decimal.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static decimal Min<T>(this T[] source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal r = decimal.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }
        // --------------------------  this Spans  --------------------------------------------

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static T Min<T>(this Span<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            Comparer<T> comparer = Comparer<T>.Default;
            T r = default(T);
            if (r == null)
            {
                r = source[0];
                for (int i = 1; i < source.Length; i++)
                {
                    if (source[i] != null && comparer.Compare(source[i], r) < 0) r = source[i];
                }
            }
            else
            {
                r = source[0];
                for (int i = 1; i < source.Length; i++)
                {
                    if (comparer.Compare(source[i], r) < 0) r = source[i];
                }
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static TResult Min<T, TResult>(this Span<T> source, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            Comparer<TResult> comparer = Comparer<TResult>.Default;
            TResult r = default(TResult);
            if (r == null)
            {
                r = selector(source[0]);
                for (int i = 1; i < source.Length; i++)
                {
                    var v = selector(source[i]);
                    if (v != null && comparer.Compare(v, r) < 0) r = v;
                }
            }
            else
            {
                r = selector(source[0]);
                for (int i = 1; i < source.Length; i++)
                {
                    var v = selector(source[i]);
                    if (comparer.Compare(v, r) < 0) r = v;
                }
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static int Min(this Span<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            int r = int.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static int Min<T>(this Span<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int r = int.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static long Min(this Span<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long r = long.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static long Min<T>(this Span<T> source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long r = long.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static float Min(this Span<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            float r = float.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
                else if (float.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static float Min<T>(this Span<T> source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            float r = float.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
                else if (float.IsNaN(v)) return v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static double Min(this Span<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double r = double.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
                else if (double.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static double Min<T>(this Span<T> source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double r = double.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
                else if (double.IsNaN(v)) return v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static decimal Min(this Span<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal r = decimal.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static decimal Min<T>(this Span<T> source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal r = decimal.MaxValue;
            for (int i = 0; i < source.Length; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }
#endif

        // --------------------------  LISTS  --------------------------------------------

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static T Min<T>(this List<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            Comparer<T> comparer = Comparer<T>.Default;
            T r = default(T);
            if (r == null)
            {
                r = source[0];
                for (int i = 1; i < source.Count; i++)
                {
                    if (source[i] != null && comparer.Compare(source[i], r) < 0) r = source[i];
                }
            }
            else
            {
                r = source[0];
                for (int i = 1; i < source.Count; i++)
                {
                    if (comparer.Compare(source[i], r) < 0) r = source[i];
                }
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static int Min(this List<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            int r = int.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static int Min<T>(this List<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int r = int.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static long Min(this List<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            long r = long.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static long Min<T>(this List<T> source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long r = long.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static float Min(this List<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            float r = float.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < r) r = source[i];
                else if (float.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static float Min<T>(this List<T> source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            float r = float.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
                else if (float.IsNaN(v)) return v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static double Min(this List<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            double r = double.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < r) r = source[i];
                else if (double.IsNaN(source[i])) return source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static double Min<T>(this List<T> source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double r = double.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
                else if (double.IsNaN(v)) return v;
            }

            return r;
        }

        /// <summary>
        /// Returns the minimum value in a sequence of values.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <returns>The minimum value in the sequence</returns>
        public static decimal Min(this List<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal r = decimal.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] < r) r = source[i];
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static decimal Min<T>(this List<T> source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            decimal r = decimal.MaxValue;
            for (int i = 0; i < source.Count; i++)
            {
                var v = selector(source[i]);
                if (v < r) r = v;
            }

            return r;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>        
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The maximum value in the transform of the sequence.</returns>
        public static TResult Min<T, TResult>(this List<T> source, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            if (source.Count == 0) throw new InvalidOperationException("Source sequence doesn't contain any elements.");

            Comparer<TResult> comparer = Comparer<TResult>.Default;
            TResult r = default(TResult);
            if (r == null)
            {
                r = selector(source[0]);
                for (int i = 1; i < source.Count; i++)
                {
                    var v = selector(source[i]);
                    if (v != null && comparer.Compare(v, r) < 0) r = v;
                }
            }
            else
            {
                r = selector(source[0]);
                for (int i = 1; i < source.Count; i++)
                {
                    var v = selector(source[i]);
                    if (comparer.Compare(v, r) < 0) r = v;
                }
            }

            return r;
        }
    }
}