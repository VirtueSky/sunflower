using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static bool IsInteger(float value)
        {
            return (value == (int)value);
        }

        public static int GetNumberInAString(string str)
        {
            try
            {
                var getNumb = Regex.Match(str, @"\d+").Value;
                return Int32.Parse(getNumb);
            }
            catch (Exception e)
            {
                return -1;
            }

            return -1;
        }

        public static float GetScreenRatio()
        {
            return (1920f / 1080f) / (Screen.height / (float)Screen.width);
        }

        public static void CallActionAndClean(ref Action action)
        {
            if (action == null) return;
            var a = action;
            a();
            action = null;
        }

        #region IsNullOrEmpty

        public static bool IsNullOrEmpty<T>(this List<T> source)
        {
            return source == null || source.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this T[] source)
        {
            return source == null || source.Length == 0;
        }

        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            return source == null || source.Keys.Count == 0;
        }

        #endregion

        #region Shuffle

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

        public static IDictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this IList<TKey> keys, IList<TValue> values)
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

        public static T PickRandom<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.Length == 0 ? default : collection[UnityEngine.Random.Range(0, collection.Length)];
        }

        public static T PickRandom<T>(this List<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.Count == 0 ? default : collection[UnityEngine.Random.Range(0, collection.Count)];
        }

        public static (T, int) PickRandomAndIndex<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            int index = UnityEngine.Random.Range(0, collection.Length);
            return collection.Length == 0 ? (default, -1) : (collection[index], index);
        }

        public static (T, int) PickRandomWithIndex<T>(this List<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            var index = UnityEngine.Random.Range(0, collection.Count);
            return collection.Count == 0 ? (default, -1) : (collection[index], index);
        }

        #endregion
    }
}