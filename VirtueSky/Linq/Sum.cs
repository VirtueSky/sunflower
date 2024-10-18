using System;
using System.Collections.Generic;


namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  ARRAYS  --------------------------------------------

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static int Sum(this int[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static int Sum<T>(this T[] source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static long Sum(this long[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static long Sum<T>(this T[] source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static float Sum(this float[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;

            foreach (var v in source)
            {
                sum += v;
            }

            return (float)sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static float Sum<T>(this T[] source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                sum += selector(v);
            }

            return (float)sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static double Sum(this double[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (var v in source)
            {
                sum += v;
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static double Sum<T>(this T[] source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                sum += selector(v);
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static decimal Sum(this decimal[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;

            foreach (var v in source)
            {
                sum += v;
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static decimal Sum<T>(this T[] source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal sum = 0;
            foreach (var v in source)
            {
                sum += selector(v);
            }

            return sum;
        }

        /*---- Spans ---*/

#if UNITY_2021_3_OR_NEWER
        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static int Sum(this Span<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static int Sum<T>(this Span<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static long Sum(this Span<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static long Sum<T>(this Span<T> source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                foreach (var v in source)
                {
                    sum += selector(v);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static float Sum(this Span<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;

            foreach (var v in source)
            {
                sum += v;
            }

            return (float)sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static float Sum<T>(this Span<T> source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                sum += selector(v);
            }

            return (float)sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static double Sum(this Span<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            foreach (var v in source)
            {
                sum += v;
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static double Sum<T>(this Span<T> source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            foreach (var v in source)
            {
                sum += selector(v);
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static decimal Sum(this Span<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;

            foreach (var v in source)
            {
                sum += v;
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static decimal Sum<T>(this Span<T> source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal sum = 0;
            foreach (var v in source)
            {
                sum += selector(v);
            }

            return sum;
        }
#endif

        // --------------------------  LISTS  --------------------------------------------

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static int Sum(this List<int> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            int sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += source[i];
                }
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static int Sum<T>(this List<T> source, Func<T, int> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            int sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static long Sum(this List<long> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += source[i];
                }
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static long Sum<T>(this List<T> source, Func<T, long> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            long sum = 0;
            checked
            {
                for (int i = 0; i < source.Count; i++)
                {
                    sum += selector(source[i]);
                }
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static float Sum(this List<float> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += source[i];
            }

            return (float)sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static float Sum<T>(this List<T> source, Func<T, float> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                sum += selector(source[i]);
            }

            return (float)sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static double Sum(this List<double> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            double sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                sum += source[i];
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static double Sum<T>(this List<T> source, Func<T, double> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            double sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        /// <summary>
        ///  Adds a sequence of values.
        /// </summary>
        /// <param name="source">The sequence to add.</param>
        /// <returns>The sum of the sequence.</returns>
        public static decimal Sum(this List<decimal> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            decimal sum = 0;

            for (int i = 0; i < source.Count; i++)
            {
                sum += source[i];
            }

            return sum;
        }

        /// <summary>
        /// Adds the transformed sequence of elements.
        /// </summary>        
        /// <param name="source">The sequence of values to transform then sum.</param>
        /// <param name="selector">A transformation function.</param>
        /// <returns>The sum of the transformed elements.</returns>
        public static decimal Sum<T>(this List<T> source, Func<T, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (selector == null) throw new ArgumentNullException(nameof(selector));

            decimal sum = 0;
            for (int i = 0; i < source.Count; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }
    }
}