using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using VirtueSky.Core;

namespace VirtueSky.Misc
{
    public static partial class Common
    {
        public static bool IsInteger(this float value)
        {
            return (value == (int)value);
        }

        public static int GetNumberInAString(this string str)
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

        public static void Clear<T>(this T[] collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            Array.Clear(collection, 0, collection.Length);
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

        #region Internet Connection

        private static IEnumerator internetConnectionCoroutine;

        public static void StopCheckInternetConnection()
        {
            App.StopCoroutine(internetConnectionCoroutine);
        }

        public static void CheckInternetConnection(Action actionConnected, Action actionDisconnected)
        {
            if (internetConnectionCoroutine != null) App.StopCoroutine(internetConnectionCoroutine);
            internetConnectionCoroutine = InternetConnection((isConnected) =>
            {
                if (isConnected)
                {
                    actionConnected?.Invoke();
                }
                else
                {
                    actionDisconnected?.Invoke();
                }
            });
            App.StartCoroutine(internetConnectionCoroutine);
        }

        public static IEnumerator InternetConnection(Action<bool> action)
        {
            bool result;
            string url = "http://google.com";
#if UNITY_ANDROID
            url = "http://google.com";
#elif UNITY_IOS
            url = "https://captive.apple.com/hotspot-detect.html";
#endif

            using (UnityWebRequest request = UnityWebRequest.Head(url))
            {
                yield return request.SendWebRequest();
                result = !request.isNetworkError && !request.isHttpError && request.responseCode == 200 &&
                         request.error == null;
            }

            action(result);
            internetConnectionCoroutine = null;
        }

        #endregion

        /// <summary>
        /// Attach a DelayHandle on to the behaviour. If the behaviour is destroyed before the DelayHandle is completed,
        /// e.g. through a scene change, the DelayHandle callback will not execute.
        /// </summary>
        /// <param name="target">The behaviour to attach this DelayHandle to.</param>
        /// <param name="duration">The duration to wait before the DelayHandle fires.</param>
        /// <param name="onComplete">The action to run when the DelayHandle elapses.</param>
        /// <param name="onUpdate">A function to call each tick of the DelayHandle. Takes the number of seconds elapsed since
        /// the start of the current cycle.</param>
        /// <param name="isLooped">Whether the DelayHandle should restart after executing.</param>
        /// <param name="useRealTime">Whether the DelayHandle uses real-time(not affected by slow-mo or pausing) or
        /// game-time(affected by time scale changes).</param>
        public static DelayHandle Delay(
            this MonoBehaviour target,
            float duration,
            Action onComplete,
            Action<float> onUpdate = null,
            bool isLooped = false,
            bool useRealTime = false)
        {
            return App.Delay(
                target,
                duration,
                onComplete,
                onUpdate,
                isLooped,
                useRealTime);
        }
    }
}