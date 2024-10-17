using System;
using System.Collections.Generic;

namespace VirtueSky.Linq
{
    public static partial class L
    {
        // --------------------------  ARRAYS  --------------------------------------------

        /// <summary>
        /// Splits the given sequence into chunks of the given size.
        /// If the sequence length isn't evenly divisible by the chunk size,
        /// the last chunk will contain all remaining elements.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource[][] Chunk<TSource>(this TSource[] source, int chunkSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));

            int size = source.Length / chunkSize;
            int extraSize = source.Length % chunkSize;
            if (extraSize != 0) size++;
            var result = new TSource[size][];
            int currentIndex = 0;
            int indexChunk = 0;
            if (size != 0)
            {
                foreach (var element in source)
                {
                    if (result[indexChunk] == null)
                    {
                        if (extraSize != 0 && indexChunk == size - 1)
                        {
                            result[indexChunk] = new TSource[extraSize];
                        }
                        else
                        {
                            result[indexChunk] = new TSource[chunkSize];
                        }
                    }

                    result[indexChunk][currentIndex++] = element;

                    if (currentIndex == chunkSize)
                    {
                        indexChunk++;
                        currentIndex = 0;
                    }
                }
            }

            return result;
        }


        // --------------------------  LISTS  --------------------------------------------

        /// <summary>
        /// Splits the given sequence into chunks of the given size.
        /// If the sequence length isn't evenly divisible by the chunk size,
        /// the last chunk will contain all remaining elements.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static List<List<TSource>> Chunk<TSource>(this List<TSource> source, int chunkSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));

            int size = source.Count / chunkSize;
            int extraSize = source.Count % chunkSize;
            if (extraSize != 0) size++;
            var result = new List<List<TSource>>(size);
            var currentIndex = 0;
            var indexChunk = 0;
            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    result.Add(new List<TSource>());
                }

                foreach (var element in source)
                {
                    result[indexChunk] = result[indexChunk] ?? new List<TSource>();
                    result[indexChunk].Add(element);
                    currentIndex++;

                    if (currentIndex == chunkSize)
                    {
                        indexChunk++;
                        currentIndex = 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Splits the given sequence into chunks of the given size.
        /// If the sequence length isn't evenly divisible by the chunk size,
        /// the last chunk will contain all remaining elements.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource[][] ChunkToArray<TSource>(this List<TSource> source, int chunkSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (chunkSize <= 0) throw new ArgumentOutOfRangeException(nameof(chunkSize));

            int size = source.Count / chunkSize;
            int extraSize = source.Count % chunkSize;
            if (extraSize != 0) size++;
            var result = new TSource[size][];
            int currentIndex = 0;
            int indexChunk = 0;
            if (size != 0)
            {
                foreach (var element in source)
                {
                    if (result[indexChunk] == null)
                    {
                        if (extraSize != 0 && indexChunk == size - 1)
                        {
                            result[indexChunk] = new TSource[extraSize];
                        }
                        else
                        {
                            result[indexChunk] = new TSource[chunkSize];
                        }
                    }

                    result[indexChunk][currentIndex++] = element;

                    if (currentIndex == chunkSize)
                    {
                        indexChunk++;
                        currentIndex = 0;
                    }
                }
            }

            return result;
        }
    }
}