using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VirtueSky.Misc
{
    public partial class Common
    {
        public static void Clear<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            Array.Clear(collection, 0, collection.Length);
        }

        #region IsNullOrEmpty

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(this List<T> source)
        {
            return source == null || source.Count == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(this T[] source)
        {
            return source == null || source.Length == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            return source == null || source.Keys.Count == 0;
        }

        #endregion

        #region Shuffle

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this T[] source)
        {
            int n = source.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (source[k], source[n]) = (source[n], source[k]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this List<T> source)
        {
            int n = source.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (source[k], source[n]) = (source[n], source[k]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<T1, T2> Shuffle<T1, T2>(this IDictionary<T1, T2> source)
        {
            var keys = source.Keys.ToArray();
            var values = source.Values.ToArray();

            int n = source.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (keys[k], keys[n]) = (keys[n], keys[k]);
                (values[k], values[n]) = (values[n], values[k]);
            }

            return MakeDictionary(keys, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this TKey[] keys, TValue[] values)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keys.Length != values.Length) throw new ArgumentException("Size keys and size values diffirent!");

            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (var i = 0; i < keys.Length; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this IList<TKey> keys,
            IList<TValue> values)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (keys.Count != values.Count) throw new ArgumentException("Size keys and size values diffirent!");

            IDictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            for (var i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i], values[i]);
            }

            return result;
        }

        #endregion

        #region Pick random

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickRandom<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.Length == 0 ? default : collection[UnityEngine.Random.Range(0, collection.Length)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickRandom<T>(this List<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.Count == 0 ? default : collection[UnityEngine.Random.Range(0, collection.Count)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T, int) PickRandomAndIndex<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            int index = UnityEngine.Random.Range(0, collection.Length);
            return collection.Length == 0 ? (default, -1) : (collection[index], index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T, int) PickRandomWithIndex<T>(this List<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var index = UnityEngine.Random.Range(0, collection.Count);
            return collection.Count == 0 ? (default, -1) : (collection[index], index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> PickRandomSubList<T>(this List<T> collection, int length)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var listTemp = collection.ToList();
            List<T> pickList = new List<T>();
            listTemp.Shuffle();
            for (int i = 0; i < listTemp.Count; i++)
            {
                if (i < length) pickList.Add(listTemp[i]);
            }

            return pickList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] PickRandomSubArray<T>(this T[] collection, int length)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            T[] arrayTemp = new T[collection.Length];
            Array.Copy(collection, arrayTemp, collection.Length);
            T[] pickArray = new T[length <= collection.Length ? length : collection.Length];
            arrayTemp.Shuffle();
            for (int i = 0; i < arrayTemp.Length; i++)
            {
                if (i < length) pickArray[i] = arrayTemp[i];
            }

            return pickArray;
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this T[] source, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0 || oldIndex > source.Length || newIndex > source.Length)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Index out of range!");
#endif
                return;
            }

            if (oldIndex == newIndex) return;
            (source[oldIndex], source[newIndex]) = (source[newIndex], source[oldIndex]);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this List<T> source, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || newIndex < 0 || oldIndex > source.Count || newIndex > source.Count)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Index out of range!");
#endif
                return;
            }

            if (oldIndex == newIndex) return;
            (source[oldIndex], source[newIndex]) = (source[newIndex], source[oldIndex]);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ForEach<T>(this T[] source, Action<T> action)
        {
            for (int i = source.Length - 1; i >= 0; i--)
            {
                action(source[i]);
            }

            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ForEach<T>(this List<T> source, Action<T> action)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                action(source[i]);
            }

            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ForEach<T>(this T[] source, Action<T, int> action)
        {
            for (int i = source.Length - 1; i >= 0; i--)
            {
                action(source[i], i);
            }

            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ForEach<T>(this List<T> source, Action<T, int> action)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                action(source[i], i);
            }

            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Removes<T>(this List<T> source, List<T> entries)
        {
            for (var i = 0; i < entries.Count; i++)
            {
                source.Remove(entries[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Removes<T>(this List<T> source, T[] entries)
        {
            foreach (var item in entries)
            {
                source.Remove(item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> Adds<T>(this List<T> source, List<T> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                source.Add(entries[i]);
            }

            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> Adds<T>(this List<T> source, T[] entries)
        {
            foreach (var e in entries)
            {
                source.Add(e);
            }

            return source;
        }
    }
}